using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiButton : MonoBehaviour 
{
    
    public int controllerInside = 0;

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
            controllerInside++;
            Debug.Log("ENTER: MultiButton ");
            if (controllerInside == 1)
            {
                FindObjectOfType<MultiButtonInteractionManager>().OnMultiButtonClick(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            Debug.Log("EXIT: MultiButton ");
            controllerInside--;
            if(controllerInside == 0)
            {
                FindObjectOfType<MultiButtonInteractionManager>().OnMultiButtonClick(false);
            }
        }
    }

}
