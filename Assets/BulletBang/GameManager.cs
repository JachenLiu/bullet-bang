using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


        private void Start()
        {
            playerManager.TestPlayers();
            InitializeGame();
        }
        private void Update()
        {
        }


        public void ChangeGameState(GameState newState)
        {
            currentState = newState;
        }
        private void InitializeGame()
        {
            deckManager.charactersDeck.Print();
            deckManager.playingDeck.Print();
            deckManager.rolesDeck.Print();
                //DealRoleCards(playerManager.GetPlayers(), deckManager.rolesDeck);
            playerManager.Print();
        }
        //public void DealRoleCards(List<Player> players, Deck roles)
        //{
        //    for(int i = 0; i < players.Count; i++)
        //    {
        //        Player p = playerManager.GetPlayer(i);
                
        //    }
        //}
        //private void InstantiateCard()
        //{
        //    GameObject newCardObject = Instantiate(cardPrefab);
        //    CardInstance cardInstance = newCardObject.GetComponent<CardInstance>();
        //    if (cardInstance == null)
        //    {
        //        Debug.LogWarning("Cardholder null");
        //    }
        //    else
        //    {
        //        Card card = cardInstance.cardData;
        //        Debug.Log("Instantiated card: " + card);
        //    }
        //}

    }
}