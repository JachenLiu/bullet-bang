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
    public class MainLobbyManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static MainLobbyManager Instance { get; private set; }

        [Header("Network Settings")]
        [SerializeField] private NetworkRunner networkRunnerPrefab;
        [SerializeField] private NetworkSceneManagerDefault sceneManagerPrefab;

        [Header("Player Prefabs")]
        [SerializeField] private NetworkObject lobbyPlayerPrefab;
        
        [Header("Game Tables")]
        [SerializeField] private Transform[] tableSpawnPoints;
        [SerializeField] private NetworkObject gameTablePrefab;
        
        private NetworkRunner _runner;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private List<NetworkObject> _gameTables = new List<NetworkObject>();

        public event Action<NetworkRunner> OnLobbyStarted;
        public event Action<NetworkRunner> OnLobbyJoined;
        public event Action OnLobbyLeft;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async Task<bool> StartLobbyHost()
        {
            if (_runner != null) return false;

            _runner = Instantiate(networkRunnerPrefab);
            _runner.name = "Network runner";

            var startGameArgs = new StartGameArgs()
            {
                GameMode = GameMode.Host,
                SessionName = "MainLobby",
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = Instantiate(sceneManagerPrefab),
                PlayerCount = 32 // Maximum players in the lobby
            };

            var result = await _runner.StartGame(startGameArgs);

            if (!result.Ok)
            {
                Debug.LogError($"Failed to start game: {result.ShutdownReason}");
                return false;
            }

            OnLobbyStarted?.Invoke(_runner);
            return true;
        }

        public async Task<bool> JoinLobby()
        {
            if (_runner != null) return false;

            _runner = Instantiate(networkRunnerPrefab);
            _runner.name = "Network runner";

            var startGameArgs = new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = "MainLobby",
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = Instantiate(sceneManagerPrefab)
            };

            var result = await _runner.StartGame(startGameArgs);

            if (!result.Ok)
            {
                Debug.LogError($"Failed to join lobby: {result.ShutdownReason}");
                return false;
            }

            OnLobbyJoined?.Invoke(_runner);
            return true;
        }

        public void LeaveLobby()
        {
            if (_runner != null)
            {
                _runner.Shutdown();
                _runner = null;
                OnLobbyLeft?.Invoke();
            }
        }

        private void SpawnGameTables()
        {
            if (_runner == null || !_runner.IsServer) return;

            for (int i = 0; i < tableSpawnPoints.Length; i++)
            {
                var table = _runner.Spawn(gameTablePrefab, tableSpawnPoints[i].position, tableSpawnPoints[i].rotation);
                _gameTables.Add(table);
            }
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                // Spawn the player character in the lobby
                Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-5f, 5f), 0, UnityEngine.Random.Range(-5f, 5f));
                NetworkObject networkPlayerObject = runner.Spawn(lobbyPlayerPrefab, spawnPosition, Quaternion.identity, player);
                _spawnedCharacters.Add(player, networkPlayerObject);

                // If this is the first player (host), spawn the game tables
                if (_gameTables.Count == 0)
                {
                    SpawnGameTables();
                }
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }

        // Required INetworkRunnerCallbacks implementation
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) 
        {
            Debug.LogError($"Failed to connect: {reason}");
        }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) 
        {
            // Accept all connection requests
            request.Accept();
        }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            LeaveLobby();
        }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            // Clear all spawned objects
            _spawnedCharacters.Clear();
            _gameTables.Clear();
            
            // Notify that we've left the lobby
            OnLobbyLeft?.Invoke();
        }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    }
} 