using System;
using System.Collections.Generic;
using System.Linq;

namespace BulletBang.Rules
{
    [Serializable]
    public sealed class PublicPlayerView
    {
        public int Id;
        public string Name;
        public int Health;
        public int MaxHealth;
        public bool Alive;
        public int HandCount;
        public RoleType? VisibleRole;
        public CharacterType Character;
        public readonly List<BangCard> InPlay = new();
    }

    [Serializable]
    public sealed class MatchView
    {
        public MatchPhase Phase;
        public MatchWinner Winner;
        public int CurrentPlayerId;
        public int DrawPileCount;
        public BangCard TopDiscard;
        public readonly List<PublicPlayerView> Players = new();
        public readonly List<BangCard> PrivateHand = new();
        public RoleType? PrivateRole;
    }

    /// <summary>
    /// Produces the only match data clients are allowed to receive. Passing null
    /// creates a spectator view and therefore never exposes a hand or hidden role.
    /// </summary>
    public static class MatchProjection
    {
        public static MatchView For(BangMatch match, int? requestingPlayerId)
        {
            var view = new MatchView
            {
                Phase = match.Phase,
                Winner = match.Winner,
                CurrentPlayerId = match.CurrentPlayer?.Id ?? -1,
                DrawPileCount = match.DrawPile.Count,
                TopDiscard = match.DiscardPile.LastOrDefault()
            };

            foreach (var player in match.Players)
            {
                var isSelf = requestingPlayerId == player.Id;
                var playerView = new PublicPlayerView
                {
                    Id = player.Id,
                    Name = player.Name,
                    Health = player.Health,
                    MaxHealth = player.MaxHealth,
                    Alive = player.Alive,
                    HandCount = player.Hand.Count,
                    Character = player.Character,
                    VisibleRole = player.Role == RoleType.Sheriff || !player.Alive || isSelf ||
                                  match.Phase == MatchPhase.Finished
                        ? player.Role : null
                };
                playerView.InPlay.AddRange(player.InPlay);
                view.Players.Add(playerView);
                if (!isSelf) continue;
                view.PrivateHand.AddRange(player.Hand);
                view.PrivateRole = player.Role;
            }
            return view;
        }
    }
}
