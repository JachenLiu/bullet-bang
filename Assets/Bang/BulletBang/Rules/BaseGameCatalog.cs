using System;
using System.Collections.Generic;

namespace BulletBang.Rules
{
    /// <summary>
    /// Canonical base-game content manifest. Visual assets are deliberately not
    /// referenced here: deck identity must remain stable if a material is missing
    /// or replaced, and expansion catalogs can be appended independently.
    /// </summary>
    public static class BaseGameCatalog
    {
        public const int BaseDeckSize = 80;

        public static IReadOnlyList<CharacterType> BaseCharacters() => new[]
        {
            CharacterType.BartCassidy, CharacterType.BlackJack, CharacterType.CalamityJanet,
            CharacterType.ElGringo, CharacterType.JesseJones, CharacterType.Jourdonnais,
            CharacterType.KitCarlson, CharacterType.LuckyDuke, CharacterType.PaulRegret,
            CharacterType.PedroRamirez, CharacterType.RoseDoolan, CharacterType.SidKetchum,
            CharacterType.SlabTheKiller, CharacterType.SuzyLafayette,
            CharacterType.VultureSam, CharacterType.WillyTheKid
        };

        public static int CharacterHealth(CharacterType character) =>
            character is CharacterType.ElGringo or CharacterType.PaulRegret ? 3 : 4;

        public static List<RoleType> RolesFor(int playerCount)
        {
            return playerCount switch
            {
                3 => new() { RoleType.Deputy, RoleType.Outlaw, RoleType.Renegade },
                4 => new() { RoleType.Sheriff, RoleType.Renegade, RoleType.Outlaw, RoleType.Outlaw },
                5 => new() { RoleType.Sheriff, RoleType.Renegade, RoleType.Outlaw, RoleType.Outlaw, RoleType.Deputy },
                6 => new() { RoleType.Sheriff, RoleType.Renegade, RoleType.Outlaw, RoleType.Outlaw, RoleType.Outlaw, RoleType.Deputy },
                7 => new() { RoleType.Sheriff, RoleType.Renegade, RoleType.Outlaw, RoleType.Outlaw, RoleType.Outlaw, RoleType.Deputy, RoleType.Deputy },
                _ => throw new ArgumentOutOfRangeException(nameof(playerCount), "Base game supports 3-7 players.")
            };
        }

        public static List<BangCard> CreatePlayingDeck()
        {
            var cards = new List<BangCard>(BaseDeckSize);
            var id = 1;
            void Add(PlayingCardName name, CardSuit suit, CardNumber rank,
                CardColor color = CardColor.Brown, int range = 0) =>
                cards.Add(new BangCard(id++, name, suit, rank, color, range));

            Add(PlayingCardName.Volcanic, CardSuit.Club, CardNumber.Ten, CardColor.Blue, 1);
            Add(PlayingCardName.Volcanic, CardSuit.Spade, CardNumber.Ten, CardColor.Blue, 1);
            Add(PlayingCardName.Schofield, CardSuit.Club, CardNumber.Jack, CardColor.Blue, 2);
            Add(PlayingCardName.Schofield, CardSuit.Club, CardNumber.Queen, CardColor.Blue, 2);
            Add(PlayingCardName.Schofield, CardSuit.Spade, CardNumber.King, CardColor.Blue, 2);
            Add(PlayingCardName.Remington, CardSuit.Club, CardNumber.King, CardColor.Blue, 3);
            Add(PlayingCardName.RevCarabine, CardSuit.Club, CardNumber.Ace, CardColor.Blue, 4);
            Add(PlayingCardName.Winchester, CardSuit.Spade, CardNumber.Eight, CardColor.Blue, 5);
            Add(PlayingCardName.Scope, CardSuit.Spade, CardNumber.Ace, CardColor.Blue);
            Add(PlayingCardName.Dynamite, CardSuit.Heart, CardNumber.Two, CardColor.Blue);
            Add(PlayingCardName.Mustang, CardSuit.Heart, CardNumber.Eight, CardColor.Blue);
            Add(PlayingCardName.Mustang, CardSuit.Heart, CardNumber.Nine, CardColor.Blue);
            Add(PlayingCardName.Barrel, CardSuit.Spade, CardNumber.Queen, CardColor.Blue);
            Add(PlayingCardName.Barrel, CardSuit.Spade, CardNumber.King, CardColor.Blue);
            Add(PlayingCardName.Jail, CardSuit.Spade, CardNumber.Jack, CardColor.Blue);
            Add(PlayingCardName.Jail, CardSuit.Spade, CardNumber.Ten, CardColor.Blue);
            Add(PlayingCardName.Jail, CardSuit.Heart, CardNumber.Four, CardColor.Blue);

            AddMany(Add, PlayingCardName.Panic, (CardSuit.Diamond, CardNumber.Eight),
                (CardSuit.Heart, CardNumber.Jack), (CardSuit.Heart, CardNumber.Queen), (CardSuit.Heart, CardNumber.Ace));
            AddMany(Add, PlayingCardName.Beer, (CardSuit.Heart, CardNumber.Six),
                (CardSuit.Heart, CardNumber.Seven), (CardSuit.Heart, CardNumber.Eight),
                (CardSuit.Heart, CardNumber.Nine), (CardSuit.Heart, CardNumber.Ten), (CardSuit.Heart, CardNumber.Jack));
            AddMany(Add, PlayingCardName.GeneralStore, (CardSuit.Club, CardNumber.Nine), (CardSuit.Club, CardNumber.Queen));
            AddMany(Add, PlayingCardName.Indians, (CardSuit.Diamond, CardNumber.Ace), (CardSuit.Diamond, CardNumber.King));
            AddMany(Add, PlayingCardName.CatBalou, (CardSuit.Diamond, CardNumber.Nine),
                (CardSuit.Diamond, CardNumber.Ten), (CardSuit.Diamond, CardNumber.Jack), (CardSuit.Heart, CardNumber.King));
            AddMany(Add, PlayingCardName.StageCoach, (CardSuit.Spade, CardNumber.Nine), (CardSuit.Spade, CardNumber.Nine));
            Add(PlayingCardName.Gatling, CardSuit.Heart, CardNumber.Ten);
            Add(PlayingCardName.WellsFargo, CardSuit.Heart, CardNumber.Three);
            AddMany(Add, PlayingCardName.Duel, (CardSuit.Spade, CardNumber.Jack),
                (CardSuit.Diamond, CardNumber.Queen), (CardSuit.Club, CardNumber.Eight));
            Add(PlayingCardName.Saloon, CardSuit.Heart, CardNumber.Five);
            for (var rank = 2; rank <= 10; rank++)
                Add(PlayingCardName.Missed, CardSuit.Spade, (CardNumber)rank);
            AddMany(Add, PlayingCardName.Missed, (CardSuit.Club, CardNumber.Jack),
                (CardSuit.Club, CardNumber.Queen), (CardSuit.Club, CardNumber.King), (CardSuit.Club, CardNumber.Ace));
            for (var rank = 2; rank <= 13; rank++)
                Add(PlayingCardName.Bang, CardSuit.Diamond, (CardNumber)rank);
            for (var rank = 2; rank <= 9; rank++)
                Add(PlayingCardName.Bang, CardSuit.Club, (CardNumber)rank);
            AddMany(Add, PlayingCardName.Bang, (CardSuit.Heart, CardNumber.Queen),
                (CardSuit.Heart, CardNumber.King), (CardSuit.Heart, CardNumber.Ace), (CardSuit.Spade, CardNumber.Ace));

            if (cards.Count != BaseDeckSize)
                throw new InvalidOperationException($"Base deck manifest contains {cards.Count}, expected {BaseDeckSize}.");
            return cards;
        }

        private static void AddMany(Action<PlayingCardName, CardSuit, CardNumber, CardColor, int> add,
            PlayingCardName name, params (CardSuit suit, CardNumber rank)[] cards)
        {
            foreach (var card in cards)
                add(name, card.suit, card.rank, CardColor.Brown, 0);
        }
    }
}
