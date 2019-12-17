using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchVRMode : MonoBehaviour {

    public List<GameObject> VRObjects;
    public List<GameObject> NoVRObjects;

    public bool VRActive = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SwitchVRModeButton()
    {
        VRActive = !VRActive;
        
        foreach(GameObject g in VRObjects)
        {
            g.SetActive(VRActive);
        }

        foreach (GameObject g in NoVRObjects)
        {
            g.SetActive(!VRActive);
        }

    }
}
