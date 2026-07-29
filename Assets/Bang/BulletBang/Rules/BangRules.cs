using System;
using System.Collections.Generic;
using System.Linq;

namespace BulletBang.Rules
{
    /// <summary>
    /// Deterministic, engine-independent authority for a single BANG! match.
    /// Fusion adapters may submit commands and publish projections, but must never
    /// mutate match collections directly. Keeping rules here makes card effects
    /// testable without loading a Unity scene and lets expansions decorate this
    /// layer instead of replacing networking or presentation code.
    /// </summary>
    public sealed class BangRules
    {
        private readonly Random _random;

        public BangRules(int seed) => _random = new Random(seed);

        public BangMatch CreateMatch(IReadOnlyList<string> names)
        {
            if (names == null || names.Count < 3 || names.Count > 7)
                throw new ArgumentException("Base game requires 3-7 players.", nameof(names));

            var match = new BangMatch();
            var roles = BaseGameCatalog.RolesFor(names.Count);
            Shuffle(roles);
            for (var i = 0; i < names.Count; i++)
                match.Players.Add(new BangPlayer { Id = i, Name = names[i], Role = roles[i] });
            match.DrawPile.AddRange(BaseGameCatalog.CreatePlayingDeck());
            Shuffle(match.DrawPile);
            return match;
        }

        public void Begin(BangMatch match)
        {
            if (match.Phase != MatchPhase.Setup) throw new InvalidOperationException("Match already started.");
            foreach (var player in match.Players)
            {
                if (player.MaxHealth <= 0) player.MaxHealth = 4;
                if (player.Role == RoleType.Sheriff) player.MaxHealth++;
                player.Health = player.MaxHealth;
                Draw(match, player, player.MaxHealth);
            }
            var sheriff = match.Players.FindIndex(p => p.Role == RoleType.Sheriff);
            match.CurrentPlayerIndex = sheriff >= 0 ? sheriff : match.Players.FindIndex(p => p.Role == RoleType.Deputy);
            match.Phase = MatchPhase.Draw;
        }

        public static int Distance(BangMatch match, int fromId, int toId)
        {
            // Only living players occupy seats for distance. Eliminated players
            // collapse out of the ring, as required by the tabletop rules.
            if (fromId == toId) return 0;
            var alive = match.Players.Where(p => p.Alive).ToList();
            var from = alive.FindIndex(p => p.Id == fromId);
            var to = alive.FindIndex(p => p.Id == toId);
            if (from < 0 || to < 0) return int.MaxValue;
            var clockwise = (to - from + alive.Count) % alive.Count;
            var distance = Math.Min(clockwise, alive.Count - clockwise);
            var source = alive[from];
            var target = alive[to];
            if (source.Character == CharacterType.RoseDoolan || source.InPlay.Any(c => c.Name == PlayingCardName.Scope))
                distance--;
            if (target.Character == CharacterType.PaulRegret || target.InPlay.Any(c => c.Name == PlayingCardName.Mustang))
                distance++;
            return Math.Max(1, distance);
        }

        public RuleResult DrawPhase(BangMatch match)
        {
            if (match.Phase != MatchPhase.Draw) return RuleResult.Fail("Not in draw phase.");
            Draw(match, match.CurrentPlayer, 2);
            match.Phase = MatchPhase.Play;
            return RuleResult.Ok();
        }

        public RuleResult PlayCard(BangMatch match, int playerId, int cardId, int? targetId = null)
        {
            if (match.Phase != MatchPhase.Play || match.CurrentPlayer?.Id != playerId)
                return RuleResult.Fail("It is not this player's play phase.");
            var player = FindAlive(match, playerId);
            var card = player?.Hand.FirstOrDefault(c => c.Id == cardId);
            if (card == null) return RuleResult.Fail("Card is not in the player's hand.");

            if (card.Color == CardColor.Blue)
                return PlayBlue(match, player, card, targetId);
            return PlayBrown(match, player, card, targetId);
        }

        public RuleResult EndPlayPhase(BangMatch match, int playerId)
        {
            if (match.Phase != MatchPhase.Play || match.CurrentPlayer?.Id != playerId)
                return RuleResult.Fail("It is not this player's play phase.");
            match.Phase = MatchPhase.Discard;
            return RuleResult.Ok();
        }

        public RuleResult Discard(BangMatch match, int playerId, int cardId)
        {
            if (match.Phase != MatchPhase.Discard || match.CurrentPlayer?.Id != playerId)
                return RuleResult.Fail("It is not this player's discard phase.");
            var player = FindAlive(match, playerId);
            var card = player?.Hand.FirstOrDefault(c => c.Id == cardId);
            if (card == null) return RuleResult.Fail("Card is not in hand.");
            player.Hand.Remove(card);
            match.DiscardPile.Add(card);
            return RuleResult.Ok();
        }

        public RuleResult EndTurn(BangMatch match, int playerId)
        {
            var player = match.CurrentPlayer;
            if (match.Phase != MatchPhase.Discard || player?.Id != playerId)
                return RuleResult.Fail("It is not this player's discard phase.");
            if (player.Hand.Count > player.Health) return RuleResult.Fail("Discard down to current health.");
            player.BangsPlayedThisTurn = 0;
            Advance(match);
            return RuleResult.Ok();
        }

        private RuleResult PlayBlue(BangMatch match, BangPlayer player, BangCard card, int? targetId)
        {
            var target = card.Name == PlayingCardName.Jail
                ? (targetId.HasValue ? FindAlive(match, targetId.Value) : null)
                : player;
            if (target == null || (card.Name == PlayingCardName.Jail && target.Role == RoleType.Sheriff))
                return RuleResult.Fail("Invalid target.");
            var isWeapon = card.Range > 0;
            // A player can have one weapon and one copy of each other blue card.
            // Replacing an existing card is legal and sends the old copy to discard.
            var duplicate = target.InPlay.FirstOrDefault(c => isWeapon ? c.Range > 0 : c.Name == card.Name);
            if (duplicate != null) { target.InPlay.Remove(duplicate); match.DiscardPile.Add(duplicate); }
            MoveFromHand(player, card, target.InPlay);
            return RuleResult.Ok();
        }

        private RuleResult PlayBrown(BangMatch match, BangPlayer player, BangCard card, int? targetId)
        {
            BangPlayer target = targetId.HasValue ? FindAlive(match, targetId.Value) : null;
            switch (card.Name)
            {
                case PlayingCardName.Beer:
                    if (player.Health >= player.MaxHealth || match.Players.Count(p => p.Alive) <= 2)
                        return RuleResult.Fail("Beer has no valid effect.");
                    player.Health++;
                    break;
                case PlayingCardName.StageCoach: Draw(match, player, 2); break;
                case PlayingCardName.WellsFargo: Draw(match, player, 3); break;
                case PlayingCardName.Saloon:
                    foreach (var p in match.Players.Where(p => p.Alive && p.Health < p.MaxHealth)) p.Health++;
                    break;
                case PlayingCardName.Panic:
                    if (target == null || target == player)
                        return RuleResult.Fail("Panic! requires another player.");
                    if (Distance(match, player.Id, target.Id) > 1)
                        return RuleResult.Fail("Panic! only reaches distance 1.");
                    var stolen = RandomOwnedCard(target);
                    if (stolen == null) return RuleResult.Fail("That player has no cards.");
                    target.Hand.Remove(stolen);
                    target.InPlay.Remove(stolen);
                    player.Hand.Add(stolen);
                    break;
                case PlayingCardName.CatBalou:
                    if (target == null || target == player)
                        return RuleResult.Fail("Cat Balou requires another player.");
                    var discarded = RandomOwnedCard(target);
                    if (discarded == null) return RuleResult.Fail("That player has no cards.");
                    target.Hand.Remove(discarded);
                    target.InPlay.Remove(discarded);
                    match.DiscardPile.Add(discarded);
                    break;
                case PlayingCardName.Bang:
                    if (target == null || target == player) return RuleResult.Fail("BANG! requires another player.");
                    if (!CanPlayBang(player)) return RuleResult.Fail("Only one BANG! may be played this turn.");
                    if (Distance(match, player.Id, target.Id) > WeaponRange(player)) return RuleResult.Fail("Target is out of range.");
                    player.BangsPlayedThisTurn++;
                    BeginResponse(match, card.Name, player.Id, new[] { target.Id },
                        player.Character == CharacterType.SlabTheKiller ? 2 : 1);
                    break;
                case PlayingCardName.Gatling:
                case PlayingCardName.Indians:
                    BeginResponse(match, card.Name, player.Id,
                        match.Players.Where(p => p.Alive && p != player).Select(p => p.Id), 1);
                    break;
                case PlayingCardName.Duel:
                    if (target == null || target == player)
                        return RuleResult.Fail("Duel requires another player.");
                    BeginResponse(match, card.Name, player.Id, new[] { target.Id }, 1);
                    match.Pending.DuelTargetPlayerId = target.Id;
                    break;
                default:
                    return RuleResult.Fail($"{card.Name} requires a selection flow not supplied by this command.");
            }
            player.Hand.Remove(card);
            match.DiscardPile.Add(card);
            return RuleResult.Ok();
        }

        public RuleResult Respond(BangMatch match, int playerId, int? cardId) =>
            Respond(match, playerId, cardId.HasValue ? new[] { cardId.Value } : Array.Empty<int>());

        public RuleResult Respond(BangMatch match, int playerId, IReadOnlyList<int> cardIds)
        {
            var pending = match.Pending;
            if (match.Phase != MatchPhase.Responding || pending == null ||
                pending.Responders.Count == 0 || pending.Responders.Peek() != playerId)
                return RuleResult.Fail("This player is not awaiting a response.");
            var player = FindAlive(match, playerId);
            var required = pending.Cause == PlayingCardName.Indians ||
                           pending.Cause == PlayingCardName.Duel
                ? PlayingCardName.Bang : PlayingCardName.Missed;
            var paid = 0;
            foreach (var cardId in cardIds)
            {
                if (paid >= pending.RequiredMissed) break;
                var card = player.Hand.FirstOrDefault(c => c.Id == cardId &&
                    (c.Name == required || (player.Character == CharacterType.CalamityJanet &&
                     (c.Name == PlayingCardName.Bang || c.Name == PlayingCardName.Missed))));
                if (card == null) return RuleResult.Fail($"A {required} is required.");
                player.Hand.Remove(card);
                match.DiscardPile.Add(card);
                paid++;
            }
            if (pending.Cause == PlayingCardName.Duel)
            {
                pending.Responders.Dequeue();
                if (paid < pending.RequiredMissed)
                {
                    var opponent = playerId == pending.SourcePlayerId
                        ? pending.DuelTargetPlayerId : pending.SourcePlayerId;
                    Damage(match, player, 1, opponent);
                    match.Pending = null;
                    if (match.Phase != MatchPhase.Finished) match.Phase = MatchPhase.Play;
                    return RuleResult.Ok();
                }

                var next = playerId == pending.SourcePlayerId
                    ? pending.DuelTargetPlayerId : pending.SourcePlayerId;
                pending.Responders.Enqueue(next);
                return RuleResult.Ok();
            }

            if (paid < pending.RequiredMissed) Damage(match, player, 1, pending.SourcePlayerId);
            pending.Responders.Dequeue();
            if (pending.Responders.Count == 0) { match.Pending = null; match.Phase = MatchPhase.Play; }
            return RuleResult.Ok();
        }

        private void BeginResponse(BangMatch match, PlayingCardName cause, int source,
            IEnumerable<int> responders, int missed)
        {
            match.Pending = new PendingResponse { Cause = cause, SourcePlayerId = source, RequiredMissed = missed };
            foreach (var responder in responders) match.Pending.Responders.Enqueue(responder);
            match.Phase = MatchPhase.Responding;
        }

        private void Damage(BangMatch match, BangPlayer player, int amount, int sourceId)
        {
            // All damage enters through this method so character triggers, kill
            // rewards, Sheriff penalties, and future expansion hooks stay ordered.
            player.Health -= amount;
            if (player.Character == CharacterType.BartCassidy) Draw(match, player, amount);
            if (player.Health > 0) return;
            player.Alive = false;
            player.Health = 0;
            var killer = match.Players.FirstOrDefault(p => p.Id == sourceId);
            if (player.Role == RoleType.Outlaw && killer != null) Draw(match, killer, 3);
            if (killer?.Role == RoleType.Sheriff && player.Role == RoleType.Deputy)
            {
                match.DiscardPile.AddRange(killer.Hand); killer.Hand.Clear();
                match.DiscardPile.AddRange(killer.InPlay); killer.InPlay.Clear();
            }
            EvaluateWinner(match);
        }

        public static void EvaluateWinner(BangMatch match)
        {
            var alive = match.Players.Where(p => p.Alive).ToList();
            if (match.Players.Count == 3)
            {
                if (alive.Count != 1) return;
                match.Winner = alive[0].Role switch
                {
                    RoleType.Deputy => MatchWinner.SheriffTeam,
                    RoleType.Outlaw => MatchWinner.Outlaws,
                    RoleType.Renegade => MatchWinner.Renegade,
                    _ => MatchWinner.None
                };
                match.Phase = MatchPhase.Finished;
                return;
            }
            var sheriffAlive = alive.Any(p => p.Role == RoleType.Sheriff);
            if (!sheriffAlive && match.Players.Any(p => p.Role == RoleType.Sheriff))
                match.Winner = alive.Count == 1 && alive[0].Role == RoleType.Renegade
                    ? MatchWinner.Renegade : MatchWinner.Outlaws;
            else if (!alive.Any(p => p.Role == RoleType.Outlaw || p.Role == RoleType.Renegade))
                match.Winner = MatchWinner.SheriffTeam;
            else return;
            match.Phase = MatchPhase.Finished;
        }

        private void Draw(BangMatch match, BangPlayer player, int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (match.DrawPile.Count == 0)
                {
                    if (match.DiscardPile.Count <= 1) return;
                    var top = match.DiscardPile[^1];
                    match.DiscardPile.RemoveAt(match.DiscardPile.Count - 1);
                    match.DrawPile.AddRange(match.DiscardPile);
                    match.DiscardPile.Clear();
                    match.DiscardPile.Add(top);
                    Shuffle(match.DrawPile);
                }
                var card = match.DrawPile[^1];
                match.DrawPile.RemoveAt(match.DrawPile.Count - 1);
                player.Hand.Add(card);
            }
        }

        private void Advance(BangMatch match)
        {
            do match.CurrentPlayerIndex = (match.CurrentPlayerIndex + 1) % match.Players.Count;
            while (!match.CurrentPlayer.Alive);
            match.Phase = MatchPhase.Draw;
        }

        private static BangPlayer FindAlive(BangMatch match, int id) =>
            match.Players.FirstOrDefault(p => p.Id == id && p.Alive);
        private static void MoveFromHand(BangPlayer player, BangCard card, List<BangCard> destination)
        { player.Hand.Remove(card); destination.Add(card); }
        private static bool CanPlayBang(BangPlayer player) =>
            player.BangsPlayedThisTurn == 0 || player.Character == CharacterType.WillyTheKid ||
            player.InPlay.Any(c => c.Name == PlayingCardName.Volcanic);
        private static int WeaponRange(BangPlayer player) =>
            player.InPlay.Where(c => c.Range > 0).Select(c => c.Range).DefaultIfEmpty(1).Max();
        private BangCard RandomOwnedCard(BangPlayer player)
        {
            var count = player.Hand.Count + player.InPlay.Count;
            if (count == 0) return null;
            var index = _random.Next(count);
            return index < player.Hand.Count
                ? player.Hand[index]
                : player.InPlay[index - player.Hand.Count];
        }
        private void Shuffle<T>(IList<T> values)
        {
            for (var i = values.Count - 1; i > 0; i--)
            {
                var j = _random.Next(i + 1);
                (values[i], values[j]) = (values[j], values[i]);
            }
        }
    }
}
