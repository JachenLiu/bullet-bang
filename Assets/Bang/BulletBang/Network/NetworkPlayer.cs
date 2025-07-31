using Fusion;
using UnityEngine;
using System;

namespace BulletBang
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [Networked] public NetworkString<_32> PlayerName { get; set; }
        [Networked] public NetworkBool IsInGame { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }
        [Networked] public NetworkBool IsTableHost { get; set; }
        
        [SerializeField] private GameObject playerModel;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 120f;
        
        private GameTable _currentTable;
        private CharacterController _characterController;
        private NetworkRunner _runner;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            if (_characterController == null)
            {
                _characterController = gameObject.AddComponent<CharacterController>();
            }
        }

        public override void Spawned()
        {
            _runner = Object.Runner;

            if (Object.HasInputAuthority)
            {
                // Setup local player
                playerCamera.gameObject.SetActive(true);
                
                // Set default name
                if (string.IsNullOrEmpty(PlayerName.ToString()))
                {
                    RPC_SetPlayerName($"Player_{UnityEngine.Random.Range(1000, 9999)}");
                }
            }
            else
            {
                // Disable camera for remote players
                if (playerCamera != null)
                {
                    playerCamera.gameObject.SetActive(false);
                }
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasInputAuthority && !IsInGame)
            {
                // Handle movement in the lobby
                var input = GetInput<NetworkInputData>();
                if (input.HasValue)
                {
                    Move(input.Value.MovementInput);
                    Rotate(input.Value.RotationInput);
                }
            }
        }

        private void Move(Vector2 input)
        {
            if (_characterController == null) return;

            Vector3 move = transform.forward * input.y + transform.right * input.x;
            _characterController.SimpleMove(move * moveSpeed);
        }

        private void Rotate(float input)
        {
            transform.Rotate(Vector3.up, input * rotationSpeed * Runner.DeltaTime);
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetPlayerName(string newName)
        {
            PlayerName = newName;
        }

        public void JoinTable(GameTable table)
        {
            if (!Object.HasInputAuthority) return;

            if (_currentTable != null)
            {
                LeaveCurrentTable();
            }

            if (table != null && table.CanJoinTable())
            {
                _currentTable = table;
                table.RPC_RequestJoinTable(this);
                IsInGame = true;
                
                // First player to join becomes table host
                if (table.CurrentPlayerCount == 0)
                {
                    IsTableHost = true;
                }
            }
        }

        public void LeaveCurrentTable()
        {
            if (!Object.HasInputAuthority || _currentTable == null) return;

            _currentTable.RPC_RequestLeaveTable(this);
            IsInGame = false;
            IsReady = false;
            IsTableHost = false;
            _currentTable = null;
        }

        public void ToggleReady()
        {
            if (!Object.HasInputAuthority || _currentTable == null) return;
            
            RPC_ToggleReady();
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        private void RPC_ToggleReady()
        {
            IsReady = !IsReady;
        }

        public void RequestStartGame()
        {
            if (!Object.HasInputAuthority || _currentTable == null || !IsTableHost) return;

            _currentTable.RPC_RequestStartGame(this);
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            if (_currentTable != null)
            {
                LeaveCurrentTable();
            }
        }
    }

    public struct NetworkInputData : INetworkInput
    {
        public Vector2 MovementInput;
        public float RotationInput;
    }
} 