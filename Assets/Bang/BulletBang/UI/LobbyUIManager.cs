using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace BulletBang
{
    public class LobbyUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshPro roomNameText;
        [SerializeField] private Text playerCountText; 
        [SerializeField] private GameObject playerListPanel;
        [SerializeField] private GameObject playerListItemPrefab;
        [SerializeField] private Transform playerListContainer;
        [SerializeField] private Button startGameButton;

        private List<GameObject> playerListItems = new List<GameObject>();

        private GameManager gameManager;

        private static LobbyUIManager _instance;
        public static LobbyUIManager Instance { get { return _instance; } private set { _instance = value; } }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        public void ClearInterface()
        {
            UIScreen.activeScreen.Defocus();
        }
        private void Start()
        {
            // Find the GameManager in the scene
            //gameManager = GameManager.Instance;

            // Initially disable the start button
            EnableStartGameButton(false);
        }

        public void DisplayRoomCreated(string roomName)
        {
            roomNameText.text = roomName;
        }
        public void DisplayRoomJoined(string roomName)
        {
            roomNameText.text = roomName;
        }
        public void AddPlayerToList(PlayerRef player)
        {
            GameObject playerItem = Instantiate(playerListItemPrefab, playerListContainer);
            playerItem.GetComponentInChildren<Text>().text = "Player " + player.PlayerId;
            playerListItems.Add(playerItem); 
            UpdateStartGameButton();

        }
        public void RemovePlayerFromList(PlayerRef player)
        {
            GameObject playerItem = playerListItems.Find(item => item.GetComponentInChildren<Text>().text == "Player " + player.PlayerId);
            if (playerItem != null)
            {
                playerListItems.Remove(playerItem);
                Destroy(playerItem);
            }
            UpdateStartGameButton();
        }
        private void UpdateStartGameButton()
        {
            startGameButton.interactable = playerListItems.Count >= 2; // Enable when 2 or more players
        }
        public void EnableStartGameButton(bool enable)
        {
            startGameButton.interactable = enable;
        }
        public void OnStartGameButtonPressed()
        {
            // Call the StartGame method from the GameManager
            gameManager.StartGame();
        }
        public void UpdatePlayerList(List<Player> players)
        {
            // Clear existing list
            //foreach (Transform child in playerListContainer)
            //{
            //    Destroy(child.gameObject);
            //}

            //// Add players to the list
            //foreach (Player player in players)
            //{
            //    GameObject listItem = Instantiate(playerListItemPrefab, playerListContainer);
            //    listItem.GetComponent<Text>().text = player.GetPlayerName(); // Assuming a Player class with a Name property
            //}
        }
    }

}
