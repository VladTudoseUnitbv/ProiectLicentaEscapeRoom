using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMapManager : MonoBehaviour
{
    [SerializeField] Animator lavaBridgeAnimator;
    [SerializeField] PhotonView photonView;

    public void BeginBridgeRaise()
    {
        photonView.RPC("RaiseLavaBridge", RpcTarget.All);
    }

    [PunRPC]
    private void RaiseLavaBridge()
    {
        lavaBridgeAnimator.SetTrigger("RaiseBridge");
    }
}
