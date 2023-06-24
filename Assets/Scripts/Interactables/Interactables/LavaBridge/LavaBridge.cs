using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Transporter", menuName = "ScriptableObjects/Interactables/Lava Bridge")]
public class LavaBridge : Interactable
{
    public override void Interact(object param)
    {
        PopupManager.Instance.LavaMapManager.BeginBridgeRaise();
    }
}
