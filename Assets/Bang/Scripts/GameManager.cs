using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Bang
{
    public class GameManager : MonoBehaviour
    {
        
        private static GameManager instance;

        private int playerCountInput = 4;
        private CardManager cardManager;
        private CardDatabase cardDatabase;
        private TurnManager turnManager;

        public static GameManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                }
                return instance;
            }
        }
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

        }


        public void StartGame()
        {
            Debug.Log("Game started");
            //add cardmanager
            if(cardManager == null)
            {

                cardManager = gameObject.AddComponent<CardManager>();

            }
            else
            {
                Debug.LogWarning("Cardmanager already attached");
            }

            GameSetup();
            //to do - deal cards to every player
            /*
             * initialize game state
             * set up players
             * spawn cards
             */
            //Server = new GameServer();
            //gameState = new GameState();



            //Table setup
            //Create Game Server

            //Create Game Clients

        }

        //sets up cards, roles, player list, characters, turnmanager
        public void GameSetup()
        {
            //Generate cards based off playercount
            cardDatabase = new CardDatabase(playerCountInput);

            //Set rolepile
            RolePile roles = cardDatabase.GetAvailableRoles();
            if (roles == null)
            {
                Debug.LogError("null roles");

            }
            //roles.Print();
            //set characters
            CharacterPile characters = cardDatabase.GetDefaultCharacterPile();
            //characters.Print();

            //creates list of players to do implement multiplayers
            PlayerPile players = cardDatabase.GetPlayers();
            //players.Print();

            //create deck with playable cards
            DrawPile draw = cardDatabase.GetDefaultGameCards();
            draw.Shuffle();
            //draw.Print();
            ServerDealCharacterCardsToPlayers(players, characters);

            foreach(PlayerCard player in players)
            {
                player.Print();
            }

            turnManager = new TurnManager(players);

        }
        public void ServerDealCharacterCardsToPlayers(PlayerPile players, CharacterPile characters)
        {
            Debug.LogWarning("DEALCHARACTER");
            players.Print();
            characters.Shuffle();
            characters.Print();
            foreach(PlayerCard player in players)
            {
                int charactersToDeal = 2;
                //create a small pile for player to choose character from
                CharacterPile playerCharacterSelection = new CharacterPile();

                for(int i = 0; i < charactersToDeal; i++)
                {
                    playerCharacterSelection.Add(characters.Draw());
                }
                Debug.LogWarning("server deal characters to Player card");
                PlayerChoosesCharacter(player, playerCharacterSelection);
            }
        }
        public void PlayerChoosesCharacter(PlayerCard player, CharacterPile characterSelection) 
        {
            //to do
            /*
             * Set character card to player
             * 
             */
            player.SetPlayerCharacter(characterSelection.GetCard(0));
            ;
        }

        public void PlayerUsesCard(PlayerCard player, PlayableCard card)
        {
            //to do
            /*
             * Uses character card
             * Uses playable card - blue brown green
             * 
             */
            ;
        }

    }
}