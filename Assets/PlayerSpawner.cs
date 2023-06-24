using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] playerSpawnPoints;

    public GameObject localPlayer;

    private void Start()
    {
        Transform spawnPoint = playerSpawnPoints[PhotonNetwork.IsMasterClient ? 0 : 1];
        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];
        localPlayer = PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);
        Camera.main.GetComponent<CameraController>().target = localPlayer.transform;
    }
}
