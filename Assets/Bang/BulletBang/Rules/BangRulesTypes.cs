using System;
using System.Collections.Generic;

namespace BulletBang.Rules
{
    // These types contain no MonoBehaviour or NetworkBehaviour state. The server
    // owns one BangMatch per table and exposes it only through MatchProjection.
    public enum MatchPhase { Setup, Draw, Play, Discard, Responding, Finished }
    public enum MatchWinner { None, SheriffTeam, Outlaws, Renegade }
    public enum CardColor { Brown, Blue }
    public enum TargetKind { None, Self, OtherPlayer, AnyPlayer, AllOtherPlayers }

    [Serializable]
    public sealed class BangCard
    {
        public int Id;
        public PlayingCardName Name;
        public CardSuit Suit;
        public CardNumber Rank;
        public CardColor Color;
        public int Range;

        public BangCard(int id, PlayingCardName name, CardSuit suit, CardNumber rank,
            CardColor color = CardColor.Brown, int range = 0)
        {
            Id = id;
            Name = name;
            Suit = suit;
            Rank = rank;
            Color = color;
            Range = range;
        }
    }

    [Serializable]
    public sealed class BangPlayer
    {
        public int Id;
        public string Name;
        public RoleType Role;
        public CharacterType Character;
        public int Health;
        public int MaxHealth;
        public bool Alive = true;
        public readonly List<BangCard> Hand = new();
        public readonly List<BangCard> InPlay = new();
        public int BangsPlayedThisTurn;
    }

    [Serializable]
    public sealed class PendingResponse
    {
        public PlayingCardName Cause;
        public int SourcePlayerId;
        public readonly Queue<int> Responders = new();
        public int RequiredMissed = 1;
        public int DuelTargetPlayerId = -1;
    }

    [Serializable]
    public sealed class BangMatch
    {
        public readonly List<BangPlayer> Players = new();
        public readonly List<BangCard> DrawPile = new();
        public readonly List<BangCard> DiscardPile = new();
        public MatchPhase Phase = MatchPhase.Setup;
        public MatchWinner Winner;
        public int CurrentPlayerIndex = -1;
        public PendingResponse Pending;

        public BangPlayer CurrentPlayer =>
            CurrentPlayerIndex >= 0 && CurrentPlayerIndex < Players.Count
                ? Players[CurrentPlayerIndex]
                : null;
    }

    public readonly struct RuleResult
    {
        public bool Success { get; }
        public string Error { get; }

        private RuleResult(bool success, string error)
        {
            Success = success;
            Error = error;
        }

        public static RuleResult Ok() => new(true, null);
        public static RuleResult Fail(string error) => new(false, error);
    }
}
