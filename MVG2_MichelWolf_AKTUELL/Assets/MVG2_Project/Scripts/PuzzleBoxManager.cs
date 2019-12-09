using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBoxManager : MonoBehaviour {

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
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSimonSuccess()
    {
        SimonSuccess = true;
        hiddenSimonMulti.SetActive(true);
        lockedSimon.SetActive(false);
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
        lockedLabyrinth.SetActive(false);
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
}
