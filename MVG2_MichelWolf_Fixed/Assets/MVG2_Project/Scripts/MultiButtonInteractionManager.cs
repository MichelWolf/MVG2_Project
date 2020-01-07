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

    public Color deactivatedColor;
    public Color activatedColor;

    public GameObject[] activationLights;

    // Use this for initialization
    void Start () {
        //numberOfMultiButtons = FindObjectsOfType<MultiButton>().Length;
        foreach (GameObject g in activationLights)
        {
            g.GetComponent<MeshRenderer>().material.color = deactivatedColor;
        }
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
        photonView.RPC("RPCSetMultiLight", RpcTarget.All);

    }

    [PunRPC]
    void RPCSetMultiLight()
    {
        for (int i = 0; i < activatedButtons; i++)
        {
            activationLights[i].GetComponent<MeshRenderer>().material.color = activatedColor;
        }
        for (int i = activatedButtons; i < numberOfMultiButtons; i++)
        {
            activationLights[i].GetComponent<MeshRenderer>().material.color = deactivatedColor;
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
