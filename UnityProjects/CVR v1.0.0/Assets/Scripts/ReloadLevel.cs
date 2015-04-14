using UnityEngine;
using System.Collections;

public class ReloadLevel : MonoBehaviour {

    public KeyCode key;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(key))
        {
            Application.LoadLevel(0);
        }
	}
}
