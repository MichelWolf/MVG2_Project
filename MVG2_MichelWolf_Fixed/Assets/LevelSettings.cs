using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour {

    public bool useAR = true;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
}
