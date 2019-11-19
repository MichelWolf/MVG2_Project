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

    [Header("Audio Clips")]
    public List<AudioClip> audioClips;
    public AudioClip buttonSound;
    public AudioClip roundCompleteSound;
    public AudioClip successSound;
    public AudioClip failSound;

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
        List<int> buttonsToPress = new List<int>();
        List<int> pressedButtons = new List<int>();
        audioClips = new List<AudioClip>();

        audioClips.Add(buttonSound);
        audioClips.Add(roundCompleteSound);
        audioClips.Add(successSound);
        audioClips.Add(failSound);
        foreach (GameObject b in gameButtons)
        {
            b.GetComponent<SimonButton>().interactable = false;
            b.gameObject.GetComponent<MeshRenderer>().material.color = b.gameObject.GetComponent<SimonButton>().normalColor;
        }
    }

    public void OnGameButtonClick(int index)
    {
        photonView.RPC("RPCPlaySimonAudio", RpcTarget.All, 0);
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
            photonView.RPC("RPCPlaySimonAudio", RpcTarget.All, 0);
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
        photonView.RPC("RPCPlaySimonAudio", RpcTarget.All, 3);
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
        photonView.RPC("RPCPlaySimonAudio", RpcTarget.All, 2);
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
    void RPCPlaySimonAudio(int index)
    {
        Debug.Log(this.gameObject.GetComponent<AudioSource>());
        Debug.Log(index);
        Debug.Log(audioClips[index]);
        Debug.Log(audioClips);
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(audioClips[index]);
    }


    [PunRPC]
    void RPCBleep(int index)
    {
        //InfoImage.color = gameButtons[index].gameObject.GetComponent<Image>().color;
        infoObjMR.material.color = gameButtons[index].gameObject.GetComponent<SimonButton>().normalColor;
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(buttonSound);
    }

    void NextRound()
    {
        pressedButtons = new List<int>();
        buttonsToPress.Add(rg.Next(0, gameButtons.Count));
        currentRound++;
        photonView.RPC("RPCPlaySimonAudio", RpcTarget.All, 1);
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
        
    }
}