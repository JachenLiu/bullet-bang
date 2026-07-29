using System;
using System.Collections.Generic;
using System.Linq;
using BulletBang.Rules;
using Fusion;
using UnityEngine;
using BulletBang.Lobby;

namespace BulletBang
{
    /// <summary>
    /// Fusion adapter for one table-owned match. The server keeps the complete
    /// BangMatch; clients receive public arrays plus targeted private setup data.
    /// </summary>
    public sealed class GameSession : NetworkBehaviour, ITableGameSession
    {
        public string DisplayName => "BANG!";
        public int MinimumPlayers => 3;
        public int MaximumPlayers => 7;
        public bool SupportsSoloTesting => true;

        [Networked] public int PlayerCount { get; private set; }
        [Networked] public MatchPhase Phase { get; private set; }
        [Networked] public int CurrentPlayerSeat { get; private set; }
        [Networked] public int DrawPileCount { get; private set; }
        [Networked] public int DiscardPileCount { get; private set; }
        [Networked] public int PendingResponderSeat { get; private set; }
        [Networked] public int TopDiscardName { get; private set; }

        [Networked, Capacity(7)] public NetworkArray<PlayerRef> SeatPlayers => default;
        [Networked, Capacity(7)] public NetworkArray<NetworkString<_32>> PlayerNames => default;
        [Networked, Capacity(7)] public NetworkArray<int> Health => default;
        [Networked, Capacity(7)] public NetworkArray<int> MaxHealth => default;
        [Networked, Capacity(7)] public NetworkArray<CharacterType> Characters => default;
        [Networked, Capacity(7)] public NetworkArray<int> VisibleRoles => default;

        private readonly Dictionary<PlayerRef, int> _seatByPlayer = new();
        private readonly Dictionary<PlayerRef, (CharacterType first, CharacterType second)> _options = new();
        private BangRules _rules;
        private BangMatch _match;

        public void Initialize(IReadOnlyList<NetworkPlayer> players)
        {
            if (!Object.HasStateAuthority)
                throw new InvalidOperationException("Only state authority may initialize a match.");
            if (players == null || players.Count < 1 || players.Count > GameTable.MAX_PLAYERS)
                throw new ArgumentOutOfRangeException(nameof(players), "A base match requires 3-7 players.");

            var seed = unchecked((int)(Runner.Tick.Raw * 397L ^ Object.Id.Raw));
            _rules = new BangRules(seed);
            var names = players.Select(player => player.PlayerName.ToString()).ToList();
            var soloTest = names.Count == 1;
            if (soloTest)
            {
                names.Add("Test Bot Left");
                names.Add("Test Bot Right");
            }
            _match = _rules.CreateMatch(names);
            if (soloTest) MakeFirstPlayerSheriff(_match);
            var characterPool = BaseGameCatalog.BaseCharacters().ToList();
            Shuffle(characterPool, new System.Random(seed ^ 0x5f3759df));

            PlayerCount = names.Count;
            Phase = MatchPhase.Setup;
            CurrentPlayerSeat = -1;
            PendingResponderSeat = -1;
            TopDiscardName = -1;
            for (var seat = 0; seat < PlayerCount; seat++)
            {
                var role = _match.Players[seat].Role;
                var choices = (characterPool[seat * 2], characterPool[seat * 2 + 1]);

                if (seat < players.Count)
                {
                    var networkPlayer = players[seat];
                    var playerRef = networkPlayer.Object.InputAuthority;
                    _seatByPlayer[playerRef] = seat;
                    _options[playerRef] = choices;
                    SeatPlayers.Set(seat, playerRef);
                    PlayerNames.Set(seat, networkPlayer.PlayerName);
                    RPC_ReceivePrivateSetup(playerRef, role, choices.Item1, choices.Item2);
                }
                else
                {
                    SeatPlayers.Set(seat, PlayerRef.None);
                    PlayerNames.Set(seat, names[seat]);
                    var bot = _match.Players[seat];
                    bot.Character = choices.Item1;
                    bot.MaxHealth = BaseGameCatalog.CharacterHealth(bot.Character);
                    Characters.Set(seat, bot.Character);
                }
                Health.Set(seat, 0);
                MaxHealth.Set(seat, 0);
                VisibleRoles.Set(seat, role == RoleType.Sheriff ? (int)role : -1);
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ReceivePrivateSetup(
            [RpcTarget] PlayerRef recipient,
            RoleType role,
            CharacterType first,
            CharacterType second)
        {
            LocalMatchPrivateState.ReceiveSetup(Object.Id, role, first, second);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_ChooseCharacter(CharacterType choice, RpcInfo info = default)
        {
            if (!_seatByPlayer.TryGetValue(info.Source, out var seat)) return;
            if (!_options.TryGetValue(info.Source, out var choices)) return;
            if (choice != choices.first && choice != choices.second) return;
            if (_match.Players[seat].MaxHealth > 0) return;

            var player = _match.Players[seat];
            player.Character = choice;
            player.MaxHealth = BaseGameCatalog.CharacterHealth(choice);
            Characters.Set(seat, choice);
            MaxHealth.Set(seat, player.MaxHealth + (player.Role == RoleType.Sheriff ? 1 : 0));
            Health.Set(seat, MaxHealth[seat]);
            _options.Remove(info.Source);

            if (_options.Count == 0)
                BeginMatch();
        }

        private void BeginMatch()
        {
            _rules.Begin(_match);
            Phase = _match.Phase;
            CurrentPlayerSeat = _match.CurrentPlayerIndex;
            for (var seat = 0; seat < PlayerCount; seat++)
            {
                Health.Set(seat, _match.Players[seat].Health);
                MaxHealth.Set(seat, _match.Players[seat].MaxHealth);
            }
            RPC_MatchReady();
            SendAllPrivateHands();
            PublishState();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_DrawPhase(RpcInfo info = default)
        {
            if (!IsCurrentPlayer(info.Source)) return;
            Apply(_rules.DrawPhase(_match), info.Source);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_PlayCard(int cardId, int targetSeat, RpcInfo info = default)
        {
            if (!IsCurrentPlayer(info.Source)) return;
            int? targetId = targetSeat >= 0 && targetSeat < PlayerCount ? targetSeat : null;
            Apply(_rules.PlayCard(_match, _seatByPlayer[info.Source], cardId, targetId), info.Source);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_EndPlayPhase(RpcInfo info = default)
        {
            if (!IsCurrentPlayer(info.Source)) return;
            Apply(_rules.EndPlayPhase(_match, _seatByPlayer[info.Source]), info.Source);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Discard(int cardId, RpcInfo info = default)
        {
            if (!IsCurrentPlayer(info.Source)) return;
            Apply(_rules.Discard(_match, _seatByPlayer[info.Source], cardId), info.Source);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_EndTurn(RpcInfo info = default)
        {
            if (!IsCurrentPlayer(info.Source)) return;
            Apply(_rules.EndTurn(_match, _seatByPlayer[info.Source]), info.Source);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_Respond(int firstCardId, int secondCardId, RpcInfo info = default)
        {
            if (!_seatByPlayer.TryGetValue(info.Source, out var seat)) return;
            var cards = new List<int>(2);
            if (firstCardId > 0) cards.Add(firstCardId);
            if (secondCardId > 0) cards.Add(secondCardId);
            Apply(_rules.Respond(_match, seat, cards), info.Source);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_ResetPrivateHand([RpcTarget] PlayerRef recipient)
        {
            LocalMatchPrivateState.ResetHand(Object.Id);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_AddPrivateCard([RpcTarget] PlayerRef recipient, int id,
            PlayingCardName name, CardSuit suit, CardNumber rank, CardColor color, int range)
        {
            LocalMatchPrivateState.AddCard(Object.Id,
                new PrivateCardView(id, name, suit, rank, color, range));
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_CommandRejected([RpcTarget] PlayerRef recipient, NetworkString<_128> reason)
        {
            LocalMatchPrivateState.SetError(Object.Id, reason.ToString());
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_MatchReady()
        {
            LocalMatchPrivateState.NotifyMatchReady(Object.Id);
        }

        private bool IsCurrentPlayer(PlayerRef player) =>
            _seatByPlayer.TryGetValue(player, out var seat) &&
            _match?.CurrentPlayerIndex == seat;

        private void Apply(RuleResult result, PlayerRef requestingPlayer)
        {
            if (!result.Success)
            {
                Debug.LogWarning($"Rejected BANG! command: {result.Error}");
                RPC_CommandRejected(requestingPlayer, result.Error);
                return;
            }
            PublishState();
            SendAllPrivateHands();
            AdvanceTestBots();
        }

        /// <summary>
        /// Solo testing retains the real three-seat rules. The two server-owned
        /// seats take intentionally simple turns so every human action, response,
        /// damage path, and turn transition remains testable without extra builds.
        /// </summary>
        private void AdvanceTestBots()
        {
            if (!Object.HasStateAuthority || _match == null) return;
            var guard = 0;
            while (_match.Phase != MatchPhase.Finished && guard++ < 32)
            {
                if (_match.Phase == MatchPhase.Responding)
                {
                    var responder = _match.Pending?.Responders.Peek() ?? -1;
                    if (responder < 0 || SeatPlayers[responder] != PlayerRef.None) break;
                    _rules.Respond(_match, responder, Array.Empty<int>());
                    continue;
                }

                var seat = _match.CurrentPlayerIndex;
                if (seat < 0 || SeatPlayers[seat] != PlayerRef.None) break;
                if (_match.Phase == MatchPhase.Draw) _rules.DrawPhase(_match);
                if (_match.Phase == MatchPhase.Play) _rules.EndPlayPhase(_match, seat);
                while (_match.Phase == MatchPhase.Discard &&
                       _match.Players[seat].Hand.Count > _match.Players[seat].Health)
                    _rules.Discard(_match, seat, _match.Players[seat].Hand[0].Id);
                if (_match.Phase == MatchPhase.Discard) _rules.EndTurn(_match, seat);
            }
            PublishState();
            SendAllPrivateHands();
        }

        private void PublishState()
        {
            Phase = _match.Phase;
            CurrentPlayerSeat = _match.CurrentPlayerIndex;
            DrawPileCount = _match.DrawPile.Count;
            DiscardPileCount = _match.DiscardPile.Count;
            TopDiscardName = _match.DiscardPile.Count > 0
                ? (int)_match.DiscardPile[^1].Name : -1;
            PendingResponderSeat = _match.Pending != null && _match.Pending.Responders.Count > 0
                ? _match.Pending.Responders.Peek() : -1;
            for (var seat = 0; seat < PlayerCount; seat++)
            {
                Health.Set(seat, _match.Players[seat].Health);
                MaxHealth.Set(seat, _match.Players[seat].MaxHealth);
                if (!_match.Players[seat].Alive)
                    VisibleRoles.Set(seat, (int)_match.Players[seat].Role);
            }
        }

        private void SendAllPrivateHands()
        {
            for (var seat = 0; seat < PlayerCount; seat++)
            {
                var recipient = SeatPlayers[seat];
                if (recipient == PlayerRef.None) continue;
                RPC_ResetPrivateHand(recipient);
                foreach (var card in _match.Players[seat].Hand)
                    RPC_AddPrivateCard(recipient, card.Id, card.Name, card.Suit,
                        card.Rank, card.Color, card.Range);
            }
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (LocalMatchPrivateState.SessionId == Object.Id)
                LocalMatchPrivateState.Clear();
        }

        private static void Shuffle<T>(IList<T> values, System.Random random)
        {
            for (var i = values.Count - 1; i > 0; i--)
            {
                var j = random.Next(i + 1);
                (values[i], values[j]) = (values[j], values[i]);
            }
        }

        private static void MakeFirstPlayerSheriff(BangMatch match)
        {
            var sheriff = match.Players.FindIndex(player => player.Role == RoleType.Sheriff);
            if (sheriff <= 0) return;
            (match.Players[0].Role, match.Players[sheriff].Role) =
                (match.Players[sheriff].Role, match.Players[0].Role);
        }
    }

    /// <summary>
    /// Client-only cache populated by targeted RPCs. Spectators never receive
    /// these calls, so public POV switching cannot reveal roles or choices.
    /// </summary>
    public static class LocalMatchPrivateState
    {
        public static NetworkId SessionId { get; private set; }
        public static RoleType Role { get; private set; }
        public static CharacterType FirstCharacter { get; private set; }
        public static CharacterType SecondCharacter { get; private set; }
        public static bool HasSetup { get; private set; }
        public static readonly List<PrivateCardView> Hand = new();
        public static string LastError { get; private set; }

        public static event Action SetupReceived;
        public static event Action MatchReady;
        public static event Action HandChanged;
        public static event Action Cleared;

        public static void ReceiveSetup(NetworkId sessionId, RoleType role,
            CharacterType first, CharacterType second)
        {
            SessionId = sessionId;
            Role = role;
            FirstCharacter = first;
            SecondCharacter = second;
            HasSetup = true;
            SetupReceived?.Invoke();
        }

        public static void NotifyMatchReady(NetworkId sessionId)
        {
            if (SessionId == sessionId) MatchReady?.Invoke();
        }

        public static void ResetHand(NetworkId sessionId)
        {
            if (SessionId != sessionId) return;
            Hand.Clear();
            LastError = null;
            HandChanged?.Invoke();
        }

        public static void AddCard(NetworkId sessionId, PrivateCardView card)
        {
            if (SessionId != sessionId) return;
            Hand.Add(card);
            HandChanged?.Invoke();
        }

        public static void SetError(NetworkId sessionId, string error)
        {
            if (SessionId == sessionId) LastError = error;
        }

        public static void Clear()
        {
            SessionId = default;
            HasSetup = false;
            Hand.Clear();
            LastError = null;
            Cleared?.Invoke();
        }
    }

    public readonly struct PrivateCardView
    {
        public readonly int Id;
        public readonly PlayingCardName Name;
        public readonly CardSuit Suit;
        public readonly CardNumber Rank;
        public readonly CardColor Color;
        public readonly int Range;

        public PrivateCardView(int id, PlayingCardName name, CardSuit suit,
            CardNumber rank, CardColor color, int range)
        {
            Id = id;
            Name = name;
            Suit = suit;
            Rank = rank;
            Color = color;
            Range = range;
        }
    }
}
