﻿using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    
	public KeyCode Exitkey = KeyCode.Escape;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(Exitkey))
		{
			Application.Quit();
		}
	}
}
