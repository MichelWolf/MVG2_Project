using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiButton : MonoBehaviourPunCallbacks, IPunObservable
{
    
    public int controllerInside = 0;

    public Color normalColor;
    public Color highlightColor;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        //TODO:: State if button active or inactive instead of enabling/disabling colliders
        if (other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            //controllerInside++;
            photonView.RPC("RPCControllerEnterMultiButton", RpcTarget.All);
            gameObject.GetComponent<MeshRenderer>().material.color = highlightColor;
            Debug.Log("ENTER: MultiButton ");
            if (controllerInside == 1)
            {
                FindObjectOfType<MultiButtonInteractionManager>().OnMultiButtonClick(true);
            }
        }
    }

    [PunRPC]
    void RPCControllerEnterMultiButton()
    {
        controllerInside++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            Debug.Log("EXIT: MultiButton ");
            //controllerInside--;
            photonView.RPC("RPCControllerExitMultiButton", RpcTarget.All);
            
            if (controllerInside == 0)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = normalColor;
                FindObjectOfType<MultiButtonInteractionManager>().OnMultiButtonClick(false);
            }
        }
    }

    [PunRPC]
    void RPCControllerExitMultiButton()
    {
        controllerInside--;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
