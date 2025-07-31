using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;

namespace BulletBang
{
    public class GameManager : NetworkBehaviour
    {
        
        public CardManager cardManager;

        private PlayerManager _playerManager;
        private TurnManager _turnManager;
        private DeckManager _deckManager;

        private void Awake()
        {
            //_playerManager = transform.Find("PlayerManager").GetComponent<PlayerManager>();
            //_turnManager = transform.Find("TurnManager").GetComponent<TurnManager>();
            //_deckManager = transform.Find("DeckManager").GetComponent<DeckManager>();
            //if (_playerManager == null || _turnManager == null || _deckManager == null)
            //{
            //    Debug.LogError("One or more manager components are missing from the GameManager prefab.");
            //}
        }
        public void Initialize(PlayerManager playerManager, TurnManager turnManager, DeckManager deckManager)
        {
            _playerManager = playerManager;
            _turnManager = turnManager;
            _deckManager = deckManager;
        }
        private DeckInstance roleDeckInstance;
        private DeckInstance characterDeckInstance;
        private DeckInstance playingDeckInstance;
        private void InitializeGame()
        {
            _deckManager.SetRolesDeck(cardManager.GetRoleCardsData(), 8);
            _deckManager.SetCharacterDeck(cardManager.GetCharacterCardsData());
            _deckManager.SetPlayingDeck(cardManager.GetPlayingCardsData());
            _deckManager.GetRoleDeck();
            _deckManager.GetCharacterDeck();
            _deckManager.GetPlayingDeck();

            
            //characterDeckInstance.CreateDeckInstance(deckManager.GetCharacterDeck());
            //playingDeckInstance.CreateDeckInstance(deckManager.GetPlayingDeck());
            //    //deckManager.GetRoleDeck();
            //    //deckInstance = Instantiate(deckInstance);
            //    //DeckInstance roleInstance = new();
            //    //roleInstance.CreateDeckInstance(deckManager.GetRoleDeck());
            //    //cardManager.TestCard();

        }

        public void StartGame()
        {
            Debug.Log("Game Started");
            InitializeGame();
        }

        public void EndGame()
        {
            Debug.Log("Game Ended");
        }

        //public DeckManager deckManager;
        //public TurnManager turnManager;
        //public int playerCount;
        //public List<Player> players;
        //public GameState gameState;

        //public GameObject playerPrefab;



        //public CardManager cardManager;
        //public DeckManager deckManager;
        //public TurnManager turnManager;
        //public PlayerManager playerManager;
        //private GameState currentState = GameState.Setup;

        //public DeckInstance deckInstance;

        //private bool gameStarted = false;
        private void Start()
        {
            //if(turnManager == null)
            //{
            //    Debug.LogError("TurnManager is not assigned in the GameManager.");
            //}
            //if(cardManager == null)
            //{
            //    Debug.LogError("CardManager is not assigned in the GameManager");
            //}
            //if(deckManager == null)
            //{
            //    Debug.LogError("DeckManager is not assigned in the GameManager.");
            }
            //InitializeGame();
            //turnManager.StartTurn();
            //player lobby
            //start game
            //turn manager
            //end game
            //playerManager.TestPlayers();
            //InitializeGame();

        }
        //private GameState currentState;
        //private void SetGameState(GameState newState)
        //{
        //    currentState = newState;
        //    OnGameStateChanged();
        //}
        //public GameState GetGameState()
        //{
        //    return currentState;
        //}

        //private void OnGameStateChanged()
        //{
        //    switch (currentState)
        //    {
        //        case GameState.Lobby:
        //            //initialize lobby
        //            break;
        //        case GameState.Setup: 
        //            //initialize game
        //            break;
        //        case GameState.StartGame:
        //            //start game
        //            break;
        //        case GameState.EndGame:
        //            //end game
        //            break;
        //        case GameState.GameOver:
        //            //clean up
        //            break;
        //    }
        //}


        //public LobbyUIManager lobbyUIManager;
        //public PlayerManager playerManager;
        //public void UpdatePlayerCount(int count)
        //{
            
        //    // Enable start game button if enough players are present
        //    if (count >= 4) // Assuming a minimum of 4 players
        //    {
        //        lobbyUIManager.EnableStartGameButton(true);
        //    }
        //    else
        //    {
        //        lobbyUIManager.EnableStartGameButton(false);
        //    }
        //}
        //private void Update()
        //{
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

        //}


        //public void ChangeGameState(GameState newState)
        //{
        //    //currentState = newState;
        //}
        //private void InitializeGame()
        //{
        //    //deckManager.SetRoleDeck(cardManager.GetRoleCards(), 8);
        //        //DealRoleCards(playerManager.GetPlayers(), deckManager.rolesDeck);
        //    //playerManager.Print();

        //    //deckManager.SetRolesDeck(cardManager.GetRoleCardsData(), 8 );
        //    //deckManager.SetCharacterDeck(cardManager.GetCharacterCardsData());
        //    //deckManager.SetPlayingDeck(cardManager.GetPlayingCardsData());

        //    //deckManager.GetRoleDeck();
        //    //deckInstance = Instantiate(deckInstance);
        //    //DeckInstance roleInstance = new();
        //    //roleInstance.CreateDeckInstance(deckManager.GetRoleDeck());
        //    //cardManager.TestCard();
        //}
        
    //}
}

