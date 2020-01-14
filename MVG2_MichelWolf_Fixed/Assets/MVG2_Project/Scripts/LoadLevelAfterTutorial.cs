using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LoadLevelAfterTutorial : MonoBehaviourPunCallbacks, IPunObservable
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        //photonView.RPC("RPCLoadLevel", RpcTarget.MasterClient);
        photonView.RPC("RPCLoadLevelAll", RpcTarget.All);
    }

    [PunRPC]
    void RPCLoadLevelAll()
    {
        Debug.Log("MASTER RECIEVED RPC to Load level");
        PhotonNetwork.LoadLevel(2);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
