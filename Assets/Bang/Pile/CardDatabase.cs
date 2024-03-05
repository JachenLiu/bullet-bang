using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang.BaseCard;
using Bang.DodgeCity;
namespace Bang
{
    //defines existing cards into game
    public class CardDatabase
    {
        private int playerCount;
        private RolePile roles;


        public void SetPlayerCount(int numPlayers)
        {
            playerCount = numPlayers;
            Debug.LogWarning("yes it is set to " + playerCount);
        }
        //private Pile<UnplayableCard> DefaultCharacters;
        //private Pile<UnplayableCard> DodgeCityCharacters;
        //private Pile<UnplayableCard> SpecialCharacters;

        //private Pile<PlayableCard> DefaultPlayingCards;
        //private Pile<PlayableCard> DodgeCityCards;

        //private Pile<PlayableCard> FistfulOfCards;
        //private Pile<PlayableCard> HighNoon;

        public CardDatabase()
        {
            GenerateDatabase();
            Debug.LogWarning("Database Generated");
        }
        public void GenerateDatabase()
        {
            SetPlayerCount(playerCount);
            InitializeRoles();
            InitializeDefaultCharacters();
            //InitializeDodgeCityCharacters();
            //InitializeSpecialCharacters();
            //InitializeDefaultGameCards();
            //InitializeDodgeCityCards();
        }
        private void InitializeRoles()
        {
            roles = new RolePile();
            int numPlayers = playerCount;
            Debug.LogWarning("playercount" + numPlayers);

            Sheriff sheriff = new Sheriff();
            Deputy deputy = new Deputy();
            Renegade renegade = new Renegade();
            Outlaw outlaw = new Outlaw();
            switch (playerCount)
            {
                case 3:
                    roles.Add(deputy);
                    roles.Add(renegade);
                    roles.Add(outlaw);
                    break;
                case 8:
                    roles.Add(renegade);
                    goto case 7;
                case 7:
                    roles.Add(deputy);
                    goto case 6;
                case 6:
                    roles.Add(outlaw);
                    goto case 5;
                case 5:
                    roles.Add(deputy);
                    goto case 4;
                case 4:
                    roles.Add(sheriff);
                    roles.Add(renegade);
                    roles.Add(outlaw);
                    roles.Add(outlaw); // Assuming you want two outlaws in a 4-player game
                    break;
            }
            if (roles == null)
            {
                Debug.LogWarning("null getavailableroles");
            }
            Debug.LogWarning("roles are initialized");
            roles.Print();
        }
        public RolePile GetAvailableRoles()
        {
            Debug.LogWarning("HEY");
            return roles;
        }


        private void InitializeDefaultCharacters()
        {
            //Default Characters
            GetDefaultCharacterPile();
        }

        public CharacterPile GetDefaultCharacterPile()
        {
            CharacterPile DefaultCharacters = new CharacterPile();

            Cassidy CASSIDY = new Cassidy();
            Jack JACK = new Jack();
            Janet JANET = new Janet();
            Gringo GRINGO = new Gringo();
            Jones JONES = new Jones();
            Jourdonnais JOURDONNAIS = new Jourdonnais();
            Carlson CARLSON = new Carlson();
            Duke DUKE = new Duke();
            Regret REGRET = new Regret();
            Ramirez RAMIREZ = new Ramirez();
            Doolan DOOLAN = new Doolan();
            Ketchum KETCHUM = new Ketchum();
            Killer KILLER = new Killer();
            Lafayette LAFAYETTE = new Lafayette();
            Sam SAM = new Sam();
            Willy WILLY = new Willy();

            DefaultCharacters.Add(CASSIDY);
            DefaultCharacters.Add(JACK);
            DefaultCharacters.Add(JANET);
            DefaultCharacters.Add(GRINGO);
            DefaultCharacters.Add(JONES);
            DefaultCharacters.Add(JOURDONNAIS);
            DefaultCharacters.Add(CARLSON);
            DefaultCharacters.Add(DUKE);
            DefaultCharacters.Add(REGRET);
            DefaultCharacters.Add(RAMIREZ);
            DefaultCharacters.Add(DOOLAN);
            DefaultCharacters.Add(KETCHUM);
            DefaultCharacters.Add(KILLER);
            DefaultCharacters.Add(LAFAYETTE);
            DefaultCharacters.Add(SAM);
            DefaultCharacters.Add(WILLY);

            return DefaultCharacters;
        }

        //private void InitializeDodgeCityCharacters()
        //{
        //    //Dodge City Characters
        //    DodgeCityCharacters = new Pile<UnplayableCard>();

        //    Hunter HUNTER = new Hunter();
        //    Fuente FUENTE = new Fuente();
        //    Stark STARK = new Stark();
        //    Wengam WENGAM = new Wengam();
        //    Joe JOE = new Joe();
        //    Mallory MALLORY = new Mallory();
        //    Custer CUSTER = new Custer();
        //    Holiday HOLIDAY = new Holiday();
        //    Brennan BRENNAN = new Brennan();
        //    Apache APACHE = new Apache();
        //    Pete PETE = new Pete();
        //    Noface NOFACE = new Noface();
        //    Star STAR = new Star();
        //    Delgado DELGADO = new Delgado();
        //    Digger DIGGER = new Digger();

        //    DodgeCityCharacters.Add(HUNTER);
        //    DodgeCityCharacters.Add(FUENTE);
        //    DodgeCityCharacters.Add(STARK);
        //    DodgeCityCharacters.Add(WENGAM);
        //    DodgeCityCharacters.Add(JOE);
        //    DodgeCityCharacters.Add(MALLORY);
        //    DodgeCityCharacters.Add(CUSTER);
        //    DodgeCityCharacters.Add(HOLIDAY);
        //    DodgeCityCharacters.Add(BRENNAN);
        //    DodgeCityCharacters.Add(APACHE);
        //    DodgeCityCharacters.Add(PETE);
        //    DodgeCityCharacters.Add(NOFACE);
        //    DodgeCityCharacters.Add(STAR);
        //    DodgeCityCharacters.Add(DELGADO);
        //    DodgeCityCharacters.Add(DIGGER);
        //}
        //private void InitializeSpecialCharacters()
        //{
        //    //Special Characters
        //    SpecialCharacters = new Pile<UnplayableCard>();

        //    Claus CLAUS = new Claus();
        //    Kisch KISCH = new Kisch();
        //    Will WILL = new Will();

        //    SpecialCharacters.Add(CLAUS);
        //    SpecialCharacters.Add(KISCH);
        //    SpecialCharacters.Add(WILL);
        //}
        //private void InitializeDefaultGameCards()
        //{

        //    //Default Game Cards
        //    DefaultPlayingCards = new Pile<PlayableCard>();

        //    //guns
        //    Volcanic Volcanic1 = new Volcanic(Number.Ten, Suit.Club);
        //    Volcanic Volcanic2 = new Volcanic(Number.Ten, Suit.Spade);
        //    Schofield Schofield1 = new Schofield(Number.Jack, Suit.Club);
        //    Schofield Schofield2 = new Schofield(Number.Queen, Suit.Club);
        //    Schofield Schofield3 = new Schofield(Number.King, Suit.Spade);
        //    BaseCard.Remington Remington1 = new BaseCard.Remington(Number.King, Suit.Club);
        //    BaseCard.Carabine Carabine1 = new BaseCard.Carabine(Number.Ace, Suit.Club);
        //    Winchester Winchester = new Winchester(Number.Eight, Suit.Club);

        //    DefaultPlayingCards.Add(Volcanic1);
        //    DefaultPlayingCards.Add(Volcanic2);
        //    DefaultPlayingCards.Add(Schofield1);
        //    DefaultPlayingCards.Add(Schofield2);
        //    DefaultPlayingCards.Add(Schofield3);
        //    DefaultPlayingCards.Add(Remington1);
        //    DefaultPlayingCards.Add(Carabine1);
        //    DefaultPlayingCards.Add(Winchester);

        //    //status cards
        //    Scope Scope1 = new Scope(Number.Ace, Suit.Spade);
        //    BaseCard.Dynamite Dynamite1 = new BaseCard.Dynamite(Number.Two, Suit.Heart);
        //    BaseCard.Mustang Mustang1 = new BaseCard.Mustang(Number.Eight, Suit.Heart);
        //    BaseCard.Barrel Barrel1 = new BaseCard.Barrel(Number.King, Suit.Spade);
        //    BaseCard.Barrel Barrel2 = new BaseCard.Barrel(Number.Queen, Suit.Spade);
        //    Jail Jail1 = new Jail(Number.Jack, Suit.Spade);
        //    Jail Jail2 = new Jail(Number.Ten, Suit.Spade);
        //    Jail Jail3 = new Jail(Number.Four, Suit.Heart);

        //    DefaultPlayingCards.Add(Scope1);
        //    DefaultPlayingCards.Add(Dynamite1);
        //    DefaultPlayingCards.Add(Mustang1);
        //    DefaultPlayingCards.Add(Barrel1);
        //    DefaultPlayingCards.Add(Barrel2);
        //    DefaultPlayingCards.Add(Jail1);
        //    DefaultPlayingCards.Add(Jail2);
        //    DefaultPlayingCards.Add(Jail3);

        //    //brown cards
        //    BaseCard.Panic Panic1 = new BaseCard.Panic(Number.Eight, Suit.Diamond);
        //    BaseCard.Panic Panic2 = new BaseCard.Panic(Number.Jack, Suit.Heart);
        //    BaseCard.Panic Panic3 = new BaseCard.Panic(Number.Queen, Suit.Heart);
        //    BaseCard.Panic Panic4 = new BaseCard.Panic(Number.Ace, Suit.Heart);

        //    DefaultPlayingCards.Add(Panic1);
        //    DefaultPlayingCards.Add(Panic2);
        //    DefaultPlayingCards.Add(Panic3);
        //    DefaultPlayingCards.Add(Panic4);

        //    BaseCard.Beer Beer1 = new BaseCard.Beer(Number.Six, Suit.Heart);
        //    BaseCard.Beer Beer2 = new BaseCard.Beer(Number.Jack, Suit.Heart);
        //    BaseCard.Beer Beer3 = new BaseCard.Beer(Number.Nine, Suit.Heart);
        //    BaseCard.Beer Beer4 = new BaseCard.Beer(Number.Eight, Suit.Heart);
        //    BaseCard.Beer Beer5 = new BaseCard.Beer(Number.Seven, Suit.Heart);
        //    BaseCard.Beer Beer6 = new BaseCard.Beer(Number.Ten, Suit.Heart);

        //    DefaultPlayingCards.Add(Beer1);
        //    DefaultPlayingCards.Add(Beer2);
        //    DefaultPlayingCards.Add(Beer3);
        //    DefaultPlayingCards.Add(Beer4);
        //    DefaultPlayingCards.Add(Beer5);
        //    DefaultPlayingCards.Add(Beer6);

        //    BaseCard.GeneralStore GeneralStore1 = new BaseCard.GeneralStore(Number.Nine, Suit.Club);
        //    BaseCard.GeneralStore GeneralStore2 = new BaseCard.GeneralStore(Number.Queen, Suit.Club);

        //    DefaultPlayingCards.Add(GeneralStore1);
        //    DefaultPlayingCards.Add(GeneralStore2);

        //    BaseCard.Indians Indians1 = new BaseCard.Indians(Number.Ace, Suit.Diamond);
        //    BaseCard.Indians Indians2 = new BaseCard.Indians(Number.King, Suit.Diamond);

        //    DefaultPlayingCards.Add(Indians1);
        //    DefaultPlayingCards.Add(Indians2);

        //    BaseCard.CatBalou CatBalou1 = new BaseCard.CatBalou(Number.Nine, Suit.Diamond);
        //    BaseCard.CatBalou CatBalou2 = new BaseCard.CatBalou(Number.Ten, Suit.Diamond);
        //    BaseCard.CatBalou CatBalou3 = new BaseCard.CatBalou(Number.Jack, Suit.Diamond);
        //    BaseCard.CatBalou CatBalou4 = new BaseCard.CatBalou(Number.King, Suit.Heart);

        //    DefaultPlayingCards.Add(CatBalou1);
        //    DefaultPlayingCards.Add(CatBalou2);
        //    DefaultPlayingCards.Add(CatBalou3);
        //    DefaultPlayingCards.Add(CatBalou4);

        //    Stagecoach Stagecoach1 = new Stagecoach(Number.Nine, Suit.Spade);
        //    Stagecoach Stagecoach2 = new Stagecoach(Number.Nine, Suit.Spade);

        //    DefaultPlayingCards.Add(Stagecoach1);
        //    DefaultPlayingCards.Add(Stagecoach2);


        //    Gatling Gatling1 = new Gatling(Number.Ten, Suit.Heart);

        //    DefaultPlayingCards.Add(Gatling1);


        //    WellsFargo WellsFargo1 = new WellsFargo(Number.Three, Suit.Heart);

        //    DefaultPlayingCards.Add(WellsFargo1);


        //    Duel Duel1 = new Duel(Number.Jack, Suit.Spade);
        //    Duel Duel2 = new Duel(Number.Queen, Suit.Diamond);
        //    Duel Duel3 = new Duel(Number.Eight, Suit.Club);

        //    DefaultPlayingCards.Add(Duel1);
        //    DefaultPlayingCards.Add(Duel2);
        //    DefaultPlayingCards.Add(Duel3);



        //    Saloon Saloon1 = new Saloon(Number.Five, Suit.Heart);

        //    DefaultPlayingCards.Add(Saloon1);


        //    BaseCard.Missed Missed1 = new BaseCard.Missed(Number.Two, Suit.Spade);
        //    BaseCard.Missed Missed2 = new BaseCard.Missed(Number.Three, Suit.Spade);
        //    BaseCard.Missed Missed3 = new BaseCard.Missed(Number.Four, Suit.Spade);
        //    BaseCard.Missed Missed4 = new BaseCard.Missed(Number.Five, Suit.Spade);
        //    BaseCard.Missed Missed5 = new BaseCard.Missed(Number.Six, Suit.Spade);
        //    BaseCard.Missed Missed6 = new BaseCard.Missed(Number.Seven, Suit.Spade);
        //    BaseCard.Missed Missed7 = new BaseCard.Missed(Number.Eight, Suit.Spade);
        //    BaseCard.Missed Missed8 = new BaseCard.Missed(Number.Nine, Suit.Spade);
        //    BaseCard.Missed Missed9 = new BaseCard.Missed(Number.Ten, Suit.Spade);
        //    BaseCard.Missed Missed10 = new BaseCard.Missed(Number.Jack, Suit.Club);
        //    BaseCard.Missed Missed11 = new BaseCard.Missed(Number.Queen, Suit.Club);
        //    BaseCard.Missed Missed12 = new BaseCard.Missed(Number.King, Suit.Club);
        //    BaseCard.Missed Missed13 = new BaseCard.Missed(Number.Ace, Suit.Club);

        //    DefaultPlayingCards.Add(Missed1);
        //    DefaultPlayingCards.Add(Missed2);
        //    DefaultPlayingCards.Add(Missed3);
        //    DefaultPlayingCards.Add(Missed4);
        //    DefaultPlayingCards.Add(Missed5);
        //    DefaultPlayingCards.Add(Missed6);
        //    DefaultPlayingCards.Add(Missed7);
        //    DefaultPlayingCards.Add(Missed8);
        //    DefaultPlayingCards.Add(Missed9);
        //    DefaultPlayingCards.Add(Missed10);
        //    DefaultPlayingCards.Add(Missed11);
        //    DefaultPlayingCards.Add(Missed12);
        //    DefaultPlayingCards.Add(Missed13);

        //    BaseCard.BangCard Bang1 = new BaseCard.BangCard(Number.Two, Suit.Diamond);
        //    BaseCard.BangCard Bang2 = new BaseCard.BangCard(Number.Three, Suit.Diamond);
        //    BaseCard.BangCard Bang3 = new BaseCard.BangCard(Number.Four, Suit.Diamond);
        //    BaseCard.BangCard Bang4 = new BaseCard.BangCard(Number.Five, Suit.Diamond);
        //    BaseCard.BangCard Bang5 = new BaseCard.BangCard(Number.Six, Suit.Diamond);
        //    BaseCard.BangCard Bang6 = new BaseCard.BangCard(Number.Seven, Suit.Diamond);
        //    BaseCard.BangCard Bang7 = new BaseCard.BangCard(Number.Eight, Suit.Diamond);
        //    BaseCard.BangCard Bang8 = new BaseCard.BangCard(Number.Nine, Suit.Diamond);
        //    BaseCard.BangCard Bang9 = new BaseCard.BangCard(Number.Ten, Suit.Diamond);
        //    BaseCard.BangCard Bang10 = new BaseCard.BangCard(Number.Jack, Suit.Diamond);
        //    BaseCard.BangCard Bang11 = new BaseCard.BangCard(Number.Queen, Suit.Diamond);
        //    BaseCard.BangCard Bang12 = new BaseCard.BangCard(Number.King, Suit.Diamond);
        //    BaseCard.BangCard Bang13 = new BaseCard.BangCard(Number.Ace, Suit.Diamond);

        //    BaseCard.BangCard Bang14 = new BaseCard.BangCard(Number.Two, Suit.Club);
        //    BaseCard.BangCard Bang15 = new BaseCard.BangCard(Number.Three, Suit.Club);
        //    BaseCard.BangCard Bang16 = new BaseCard.BangCard(Number.Four, Suit.Club);
        //    BaseCard.BangCard Bang17 = new BaseCard.BangCard(Number.Five, Suit.Club);
        //    BaseCard.BangCard Bang18 = new BaseCard.BangCard(Number.Six, Suit.Club);
        //    BaseCard.BangCard Bang19 = new BaseCard.BangCard(Number.Seven, Suit.Club);
        //    BaseCard.BangCard Bang20 = new BaseCard.BangCard(Number.Eight, Suit.Club);
        //    BaseCard.BangCard Bang21 = new BaseCard.BangCard(Number.Nine, Suit.Club);
        //    BaseCard.BangCard Bang22 = new BaseCard.BangCard(Number.Queen, Suit.Heart);
        //    BaseCard.BangCard Bang23 = new BaseCard.BangCard(Number.King, Suit.Heart);
        //    BaseCard.BangCard Bang24 = new BaseCard.BangCard(Number.Ace, Suit.Heart);
        //    BaseCard.BangCard Bang25 = new BaseCard.BangCard(Number.Ace, Suit.Spade);


        //    DefaultPlayingCards.Add(Bang1);
        //    DefaultPlayingCards.Add(Bang2);
        //    DefaultPlayingCards.Add(Bang3);
        //    DefaultPlayingCards.Add(Bang4);
        //    DefaultPlayingCards.Add(Bang5);
        //    DefaultPlayingCards.Add(Bang6);
        //    DefaultPlayingCards.Add(Bang7);
        //    DefaultPlayingCards.Add(Bang8);
        //    DefaultPlayingCards.Add(Bang9);
        //    DefaultPlayingCards.Add(Bang10);
        //    DefaultPlayingCards.Add(Bang11);
        //    DefaultPlayingCards.Add(Bang12);
        //    DefaultPlayingCards.Add(Bang13);
        //    DefaultPlayingCards.Add(Bang14);
        //    DefaultPlayingCards.Add(Bang15);
        //    DefaultPlayingCards.Add(Bang16);
        //    DefaultPlayingCards.Add(Bang17);
        //    DefaultPlayingCards.Add(Bang18);
        //    DefaultPlayingCards.Add(Bang19);
        //    DefaultPlayingCards.Add(Bang20);
        //    DefaultPlayingCards.Add(Bang21);
        //    DefaultPlayingCards.Add(Bang22);
        //    DefaultPlayingCards.Add(Bang23);
        //    DefaultPlayingCards.Add(Bang24);
        //    DefaultPlayingCards.Add(Bang25);


        //}
        //private void InitializeDodgeCityCards()
        //{
        //    //Dodge City Cards
        //    //Guns
        //    DodgeCity.Carabine Carabine2 = new DodgeCity.Carabine(Number.Five, Suit.Spade);
        //    DodgeCity.Remington Remington2 = new DodgeCity.Remington(Number.Six, Suit.Diamond);

        //    //status
        //    DodgeCity.Mustang Mustang2 = new DodgeCity.Mustang(Number.Five, Suit.Heart);
        //    Hideout Hideout1 = new Hideout(Number.King, Suit.Diamond);
        //    DodgeCity.Barrel Barrel3 = new DodgeCity.Barrel(Number.Ace, Suit.Club);
        //    Binocular Binocular1 = new Binocular(Number.Ten, Suit.Diamond);
        //    DodgeCity.Dynamite Dynamite2 = new DodgeCity.Dynamite(Number.Ten, Suit.Club);

        //    //greens
        //    CanCan CanCan1 = new CanCan(Number.Jack, Suit.Club);
        //    Knife Knife1 = new Knife(Number.Eight, Suit.Heart);
        //    Sombrero Sombrero1 = new Sombrero(Number.Seven, Suit.Club);
        //    Bible Bible1 = new Bible(Number.Ten, Suit.Heart);
        //    PonyExpress PonyExpress1 = new PonyExpress(Number.Queen, Suit.Diamond);
        //    BuffaloRifle BuffaloRifle1 = new BuffaloRifle(Number.Queen, Suit.Club);
        //    Canteen Canteen1 = new Canteen(Number.Seven, Suit.Heart);
        //    Pepperbox Pepperbox1 = new Pepperbox(Number.Ace, Suit.Heart);
        //    Howitzer Howitzer1 = new Howitzer(Number.Nine, Suit.Spade);
        //    Conestoga Constoga1 = new Conestoga(Number.Nine, Suit.Diamond);
        //    Derringer Derringer1 = new Derringer(Number.Seven, Suit.Spade);
        //    TenGallonHat TenGallonHat1 = new TenGallonHat(Number.Jack, Suit.Diamond);
        //    IronPlate Ironplate1 = new IronPlate(Number.Queen, Suit.Spade);
        //    IronPlate Ironplate2 = new IronPlate(Number.Ace, Suit.Diamond);

        //    //brown cards
        //    DodgeCity.BangCard Bang26 = new DodgeCity.BangCard(Number.King, Suit.Club);
        //    DodgeCity.BangCard Bang27 = new DodgeCity.BangCard(Number.Eight, Suit.Spade);
        //    DodgeCity.BangCard Bang28 = new DodgeCity.BangCard(Number.Five, Suit.Club);
        //    DodgeCity.BangCard Bang29 = new DodgeCity.BangCard(Number.Six, Suit.Club);
        //    Dodge Dodge1 = new Dodge(Number.Seven, Suit.Diamond);
        //    Dodge Dodge2 = new Dodge(Number.King, Suit.Heart);
        //    DodgeCity.Beer Beer7 = new DodgeCity.Beer(Number.Six, Suit.Spade);
        //    DodgeCity.Beer Beer8 = new DodgeCity.Beer(Number.Six, Suit.Heart);
        //    DodgeCity.Indians Indians3 = new DodgeCity.Indians(Number.Five, Suit.Diamond);
        //    DodgeCity.Missed Missed14 = new DodgeCity.Missed(Number.Eight, Suit.Diamond);
        //    Springfield Springfield1 = new Springfield(Number.King, Suit.Spade);
        //    RagTime RagTime1 = new RagTime(Number.Nine, Suit.Heart);
        //    Punch Punch1 = new Punch(Number.Ten, Suit.Spade);
        //    DodgeCity.GeneralStore GeneralStore3 = new DodgeCity.GeneralStore(Number.Ace, Suit.Spade);
        //    DodgeCity.CatBalou CatBalou5 = new DodgeCity.CatBalou(Number.Eight, Suit.Club);
        //    Brawl Brawl1 = new Brawl(Number.Jack, Suit.Spade);
        //    Whisky Whisky1 = new Whisky(Number.Queen, Suit.Heart);
        //    Tequila Tequila1 = new Tequila(Number.Nine, Suit.Club);
        //    DodgeCity.Panic Panic5 = new DodgeCity.Panic(Number.Jack, Suit.Heart);
        //}
        //public List<UnplayableCard> GetRoles(int numPlayers)
        //{

        //    switch (numPlayers)
        //    {
        //        case 3:
        //            Roles.Add(DEPUTY);
        //            Roles.Add(RENEGADE);
        //            Roles.Add(OUTLAW);
        //            break;

        //        case 8:
        //            Roles.Add(RENEGADE);
        //            goto case 7;

        //        case 7:
        //            Roles.Add(DEPUTY);
        //            goto case 6;

        //        case 6:
        //            Roles.Add(OUTLAW);
        //            goto case 5;

        //        case 5:
        //            Roles.Add(DEPUTY);
        //            goto case 4;

        //        case 4:
        //            Roles.Add(SHERIFF);
        //            Roles.Add(RENEGADE);
        //            Roles.Add(OUTLAW);
        //            Roles.Add(OUTLAW);
        //            break;

        //    }
        //    Debug.Log("hi");
        //    return Roles;
        //}

        //public Pile<UnplayableCard> GetDefaultCharacters()
        //{
        //    return DefaultCharacters;
        //}
        //public Pile<UnplayableCard> GetDodgeCityCharacters()
        //{
        //    return DodgeCityCharacters;
        //}
        //public Pile<UnplayableCard> GetSpecialCharacters()
        //{
        //    return SpecialCharacters;
        //}
        //public Pile<UnplayableCard> GetDefaultAndDodge()
        //{
        //    Pile<UnplayableCard> DefaultAndDodge = new Pile<UnplayableCard>();

        //    DefaultAndDodge.AddRange(DefaultCharacters);
        //    DefaultAndDodge.AddRange(DodgeCityCharacters);

        //    return DefaultAndDodge;
        //}
        //public Pile<UnplayableCard> GetDefaultAndSpecial()
        //{
        //    Pile<UnplayableCard> DefaultAndSpecial = new Pile<UnplayableCard>();

        //    DefaultAndSpecial.AddRange(DefaultCharacters);
        //    DefaultAndSpecial.AddRange(SpecialCharacters);

        //    return DefaultAndSpecial;
        //}
        //public Pile<UnplayableCard> GetDefaultAndDodgeAndSpecial()
        //{
        //    Pile<UnplayableCard> DefaultAndDodgeAndSpecial = new Pile<UnplayableCard>();

        //    DefaultAndDodgeAndSpecial.AddRange(DefaultCharacters);
        //    DefaultAndDodgeAndSpecial.AddRange(SpecialCharacters);
        //    DefaultAndDodgeAndSpecial.AddRange(DodgeCityCharacters);

        //    return DefaultAndDodgeAndSpecial;
        //}
        //public Pile<PlayableCard> GetDefaultCards()
        //{
        //    return DefaultPlayingCards;
        //}
        //public Pile<PlayableCard> GetDodgeCityCards()
        //{
        //    return DodgeCityCards;
        //}

    }
}
/*
 * 

 * 
 * 
 * 
 * 
 *         public CharacterPile SetBaseCharacters()
        {
        //    BaseCharacter CASSIDY = new BaseCharacter("Bart Cassidy",
        //"Each time he is hit, he draws a card.", 4);

        //    BaseCharacter JACK = new BaseCharacter("Black Jack",
        //            "He shows the second card he draws. On Heart or Diamonds, he draws one more card.", 4);

        //    BaseCharacter JANET = new BaseCharacter("Calamity Janet",
        //            "She can play BANG! cards as Missed! cards and vice versa.", 4);

        //    BaseCharacter GRINGO = new BaseCharacter("El Gringo",
        //            "Each time he is hit by a player, he draws a card from the hand of that player.", 3);

        //    BaseCharacter JONES = new BaseCharacter("Jesse Jones",
        //            "He may draw his first card from the hand of a player.", 4);

        //    BaseCharacter JOURDONNAIS = new BaseCharacter("Jourdonnais",
        //            "Whenever he is the target of a BANG!, he may “draw!”: on a heart, he is missed.", 4);

        //    BaseCharacter CARLSON = new BaseCharacter("Kit Carlson",
        //            "He looks at the top 3 cards of the deck and chooses the 2 to draw.", 4);

        //    BaseCharacter DUKE = new BaseCharacter("Lucky Duke",
        //            "Each time he “draws!”, he flips the top 2 cards and chooses 1.", 4);

        //    BaseCharacter REGRET = new BaseCharacter("Paul Regret",
        //            "All players see him at a distance increased by 1.", 3);

        //    BaseCharacter RAMIREZ = new BaseCharacter("Pedro Ramirez",
        //            "He may draw his first card from the discard pile.", 4);

        //    BaseCharacter DOOLAN = new BaseCharacter("Rose Doolan",
        //            "She sees all players at a distance decreased by 1.", 4);

        //    BaseCharacter KETCHUM = new BaseCharacter("Sid Ketchum",
        //            "He may discard 2 cards to regain one life point.", 4);

        //    BaseCharacter KILLER = new BaseCharacter("Slab the Killer",
        //            "Player needs 2 Missed! cards to cancel his BANG! card.", 4);

        //    BaseCharacter LAFAYETTE = new BaseCharacter("Suzy Lafayette",
        //            "As soon as she has no cards in hand, she draws a card.", 4);

        //    BaseCharacter SAM = new BaseCharacter("Vulture Sam",
        //            "Whenever a player is eliminated from play, he takes in hand all the cards of that player.", 4);

        //    BaseCharacter WILLY = new BaseCharacter("Willy the Kid",
        //            "He can play any number of BANG! cards.", 4);

            CharacterPile characters = new CharacterPile();

            //characters.add(CASSIDY);
            //characters.add(JACK);
            //characters.add(JANET);
            //characters.add(GRINGO);
            //characters.add(JONES);
            //characters.add(JOURDONNAIS);
            //characters.add(CARLSON);
            //characters.add(DUKE);
            //characters.add(REGRET);
            //characters.add(RAMIREZ);
            //characters.add(DOOLAN);
            //characters.add(KETCHUM);
            //characters.add(KILLER);
            //characters.add(LAFAYETTE);
            //characters.add(SAM);
            //characters.add(WILLY);

            //characters.Print();

            return characters;
        }
        public CharacterPile SetDodgeCityCharacters()
        {
 

            DodgeCityCharacters.Print();

            return DodgeCityCharacters;

        }
        public void SetSpecialCharacters()
        {
                SpecialCharacter CLAUS = new SpecialCharacter("Claus \"The Saint\"",
                    "He draws one card more than the number of players, keeps 2 for himself, then gives 1 to each player.", 3);

                SpecialCharacter KISCH = new SpecialCharacter("Johnny Kisch",
                        "Each time he puts a card into play, all other cards in play with the same name are discarded.", 4);

                SpecialCharacter WILL = new SpecialCharacter("Uncle Will",
                        "Once during his turn, he may play any card from hand as a General Store.", 4);

            CharacterPile characters = new CharacterPile();

            characters.add(CLAUS);
           characters.add(KISCH);
            characters.add(WILL);

            characters.Print();


        }
            Roles = new RolePile();

            Sheriff SHERIFF = new Sheriff();
            Deputy DEPUTY = new Deputy();
            Renegade RENEGADE = new Renegade();
            Outlaw OUTLAW = new Outlaw();

            Roles.add(SHERIFF);
            Roles.add(DEPUTY);
            Roles.add(RENEGADE);
            Roles.add(OUTLAW);
        public RolePile GetAvailableRoles(int numPlayers)
        {
            Sheriff sheriff = new Sheriff();
            Deputy deputy = new Deputy();
            Deputy deputy2 = new Deputy();
            Renegade renegade = new Renegade();
            Renegade renegade2 = new Renegade();
            Outlaw outlaw = new Outlaw();
            Outlaw outlaw2 = new Outlaw();
            Outlaw outlaw3 = new Outlaw();

            RolePile roles = new RolePile();

            switch (numPlayers)
            {
                case 3:
                    roles.add(deputy);
                    roles.add(renegade);
                    roles.add(outlaw);
                    break;

                case 8:
                    roles.add(renegade2);
                    goto case 7;

                case 7:
                    roles.add(deputy2);
                    goto case 6;

                case 6:
                    roles.add(outlaw3);
                    goto case 5;

                case 5:
                    roles.add(deputy);
                    goto case 4;

                case 4:
                    roles.add(sheriff);
                    roles.add(renegade);
                    roles.add(outlaw);
                    roles.add(outlaw2);
                    break;

            }
            roles.Print();
            return roles;

        }
*/
