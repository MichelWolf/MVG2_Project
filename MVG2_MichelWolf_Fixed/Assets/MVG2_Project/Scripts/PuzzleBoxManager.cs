using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PuzzleBoxManager : MonoBehaviourPunCallbacks, IPunObservable  {

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

    void Start () 
    {
        lockedSimon.SetActive(true);
        lockedLabyrinth.SetActive(true);

        hiddenSimonMulti.SetActive(false);
        hiddenLabyrinthMulti.SetActive(false);
        
        if(PhotonNetwork.IsConnected)
        {
            l = PhotonNetwork.Instantiate("ContrModelLeft", ctrlLeftParent.transform.position, ctrlLeftParent.transform.rotation);
            l.transform.SetParent(ctrlLeftParent.transform);
            r = PhotonNetwork.Instantiate("ContrModelRight", ctrlRightParent.transform.position, ctrlRightParent.transform.rotation);
            r.transform.SetParent(ctrlRightParent.transform);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSimonSuccess()
    {
        SimonSuccess = true;
        hiddenSimonMulti.SetActive(true);
        CheckSuccess();
    }

    public void SetMultiSuccess()
    {
        MultiSuccess = true;
        CheckSuccess();
    }

    public void SetLabyrinthSuccess()
    {
        LabyrinthSuccess = true;
        hiddenLabyrinthMulti.SetActive(true);
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
        Debug.Log("Puzzlebox vollständig gelöst!");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }
}
