using Fusion;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace BulletBang
{
    public class GameTable : NetworkBehaviour
    {
        [Networked] public NetworkString<_16> TableName { get; set; }
        [Networked] public NetworkBool IsGameInProgress { get; set; }
        [Networked] public int CurrentPlayerCount { get; set; }
        [Networked] public NetworkBool IsTableLocked { get; set; }
        
        public const int MIN_PLAYERS = 3;
        public const int MAX_PLAYERS = 8;

        [Networked, Capacity(8)]
        private NetworkDictionary<PlayerRef, NetworkPlayer> TablePlayers => default;

        [SerializeField] private Transform[] playerSeats;
        [SerializeField] private NetworkPrefabRef gameSessionPrefab;
        
        private NetworkObject _currentGameSession;
        private Dictionary<PlayerRef, Transform> _playerSeatAssignments = new Dictionary<PlayerRef, Transform>();

        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                TableName = $"Table_{UnityEngine.Random.Range(1000, 9999)}";
                IsGameInProgress = false;
                IsTableLocked = false;
                CurrentPlayerCount = 0;
            }
        }

        public bool CanJoinTable()
        {
            return !IsTableLocked && !IsGameInProgress && CurrentPlayerCount < MAX_PLAYERS;
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_RequestJoinTable(NetworkPlayer player)
        {
            if (!CanJoinTable()) return;

            if (Object.HasStateAuthority)
            {
                PlayerRef playerRef = player.Object.InputAuthority;
                if (!TablePlayers.ContainsKey(playerRef))
                {
                    TablePlayers.Add(playerRef, player);
                    CurrentPlayerCount++;

                    // Assign seat position
                    if (CurrentPlayerCount <= playerSeats.Length)
                    {
                        Transform seat = playerSeats[CurrentPlayerCount - 1];
                        _playerSeatAssignments[playerRef] = seat;
                        player.transform.position = seat.position;
                        player.transform.rotation = seat.rotation;
                    }

                    RPC_NotifyPlayerJoined(playerRef);
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyPlayerJoined(PlayerRef player)
        {
            Debug.Log($"Player {player} joined table {TableName}");
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_RequestLeaveTable(NetworkPlayer player)
        {
            if (Object.HasStateAuthority)
            {
                PlayerRef playerRef = player.Object.InputAuthority;
                if (TablePlayers.ContainsKey(playerRef))
                {
                    TablePlayers.Remove(playerRef);
                    CurrentPlayerCount--;
                    _playerSeatAssignments.Remove(playerRef);

                    RPC_NotifyPlayerLeft(playerRef);

                    // If game is in progress and not enough players, end the game
                    if (IsGameInProgress && CurrentPlayerCount < MIN_PLAYERS)
                    {
                        EndGame();
                    }
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyPlayerLeft(PlayerRef player)
        {
            Debug.Log($"Player {player} left table {TableName}");
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_RequestStartGame(NetworkPlayer requestingPlayer)
        {
            if (!Object.HasStateAuthority) return;
            
            if (IsGameInProgress || CurrentPlayerCount < MIN_PLAYERS) return;

            // Check if requesting player is at the table
            if (!TablePlayers.ContainsKey(requestingPlayer.Object.InputAuthority)) return;

            StartGame();
        }

        private void StartGame()
        {
            if (!Object.HasStateAuthority) return;

            IsGameInProgress = true;
            IsTableLocked = true;

            // Spawn the game session
            _currentGameSession = Runner.Spawn(gameSessionPrefab, transform.position, Quaternion.identity);
            
            // Initialize the game session with current players
            var gameSession = _currentGameSession.GetComponent<GameSession>();
            foreach (var player in TablePlayers)
            {
                gameSession.PlayerJoined(player.Key);
            }

            RPC_NotifyGameStarted();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyGameStarted()
        {
            Debug.Log($"Game started at table {TableName}");
        }

        public void EndGame()
        {
            if (!Object.HasStateAuthority) return;

            if (_currentGameSession != null)
            {
                Runner.Despawn(_currentGameSession);
                _currentGameSession = null;
            }

            IsGameInProgress = false;
            IsTableLocked = false;

            RPC_NotifyGameEnded();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyGameEnded()
        {
            Debug.Log($"Game ended at table {TableName}");
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _playerSeatAssignments.Clear();
            if (_currentGameSession != null && hasState)
            {
                runner.Despawn(_currentGameSession);
            }
        }
    }
} 