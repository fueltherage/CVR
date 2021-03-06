﻿using UnityEngine;
using System.Collections;

public class RigidBodyController : MonoBehaviour {

	public float ForceSpeed = 10;
	bool gravity = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T))
		{   gravity = !gravity;
			rigidbody.useGravity = gravity;
		}
		if(Input.GetKey (KeyCode.A))
		{
			this.rigidbody.AddForce(-this.transform.right * ForceSpeed);
		}
		if(Input.GetKey (KeyCode.D))
		{
			this.rigidbody.AddForce(this.transform.right * ForceSpeed);
		}
		if(Input.GetKey (KeyCode.W))
		{
			this.rigidbody.AddForce(this.transform.forward * ForceSpeed);
		}
		if(Input.GetKey (KeyCode.S))
		{
			this.rigidbody.AddForce(-this.transform.forward * ForceSpeed);
		}	
	}
}
