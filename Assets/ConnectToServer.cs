using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks {
    public TMP_InputField usernameInput;
    public TMP_Text connectButtonText;

    public void OnClickConnect()
    {
        if(usernameInput.text.Length >= 3)
        {
            PhotonNetwork.NickName = usernameInput.text;
            connectButtonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("PregameLobby");
    }
}
