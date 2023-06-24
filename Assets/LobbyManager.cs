using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField roomNameInput;
    [SerializeField] GameObject notEnoughPlayersText;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject startGameButton;
    [SerializeField] Transform roomItemsHolder;
    [SerializeField] TMP_Text roomName;
    [SerializeField] RoomItem roomItemPrefab;
    List<RoomItem> activeRoomItems = new List<RoomItem>();

    [SerializeField] float timeBetweenRoomUpdates;
    float nextUpdateTime;

    List<PlayerItem> activePlayerItems = new List<PlayerItem>();
    [SerializeField] PlayerItem playerItemPrefab;
    [SerializeField] Transform playerItemHolder;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void OnClickCreate()
    {
        if (roomNameInput.text.Length >= 3)
        {
            PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions() { MaxPlayers = 2, BroadcastPropsChangeToAll = true}); //BroadcastPropsChangeToAll allows communicating playerProperties between players
        }
    }

    public void OnClickStartGame()
    {
        notEnoughPlayersText.SetActive(false);
        if (PhotonNetwork.CountOfPlayers >= 2)
        {
            PhotonNetwork.LoadLevel("MainScene");
            return;
        }
        notEnoughPlayersText.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = $"Room name: {PhotonNetwork.CurrentRoom.Name}";
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenRoomUpdates;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach(RoomItem item in activeRoomItems)
        {
            Destroy(item.gameObject);
        }
        activeRoomItems.Clear();

        foreach(RoomInfo roomInfo in roomList)
        {
           RoomItem newRoom =  Instantiate(roomItemPrefab, roomItemsHolder);
            newRoom.Init(roomInfo.Name, this);
            activeRoomItems.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerList()
    {

        foreach (PlayerItem item in activePlayerItems)
        {
            Destroy(item.gameObject);
        }
        activePlayerItems.Clear();

        if (PhotonNetwork.CurrentRoom == null) return;

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayer = Instantiate(playerItemPrefab, playerItemHolder);
            newPlayer.Init(player.Value);
            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayer.ApplyPersonalChanges();
            }
            activePlayerItems.Add(newPlayer);
        }
    }
}
