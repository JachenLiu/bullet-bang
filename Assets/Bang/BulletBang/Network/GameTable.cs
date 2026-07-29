using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace BulletBang
{
    /// <summary>
    /// Generic social table. It owns seats and public spectators only; individual
    /// games can later attach their own session adapter without changing lobby code.
    /// </summary>
    public sealed class GameTable : NetworkBehaviour
    {
        /// <summary>Maximum number of active players supported by a social table.</summary>
        public const int MaxSeats = 8;

        [Networked] public NetworkString<_16> TableName { get; private set; }
        [Networked] public int CurrentPlayerCount { get; private set; }
        [Networked] public PlayerRef TableHost { get; private set; }
        [Networked, Capacity(MaxSeats)]
        private NetworkDictionary<PlayerRef, NetworkPlayer> Players => default;
        [Networked, Capacity(MaxSeats)]
        private NetworkArray<PlayerRef> SeatOrder => default;
        [Networked, Capacity(24)]
        private NetworkDictionary<PlayerRef, NetworkPlayer> Spectators => default;

        [SerializeField] private Transform[] playerSeats;

        public override void Spawned()
        {
            if (!Object.HasStateAuthority) return;
            TableName = $"Table_{Random.Range(1000, 9999)}";
            TableHost = PlayerRef.None;
        }

        /// <summary>Returns whether another active player can occupy this table.</summary>
        public bool CanJoinTable() => CurrentPlayerCount < MaxSeats;

        /// <summary>Sends an authority-validated request to occupy a seat.</summary>
        /// <param name="player">The requesting network player.</param>
        public void RequestJoin(NetworkPlayer player, int requestedSeat)
        {
            if (Object.HasStateAuthority) ServerJoin(player, requestedSeat);
            else RPC_Join(requestedSeat);
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_Join(int requestedSeat, RpcInfo info = default)
        {
            if (TryResolve(info.Source, out var player)) ServerJoin(player, requestedSeat);
        }

        private void ServerJoin(NetworkPlayer player, int requestedSeat)
        {
            if (!Object.HasStateAuthority || player == null || !CanJoinTable()) return;
            var playerRef = player.Object.InputAuthority;
            if (Players.ContainsKey(playerRef)) return;
            if (!IsValidSeat(requestedSeat) || SeatOrder[requestedSeat] != PlayerRef.None)
            {
                player.RPC_TableRequestResult(playerRef, false, "That seat is occupied.");
                return;
            }
            Spectators.Remove(playerRef);
            Players.Add(playerRef, player);
            SeatOrder.Set(requestedSeat, playerRef);
            CurrentPlayerCount++;
            if (TableHost == PlayerRef.None) TableHost = playerRef;
            PlaceAtSeat(player, requestedSeat);
            player.ServerSetTableState(PlayerViewMode.SeatedFirstPerson, playerRef == TableHost);
            player.RPC_TableRequestResult(playerRef, true,
                playerRef == TableHost ? "Joined as table host." : "Joined table.");
        }

        /// <summary>Sends an authority-validated request to watch public table state.</summary>
        public void RequestSpectate(NetworkPlayer player)
        {
            if (Object.HasStateAuthority) ServerSpectate(player);
            else RPC_Spectate();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_Spectate(RpcInfo info = default)
        {
            if (TryResolve(info.Source, out var player)) ServerSpectate(player);
        }

        private void ServerSpectate(NetworkPlayer player)
        {
            if (!Object.HasStateAuthority || player == null) return;
            var playerRef = player.Object.InputAuthority;
            if (Players.ContainsKey(playerRef) || Spectators.ContainsKey(playerRef)) return;
            Spectators.Add(playerRef, player);
            player.ServerSetTableState(PlayerViewMode.Spectating, false, FirstPlayer());
            player.RPC_TableRequestResult(playerRef, true, "Spectating table.");
        }

        /// <summary>Sends an authority-validated request to leave this table.</summary>
        public void RequestLeave(NetworkPlayer player)
        {
            if (Object.HasStateAuthority) ServerLeave(player);
            else RPC_Leave();
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        private void RPC_Leave(RpcInfo info = default)
        {
            if (TryResolve(info.Source, out var player)) ServerLeave(player);
        }

        private void ServerLeave(NetworkPlayer player)
        {
            if (!Object.HasStateAuthority || player == null) return;
            var playerRef = player.Object.InputAuthority;
            if (Players.ContainsKey(playerRef))
            {
                var removedSeat = FindSeat(playerRef);
                Players.Remove(playerRef);
                CurrentPlayerCount--;
                if (removedSeat >= 0) SeatOrder.Set(removedSeat, PlayerRef.None);
                if (TableHost == playerRef) TableHost = FirstPlayer();
                if (TableHost != PlayerRef.None && Players.TryGet(TableHost, out var host))
                    host.ServerSetTableState(PlayerViewMode.SeatedFirstPerson, true);
            }
            Spectators.Remove(playerRef);
            player.ServerSetTableState(PlayerViewMode.LobbyThirdPerson, false);
            player.RPC_TableRequestResult(playerRef, true, "Returned to lobby.");
        }

        [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
        public void RPC_RequestNextSpectatorTarget(RpcInfo info = default)
        {
            if (!TryResolve(info.Source, out var player) ||
                !Spectators.ContainsKey(info.Source) || CurrentPlayerCount == 0) return;
            var current = FindSeat(player.SpectatedPlayer);
            var next = FindNextOccupiedSeat(current);
            if (next < 0) return;
            player.ServerSetTableState(PlayerViewMode.Spectating, false,
                SeatOrder[next]);
        }

        /// <summary>
        /// Removes a disconnected participant. This method is valid only on state
        /// authority and is called by the lobby lifecycle owner.
        /// </summary>
        public void ServerHandleDisconnected(PlayerRef playerRef)
        {
            if (!Object.HasStateAuthority) return;
            if (Runner.TryGetPlayerObject(playerRef, out var playerObject))
                ServerLeave(playerObject.GetComponent<NetworkPlayer>());
            else
            {
                Players.Remove(playerRef);
                Spectators.Remove(playerRef);
            }
        }

        private void PlaceAtSeat(NetworkPlayer player, int seat)
        {
            if (playerSeats != null && seat < playerSeats.Length && playerSeats[seat] != null)
            {
                player.transform.SetPositionAndRotation(playerSeats[seat].position, playerSeats[seat].rotation);
                return;
            }
            var angle = seat * Mathf.PI * 2f / MaxSeats;
            var position = transform.TransformPoint(new Vector3(Mathf.Sin(angle) * 3.1f, 1f,
                Mathf.Cos(angle) * 3.1f));
            player.transform.SetPositionAndRotation(position,
                Quaternion.LookRotation(transform.position - position, Vector3.up));
        }

        /// <summary>Finds the physical chair closest to a world-space selection point.</summary>
        public int FindClosestSeat(Vector3 worldPoint)
        {
            var nearest = 0;
            var nearestDistance = float.PositiveInfinity;
            for (var seat = 0; seat < MaxSeats; seat++)
            {
                var distance = (SeatPosition(seat) - worldPoint).sqrMagnitude;
                if (distance >= nearestDistance) continue;
                nearestDistance = distance;
                nearest = seat;
            }
            return nearest;
        }

        private PlayerRef FirstPlayer()
        {
            var seat = FindNextOccupiedSeat(-1);
            return seat >= 0 ? SeatOrder[seat] : PlayerRef.None;
        }

        private int FindSeat(PlayerRef player)
        {
            for (var seat = 0; seat < MaxSeats; seat++)
                if (SeatOrder[seat] == player) return seat;
            return -1;
        }

        private int FindNextOccupiedSeat(int currentSeat)
        {
            for (var offset = 1; offset <= MaxSeats; offset++)
            {
                var seat = (currentSeat + offset + MaxSeats) % MaxSeats;
                if (SeatOrder[seat] != PlayerRef.None) return seat;
            }
            return -1;
        }

        private bool IsValidSeat(int seat) => seat >= 0 && seat < MaxSeats;

        private Vector3 SeatPosition(int seat)
        {
            if (playerSeats != null && seat < playerSeats.Length && playerSeats[seat] != null)
                return playerSeats[seat].position;
            var angle = seat * Mathf.PI * 2f / MaxSeats;
            return transform.TransformPoint(
                new Vector3(Mathf.Sin(angle) * 3.1f, 1f, Mathf.Cos(angle) * 3.1f));
        }

        private bool TryResolve(PlayerRef playerRef, out NetworkPlayer player)
        {
            player = null;
            if (!Object.HasStateAuthority ||
                !Runner.TryGetPlayerObject(playerRef, out var playerObject)) return false;
            player = playerObject.GetComponent<NetworkPlayer>();
            return player != null;
        }
    }
}
