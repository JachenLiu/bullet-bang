using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BulletBang
{
    public enum GameState
    {
        Setup,
        StartGame,
        EndGame,
        GameOver,
    }
    public enum CardType
    {
        Role,
        Character,
        Playing
    }
    public enum RoleType
    {
        Sheriff,
        Deputy,
        Renegade,
        Outlaw
    }
    public enum CharacterType
    {
        BartCassidy,
        BlackJack,
        CalamityJanet,
        ElGringo,
        JesseJones,
        Jourdonnais,
        KitCarlson,
        LuckyDuke,
        PaulRegret,
        PedroRamirez,
        RoseDoolan,
        SidKetchum,
        SlabTheKiller,
        SuzyLafayette,
        VultureSam,
        WillyTheKid,
        
        //special
        ClausTheSaint,
        JohnnyKisch,
        UncleWill,

        //dodge city
        HerbHunter,
        ElenaFuente,
        MollyStark,
        ChuckWengam,
        TequilaJoe,
        SeanMallory,
        VeraCuster,
        DocHoliday,
        PatBrennan,
        ApacheKid,
        PixiePete,
        BillNoface,
        BelleStar,
        JoseDelgado,
        GregDigger

        
    }
    public enum PlayingCardType
    {
        Brown,
        Blue,
        Green
    }
    public enum CardSuit
    {
        Spade, Heart, Club, Diamond
    };
    public enum CardNumber
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    };
    public enum PlayingCardName
    {
        //blue
        Volcanic,
        Schofield,
        Remington,
        RevCarabine,
        Winchester,
        Scope,
        Dynamite,
        Mustang,
        Barrel,
        Jail,
        //dodge city
        Hideout,
        Binocular,
        

        //brown
        Panic,
        Beer,
        GeneralStore,
        Indians,
        CatBalou,
        StageCoach,
        Gatling,
        WellsFargo,
        Duel,
        Saloon,
        Missed,
        Bang,
        //dodge city
        Dodge,
        Springfield,
        RagTime,
        Punch,
        Brawl,
        Whisky,
        Tequila,
        //dodge city green

        CanCan,
        Knife,
        Sombrero,
        Bible,
        PonyExpress,
        BuffaloRifle,
        Canteen,
        Pepperbox,
        Howitzer,
        Conestoga,
        Derringer,
        TenGallonHat,
        IronPlate

        ////Characters
        //DrawExtraCards, //claus the saint
        //DiscardDuplicateCards, //johnny kisch
        //PlayAsGeneralStore, //uncle will
        //DrawCardWhenHit, //bart cassidy
        //ShowSecondCardDrawn, //black jack
        //PlayBangAsMissed, //calamity janet
        //DrawCardWhenHitByPlayer, //el gringo
        //DrawFirstCardFromPlayerHand, //jesse jones
        //DrawWhenTargetOfBang, //jourdonnais
        //PeekTopCards, //kit carlson
        //FlipTopCards, //lucky duke
        //IncreaseDistanceByOne, //paul regret
        //DrawFirstCardFromDiscardPile, //pedro rameriz
        //DecreaseDistanceByOne, //rose doolan
        //DiscardToRegainHealth, //sid ketchum
        //TwoMissedToCancelBang, //slab the killer
        //DrawCardWhenOutOfCards, //suzy lafayette
        //TakeCardsFromEliminatedPlayer, //vulture sam
        //PlayAnyNumberOfBangCards, //willy the kid

        //// Blue Bordered Cards
        //ViewOthersAtDistanceMinusOne, //scope
        //Dynamite,
        //Mustang,
        //Barrel,
        //Jail,

        //// Brown Bordered Cards
        //DrawCardFromPlayerWithinDistance, //panic
        //IncreaseHealth, //beer
        //DiscardForLifePoint, 
        //DiscardForHealing,
        //DiscardForCardDraw,
        //GainAbilityOfAnotherCharacter,
        //DiscardForShootBang,
        //DrawOneCardFromPlayer,
        //ImmunityToDiamonds,
        //DrawExtraCard,
        //DrawCardBasedOnWounds,
        //NullifyOthersEffects,
        //DiscardForDraw,
        //HealWhenAnotherPlayerEliminated,

        //// Green Bordered Cards
        //DiscardCardFromPlayer,
        //BangWithinOneDistance,
        //Missed,
        //DrawCards,
        //BangAtAnyDistance,
        //DrawCardFromOnePlayer,
        //DrawCardAndBangWithinOneDistance,
        //MissedAndDraw,
    }
}