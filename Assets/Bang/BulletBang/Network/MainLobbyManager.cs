using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;

namespace BulletBang
{
    /// <summary>
    /// Owns the generic shared social-lobby Fusion session: presence, roaming
    /// avatars, and table discovery. It contains no game-specific rules.
    /// </summary>
    public sealed class MainLobbyManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        /// <summary>The active lobby network lifecycle owner, if one exists.</summary>
        public static MainLobbyManager Instance { get; private set; }

        [Header("Network Settings")]
        [SerializeField] private NetworkRunner networkRunnerPrefab;

        [Header("Player Prefabs")]
        [SerializeField] private NetworkObject lobbyPlayerPrefab;
        
        [Header("Game Tables")]
        [SerializeField] private Transform[] tableSpawnPoints;
        [SerializeField] private NetworkObject gameTablePrefab;
        
        private NetworkRunner _runner;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
        private List<NetworkObject> _gameTables = new List<NetworkObject>();
        private bool _isShuttingDown;

        /// <summary>Raised after this client successfully creates the shared lobby.</summary>
        public event Action<NetworkRunner> OnLobbyStarted;

        /// <summary>Raised after this client successfully joins the shared lobby.</summary>
        public event Action<NetworkRunner> OnLobbyJoined;

        /// <summary>Raised after the local runner has shut down.</summary>
        public event Action OnLobbyLeft;

        /// <summary>
        /// Current public entry path. Every player targets the same named lobby;
        /// Fusion joins the existing host or makes the first player the host.
        /// Explicit StartLobbyHost/JoinLobby remain available for future private
        /// rooms, regions, matchmaking, or lobby sharding.
        /// </summary>
        public async Task<bool> ConnectMainLobby()
        {
            if (_runner != null || networkRunnerPrefab == null) return false;

            _runner = Instantiate(networkRunnerPrefab);
            var startingRunner = _runner;
            _runner.name = "Main Lobby Network Runner";
            ConfigureRunner(_runner);

            var args = new StartGameArgs
            {
                GameMode = GameMode.AutoHostOrClient,
                SessionName = "MainLobby",
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = GetSceneManager(_runner),
                PlayerCount = 32
            };

            var result = await _runner.StartGame(args);
            if (!result.Ok)
            {
                Debug.LogError($"Failed to enter the main lobby: {result.ShutdownReason}");
                await CleanupFailedRunner(startingRunner);
                return false;
            }

            if (_runner.IsServer) OnLobbyStarted?.Invoke(_runner);
            else OnLobbyJoined?.Invoke(_runner);
            return true;
        }

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
            if (networkRunnerPrefab == null)
            {
                Debug.LogError("Network runner prefab is not assigned.");
                return false;
            }

            _runner = Instantiate(networkRunnerPrefab);
            var startingRunner = _runner;
            _runner.name = "Network runner";
            ConfigureRunner(_runner);

            var startGameArgs = new StartGameArgs()
            {
                GameMode = GameMode.Host,
                SessionName = "MainLobby",
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = GetSceneManager(_runner),
                PlayerCount = 32 // Maximum players in the lobby
            };

            var result = await _runner.StartGame(startGameArgs);

            if (!result.Ok)
            {
                Debug.LogError($"Failed to start game: {result.ShutdownReason}");
                if (startingRunner != null) Destroy(startingRunner.gameObject);
                _runner = null;
                return false;
            }

            OnLobbyStarted?.Invoke(_runner);
            return true;
        }

        public async Task<bool> JoinLobby()
        {
            if (_runner != null) return false;
            if (networkRunnerPrefab == null)
            {
                Debug.LogError("Network runner prefab is not assigned.");
                return false;
            }

            _runner = Instantiate(networkRunnerPrefab);
            var startingRunner = _runner;
            _runner.name = "Network runner";
            ConfigureRunner(_runner);

            // A client-only join must fail promptly when no host exists. Without
            // this cancellation token Photon can leave the menu looking stuck
            // while its normal cloud/network timeout elapses.
            using var joinTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var startGameArgs = new StartGameArgs()
            {
                GameMode = GameMode.Client,
                SessionName = "MainLobby",
                Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
                SceneManager = GetSceneManager(_runner),
                StartGameCancellationToken = joinTimeout.Token
            };

            StartGameResult result;
            try
            {
                result = await _runner.StartGame(startGameArgs);
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("No hosted saloon was found before the join timeout.");
                await CleanupFailedRunner(startingRunner);
                return false;
            }

            if (!result.Ok)
            {
                Debug.LogError($"Failed to join lobby: {result.ShutdownReason}");
                await CleanupFailedRunner(startingRunner);
                return false;
            }

            OnLobbyJoined?.Invoke(_runner);
            return true;
        }

        private async Task CleanupFailedRunner(NetworkRunner failedRunner)
        {
            if (failedRunner != null && failedRunner.IsRunning)
                await failedRunner.Shutdown();
            if (failedRunner != null)
                Destroy(failedRunner.gameObject);
            if (_runner == failedRunner)
                _runner = null;
            _isShuttingDown = false;
        }

        public async void LeaveLobby()
        {
            if (_runner != null && !_isShuttingDown)
            {
                _isShuttingDown = true;
                var runner = _runner;
                _runner = null;
                await runner.Shutdown();
                if (runner != null) Destroy(runner.gameObject);
            }
        }

        private void SpawnGameTables()
        {
            if (_runner == null || !_runner.IsServer) return;
            LobbyPlayModePreview.EnsureEnvironment();

            if (gameTablePrefab == null)
            {
                Debug.LogError("MainLobbyManager requires a GameTable network prefab.");
                return;
            }

            var defaultPositions = new[] { new Vector3(0f, 0f, 3f) };
            var count = tableSpawnPoints != null && tableSpawnPoints.Length > 0
                ? tableSpawnPoints.Length : defaultPositions.Length;
            for (int i = 0; i < count; i++)
            {
                var position = tableSpawnPoints != null && tableSpawnPoints.Length > 0
                    ? tableSpawnPoints[i].position : defaultPositions[i];
                var rotation = tableSpawnPoints != null && tableSpawnPoints.Length > 0
                    ? tableSpawnPoints[i].rotation : Quaternion.identity;
                var table = _runner.Spawn(gameTablePrefab, position, rotation);
                table.transform.SetPositionAndRotation(position, rotation);
                _gameTables.Add(table);
            }
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                if (lobbyPlayerPrefab == null)
                {
                    Debug.LogError("MainLobbyManager requires a LobbyPlayer network prefab.");
                    return;
                }
                // Spawn the player character in the lobby
                // CharacterController capsules use their transform as the base in
                // this prefab, so keep them above the saloon floor on spawn.
                Vector3 spawnPosition = new Vector3(
                    UnityEngine.Random.Range(-4f, 4f), 1.05f,
                    UnityEngine.Random.Range(-8f, -5f));
                NetworkObject networkPlayerObject = runner.Spawn(lobbyPlayerPrefab, spawnPosition, Quaternion.identity, player);
                networkPlayerObject.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
                runner.SetPlayerObject(player, networkPlayerObject);
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
            foreach (var tableObject in _gameTables)
            {
                if (tableObject != null)
                    tableObject.GetComponent<GameTable>()?.ServerHandleDisconnected(player);
            }
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
            _runner = null;
        }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            input.Set(new NetworkInputData
            {
                  MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")),
                  RotationInput = Input.GetAxis("Mouse X"),
                  JumpHeld = Input.GetButton("Jump"),
                  CrouchHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C)
              });
        }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
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
            _runner = null;
            _isShuttingDown = false;
            OnLobbyLeft?.Invoke();
        }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

        private void ConfigureRunner(NetworkRunner runner)
        {
            runner.ProvideInput = true;
            runner.AddCallbacks(this);
        }

        private static NetworkSceneManagerDefault GetSceneManager(NetworkRunner runner)
        {
            var manager = runner.GetComponent<NetworkSceneManagerDefault>();
            return manager != null ? manager : runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
    }
} 
