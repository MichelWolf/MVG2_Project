using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum direction { up, down, left, right};
public class ArrowButton : MonoBehaviour {

    public direction dir;
    public bool pressed;

    public Color normalColor;
    public Color highlightColor;
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
            gameObject.GetComponent<MeshRenderer>().material.color = highlightColor;
            //controllerInside++;
            FindObjectOfType<LabyrinthManager>().HandleArrowButton(dir);
            
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            if(pressed)
            {
                FindObjectOfType<LabyrinthManager>().StopPlayer(dir);
                pressed = false;
                gameObject.GetComponent<MeshRenderer>().material.color = normalColor;
            }
            Debug.Log("EXIT: MultiButton ");
        }
    }

}
