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

        private int playerCountInput = 7;
        private CardManager cardManager;
        private CardDatabase cardDatabase;

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
            //Generate cards
            cardDatabase = new CardDatabase();

            //Set playercount
            cardDatabase.SetPlayerCount(playerCountInput);
            Debug.LogWarning("playercount is set to" + playerCountInput);
          
            //Set rolepile
            RolePile roles = cardDatabase.GetAvailableRoles();
            if(roles == null)
            {
                Debug.LogWarning("null rolls");

            }
            roles.Print();
            //set characters
            //CharacterPile characters = cardDatabase.GetDefaultCharacterPile();
            

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
        public CardDatabase GetCardDatabase()
        {
            return cardDatabase;
        }
        

    }
}