using Fusion;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

namespace BulletBang
{
    /// <summary>
    /// Replicated aggregate for one lobby participant. State authority validates
    /// table state while input authority supplies locomotion and local presentation.
    /// Game-specific player state must live in a separate game-session module.
    /// </summary>
    public sealed class NetworkPlayer : NetworkBehaviour
    {
        [Networked] public NetworkString<_32> PlayerName { get; set; }
        [Networked] public NetworkBool IsInGame { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }
        [Networked] public NetworkBool IsTableHost { get; set; }
        [Networked] public PlayerViewMode ViewMode { get; set; }
        [Networked] public PlayerRef SpectatedPlayer { get; set; }
        [Networked] public NetworkBool IsCrouching { get; private set; }
        [Networked] public NetworkBool IsGrounded { get; private set; }
        [Networked] public float VisualSpeed { get; private set; }
        [Networked] private float VerticalVelocity { get; set; }
        [Networked] private NetworkBool WasJumpHeld { get; set; }
        
        [SerializeField] private GameObject playerModel;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float crouchSpeed = 2.4f;
        [SerializeField] private float jumpHeight = 1.35f;
        [SerializeField] private float rotationSpeed = 120f;
        
        private GameTable _currentTable;
        private CharacterController _characterController;
        private NetworkRunner _runner;
        private Vector3 _cameraVelocity;
        private TextMeshPro _nameplate;
        private float _cameraPitch = 14f;
        private float _smoothCameraPitch = 14f;
        private float _smoothCameraYaw;
        private float _cameraPitchVelocity;
        private float _cameraYawVelocity;
        private float _seatedYaw;
        private float _seatedPitch;
        private float _smoothSeatedYaw;
        private float _smoothSeatedPitch;
        private float _seatedYawVelocity;
        private float _seatedPitchVelocity;
        private PlayerViewMode _previousViewMode;
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
            // The humanoid is part of the network prefab, so every client renders
            // the same model without spawning a separate local-only visual.
            var avatarAnimator = playerModel != null
                ? playerModel.GetComponent<LobbyAvatarAnimator>()
                : null;
            if (avatarAnimator == null && playerModel != null)
                avatarAnimator = playerModel.AddComponent<LobbyAvatarAnimator>();
            if (avatarAnimator != null) avatarAnimator.Player = this;
            CreateNameplate();
            _smoothCameraYaw = transform.eulerAngles.y;

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
                Move(input);
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
                _smoothCameraPitch = Mathf.SmoothDampAngle(
                    _smoothCameraPitch, _cameraPitch, ref _cameraPitchVelocity, 0.1f);
                _smoothCameraYaw = Mathf.SmoothDampAngle(
                    _smoothCameraYaw, target.eulerAngles.y, ref _cameraYawVelocity, 0.1f);
                var orbit = Quaternion.Euler(_smoothCameraPitch, _smoothCameraYaw, 0);
                desiredPosition = target.position + orbit * LobbyCameraOffset;
                playerCamera.transform.rotation = orbit;
            }
            else if (ViewMode == PlayerViewMode.SeatedFirstPerson)
            {
                // Seated players can look around the table, but the limits keep
                // the viewpoint plausibly attached to the character's head.
                _smoothSeatedYaw = Mathf.SmoothDampAngle(
                    _smoothSeatedYaw, _seatedYaw, ref _seatedYawVelocity, 0.075f);
                _smoothSeatedPitch = Mathf.SmoothDampAngle(
                    _smoothSeatedPitch, _seatedPitch, ref _seatedPitchVelocity, 0.075f);
                playerCamera.transform.rotation =
                    target.rotation * Quaternion.Euler(_smoothSeatedPitch, _smoothSeatedYaw, 0f);
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

            if (ViewMode != _previousViewMode)
            {
                _seatedYaw = _smoothSeatedYaw = 0f;
                _seatedPitch = _smoothSeatedPitch = 0f;
                _previousViewMode = ViewMode;
            }

            if (ViewMode == PlayerViewMode.SeatedFirstPerson)
            {
                _seatedYaw = Mathf.Clamp(
                    _seatedYaw + Input.GetAxis("Mouse X") * 2.2f, -75f, 75f);
                _seatedPitch = Mathf.Clamp(
                    _seatedPitch - Input.GetAxis("Mouse Y") * 2.2f, -35f, 55f);
            }
            else if (ViewMode == PlayerViewMode.LobbyThirdPerson)
            {
                _cameraPitch = Mathf.Clamp(
                    _cameraPitch - Input.GetAxis("Mouse Y") * 2.5f, -35f, 70f);
            }

            if (Input.GetKeyDown(KeyCode.E) && ViewMode == PlayerViewMode.LobbyThirdPerson)
            {
                var table = FindInteractionTable(out var seat);
                if (table != null) JoinTable(table, seat);
                else ShowInteractionStatus("Move closer to a table.");
            }
            if (Input.GetKeyDown(KeyCode.Q) && ViewMode == PlayerViewMode.LobbyThirdPerson)
            {
                var table = FindInteractionTable(out _);
                if (table != null) SpectateTable(table);
                else ShowInteractionStatus("Move closer to a table.");
            }
            if (Input.GetKeyDown(KeyCode.Escape) && _currentTable != null)
                LeaveCurrentTable();
            if (Input.GetKeyDown(KeyCode.Tab) && ViewMode == PlayerViewMode.Spectating)
                SpectateNextPlayer();
        }

        private void OnGUI()
        {
            if (!Object || !Object.HasInputAuthority) return;
            var message = ViewMode switch
            {
                PlayerViewMode.LobbyThirdPerson =>
                    FindInteractionTable(out _) != null
                        ? "E: Join table    Q: Spectate table"
                        : "WASD: Move    Mouse: Look    Space: Jump    Ctrl/C: Crouch",
                PlayerViewMode.SeatedFirstPerson =>
                    IsTableHost
                        ? "Table host  •  Mouse: Look around  •  Esc: Leave table"
                        : "Joined table  •  Mouse: Look around  •  Esc: Leave table",
                PlayerViewMode.Spectating => "Tab: Next player POV    Esc: Stop spectating",
                _ => string.Empty
            };
            GUI.Box(new Rect(Screen.width * 0.5f - 220, Screen.height - 64, 440, 38), message);
            GUI.Label(new Rect(Screen.width * 0.5f - 8, Screen.height * 0.5f - 12, 20, 20), "+");
            if (Time.unscaledTime < _interactionStatusUntil)
                GUI.Box(new Rect(Screen.width * 0.5f - 190, Screen.height - 108, 380, 34),
                    _interactionStatus);
        }

        private GameTable FindInteractionTable(out int seat)
        {
            seat = -1;
            if (playerCamera != null &&
                Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                    out var hit, 12f))
            {
                var aimedTable = hit.collider.GetComponentInParent<GameTable>();
                if (aimedTable != null)
                {
                    seat = aimedTable.FindClosestSeat(hit.point);
                    return aimedTable;
                }
            }
            var nearest = FindNearestTable(7.5f);
            if (nearest != null) seat = nearest.FindClosestSeat(transform.position);
            return nearest;
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

        private void Move(NetworkInputData input)
        {
            if (_characterController == null) return;

            IsGrounded = _characterController.isGrounded;
            IsCrouching = input.CrouchHeld;
            var jumpPressed = input.JumpHeld && !WasJumpHeld;
            WasJumpHeld = input.JumpHeld;

            _characterController.height = IsCrouching ? 1.15f : 2f;
            _characterController.center = new Vector3(0f, _characterController.height * 0.5f, 0f);

            Vector3 move = transform.forward * input.MovementInput.y +
                           transform.right * input.MovementInput.x;
            if (move.sqrMagnitude > 1f) move.Normalize();
            // CharacterController.Move is deterministic for Fusion's simulation
            // tick; SimpleMove internally uses frame delta and can be swallowed by
            // NetworkTransform prediction/resimulation.
            VisualSpeed = move.magnitude;
            var velocity = move * (IsCrouching ? crouchSpeed : moveSpeed);
            if (IsGrounded && VerticalVelocity < 0f) VerticalVelocity = -2f;
            if (IsGrounded && jumpPressed && !IsCrouching)
                VerticalVelocity = Mathf.Sqrt(jumpHeight * 2f * 9.81f);
            VerticalVelocity -= 9.81f * Runner.DeltaTime;
            velocity.y = VerticalVelocity;
            _characterController.Move(velocity * Runner.DeltaTime);
        }

        private void Rotate(float input)
        {
            transform.Rotate(Vector3.up, input * rotationSpeed * Runner.DeltaTime);
        }

        /// <summary>Requests an authority-owned update to this player's display name.</summary>
        [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
        public void RPC_SetPlayerName(string newName)
        {
            PlayerName = newName;
        }

        /// <summary>Requests a validated active seat at the supplied social table.</summary>
        public void JoinTable(GameTable table, int seat)
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
                table.RequestJoin(this, seat);
            }
        }

        /// <summary>Requests a public spectator position at the supplied table.</summary>
        public void SpectateTable(GameTable table)
        {
            if (!Object.HasInputAuthority || table == null) return;
            if (_currentTable != null) LeaveCurrentTable();
            _currentTable = table;
            ShowInteractionStatus("Requesting spectator view...");
            table.RequestSpectate(this);
        }

        /// <summary>Cycles to the next public player viewpoint at the current table.</summary>
        public void SpectateNextPlayer()
        {
            if (!Object.HasInputAuthority || _currentTable == null ||
                ViewMode != PlayerViewMode.Spectating) return;
            _currentTable.RPC_RequestNextSpectatorTarget();
        }

        /// <summary>Leaves the current active or spectator table membership.</summary>
        public void LeaveCurrentTable()
        {
            if (!Object.HasInputAuthority || _currentTable == null) return;

            _currentTable.RequestLeave(this);
            _currentTable = null;
        }

        /// <summary>Toggles this player's ready intention for a future table game.</summary>
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

        /// <summary>Delivers an authority-generated table request result to one client.</summary>
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

} 
