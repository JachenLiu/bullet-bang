using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using Fusion.Sockets;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace BulletBang
{

    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        private NetworkRunner _networkRunner;
        private GameSession _gameSession;



        //private static NetworkManager _instance;
        //public static NetworkManager Instance { get { return _instance; } private set { _instance = value; } }
        public System.Action<List<string>> OnRoomListUpdated;
        //public static NetworkRunner runnerInstance;


        public string lobbyName = "default";
        public Transform sessionListContentParent;
        public GameObject sessionListEntryPrefab;
        public Dictionary<string, GameObject> sessionListUIDictionary = new Dictionary<string, GameObject>();

        public SceneAsset gameplayScene;
        private void Awake()
        {
            //if (_instance != null && _instance != this)
            //{
            //    Destroy(gameObject);
            //}
            //else
            //{
            //    _instance = this;
            //    DontDestroyOnLoad(gameObject);
            //}
            //runnerInstance = gameObject.GetComponent<NetworkRunner>();

            //if (runnerInstance == null)   
            //{
            //    runnerInstance = gameObject.AddComponent<NetworkRunner>();
            //}
        }
        private void Start()
        {
            //runnerInstance.JoinSessionLobby(SessionLobby.Shared, lobbyName);
            _networkRunner = gameObject.AddComponent<NetworkRunner>();
            _networkRunner.ProvideInput = true;
            _networkRunner.AddCallbacks(this);

            //_gameSession = gameObject.AddComponent<GameSession>();
        }
        private async Task StartGame(GameMode mode, string roomName)
        {
            var startGameArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomName,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            };
            await _networkRunner.StartGame(startGameArgs);
        }
        public async void CreateRoom(string roomName)
        {
            await StartGame(GameMode.Shared, roomName);
            Debug.Log($"Room Created {roomName}");

        }
        public async void JoinRoom(string roomName)
        {
            await StartGame(GameMode.Client, roomName);
            Debug.Log($"Room Joined {roomName}");
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {

        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var inputs = new MyNetworkInput();

            if (Input.GetKey(KeyCode.W)) {
                inputs.Buttons.Set(MyNetworkInput.BUTTON_FORWARD, true);
            }

            if (Input.GetKey(KeyCode.S)) {
                inputs.Buttons.Set(MyNetworkInput.BUTTON_BACKWARD, true);
            }

            if (Input.GetKey(KeyCode.A)) {
                inputs.Buttons.Set(MyNetworkInput.BUTTON_LEFT, true);
            }

            if (Input.GetKey(KeyCode.D)) {
                inputs.Buttons.Set(MyNetworkInput.BUTTON_RIGHT, true);
            }

            if (Input.GetKey(KeyCode.Space)) {
                inputs.Buttons.Set(MyNetworkInput.BUTTON_JUMP, true);
            }

            if (Input.GetKey(KeyCode.E)) {
                inputs.Buttons.Set(MyNetworkInput.BUTTON_ACTION1, true);
            }
            
            if (Input.GetMouseButton(0)) {
                inputs.Buttons.Set(MyNetworkInput.BUTTON_FIRE, true);
            }
 
            input.Set(inputs);
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player joined {player.PlayerId}");

            //var playerObject = runner.Spawn()
            //Player newPlayer = new Player() { }; // Example player creation
            //GameManager.Instance.playerManager.AddPlayer(newPlayer);
            //if(player == runnerInstance.LocalPlayer)
            //{
            //    SceneManager.LoadScene(gameplayScene.name);
            //}
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log($"Player {player.PlayerId} left");

            //Player leavingPlayer = GameManager.Instance.playerManager.GetPlayer(player.PlayerId);
            //GameManager.Instance.playerManager.RemovePlayer(leavingPlayer);
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            List<string> roomNames = new List<string>();
            foreach (var session in sessionList)
            {
                roomNames.Add(session.Name);
            }
            OnRoomListUpdated?.Invoke(roomNames);
            //DeleteOldSessionsFromUI(sessionList);
            //CompareLists(sessionList);
        }
        public void CreateRandomSession()
        {
            //int randomInt = UnityEngine.Random.Range(1000, 9999);

            //string randomSessionName = "Room-" + randomInt.ToString();
            //runnerInstance.StartGame(new StartGameArgs()
            //{
            //    SessionName = randomSessionName,
            //    GameMode = GameMode.Shared,
            //});
        }
        private void DeleteOldSessionsFromUI(List<SessionInfo> sessionList)
        {
            bool isContained = false;
            GameObject uiToDelete = null;

            foreach (KeyValuePair<string, GameObject> kvp in sessionListUIDictionary)
            {
                string sessionKey = kvp.Key;

                foreach (SessionInfo sessionInfo in sessionList)
                {
                    if (sessionInfo.Name == sessionKey)
                    {
                        isContained = true;
                        break;
                    }
                }
                if (!isContained)
                {
                    uiToDelete = kvp.Value;
                    sessionListUIDictionary.Remove(sessionKey);
                    Destroy(uiToDelete);
                }
            }
        }
        private void CompareLists(List<SessionInfo> sessionList)
        {
            foreach (SessionInfo session in sessionList)
            {
                if (sessionListUIDictionary.ContainsKey(session.Name))
                {
                    UpdateEntryUI(session);
                }
                else
                {
                    CreateEntryUI(session);
                }
            }
        }
        private void CreateEntryUI(SessionInfo session)
        {
            GameObject newEntry = GameObject.Instantiate(sessionListEntryPrefab);
            newEntry.transform.parent = sessionListContentParent;
            SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();
            sessionListUIDictionary.Add(session.Name, newEntry);

            entryScript.roomName.text = session.Name;
            entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
            entryScript.joinButton.interactable = session.IsOpen;

            newEntry.SetActive(session.IsVisible);
        }

        private void UpdateEntryUI(SessionInfo session)
        {
            sessionListUIDictionary.TryGetValue(session.Name, out GameObject newEntry);
            SessionListEntry entryScript = newEntry.GetComponent<SessionListEntry>();

            entryScript.roomName.text = session.Name;
            entryScript.playerCount.text = session.PlayerCount.ToString() + "/" + session.MaxPlayers.ToString();
            entryScript.joinButton.interactable = session.IsOpen;

            newEntry.SetActive(session.IsVisible);
        }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }
    }

}