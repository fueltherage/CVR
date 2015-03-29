using UnityEngine;
using System.Collections;

public class DebugLine : MonoBehaviour {

	// Use this for initialization
	
    void Start () {
        GetComponent<LineRenderer>().enabled = false;
        Destroy(this);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
