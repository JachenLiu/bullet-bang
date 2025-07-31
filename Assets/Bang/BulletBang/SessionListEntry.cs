using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class SessionListEntry : MonoBehaviour
{
    public TextMeshProUGUI roomName, playerCount;
    public Button joinButton;
    public void JoinRoom()
    {
        //NetworkManager.runnerInstance.StartGame(new StartGameArgs()
        //{
        //    SessionName = roomName.text,
        //});
    }
}
