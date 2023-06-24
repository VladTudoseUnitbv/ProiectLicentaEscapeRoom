using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text playerNameText;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image playerAvatarImage;
    [SerializeField] List<Sprite> playerAvatars;
    [SerializeField] Color highlightColor;
    [SerializeField] GameObject rightArrowButton;
    [SerializeField] GameObject leftArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    Player player;

    public void Init(Player _player)
    {
        playerNameText.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);
    }

    public void ApplyPersonalChanges()
    {
        backgroundImage.color = highlightColor;
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = playerAvatars.Count - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow()
    {
        if ((int)playerProperties["playerAvatar"] == playerAvatars.Count - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(player == targetPlayer)
        {
            Init(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if(player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatarImage.sprite = playerAvatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
            playerAvatarImage.sprite = playerAvatars[0];
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
}
