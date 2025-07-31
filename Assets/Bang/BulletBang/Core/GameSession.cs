using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class GameSession : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        private GameManager _gameManager;
        //private PlayerManager _playerManager;
        //private TurnManager _turnManager;
        //private DeckManager _deckManager;
        private NetworkRunner _networkRunner;
        [SerializeField]
        private NetworkPrefabRef playerPrefab;
        [Networked, Capacity(8)] private NetworkDictionary<PlayerRef, Player> Players => default;
        [Networked] public NetworkBool IsGameStarted { get; set; }
        [Networked] public int CurrentTurnIndex { get; set; }
        [Networked] public NetworkBool IsSetupPhase { get; set; }
        
        private Dictionary<PlayerRef, PlayerState> _players = new Dictionary<PlayerRef, PlayerState>();
        public static GameSession Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _networkRunner = FindObjectOfType<NetworkRunner>();

            Debug.Log(_networkRunner);
            //GameObject playerManagerObject = new GameObject("PlayerManager");
            //_playerManager = _networkRunner.Spawn(playerManagerObject, Vector3.zero, Quaternion.identity).GetComponent<PlayerManager>();

            //GameObject turnManagerObject = new GameObject("TurnManager");
            //_turnManager = _networkRunner.Spawn(turnManagerObject, Vector3.zero, Quaternion.identity).GetComponent<TurnManager>();

            //GameObject deckManagerObject = new GameObject("DeckManager");
            //_deckManager = _networkRunner.Spawn(deckManagerObject, Vector3.zero, Quaternion.identity).GetComponent<DeckManager>();

            //GameObject gameManagerObject = new GameObject("GameManager");
            //gameManagerObject.AddComponent<GameManager>();
            //Debug.Log(_gameManager);
            //_gameManager = Instantiate(GameManagerPrefab, gameManagerObject.transform);
            //_gameManager.Initialize(_playerManager, _turnManager, _deckManager);

            //StartGameSession();
        }
        public void StartGameSession(GameObject gameManager)
        {
            //_gameManager.StartGame(); 
            //_gameManager = gameManager;
            Debug.Log(gameManager);
            gameManager.GetComponent<GameManager>();
            //Runner.Spawn(gameManager);
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (HasStateAuthority)
            {
                NetworkObject playerObject = Runner.Spawn(playerPrefab, Vector3.up, Quaternion.identity, player);
                Players.Add(player, playerObject.GetComponent<Player>());
            }
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (!HasStateAuthority)
                return;
            if(Players.TryGet(player, out Player playerObject))
            {
                Players.Remove(player);
                Runner.Despawn(playerObject.Object);
            }
        }

        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                IsGameStarted = false;
                IsSetupPhase = true;
                CurrentTurnIndex = -1;
            }
        }

        public void RegisterPlayer(PlayerRef playerRef, PlayerState playerState)
        {
            if (Object.HasStateAuthority)
            {
                _players[playerRef] = playerState;
                RPC_UpdatePlayerList();
            }
        }

        public void UnregisterPlayer(PlayerRef playerRef)
        {
            if (Object.HasStateAuthority && _players.ContainsKey(playerRef))
            {
                _players.Remove(playerRef);
                RPC_UpdatePlayerList();
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_UpdatePlayerList()
        {
            // Notify UI about player list changes
        }

        public void StartGame()
        {
            if (!Object.HasStateAuthority) return;

            if (_players.Count < 4 || _players.Count > 7)
            {
                Debug.LogWarning("Invalid player count for Bang!");
                return;
            }

            IsGameStarted = true;
            IsSetupPhase = false;
            AssignRolesAndCharacters();
            RPC_NotifyGameStart();
        }

        private void AssignRolesAndCharacters()
        {
            if (!Object.HasStateAuthority) return;

            List<RoleType> roles = GenerateRoleList(_players.Count);
            List<CharacterType> characters = GenerateCharacterList(_players.Count);

            // Shuffle roles and characters
            ShuffleList(roles);
            ShuffleList(characters);

            int index = 0;
            foreach (var player in _players.Values)
            {
                player.Role = roles[index];
                player.Character = characters[index];
                if (player.Role == RoleType.Sheriff)
                {
                    player.MaxHealth += 1;
                    player.CurrentHealth += 1;
                }
                index++;
            }
        }

        private List<RoleType> GenerateRoleList(int playerCount)
        {
            List<RoleType> roles = new List<RoleType> { RoleType.Sheriff };
            
            // Add roles based on player count according to Bang! rules
            switch (playerCount)
            {
                case 4:
                    roles.AddRange(new[] { RoleType.Renegade, RoleType.Outlaw, RoleType.Outlaw });
                    break;
                case 5:
                    roles.AddRange(new[] { RoleType.Renegade, RoleType.Outlaw, RoleType.Outlaw, RoleType.Deputy });
                    break;
                case 6:
                    roles.AddRange(new[] { RoleType.Renegade, RoleType.Outlaw, RoleType.Outlaw, RoleType.Outlaw, RoleType.Deputy });
                    break;
                case 7:
                    roles.AddRange(new[] { RoleType.Renegade, RoleType.Outlaw, RoleType.Outlaw, RoleType.Outlaw, RoleType.Deputy, RoleType.Deputy });
                    break;
            }
            
            return roles;
        }

        private List<CharacterType> GenerateCharacterList(int playerCount)
        {
            // Get all available characters
            List<CharacterType> allCharacters = new List<CharacterType>();
            foreach (CharacterType character in System.Enum.GetValues(typeof(CharacterType)))
            {
                allCharacters.Add(character);
            }
            
            // Return more characters than needed so players can choose
            ShuffleList(allCharacters);
            return allCharacters.GetRange(0, playerCount * 2);
        }

        private void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_NotifyGameStart()
        {
            // Notify all clients that the game has started
            // Initialize UI and game state
        }
    }
}