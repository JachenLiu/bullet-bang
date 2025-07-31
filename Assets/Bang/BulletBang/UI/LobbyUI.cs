using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Fusion;

namespace BulletBang
{
    public class LobbyUI : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private TMP_InputField playerNameInput;

        [Header("Loading")]
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI loadingText;

        [Header("Error")]
        [SerializeField] private GameObject errorPanel;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private Button errorCloseButton;

        private MainLobbyManager _lobbyManager;

        private void Start()
        {
            _lobbyManager = MainLobbyManager.Instance;

            // Setup UI events
            hostButton.onClick.AddListener(OnHostButtonClicked);
            joinButton.onClick.AddListener(OnJoinButtonClicked);
            errorCloseButton.onClick.AddListener(() => errorPanel.SetActive(false));

            // Subscribe to lobby events
            _lobbyManager.OnLobbyStarted += OnLobbyStarted;
            _lobbyManager.OnLobbyJoined += OnLobbyJoined;
            _lobbyManager.OnLobbyLeft += OnLobbyLeft;

            // Show main menu
            ShowMainMenu();
        }

        private void OnDestroy()
        {
            if (_lobbyManager != null)
            {
                _lobbyManager.OnLobbyStarted -= OnLobbyStarted;
                _lobbyManager.OnLobbyJoined -= OnLobbyJoined;
                _lobbyManager.OnLobbyLeft -= OnLobbyLeft;
            }
        }

        private void ShowMainMenu()
        {
            mainMenuPanel.SetActive(true);
            loadingPanel.SetActive(false);
            errorPanel.SetActive(false);
        }

        private void ShowLoading(string message)
        {
            mainMenuPanel.SetActive(false);
            loadingPanel.SetActive(true);
            errorPanel.SetActive(false);
            loadingText.text = message;
        }

        private void ShowError(string message)
        {
            mainMenuPanel.SetActive(false);
            loadingPanel.SetActive(false);
            errorPanel.SetActive(true);
            errorText.text = message;
        }

        private async void OnHostButtonClicked()
        {
            if (string.IsNullOrEmpty(playerNameInput.text))
            {
                ShowError("Please enter a player name");
                return;
            }

            ShowLoading("Creating lobby...");
            bool success = await _lobbyManager.StartLobbyHost();
            
            if (!success)
            {
                ShowError("Failed to create lobby");
            }
        }

        private async void OnJoinButtonClicked()
        {
            if (string.IsNullOrEmpty(playerNameInput.text))
            {
                ShowError("Please enter a player name");
                return;
            }

            ShowLoading("Joining lobby...");
            bool success = await _lobbyManager.JoinLobby();
            
            if (!success)
            {
                ShowError("Failed to join lobby");
            }
        }

        private void OnLobbyStarted(NetworkRunner runner)
        {
            loadingPanel.SetActive(false);
            // Additional UI setup for host
        }

        private void OnLobbyJoined(NetworkRunner runner)
        {
            loadingPanel.SetActive(false);
            // Additional UI setup for client
        }

        private void OnLobbyLeft()
        {
            ShowMainMenu();
        }
    }
} 