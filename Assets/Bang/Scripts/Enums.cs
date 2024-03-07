using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BulletBang
{
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
    public enum BrownCardType
    {
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
        Bang
    }
    public enum BlueCardType
    {
        Volcanic,
        Schofield,
        Remington,
        RevCarabine,
        Winchester,
        Scope,
        Dynamite,
        Mustang,
        Barrel,
        Jail
    }
}