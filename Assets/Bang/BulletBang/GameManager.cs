using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;

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

        //private void Start()
        //{
        //    //player lobby
        //    //start game
        //    //turn manager
        //    //end game
        //    playerManager.TestPlayers();
        //    InitializeGame();

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



//// prefab for the player
//[SerializeField] private NetworkPrefabRef _playerPrefab;
//// store the spawned players
//private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

//public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) 
//{
//    if (runner.IsServer)
//    {
//        //Create Unique Spawn Position
//        Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
//        //Spawn the player
//        NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
//        //Add the player to the list of spawned characters
//        _spawnedCharacters.Add(player, networkPlayerObject);
//    }
//}
//public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) 
//{ 
//    if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
//    {
//        runner.Despawn(networkObject);
//        _spawnedCharacters.Remove(player);
//    }
//}
//private bool _mouseButton0;
//private bool _mouseButton1;
//private void Update()
//{
//    _mouseButton0 = _mouseButton0 | Input.GetMouseButton(0);
//    _mouseButton1 = _mouseButton1 | Input.GetMouseButton(1);
//}
//public void OnInput(NetworkRunner runner, NetworkInput input)
//{
//    var data = new NetworkInputData();

//    if(Input.GetKey(KeyCode.W))
//    {
//        data.direction += Vector3.forward;
//    }
//    else if(Input.GetKey(KeyCode.S))
//    {
//        data.direction += Vector3.back;
//    }
//    else if(Input.GetKey(KeyCode.A))
//    {
//        data.direction += Vector3.left;
//    }
//    else if(Input.GetKey(KeyCode.D))
//    {
//        data.direction += Vector3.right;
//    }
//    else
//    {
//        data.direction += Vector3.zero;
//    }
//    data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _mouseButton0);
//    _mouseButton0 = false;
//    data.buttons.Set(NetworkInputData.MOUSEBUTTON1, _mouseButton1);
//    _mouseButton1 = false;
//    input.Set(data);
//}
//public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
//public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
//public void OnConnectedToServer(NetworkRunner runner) { }
//public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
//public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
//public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
//public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
//public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
//public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
//public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
//public void OnSceneLoadDone(NetworkRunner runner) { }
//public void OnSceneLoadStart(NetworkRunner runner) { }
//public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
//public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
//public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
//public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }


//private NetworkRunner _runner;

//async void StartGame(GameMode mode)
//{
//    // Create the Fusion runner and let it know that we will be providing user input
//    _runner = gameObject.AddComponent<NetworkRunner>();
//    _runner.ProvideInput = true;

//    // Create the NetworkSceneInfo from the current scene
//    var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
//    var sceneInfo = new NetworkSceneInfo();
//    if (scene.IsValid)
//    {
//        sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
//    }

//    // Start or join (depends on gamemode) a session with a specific name
//    await _runner.StartGame(new StartGameArgs()
//    {
//        GameMode = mode,
//        SessionName = "Test Room",
//        Scene = scene,
//        SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
//    });
//}
//private void OnGUI()
//{
//    if (_runner == null)
//    {
//        if (GUI.Button(new Rect(200, 0, 200, 40), "Host"))
//        {
//            StartGame(GameMode.Host);
//        }
//        if (GUI.Button(new Rect(200, 40, 200, 40), "Join"))
//        {
//            StartGame(GameMode.Client);
//        }
//    }
//}