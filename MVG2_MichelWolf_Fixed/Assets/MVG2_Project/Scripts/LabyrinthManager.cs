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

    [Header("Audio Clips")]
    public List<AudioClip> labyAudioClips;
    public AudioClip labySuccess;
    public AudioClip labyFail;
    internal PuzzleBoxManager pbm;

    // Use this for initialization
    void Start () {
        moveDirection = new Vector3();
        labyrinthPlayer.transform.position = labyrinthStart.transform.position;
        pbm = FindObjectOfType<PuzzleBoxManager>();
        labyAudioClips = new List<AudioClip>
        {
            labyFail,
            labySuccess
        };
    }
	
	// Update is called once per frame
	void Update () {
        if (!pbm.LabyrinthSuccess)
        {
            labyrinthPlayer.transform.position += moveDirection * Time.deltaTime * speed;
        }
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
            moveDirection += new Vector3(-1, 0, 0);
        }
        else if (dir == direction.right)
        {
            moveDirection += new Vector3(1, 0, 0);
        }
    }

    internal void ResetLabyrinth()
    {
        labyrinthPlayer.transform.position = labyrinthStart.transform.position;
        moveDirection = new Vector3();
        foreach (GameObject g in arrowButtons)
        {
            g.GetComponent<ArrowButton>().pressed = false;
            g.GetComponent<ArrowButton>().GetComponent<MeshRenderer>().material.color = g.GetComponent<ArrowButton>().normalColor;
        }
        photonView.RPC("RPCPlayLabyAudio", RpcTarget.All, 0);
    }

    internal void LabyrinthSuccess()
    {
        photonView.RPC("RPCLabyrinthSuccess", RpcTarget.All);
    }

    [PunRPC]
    void RPCLabyrinthSuccess()
    {
        StartCoroutine(LabyrinthUnlock());
    }

    IEnumerator LabyrinthUnlock()
    {
        photonView.RPC("RPCPlayLabyAudio", RpcTarget.All, 1);
        labyrinthPlayer.gameObject.SetActive(false);
        FindObjectOfType<PuzzleBoxManager>().SetLabyrinthSuccess();

        //labyAudioClips[1].length

        float totalTime = labyAudioClips[1].length; // fade audio out over 3 seconds
        float currentTime = 0;
        float dissolveValue = 0;
        while (dissolveValue < 1)
        {
            currentTime += Time.deltaTime;
            dissolveValue = Mathf.Lerp(0,1, currentTime / totalTime);
            FindObjectOfType<PuzzleBoxManager>().lockedLabyrinth.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", dissolveValue);
            yield return null;
        }
        FindObjectOfType<PuzzleBoxManager>().lockedLabyrinth.SetActive(false);
        yield return null;
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
        else if (dir == direction.down)
        {
            moveDirection -= new Vector3(0, -1, 0);
        }
        else if (dir == direction.left)
        {
            moveDirection -= new Vector3(-1, 0, 0);
        }
        else if (dir == direction.right)
        {
            moveDirection -= new Vector3(1, 0, 0);
        }
    }

    [PunRPC]
    void RPCPlayLabyAudio(int index)
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(labyAudioClips[index]);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
