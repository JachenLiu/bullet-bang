using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

namespace BulletBang
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance;} }


        public CardManager cardManager;
        public DeckManager deckManager;
        public TurnManager turnManager;
        public PlayerManager playerManager;
        private GameState currentState = GameState.Setup;

        public DeckInstance deckInstance;

        private void Start()
        {
            //player lobby
            //start game
            //turn manager
            //end game
            playerManager.TestPlayers();
            InitializeGame();

        }
        private void Update()
        {
            //switch (currentState)
            //{
            //    case GameState.Setup:
            //        break;
            //    case GameState.StartGame:
            //        break;
            //    case GameState.EndGame:
            //        break;
            //    default:
            //        break;
            //}

        }


        public void ChangeGameState(GameState newState)
        {
            currentState = newState;
        }
        private void InitializeGame()
        {
            //deckManager.SetRoleDeck(cardManager.GetRoleCards(), 8);
                //DealRoleCards(playerManager.GetPlayers(), deckManager.rolesDeck);
            playerManager.Print();
            deckManager.SetRolesDeck(cardManager.GetRoleCardsData(), 8 );
            deckManager.SetCharacterDeck(cardManager.GetCharacterCardsData());
            deckManager.SetPlayingDeck(cardManager.GetPlayingCardsData());

            deckManager.GetRoleDeck();
            deckInstance = Instantiate(deckInstance);
            //DeckInstance roleInstance = new();
            //roleInstance.CreateDeckInstance(deckManager.GetRoleDeck());
            //cardManager.TestCard();
        }
        
    }
}
