using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] private Text roomNameText;
    private string _roomName;
    private System.Action<string> _joinRoomCallback;

    public void Setup(string roomName, System.Action<string> joinRoomCallback)
    {
        _roomName = roomName;
        _joinRoomCallback = joinRoomCallback;
        roomNameText.text = roomName;
    }

    public void OnClick()
    {
        _joinRoomCallback?.Invoke(_roomName);
    }
}
