using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class SimonSaysManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Buttons")]
    //pull button objects from hierarchy into list
    public List<GameObject> gameButtons;

    //Object that shows which color to press
    public GameObject infoObj;
    private MeshRenderer infoObjMR;

    [Header("Round Settings")]
    public int maxRounds = 4;
    public int currentRound = 0;

    List<int> buttonsToPress;
    List<int> pressedButtons;

    public bool simonRunning = false;

    System.Random rg;

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
        }
        if (infoObj.gameObject.GetComponent<MeshRenderer>() != null)
        {
            infoObjMR = infoObj.gameObject.GetComponent<MeshRenderer>();
            infoObjMR.material.color = Color.black;
        }
        else
        {
            Debug.Log("No Info-Object set");
        }

        foreach (GameObject b in gameButtons)
        {
            b.GetComponent<SimonButton>().interactable = false;
            b.gameObject.GetComponent<MeshRenderer>().material.color = b.gameObject.GetComponent<SimonButton>().normalColor;
        }
    }

    public void OnGameButtonClick(int index)
    {
        photonView.RPC("RPCPlaySimonAudio", RpcTarget.All);
        photonView.RPC("RPCProcessSimonButton", RpcTarget.MasterClient, index);
    }

    //ONLY MASTER CLIENT
    [PunRPC]
    void RPCProcessSimonButton(int index)
    {
        pressedButtons.Add(index);
        if (buttonsToPress[pressedButtons.Count - 1] != index)
        {
            GameOver();
            return;
        }
        if (buttonsToPress.Count == pressedButtons.Count)
        {
            photonView.RPC("RPCStartSimonSaysCoroutine", RpcTarget.MasterClient);
        }
    }

    void GameOver()
    {
        buttonsToPress = new List<int>();
        pressedButtons = new List<int>();
        currentRound = 0;
        photonView.RPC("RPCSimonGameOver", RpcTarget.All);
    }

    [PunRPC]
    void RPCSimonGameOver()
    {
        foreach (GameObject b in gameButtons)
        {
            b.GetComponent<SimonButton>().interactable = false;
            b.gameObject.GetComponent<MeshRenderer>().material.color = b.gameObject.GetComponent<SimonButton>().normalColor;
        }
        //simonRunning = false;
    }

    void Success()
    {
        buttonsToPress = new List<int>();
        pressedButtons = new List<int>();
        currentRound = 0;
        //simonRunning = false;

        photonView.RPC("RPCSimonSuccess", RpcTarget.All);
    }

    [PunRPC]
    void RPCSimonSuccess()
    {
        foreach (GameObject b in gameButtons)
        {
            b.GetComponent<SimonButton>().interactable = false;
            b.gameObject.GetComponent<MeshRenderer>().material.color = b.gameObject.GetComponent<SimonButton>().normalColor;
        }
        Debug.Log("Simon Says erfolgreich");
    }
    IEnumerator SimonSays()
    {
        if (currentRound == maxRounds)
        {
            Success();
            yield break;
        }

        photonView.RPC("RPCDeactivateSimonButtons", RpcTarget.All);


        //"hakunamatata".GetHashCode()


        NextRound();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < buttonsToPress.Count; i++)
        {
            photonView.RPC("RPCBleep", RpcTarget.All, buttonsToPress[i]);
            //Bleep(buttonsToPress[i]);

            yield return new WaitForSeconds(0.6f);
        }

        infoObjMR.material.color = Color.black;
        photonView.RPC("RPCActivateSimonButtons", RpcTarget.All);

        yield return null;
    }

    [PunRPC]
    void RPCDeactivateSimonButtons()
    {
        foreach (GameObject b in gameButtons)
        {
            b.GetComponent<SimonButton>().interactable = false;
            b.gameObject.GetComponent<MeshRenderer>().material.color = b.gameObject.GetComponent<SimonButton>().normalColor;
        }
    }

    [PunRPC]
    void RPCActivateSimonButtons()
    {
        foreach (GameObject b in gameButtons)
        {
            b.GetComponent<SimonButton>().interactable = true;
            b.gameObject.GetComponent<MeshRenderer>().material.color = b.gameObject.GetComponent<SimonButton>().normalColor;
        }
    }

    [PunRPC]
    void RPCPlaySimonAudio()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.gameObject.GetComponent<AudioSource>().clip);
    }


    [PunRPC]
    void RPCBleep(int index)
    {
        //InfoImage.color = gameButtons[index].gameObject.GetComponent<Image>().color;
        infoObjMR.material.color = gameButtons[index].gameObject.GetComponent<SimonButton>().normalColor;
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.gameObject.GetComponent<AudioSource>().clip);

    }

    void NextRound()
    {
        pressedButtons = new List<int>();
        buttonsToPress.Add(rg.Next(0, gameButtons.Count));
        currentRound++;
    }

    public void StartSimonSays()
    {
        //if (simonRunning == false)
        //{
        photonView.RPC("RPCResetSimon", RpcTarget.MasterClient);
        photonView.RPC("RPCStartSimonSaysCoroutine", RpcTarget.MasterClient);
        //}
    }

    [PunRPC]
    void RPCResetSimon()
    {
        currentRound = 0;
        buttonsToPress = new List<int>();
        pressedButtons = new List<int>();
    }

    [PunRPC]
    void RPCStartSimonSaysCoroutine()
    {
        if (currentRound == 0)
        {
            //simonRunning = true;
            rg = new System.Random();
            buttonsToPress = new List<int>();
            pressedButtons = new List<int>();

            for (int i = 0; i < 2; i++)
            {
                buttonsToPress.Add(rg.Next(0, gameButtons.Count));
            }
        }
        StartCoroutine(SimonSays());
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting == true)
        {
            foreach(int i in buttonsToPress)
            {
                stream.SendNext(buttonsToPress[i]);
            }
            foreach (int i in pressedButtons)
            {
                stream.SendNext(pressedButtons[i]);
            }
            stream.SendNext(currentRound);
            //stream.SendNext(simonRunning);
        }
        else
        {
            foreach (int i in buttonsToPress)
            {
                buttonsToPress[i] = (int)stream.ReceiveNext();
            }
            foreach (int i in pressedButtons)
            {
                pressedButtons[i] = (int)stream.ReceiveNext();
            }
            currentRound = (int)stream.ReceiveNext();
            //simonRunning = (bool)stream.ReceiveNext();
        }
    }
}