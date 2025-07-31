using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
namespace BulletBang
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] private InputField playerNameInput;
        [SerializeField] private InputField roomNameInput;
        [SerializeField] private Button createRoomButton;
        [SerializeField] private Button joinRoomButton;
        [SerializeField] private Transform roomListParent;
        [SerializeField] private GameObject roomListItemPrefab;

        private NetworkManager _networkManager;
        public static LobbyManager instance;
        private PlayerManager _playerManager;
        private void Start()
        {
            //_networkManager = NetworkManager.Instance;

            //createRoomButton.onClick.AddListener(OnCreateRoomClicked);
            //joinRoomButton.onClick.AddListener(OnJoinRoomClicked);

            //_networkManager.OnRoomListUpdated += UpdateRoomList;
        }

        private void OnCreateRoomClicked()
        {
            string roomName = roomNameInput.text;
            _networkManager.CreateRoom(roomName);

            LobbyUIManager.Instance.DisplayRoomCreated(roomName);
        }

        private void OnJoinRoomClicked()
        {
            string roomName = roomNameInput.text;
            _networkManager.JoinRoom(roomName);

            LobbyUIManager.Instance.DisplayRoomJoined(roomName);
        }

        private Player playerInstance;
        public void OnPlayerJoined(PlayerRef player)
        {
            playerInstance.SetPlayerID(player);
            _playerManager.AddPlayer(playerInstance);
            Debug.Log("Player Added");
        }
        private void UpdateRoomList(List<string> roomNames)
        {
            foreach (Transform child in roomListParent)
            {
                Destroy(child.gameObject);
            }

            foreach (string roomName in roomNames)
            {
                GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);
                //roomItem.GetComponent<RoomListItem>().Setup(roomName, OnJoinRoomClicked();
            }
        }
    }

}