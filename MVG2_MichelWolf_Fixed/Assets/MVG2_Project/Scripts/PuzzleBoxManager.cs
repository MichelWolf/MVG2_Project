using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PuzzleBoxManager : MonoBehaviourPunCallbacks, IPunObservable  {

    [Header("Tutorial")]
    public bool tutorialMode = false;
    public GameObject TutorialSimonCanvas;
    public GameObject TutorialMultiCanvas;
    public GameObject TutorialLabyCanvas;
    public GameObject LoadLevelButton;

    [Header("Simon Says")]
    public bool SimonSuccess;
    public GameObject hiddenSimonMulti;
    public GameObject lockedSimon;

    [Header("Multi Button")]
    public bool MultiSuccess;

    [Header("Labyrinth")]
    public bool LabyrinthSuccess;
    public GameObject hiddenLabyrinthMulti;
    public GameObject lockedLabyrinth;
    // Use this for initialization

    [Header("Controller Setup")]
    public GameObject ctrlLeftParent;
    public GameObject ctrlRightParent;

    public GameObject LeftPrefab;
    public GameObject RightPrefab;

    private GameObject l;
    private GameObject r;

    public AudioClip finishedPuzzleClip;
    public GameObject finishedText;

    internal float startTime;

    public GameObject ViveSR;

    void Start () 
    {
        FindObjectOfType<MultiButtonInteractionManager>().numberOfMultiButtons = FindObjectsOfType<MultiButton>().Length;
        lockedSimon.SetActive(true);
        lockedLabyrinth.SetActive(true);
        if (hiddenSimonMulti != null)
        {
            hiddenSimonMulti.SetActive(false);
        }
        if (hiddenLabyrinthMulti != null)
        {
            hiddenLabyrinthMulti.SetActive(false);
        }
        if(LoadLevelButton != null)
        {
            LoadLevelButton.SetActive(false);
        }

        finishedText.SetActive(false);

        if(PhotonNetwork.IsConnected)
        {
            l = PhotonNetwork.Instantiate("ContrModelLeft", ctrlLeftParent.transform.position, ctrlLeftParent.transform.rotation);
            l.transform.SetParent(ctrlLeftParent.transform);
            r = PhotonNetwork.Instantiate("ContrModelRight", ctrlRightParent.transform.position, ctrlRightParent.transform.rotation);
            r.transform.SetParent(ctrlRightParent.transform);
        }
        startTime = Time.time;

        if(FindObjectOfType<LevelSettings>().useAR)
        {
            ViveSR.SetActive(true);
        }
        else
        {
            ViveSR.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSimonSuccess()
    {
        SimonSuccess = true;
        if(hiddenLabyrinthMulti != null)
        {
            hiddenSimonMulti.SetActive(true);
        }
        if(tutorialMode)
        {
            TutorialSimonCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "NICE!";
            TutorialSimonCanvas.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
        }
        CheckSuccess();
    }

    public void SetMultiSuccess()
    {
        MultiSuccess = true;
        if (tutorialMode)
        {
            TutorialMultiCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "GOOD JOB!";
            TutorialMultiCanvas.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;

        }
        CheckSuccess();
    }

    public void SetLabyrinthSuccess()
    {
        LabyrinthSuccess = true;
        if (hiddenLabyrinthMulti != null)
        {
            hiddenLabyrinthMulti.SetActive(true);
        }
        if (tutorialMode)
        {
            TutorialLabyCanvas.GetComponentInChildren<TextMeshProUGUI>().text = "EXCELLENT!";
            TutorialLabyCanvas.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;

        }
        CheckSuccess();
    }

    public void CheckSuccess()
    {
        if (SimonSuccess && MultiSuccess && LabyrinthSuccess)
        {
            FinishPuzzleBox();
        }
    }

    public void FinishPuzzleBox()
    {
        photonView.RPC("RPCFinishedPuzzle", RpcTarget.All);
    }

    [PunRPC]
    void RPCFinishedPuzzle()
    {
        Debug.Log("Puzzlebox vollständig gelöst!");
        GetComponent<AudioSource>().PlayOneShot(finishedPuzzleClip);
        finishedText.SetActive(true);

        
        
        if (tutorialMode)
        {
            LoadLevelButton.SetActive(true);
            finishedText.GetComponentInChildren<TextMeshProUGUI>().text += "\nPress red ball to load the level";
        }
        else
        {
            float totalSeconds = (Time.time - startTime);
            string minutes = Mathf.Floor(totalSeconds / 60).ToString("00");
            string seconds = (totalSeconds % 60).ToString("00");

            finishedText.GetComponentInChildren<TextMeshProUGUI>().text = "Well done!\nTime: " + minutes + ":" + seconds + " minutes";
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
