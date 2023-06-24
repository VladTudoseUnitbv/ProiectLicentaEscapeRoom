using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomItem : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    LobbyManager lobbyManager;

    public void Init(string _roomName, LobbyManager lobbyManager)
    {
        roomName.text = _roomName;
        this.lobbyManager = lobbyManager;
    }

    public void OnClickItem()
    {
        lobbyManager.JoinRoom(roomName.text);
    }
    
}
