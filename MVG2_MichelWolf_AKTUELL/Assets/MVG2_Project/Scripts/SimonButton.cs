using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SimonButton : MonoBehaviour
{ 
    public int buttonID;
    public bool interactable = false;
    private MeshRenderer simonButtonMR;
    public Color normalColor;
    public Color highlightColor;
    void Start()
    {
        if (gameObject.GetComponent<MeshRenderer>() != null)
        {
            simonButtonMR = gameObject.GetComponent<MeshRenderer>();
            simonButtonMR.material.color = normalColor;
        }
        else
        {
            Debug.Log("SimonButton: Mesh Renderer missing");
        }
    }

    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {

        //TODO:: State if button active or inactive instead of enabling/disabling colliders
        if (other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            Debug.Log("ENTER: SimonButton " + buttonID);
            gameObject.GetComponent<MeshRenderer>().material.color = highlightColor;
            if (interactable == true)
            {
                FindObjectOfType<SimonSaysManager>().OnGameButtonClick(buttonID);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "ControllerColliderL" || other.gameObject.tag == "ControllerColliderR")
        {
            Debug.Log("EXIT: SimonButton " + buttonID);
            gameObject.GetComponent<MeshRenderer>().material.color = normalColor;
        }
    }
}
