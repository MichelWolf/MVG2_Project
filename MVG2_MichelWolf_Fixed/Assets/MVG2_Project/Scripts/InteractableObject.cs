using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            FindObjectOfType<SimonSaysManager>().StartSimonSays();
        }
    }
}
