using Fusion;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

namespace BulletBang
{
    public class NetworkPlayer : NetworkBehaviour
    {
        [Networked] public NetworkString<_32> PlayerName { get; set; }
        [Networked] public NetworkBool IsInGame { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }
        [Networked] public NetworkBool IsTableHost { get; set; }
        [Networked] public PlayerViewMode ViewMode { get; set; }
        [Networked] public PlayerRef SpectatedPlayer { get; set; }
        
        [SerializeField] private GameObject playerModel;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 120f;
        
        private GameTable _currentTable;
        private CharacterController _characterController;
        private NetworkRunner _runner;
        private Vector3 _cameraVelocity;
        private TextMeshPro _nameplate;
        private float _cameraPitch = 14f;
        private string _interactionStatus = string.Empty;
        private float _interactionStatusUntil;

        private static readonly Vector3 LobbyCameraOffset = new(0f, 2.6f, -4.8f);
        private static readonly Vector3 FirstPersonCameraOffset = new(0f, 1.65f, 0.08f);

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
            CreateNameplate();

            if (Object.HasInputAuthority)
            {
                // Setup local player
                playerCamera.gameObject.SetActive(true);
                ActivateLocalCamera();
                
                RPC_SetPlayerName(LocalPlayerData.NickName);
                if (EventSystem.current != null)
                    EventSystem.current.SetSelectedGameObject(null);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
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
            if (ViewMode == PlayerViewMode.LobbyThirdPerson &&
                GetInput(out NetworkInputData input))
            {
                Move(input.MovementInput);
                Rotate(input.RotationInput);
            }
        }

        public override void Render()
        {
            if (_nameplate == null) return;
            var displayedName = PlayerName.ToString();
            _nameplate.text = string.IsNullOrWhiteSpace(displayedName) ? "Player" : displayedName;

            // A world-space label must face each client's active camera.
            var viewingCamera = Camera.main;
            if (viewingCamera != null)
                _nameplate.transform.rotation = viewingCamera.transform.rotation;
        }

        private void CreateNameplate()
        {
            var label = new GameObject("Player Nameplate", typeof(TextMeshPro));
            label.transform.SetParent(transform, false);
            label.transform.localPosition = new Vector3(0, 2.25f, 0);
            _nameplate = label.GetComponent<TextMeshPro>();
            _nameplate.text = "Player";
            _nameplate.fontSize = 3.2f;
            _nameplate.alignment = TextAlignmentOptions.Center;
            _nameplate.color = new Color(1f, 0.84f, 0.5f);
            _nameplate.outlineWidth = 0.2f;
            _nameplate.outlineColor = new Color32(35, 12, 4, 255);
            _nameplate.rectTransform.sizeDelta = new Vector2(4.5f, 0.8f);
        }

        private void ActivateLocalCamera()
        {
            var preview = FindFirstObjectByType<LobbyPlayModePreview>();
            if (preview != null) preview.enabled = false;

            foreach (var camera in Camera.allCameras)
            {
                if (camera != playerCamera) camera.gameObject.SetActive(false);
            }
            foreach (var listener in FindObjectsByType<AudioListener>(FindObjectsSortMode.None))
            {
                listener.enabled = listener.gameObject == playerCamera.gameObject;
            }
            playerCamera.tag = "MainCamera";
        }

        private void LateUpdate()
        {
            if (!Object.HasInputAuthority || playerCamera == null) return;

            Transform target = transform;
            var localOffset = ViewMode == PlayerViewMode.LobbyThirdPerson
                ? LobbyCameraOffset
                : FirstPersonCameraOffset;

            if (ViewMode == PlayerViewMode.Spectating &&
                Runner != null && Runner.TryGetPlayerObject(SpectatedPlayer, out var spectatedObject))
            {
                target = spectatedObject.transform;
            }

            var desiredPosition = target.TransformPoint(localOffset);
            if (ViewMode == PlayerViewMode.LobbyThirdPerson)
            {
                var orbit = Quaternion.Euler(_cameraPitch, target.eulerAngles.y, 0);
                desiredPosition = target.position + orbit * LobbyCameraOffset;
                playerCamera.transform.rotation = orbit;
            }
            else
            {
                playerCamera.transform.rotation = target.rotation;
            }
            playerCamera.transform.position = Vector3.SmoothDamp(
                playerCamera.transform.position, desiredPosition, ref _cameraVelocity, 0.08f);
        }

        private void Update()
        {
            if (!Object || !Object.HasInputAuthority) return;

            _cameraPitch = Mathf.Clamp(
                _cameraPitch - Input.GetAxis("Mouse Y") * 2.5f, -35f, 70f);

            if (Input.GetKeyDown(KeyCode.E) && ViewMode == PlayerViewMode.LobbyThirdPerson)
            {
                var table = FindInteractionTable();
                if (table != null) JoinTable(table);
                else ShowInteractionStatus("Move closer to a table.");
            }
            if (Input.GetKeyDown(KeyCode.Q) && ViewMode == PlayerViewMode.LobbyThirdPerson)
            {
                var table = FindInteractionTable();
                if (table != null) SpectateTable(table);
                else ShowInteractionStatus("Move closer to a table.");
            }
            if (Input.GetKeyDown(KeyCode.Escape) && _currentTable != null)
                LeaveCurrentTable();
            if (Input.GetKeyDown(KeyCode.R) && IsTableHost)
                RequestStartGame();
            if (Input.GetKeyDown(KeyCode.Tab) && ViewMode == PlayerViewMode.Spectating)
                SpectateNextPlayer();
        }

        private void OnGUI()
        {
            if (!Object || !Object.HasInputAuthority) return;
            var message = ViewMode switch
            {
                PlayerViewMode.LobbyThirdPerson =>
                    FindInteractionTable() != null
                        ? "E: Join table    Q: Spectate table"
                        : "WASD: Move    Mouse: Look    Approach a table",
                PlayerViewMode.SeatedFirstPerson =>
                    IsTableHost
                        ? "Table host  •  R: Start BANG! (solo test)  •  Esc: Leave"
                        : "Joined table  •  Waiting for host to start  •  Esc: Leave",
                PlayerViewMode.Spectating => "Tab: Next player POV    Esc: Stop spectating",
                _ => string.Empty
            };
            GUI.Box(new Rect(Screen.width * 0.5f - 220, Screen.height - 64, 440, 38), message);
            GUI.Label(new Rect(Screen.width * 0.5f - 8, Screen.height * 0.5f - 12, 20, 20), "+");
            if (Time.unscaledTime < _interactionStatusUntil)
                GUI.Box(new Rect(Screen.width * 0.5f - 190, Screen.height - 108, 380, 34),
                    _interactionStatus);
        }

        private GameTable FindInteractionTable()
        {
            if (playerCamera != null &&
                Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                    out var hit, 12f))
            {
                var aimedTable = hit.collider.GetComponentInParent<GameTable>();
                if (aimedTable != null) return aimedTable;
            }
            return FindNearestTable(7.5f);
        }

        private GameTable FindNearestTable(float maximumDistance)
        {
            GameTable nearest = null;
            var nearestSquared = maximumDistance * maximumDistance;
            foreach (var table in FindObjectsByType<GameTable>(FindObjectsSortMode.None))
            {
                var distance = (table.transform.position - transform.position).sqrMagnitude;
                if (distance >= nearestSquared) continue;
                nearestSquared = distance;
                nearest = table;
            }
            return nearest;
        }

        private void Move(Vector2 input)
        {
            if (_characterController == null) return;

            Vector3 move = transform.forward * input.y + transform.right * input.x;
            if (move.sqrMagnitude > 1f) move.Normalize();
            // CharacterController.Move is deterministic for Fusion's simulation
            // tick; SimpleMove internally uses frame delta and can be swallowed by
            // NetworkTransform prediction/resimulation.
            var velocity = move * moveSpeed;
            velocity.y = _characterController.isGrounded ? -1f : -9.81f;
            _characterController.Move(velocity * Runner.DeltaTime);
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
                ShowInteractionStatus("Requesting seat...");
                table.RequestJoin(this);
            }
        }

        public void SpectateTable(GameTable table)
        {
            if (!Object.HasInputAuthority || table == null) return;
            if (_currentTable != null) LeaveCurrentTable();
            _currentTable = table;
            ShowInteractionStatus("Requesting spectator view...");
            table.RequestSpectate(this);
        }

        public void SpectateNextPlayer()
        {
            if (!Object.HasInputAuthority || _currentTable == null ||
                ViewMode != PlayerViewMode.Spectating) return;
            _currentTable.RPC_RequestNextSpectatorTarget();
        }

        public void LeaveCurrentTable()
        {
            if (!Object.HasInputAuthority || _currentTable == null) return;

            _currentTable.RequestLeave(this);
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

            _currentTable.RequestStart(this);
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_TableRequestResult([RpcTarget] PlayerRef recipient, NetworkBool accepted,
            NetworkString<_64> message)
        {
            if (!accepted && ViewMode == PlayerViewMode.LobbyThirdPerson)
                _currentTable = null;
            ShowInteractionStatus(message.ToString());
        }

        private void ShowInteractionStatus(string message)
        {
            _interactionStatus = message;
            _interactionStatusUntil = Time.unscaledTime + 3f;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _currentTable = null;
        }

        // These methods are called only by a state-authoritative GameTable after it
        // validates membership. Clients cannot grant themselves a seat or host flag.
        internal void ServerSetTableState(PlayerViewMode mode, bool isHost, PlayerRef spectatedPlayer = default)
        {
            if (!Object.HasStateAuthority) return;
            ViewMode = mode;
            IsInGame = mode == PlayerViewMode.SeatedFirstPerson;
            IsTableHost = isHost;
            IsReady = false;
            SpectatedPlayer = spectatedPlayer;
        }
    }

    public struct NetworkInputData : INetworkInput
    {
        public Vector2 MovementInput;
        public float RotationInput;
    }
} 
