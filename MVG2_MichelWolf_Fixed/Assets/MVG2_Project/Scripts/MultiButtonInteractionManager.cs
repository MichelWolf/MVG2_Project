using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiButtonInteractionManager : MonoBehaviourPunCallbacks, IPunObservable
{

    public int numberOfMultiButtons;

    public int activatedButtons;
    public bool multiButtonsDone = false;

    // Use this for initialization
    void Start () {
        numberOfMultiButtons = FindObjectsOfType<MultiButton>().Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMultiButtonClick(bool activated)
    {
        photonView.RPC("RPCProcessMultiButton", RpcTarget.MasterClient, activated);
    }

    [PunRPC]
    void RPCProcessMultiButton(bool activated)
    {
        if (activated)
        {
            activatedButtons++;

            if (activatedButtons == numberOfMultiButtons)
            {
                photonView.RPC("RPCSuccessMultiButton", RpcTarget.All);
            }
        }
        else
        {
            activatedButtons--;
        }
    }

    [PunRPC]
    void RPCSuccessMultiButton()
    {
        FindObjectOfType<PuzzleBoxManager>().SetMultiSuccess();
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
