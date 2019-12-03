using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class LabyrinthManager : MonoBehaviourPunCallbacks, IPunObservable
{

    public GameObject labyrinthPlayer;
    Vector3 moveDirection;
    public float speed;
    public GameObject labyrinthStart;
    public List<GameObject> arrowButtons;

    // Use this for initialization
    void Start () {
        moveDirection = new Vector3();
        labyrinthPlayer.transform.position = labyrinthStart.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        labyrinthPlayer.transform.position += moveDirection * Time.deltaTime * speed;
	}

    public void HandleArrowButton(direction dir)
    {
        photonView.RPC("RPCArrowButtonIn", RpcTarget.All, dir);
    }

    [PunRPC]
    void RPCArrowButtonIn(direction dir)
    {
        if (dir == direction.up)
        {
            moveDirection += new Vector3(0, 1, 0);
        }
        else if (dir == direction.down)
        {
            moveDirection += new Vector3(0, -1, 0);
        }
        else if (dir == direction.left)
        {
            moveDirection += new Vector3(1, 0, 0);
        }
        else if (dir == direction.right)
        {
            moveDirection += new Vector3(-1, 0, 0);
        }
    }

    internal void ResetLabyrinth()
    {
        labyrinthPlayer.transform.position = labyrinthStart.transform.position;
        moveDirection = new Vector3();
        foreach (GameObject g in arrowButtons)
        {
            g.GetComponent<ArrowButton>().pressed = false;
        }
    }


    public void StopPlayer(direction dir)
    {
        photonView.RPC("RPCArrowButtonOut", RpcTarget.All, dir);
    }
    [PunRPC]
    void RPCArrowButtonOut(direction dir)
    {
        if (dir == direction.up)
        {
            moveDirection -= new Vector3(0, 1, 0);
        }
        else if (dir == direction.up)
        {
            moveDirection -= new Vector3(0, -1, 0);
        }
        else if (dir == direction.up)
        {
            moveDirection -= new Vector3(1, 0, 0);
        }
        else if (dir == direction.up)
        {
            moveDirection -= new Vector3(-1, 0, 0);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
