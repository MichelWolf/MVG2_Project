using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthBorder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {

        //TODO:: State if button active or inactive instead of enabling/disabling colliders
        
        if(other.gameObject.transform.parent.tag == "LabyrinthPlayer")
        {
            FindObjectOfType<LabyrinthManager>().ResetLabyrinth();
        }

        
    }
}
