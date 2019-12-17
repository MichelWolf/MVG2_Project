using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ControllerSetup : MonoBehaviourPunCallbacks, IPunObservable
{
    // Use this for initialization
    void Start () {
		if (PhotonNetwork.IsConnected)
        {
            if(!photonView.IsMine)
            {
                GetComponentInChildren<SphereCollider>().enabled = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

}
