using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ObjectOverVRTracker : MonoBehaviour {

    public GameObject VRTracker;
	// Use this for initialization
	void Start () {
        //VRTracker = GameObject.Find("VRTracker");
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position = VRTracker.gameObject.transform.position + Vector3.up;
	}
}
