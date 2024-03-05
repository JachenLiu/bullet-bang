using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    //defines existing cards into game
    public class CardDatabase
    {
        private int playerCount;
        private RolePile roles;
        private PlayerPile players;
        private CharacterPile defaultCharacters;
        private CharacterPile dodgeCityCharacters;
        private CharacterPile specialCharacters;

        private DrawPile defaultPlayingCards;
        private DrawPile dodgeCityPlayingCards;

        //todo
        //private Pile<PlayableCard> FistfulOfCards;
        //private Pile<PlayableCard> HighNoon;
        public void SetPlayerCount(int numPlayers)
        {
            playerCount = numPlayers;
            Debug.LogWarning("playercount set to " + playerCount);
        }
        public int GetPlayerCount()
        {
            return playerCount;
        }


        //initializes database - uses number of players
        public CardDatabase(int numPlayers)
        {
            SetPlayerCount(numPlayers);
            GenerateDatabase();
            Debug.LogWarning("Database Generated");
        }
        public void GenerateDatabase()
        {
            //SetPlayerCount(playerCount);
            InitializeRoles();
            InitializeDefaultCharacters();
            InitializePlayers(GetAvailableRoles());
            //InitializeDodgeCityCharacters();
            //InitializeSpecialCharacters();
            InitializeDefaultGameCards();
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
                Debug.LogWarning("null initializeroles");
            }
            Debug.LogWarning("roles are initialized");
            roles.Shuffle();
            Debug.LogWarning("roles are shuffled");
        }
        public RolePile GetAvailableRoles()
        {
            return roles;
        }

        private void InitializePlayers(RolePile roles)
        {
            if(roles == null)
            {
                Debug.LogWarning("Null");

            }
            players = new PlayerPile();
            for(int i = 0; i < GetPlayerCount(); i++)
            {
                PlayerCard player = new PlayerCard(roles.GetCard(i));
                players.Add(player);
            }
            Debug.LogWarning("players initialized");
        }
        public PlayerPile GetPlayers()
        {
            return players;
        }

        private void InitializeDefaultCharacters()
        {
            //Default Characters
            defaultCharacters = new CharacterPile();

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

            defaultCharacters.Add(CASSIDY);
            defaultCharacters.Add(JACK);
            defaultCharacters.Add(JANET);
            defaultCharacters.Add(GRINGO);
            defaultCharacters.Add(JONES);
            defaultCharacters.Add(JOURDONNAIS);
            defaultCharacters.Add(CARLSON);
            defaultCharacters.Add(DUKE);
            defaultCharacters.Add(REGRET);
            defaultCharacters.Add(RAMIREZ);
            defaultCharacters.Add(DOOLAN);
            defaultCharacters.Add(KETCHUM);
            defaultCharacters.Add(KILLER);
            defaultCharacters.Add(LAFAYETTE);
            defaultCharacters.Add(SAM);
            defaultCharacters.Add(WILLY);
        }

        public CharacterPile GetDefaultCharacterPile()
        {
            return defaultCharacters;
        }

        private void InitializeDodgeCityCharacters()
        {
            //Dodge City Characters
            dodgeCityCharacters = new CharacterPile();

            Hunter HUNTER = new Hunter();
            Fuente FUENTE = new Fuente();
            Stark STARK = new Stark();
            Wengam WENGAM = new Wengam();
            Joe JOE = new Joe();
            Mallory MALLORY = new Mallory();
            Custer CUSTER = new Custer();
            Holiday HOLIDAY = new Holiday();
            Brennan BRENNAN = new Brennan();
            Apache APACHE = new Apache();
            Pete PETE = new Pete();
            Noface NOFACE = new Noface();
            Star STAR = new Star();
            Delgado DELGADO = new Delgado();
            Digger DIGGER = new Digger();

            dodgeCityCharacters.Add(HUNTER);
            dodgeCityCharacters.Add(FUENTE);
            dodgeCityCharacters.Add(STARK);
            dodgeCityCharacters.Add(WENGAM);
            dodgeCityCharacters.Add(JOE);
            dodgeCityCharacters.Add(MALLORY);
            dodgeCityCharacters.Add(CUSTER);
            dodgeCityCharacters.Add(HOLIDAY);
            dodgeCityCharacters.Add(BRENNAN);
            dodgeCityCharacters.Add(APACHE);
            dodgeCityCharacters.Add(PETE);
            dodgeCityCharacters.Add(NOFACE);
            dodgeCityCharacters.Add(STAR);
            dodgeCityCharacters.Add(DELGADO);
            dodgeCityCharacters.Add(DIGGER);
        }
        public CharacterPile GetDodgeCityCharacters()
        {
            return dodgeCityCharacters;
        }
        private void InitializeSpecialCharacters()
        {
            //Special Characters
            specialCharacters = new CharacterPile();

            Claus CLAUS = new Claus();
            Kisch KISCH = new Kisch();
            Will WILL = new Will();

            specialCharacters.Add(CLAUS);
            specialCharacters.Add(KISCH);
            specialCharacters.Add(WILL);
        }
        public CharacterPile GetSpecialCharacters()
        {
            return specialCharacters;
        }
        private void InitializeDefaultGameCards()
        {

            //Default Game Cards
            defaultPlayingCards = new DrawPile();

            //guns
            Volcanic Volcanic1 = new Volcanic(Number.Ten, Suit.Club);
            Volcanic Volcanic2 = new Volcanic(Number.Ten, Suit.Spade);
            Schofield Schofield1 = new Schofield(Number.Jack, Suit.Club);
            Schofield Schofield2 = new Schofield(Number.Queen, Suit.Club);
            Schofield Schofield3 = new Schofield(Number.King, Suit.Spade);
            Remington Remington1 = new Remington(Number.King, Suit.Club);
            Carabine Carabine1 = new Carabine(Number.Ace, Suit.Club);
            Winchester Winchester = new Winchester(Number.Eight, Suit.Club);

            defaultPlayingCards.Add(Volcanic1);
            defaultPlayingCards.Add(Volcanic2);
            defaultPlayingCards.Add(Schofield1);
            defaultPlayingCards.Add(Schofield2);
            defaultPlayingCards.Add(Schofield3);
            defaultPlayingCards.Add(Remington1);
            defaultPlayingCards.Add(Carabine1);
            defaultPlayingCards.Add(Winchester);

            //status cards
            Scope Scope1 = new Scope(Number.Ace, Suit.Spade);
            Dynamite Dynamite1 = new Dynamite(Number.Two, Suit.Heart);
            Mustang Mustang1 = new Mustang(Number.Eight, Suit.Heart);
            Barrel Barrel1 = new Barrel(Number.King, Suit.Spade);
            Barrel Barrel2 = new Barrel(Number.Queen, Suit.Spade);
            Jail Jail1 = new Jail(Number.Jack, Suit.Spade);
            Jail Jail2 = new Jail(Number.Ten, Suit.Spade);
            Jail Jail3 = new Jail(Number.Four, Suit.Heart);

            defaultPlayingCards.Add(Scope1);
            defaultPlayingCards.Add(Dynamite1);
            defaultPlayingCards.Add(Mustang1);
            defaultPlayingCards.Add(Barrel1);
            defaultPlayingCards.Add(Barrel2);
            defaultPlayingCards.Add(Jail1);
            defaultPlayingCards.Add(Jail2);
            defaultPlayingCards.Add(Jail3);

            //brown cards
            Panic Panic1 = new Panic(Number.Eight, Suit.Diamond);
            Panic Panic2 = new Panic(Number.Jack, Suit.Heart);
            Panic Panic3 = new Panic(Number.Queen, Suit.Heart);
            Panic Panic4 = new Panic(Number.Ace, Suit.Heart);

            defaultPlayingCards.Add(Panic1);
            defaultPlayingCards.Add(Panic2);
            defaultPlayingCards.Add(Panic3);
            defaultPlayingCards.Add(Panic4);

            Beer Beer1 = new Beer(Number.Six, Suit.Heart);
            Beer Beer2 = new Beer(Number.Jack, Suit.Heart);
            Beer Beer3 = new Beer(Number.Nine, Suit.Heart);
            Beer Beer4 = new Beer(Number.Eight, Suit.Heart);
            Beer Beer5 = new Beer(Number.Seven, Suit.Heart);
            Beer Beer6 = new Beer(Number.Ten, Suit.Heart);

            defaultPlayingCards.Add(Beer1);
            defaultPlayingCards.Add(Beer2);
            defaultPlayingCards.Add(Beer3);
            defaultPlayingCards.Add(Beer4);
            defaultPlayingCards.Add(Beer5);
            defaultPlayingCards.Add(Beer6);

            GeneralStore GeneralStore1 = new GeneralStore(Number.Nine, Suit.Club);
            GeneralStore GeneralStore2 = new GeneralStore(Number.Queen, Suit.Club);

            defaultPlayingCards.Add(GeneralStore1);
            defaultPlayingCards.Add(GeneralStore2);

            Indians Indians1 = new Indians(Number.Ace, Suit.Diamond);
            Indians Indians2 = new Indians(Number.King, Suit.Diamond);

            defaultPlayingCards.Add(Indians1);
            defaultPlayingCards.Add(Indians2);

            CatBalou CatBalou1 = new CatBalou(Number.Nine, Suit.Diamond);
            CatBalou CatBalou2 = new CatBalou(Number.Ten, Suit.Diamond);
            CatBalou CatBalou3 = new CatBalou(Number.Jack, Suit.Diamond);
            CatBalou CatBalou4 = new CatBalou(Number.King, Suit.Heart);

            defaultPlayingCards.Add(CatBalou1);
            defaultPlayingCards.Add(CatBalou2);
            defaultPlayingCards.Add(CatBalou3);
            defaultPlayingCards.Add(CatBalou4);

            Stagecoach Stagecoach1 = new Stagecoach(Number.Nine, Suit.Spade);
            Stagecoach Stagecoach2 = new Stagecoach(Number.Nine, Suit.Spade);

            defaultPlayingCards.Add(Stagecoach1);
            defaultPlayingCards.Add(Stagecoach2);


            Gatling Gatling1 = new Gatling(Number.Ten, Suit.Heart);

            defaultPlayingCards.Add(Gatling1);


            WellsFargo WellsFargo1 = new WellsFargo(Number.Three, Suit.Heart);

            defaultPlayingCards.Add(WellsFargo1);


            Duel Duel1 = new Duel(Number.Jack, Suit.Spade);
            Duel Duel2 = new Duel(Number.Queen, Suit.Diamond);
            Duel Duel3 = new Duel(Number.Eight, Suit.Club);

            defaultPlayingCards.Add(Duel1);
            defaultPlayingCards.Add(Duel2);
            defaultPlayingCards.Add(Duel3);



            Saloon Saloon1 = new Saloon(Number.Five, Suit.Heart);

            defaultPlayingCards.Add(Saloon1);


            Missed Missed1 = new Missed(Number.Two, Suit.Spade);
            Missed Missed2 = new Missed(Number.Three, Suit.Spade);
            Missed Missed3 = new Missed(Number.Four, Suit.Spade);
            Missed Missed4 = new Missed(Number.Five, Suit.Spade);
            Missed Missed5 = new Missed(Number.Six, Suit.Spade);
            Missed Missed6 = new Missed(Number.Seven, Suit.Spade);
            Missed Missed7 = new Missed(Number.Eight, Suit.Spade);
            Missed Missed8 = new Missed(Number.Nine, Suit.Spade);
            Missed Missed9 = new Missed(Number.Ten, Suit.Spade);
            Missed Missed10 = new Missed(Number.Jack, Suit.Club);
            Missed Missed11 = new Missed(Number.Queen, Suit.Club);
            Missed Missed12 = new Missed(Number.King, Suit.Club);
            Missed Missed13 = new Missed(Number.Ace, Suit.Club);

            defaultPlayingCards.Add(Missed1);
            defaultPlayingCards.Add(Missed2);
            defaultPlayingCards.Add(Missed3);
            defaultPlayingCards.Add(Missed4);
            defaultPlayingCards.Add(Missed5);
            defaultPlayingCards.Add(Missed6);
            defaultPlayingCards.Add(Missed7);
            defaultPlayingCards.Add(Missed8);
            defaultPlayingCards.Add(Missed9);
            defaultPlayingCards.Add(Missed10);
            defaultPlayingCards.Add(Missed11);
            defaultPlayingCards.Add(Missed12);
            defaultPlayingCards.Add(Missed13);

            BangCard Bang1 = new BangCard(Number.Two, Suit.Diamond);
            BangCard Bang2 = new BangCard(Number.Three, Suit.Diamond);
            BangCard Bang3 = new BangCard(Number.Four, Suit.Diamond);
            BangCard Bang4 = new BangCard(Number.Five, Suit.Diamond);
            BangCard Bang5 = new BangCard(Number.Six, Suit.Diamond);
            BangCard Bang6 = new BangCard(Number.Seven, Suit.Diamond);
            BangCard Bang7 = new BangCard(Number.Eight, Suit.Diamond);
            BangCard Bang8 = new BangCard(Number.Nine, Suit.Diamond);
            BangCard Bang9 = new BangCard(Number.Ten, Suit.Diamond);
            BangCard Bang10 = new BangCard(Number.Jack, Suit.Diamond);
            BangCard Bang11 = new BangCard(Number.Queen, Suit.Diamond);
            BangCard Bang12 = new BangCard(Number.King, Suit.Diamond);
            BangCard Bang13 = new BangCard(Number.Ace, Suit.Diamond);

            BangCard Bang14 = new BangCard(Number.Two, Suit.Club);
            BangCard Bang15 = new BangCard(Number.Three, Suit.Club);
            BangCard Bang16 = new BangCard(Number.Four, Suit.Club);
            BangCard Bang17 = new BangCard(Number.Five, Suit.Club);
            BangCard Bang18 = new BangCard(Number.Six, Suit.Club);
            BangCard Bang19 = new BangCard(Number.Seven, Suit.Club);
            BangCard Bang20 = new BangCard(Number.Eight, Suit.Club);
            BangCard Bang21 = new BangCard(Number.Nine, Suit.Club);
            BangCard Bang22 = new BangCard(Number.Queen, Suit.Heart);
            BangCard Bang23 = new BangCard(Number.King, Suit.Heart);
            BangCard Bang24 = new BangCard(Number.Ace, Suit.Heart);
            BangCard Bang25 = new BangCard(Number.Ace, Suit.Spade);


            defaultPlayingCards.Add(Bang1);
            defaultPlayingCards.Add(Bang2);
            defaultPlayingCards.Add(Bang3);
            defaultPlayingCards.Add(Bang4);
            defaultPlayingCards.Add(Bang5);
            defaultPlayingCards.Add(Bang6);
            defaultPlayingCards.Add(Bang7);
            defaultPlayingCards.Add(Bang8);
            defaultPlayingCards.Add(Bang9);
            defaultPlayingCards.Add(Bang10);
            defaultPlayingCards.Add(Bang11);
            defaultPlayingCards.Add(Bang12);
            defaultPlayingCards.Add(Bang13);
            defaultPlayingCards.Add(Bang14);
            defaultPlayingCards.Add(Bang15);
            defaultPlayingCards.Add(Bang16);
            defaultPlayingCards.Add(Bang17);
            defaultPlayingCards.Add(Bang18);
            defaultPlayingCards.Add(Bang19);
            defaultPlayingCards.Add(Bang20);
            defaultPlayingCards.Add(Bang21);
            defaultPlayingCards.Add(Bang22);
            defaultPlayingCards.Add(Bang23);
            defaultPlayingCards.Add(Bang24);
            defaultPlayingCards.Add(Bang25);


        }
        public DrawPile GetDefaultGameCards()
        {
            return defaultPlayingCards;
        }
        private void InitializeDodgeCityCards()
        {
            //Dodge City Cards
            //Guns
            Carabine Carabine2 = new Carabine(Number.Five, Suit.Spade);
            Remington Remington2 = new Remington(Number.Six, Suit.Diamond);

            dodgeCityPlayingCards.Add(Carabine2);
            dodgeCityPlayingCards.Add(Remington2);

            //status
            Mustang Mustang2 = new Mustang(Number.Five, Suit.Heart);
            Hideout Hideout1 = new Hideout(Number.King, Suit.Diamond);
            Barrel Barrel3 = new Barrel(Number.Ace, Suit.Club);
            Binocular Binocular1 = new Binocular(Number.Ten, Suit.Diamond);
            Dynamite Dynamite2 = new Dynamite(Number.Ten, Suit.Club);

            dodgeCityPlayingCards.Add(Mustang2);
            dodgeCityPlayingCards.Add(Hideout1);
            dodgeCityPlayingCards.Add(Barrel3);
            dodgeCityPlayingCards.Add(Binocular1);
            dodgeCityPlayingCards.Add(Dynamite2);

            //greens
            CanCan CanCan1 = new CanCan(Number.Jack, Suit.Club);
            Knife Knife1 = new Knife(Number.Eight, Suit.Heart);
            Sombrero Sombrero1 = new Sombrero(Number.Seven, Suit.Club);
            Bible Bible1 = new Bible(Number.Ten, Suit.Heart);
            PonyExpress PonyExpress1 = new PonyExpress(Number.Queen, Suit.Diamond);
            BuffaloRifle BuffaloRifle1 = new BuffaloRifle(Number.Queen, Suit.Club);
            Canteen Canteen1 = new Canteen(Number.Seven, Suit.Heart);
            Pepperbox Pepperbox1 = new Pepperbox(Number.Ace, Suit.Heart);
            Howitzer Howitzer1 = new Howitzer(Number.Nine, Suit.Spade);
            Conestoga Constoga1 = new Conestoga(Number.Nine, Suit.Diamond);
            Derringer Derringer1 = new Derringer(Number.Seven, Suit.Spade);
            TenGallonHat TenGallonHat1 = new TenGallonHat(Number.Jack, Suit.Diamond);
            IronPlate Ironplate1 = new IronPlate(Number.Queen, Suit.Spade);
            IronPlate Ironplate2 = new IronPlate(Number.Ace, Suit.Diamond);

            dodgeCityPlayingCards.Add(CanCan1);
            dodgeCityPlayingCards.Add(Knife1);
            dodgeCityPlayingCards.Add(Sombrero1);
            dodgeCityPlayingCards.Add(Bible1);
            dodgeCityPlayingCards.Add(PonyExpress1);
            dodgeCityPlayingCards.Add(BuffaloRifle1);
            dodgeCityPlayingCards.Add(Canteen1);
            dodgeCityPlayingCards.Add(Pepperbox1);
            dodgeCityPlayingCards.Add(Howitzer1);
            dodgeCityPlayingCards.Add(Constoga1);
            dodgeCityPlayingCards.Add(Derringer1);
            dodgeCityPlayingCards.Add(TenGallonHat1);
            dodgeCityPlayingCards.Add(Ironplate1);
            dodgeCityPlayingCards.Add(Ironplate2);

            //brown cards
            BangCard Bang26 = new BangCard(Number.King, Suit.Club);
            BangCard Bang27 = new BangCard(Number.Eight, Suit.Spade);
            BangCard Bang28 = new BangCard(Number.Five, Suit.Club);
            BangCard Bang29 = new BangCard(Number.Six, Suit.Club);
            Dodge Dodge1 = new Dodge(Number.Seven, Suit.Diamond);
            Dodge Dodge2 = new Dodge(Number.King, Suit.Heart);
            Beer Beer7 = new Beer(Number.Six, Suit.Spade);
            Beer Beer8 = new Beer(Number.Six, Suit.Heart);
            Indians Indians3 = new Indians(Number.Five, Suit.Diamond);
            Missed Missed14 = new Missed(Number.Eight, Suit.Diamond);
            Springfield Springfield1 = new Springfield(Number.King, Suit.Spade);
            RagTime RagTime1 = new RagTime(Number.Nine, Suit.Heart);
            Punch Punch1 = new Punch(Number.Ten, Suit.Spade);
            GeneralStore GeneralStore3 = new GeneralStore(Number.Ace, Suit.Spade);
            CatBalou CatBalou5 = new CatBalou(Number.Eight, Suit.Club);
            Brawl Brawl1 = new Brawl(Number.Jack, Suit.Spade);
            Whisky Whisky1 = new Whisky(Number.Queen, Suit.Heart);
            Tequila Tequila1 = new Tequila(Number.Nine, Suit.Club);
            Panic Panic5 = new Panic(Number.Jack, Suit.Heart);

            dodgeCityPlayingCards.Add(Bang26);
            dodgeCityPlayingCards.Add(Bang27);
            dodgeCityPlayingCards.Add(Bang28);
            dodgeCityPlayingCards.Add(Bang29);
            dodgeCityPlayingCards.Add(Dodge1);
            dodgeCityPlayingCards.Add(Dodge2);
            dodgeCityPlayingCards.Add(Beer7);
            dodgeCityPlayingCards.Add(Beer8);
            dodgeCityPlayingCards.Add(Indians3);
            dodgeCityPlayingCards.Add(Missed14);
            dodgeCityPlayingCards.Add(Springfield1);
            dodgeCityPlayingCards.Add(RagTime1);
            dodgeCityPlayingCards.Add(Punch1);
            dodgeCityPlayingCards.Add(GeneralStore3);
            dodgeCityPlayingCards.Add(CatBalou5);
            dodgeCityPlayingCards.Add(Brawl1);
            dodgeCityPlayingCards.Add(Whisky1);
            dodgeCityPlayingCards.Add(Tequila1);
            dodgeCityPlayingCards.Add(Panic5);
        }
        public DrawPile GetDodgeCityPlayingCards()
        {
            return dodgeCityPlayingCards;
        }


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
