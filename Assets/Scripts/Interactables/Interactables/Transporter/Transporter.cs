using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transporter", menuName = "ScriptableObjects/Interactables/Transporter")]
public class Transporter : Interactable
{
    public override void Interact(object param)
    {
        PopupManager.Instance.PlayerSpawner.localPlayer.transform.position = (param as List<Transform>)[PhotonNetwork.IsMasterClient ? 0 : 1].position;
    }
}
