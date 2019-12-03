using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour {

    public string direction;

    public bool pressed;
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
            pressed = true;
            //controllerInside++;
            FindObjectOfType<LabyrinthManager>().HandleArrowButton(direction);
            
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            if(pressed)
            {
                FindObjectOfType<LabyrinthManager>().StopPlayer(direction);
                pressed = false;
            }
            Debug.Log("EXIT: MultiButton ");
        }
    }

}
