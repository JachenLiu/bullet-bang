using Fusion;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using BulletBang.Lobby;

namespace BulletBang.Lobby
{
    /// <summary>
    /// The only contract the generic lobby/table system has with a game.
    /// Any future game can supply a network prefab implementing this interface.
    /// </summary>
    public interface ITableGameSession
    {
        string DisplayName { get; }
        int MinimumPlayers { get; }
        int MaximumPlayers { get; }
        bool SupportsSoloTesting { get; }
        void Initialize(IReadOnlyList<NetworkPlayer> players);
    }
}

namespace BulletBang
{
    /// <summary>
    /// Networked boundary between roaming lobby players and one match. Requests
    /// are validated by state authority; clients never assign their own seat,
    /// host status, spectator status, or game membership.
    /// </summary>
    public class GameTable : NetworkBehaviour
    {
        [Networked] public NetworkString<_16> TableName { get; set; }
        [Networked] public NetworkBool IsGameInProgress { get; set; }
        [Networked] public int CurrentPlayerCount { get; set; }
        [Networked] public NetworkBool IsTableLocked { get; set; }
        [Networked] public PlayerRef TableHost { get; set; }
        
        public const int MIN_PLAYERS = 3;
        // Dodge City adds the eighth seat. The base-only first release supports 7.
        public const int MAX_PLAYERS = 7;

        [Networked, Capacity(7)]
        private NetworkDictionary<PlayerRef, NetworkPlayer> TablePlayers => default;
        [Networked, Capacity(7)]
        private NetworkArray<PlayerRef> SeatOrder => default;
        [Networked, Capacity(24)]
        private NetworkDictionary<PlayerRef, NetworkPlayer> Spectators => default;

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
                TableHost = PlayerRef.None;
            }
        }

        public bool CanJoinTable()
        {
            return !IsTableLocked && !IsGameInProgress && CurrentPlayerCount < MAX_PLAYERS;
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestJoinTable(RpcInfo info = default)
        {
            if (!TryResolveSender(info, out var player)) return;
            ServerRequestJoin(player);
        }

        public void RequestJoin(NetworkPlayer player)
        {
            if (Object.HasStateAuthority) ServerRequestJoin(player);
            else RPC_RequestJoinTable();
        }

        private void ServerRequestJoin(NetworkPlayer player)
        {
            if (!Object.HasStateAuthority || player == null) return;
            if (!CanJoinTable())
            {
                player.RPC_TableRequestResult(player.Object.InputAuthority, false,
                    "This table is unavailable.");
                return;
            }

            PlayerRef playerRef = player.Object.InputAuthority;
            if (!TablePlayers.ContainsKey(playerRef))
            {
                TablePlayers.Add(playerRef, player);
                CurrentPlayerCount++;
                SeatOrder.Set(CurrentPlayerCount - 1, playerRef);
                if (TableHost == PlayerRef.None)
                    TableHost = playerRef;

                AssignSeat(playerRef, player, CurrentPlayerCount - 1);
                player.ServerSetTableState(PlayerViewMode.SeatedFirstPerson, playerRef == TableHost);
                RPC_NotifyPlayerJoined(playerRef);
                player.RPC_TableRequestResult(playerRef, true,
                    playerRef == TableHost ? "Joined as table host." : "Joined table.");
            }
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestSpectateTable(RpcInfo info = default)
        {
            if (!TryResolveSender(info, out var player)) return;
            ServerRequestSpectate(player);
        }

        public void RequestSpectate(NetworkPlayer player)
        {
            if (Object.HasStateAuthority) ServerRequestSpectate(player);
            else RPC_RequestSpectateTable();
        }

        private void ServerRequestSpectate(NetworkPlayer player)
        {
            if (!Object.HasStateAuthority || player == null) return;
            var playerRef = player.Object.InputAuthority;
            if (TablePlayers.ContainsKey(playerRef) || Spectators.ContainsKey(playerRef)) return;
            Spectators.Add(playerRef, player);
            player.ServerSetTableState(PlayerViewMode.Spectating, false, FirstSeatedPlayer());
            player.RPC_TableRequestResult(playerRef, true, "Spectating table.");
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestNextSpectatorTarget(RpcInfo info = default)
        {
            if (!TryResolveSender(info, out var player)) return;
            var playerRef = player.Object.InputAuthority;
            if (!Spectators.ContainsKey(playerRef) || TablePlayers.Count == 0) return;

            var refs = new List<PlayerRef>();
            for (var seat = 0; seat < CurrentPlayerCount; seat++) refs.Add(SeatOrder[seat]);
            var current = refs.IndexOf(player.SpectatedPlayer);
            player.ServerSetTableState(PlayerViewMode.Spectating, false, refs[(current + 1) % refs.Count]);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyPlayerJoined(PlayerRef player)
        {
            Debug.Log($"Player {player} joined table {TableName}");
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestLeaveTable(RpcInfo info = default)
        {
            if (!TryResolveSender(info, out var player)) return;
            ServerRequestLeave(player);
        }

        public void RequestLeave(NetworkPlayer player)
        {
            if (Object.HasStateAuthority) ServerRequestLeave(player);
            else RPC_RequestLeaveTable();
        }

        private void ServerRequestLeave(NetworkPlayer player)
        {
            if (!Object.HasStateAuthority || player == null) return;
            if (Object.HasStateAuthority)
            {
                PlayerRef playerRef = player.Object.InputAuthority;
                if (TablePlayers.ContainsKey(playerRef))
                {
                    TablePlayers.Remove(playerRef);
                    CurrentPlayerCount--;
                    RemoveSeat(playerRef);
                    _playerSeatAssignments.Remove(playerRef);
                    if (playerRef == TableHost)
                    {
                        TableHost = FirstSeatedPlayer();
                        if (TableHost != PlayerRef.None && TablePlayers.TryGet(TableHost, out var newHost))
                            newHost.ServerSetTableState(PlayerViewMode.SeatedFirstPerson, true);
                    }
                    player.ServerSetTableState(PlayerViewMode.LobbyThirdPerson, false);

                    RPC_NotifyPlayerLeft(playerRef);

                    // If game is in progress and not enough players, end the game
                    if (IsGameInProgress && CurrentPlayerCount < MIN_PLAYERS)
                    {
                        EndGame();
                    }
                }
                else if (Spectators.ContainsKey(playerRef))
                {
                    Spectators.Remove(playerRef);
                    player.ServerSetTableState(PlayerViewMode.LobbyThirdPerson, false);
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyPlayerLeft(PlayerRef player)
        {
            Debug.Log($"Player {player} left table {TableName}");
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestStartGame(RpcInfo info = default)
        {
            if (!TryResolveSender(info, out var requestingPlayer)) return;
            ServerRequestStart(requestingPlayer);
        }

        public void RequestStart(NetworkPlayer player)
        {
            if (Object.HasStateAuthority) ServerRequestStart(player);
            else RPC_RequestStartGame();
        }

        private void ServerRequestStart(NetworkPlayer requestingPlayer)
        {
            if (!Object.HasStateAuthority || requestingPlayer == null) return;

            var requester = requestingPlayer.Object.InputAuthority;
            if (IsGameInProgress)
            {
                requestingPlayer.RPC_TableRequestResult(requester, false,
                    "A game is already running at this table.");
                return;
            }
            if (requester != TableHost)
            {
                requestingPlayer.RPC_TableRequestResult(requester, false,
                    "Only the table host can start the selected game.");
                return;
            }
            if (CurrentPlayerCount == 2)
            {
                var missing = MIN_PLAYERS - CurrentPlayerCount;
                requestingPlayer.RPC_TableRequestResult(requester, false,
                    $"BANG! needs {MIN_PLAYERS} players. Waiting for {missing} more.");
                return;
            }

            StartGame(requestingPlayer);
        }

        private void StartGame(NetworkPlayer requestingPlayer)
        {
            if (!Object.HasStateAuthority) return;

            var players = new List<NetworkPlayer>();
            for (var seat = 0; seat < CurrentPlayerCount; seat++)
            {
                if (TablePlayers.TryGet(SeatOrder[seat], out var player))
                    players.Add(player);
            }

            _currentGameSession = Runner.Spawn(gameSessionPrefab, transform.position, Quaternion.identity);
            var gameSession = _currentGameSession.GetComponents<MonoBehaviour>()
                .OfType<ITableGameSession>().FirstOrDefault();
            if (gameSession == null)
            {
                Runner.Despawn(_currentGameSession);
                _currentGameSession = null;
                requestingPlayer.RPC_TableRequestResult(requestingPlayer.Object.InputAuthority,
                    false, "The selected game prefab is missing its table-session adapter.");
                return;
            }
            var soloTest = players.Count == 1 && gameSession.SupportsSoloTesting;
            if ((!soloTest && players.Count < gameSession.MinimumPlayers) ||
                players.Count > gameSession.MaximumPlayers)
            {
                Runner.Despawn(_currentGameSession);
                _currentGameSession = null;
                requestingPlayer.RPC_TableRequestResult(requestingPlayer.Object.InputAuthority,
                    false, $"{gameSession.DisplayName} supports {gameSession.MinimumPlayers}-" +
                           $"{gameSession.MaximumPlayers} players.");
                return;
            }

            IsGameInProgress = true;
            IsTableLocked = true;
            gameSession.Initialize(players);
            requestingPlayer.RPC_TableRequestResult(requestingPlayer.Object.InputAuthority,
                true, $"Starting {gameSession.DisplayName}");

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

        public void ServerHandleDisconnected(PlayerRef playerRef)
        {
            if (!Object.HasStateAuthority) return;
            if (TablePlayers.ContainsKey(playerRef))
            {
                TablePlayers.Remove(playerRef);
                CurrentPlayerCount = Mathf.Max(0, CurrentPlayerCount - 1);
                RemoveSeat(playerRef);
                _playerSeatAssignments.Remove(playerRef);
                if (playerRef == TableHost)
                {
                    TableHost = FirstSeatedPlayer();
                    if (TableHost != PlayerRef.None && TablePlayers.TryGet(TableHost, out var newHost))
                        newHost.ServerSetTableState(PlayerViewMode.SeatedFirstPerson, true);
                }
                if (IsGameInProgress && CurrentPlayerCount < MIN_PLAYERS) EndGame();
            }
            Spectators.Remove(playerRef);
        }

        private PlayerRef FirstSeatedPlayer()
        {
            return CurrentPlayerCount > 0 ? SeatOrder[0] : PlayerRef.None;
        }

        private void RemoveSeat(PlayerRef playerRef)
        {
            var removed = -1;
            // CurrentPlayerCount has already been decremented by callers, so the
            // previous occupied length is CurrentPlayerCount + 1.
            for (var i = 0; i <= CurrentPlayerCount; i++)
            {
                if (SeatOrder[i] == playerRef) { removed = i; break; }
            }
            if (removed < 0) return;
            for (var i = removed; i < CurrentPlayerCount; i++)
                SeatOrder.Set(i, SeatOrder[i + 1]);
            SeatOrder.Set(CurrentPlayerCount, PlayerRef.None);
        }

        private void AssignSeat(PlayerRef playerRef, NetworkPlayer player, int index)
        {
            if (playerSeats != null && index < playerSeats.Length && playerSeats[index] != null)
            {
                var seat = playerSeats[index];
                _playerSeatAssignments[playerRef] = seat;
                player.transform.SetPositionAndRotation(seat.position, seat.rotation);
                return;
            }

            // Functional fallback for prototype prefabs without authored seat
            // transforms. Seats face the center of the table.
            var angle = index * Mathf.PI * 0.25f;
            var localPosition = new Vector3(Mathf.Sin(angle) * 3.1f, 0, Mathf.Cos(angle) * 3.1f);
            var worldPosition = transform.TransformPoint(localPosition);
            var rotation = Quaternion.LookRotation(transform.position - worldPosition, Vector3.up);
            player.transform.SetPositionAndRotation(worldPosition, rotation);
        }

        private bool TryResolveSender(RpcInfo info, out NetworkPlayer player)
        {
            player = null;
            if (!Object.HasStateAuthority ||
                !Runner.TryGetPlayerObject(info.Source, out var playerObject))
                return false;
            player = playerObject.GetComponent<NetworkPlayer>();
            return player != null && player.Object.InputAuthority == info.Source;
        }
    }
} 
