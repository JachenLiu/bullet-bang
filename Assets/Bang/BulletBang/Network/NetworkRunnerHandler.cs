using Fusion;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace BulletBang
{
    public class NetworkRunnerHandler : MonoBehaviour
    {
        public static NetworkRunnerHandler Instance { get; private set; }
        
        [SerializeField] private NetworkRunner _networkRunnerPrefab;
        private NetworkRunner _runner;
        
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

        public async Task<NetworkRunner> StartGame(GameMode mode, string sessionName = "MainLobby")
        {
            if (_runner == null)
            {
                _runner = Instantiate(_networkRunnerPrefab);
                _runner.name = "Network runner";
                
                // Enable Object Pooling
                _runner.ProvideInput = true;
                _runner.AddCallbacks(GetComponent<INetworkRunnerCallbacks>());
            }

            var startGameArgs = new StartGameArgs()
            {
                GameMode = mode,
                SessionName = sessionName,
                PlayerCount = 8,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            };

            var result = await _runner.StartGame(startGameArgs);

            if (!result.Ok)
            {
                Debug.LogError($"Failed to start game: {result.ShutdownReason}");
                return null;
            }

            return _runner;
        }

        public void Shutdown()
        {
            if (_runner != null)
            {
                _runner.Shutdown();
                _runner = null;
            }
        }
    }
} 