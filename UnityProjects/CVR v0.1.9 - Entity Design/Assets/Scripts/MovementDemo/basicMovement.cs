﻿using UnityEngine;
using System.Collections;

public class basicMovement : MonoBehaviour {

	// Use this for initialization
	public float rotateSpeed = 1;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKey(KeyCode.W))
		{
			Vector3 v = new Vector3(CameraState.CurrentCamera.transform.forward.x,0, CameraState.CurrentCamera.transform.forward.z);
			v.Normalize();
			this.transform.Translate(v);
		}
		if(Input.GetKey(KeyCode.S))
		{
			Vector3 v = new Vector3(CameraState.CurrentCamera.transform.forward.x,0, CameraState.CurrentCamera.transform.forward.z);
			v.Normalize();
			this.transform.Translate(-v);
		}

		if(Input.GetKey(KeyCode.Space))		 
		{
			if(Input.GetKey(KeyCode.LeftShift))
			{
				this.transform.Translate(new Vector3(0,-0.5f,0));
			}else this.transform.Translate(new Vector3(0,0.5f,0));
		}


		if(Input.GetKey(KeyCode.A))
		{
			this.transform.Translate(-CameraState.CurrentCamera.transform.right);
		}
		
		if(Input.GetKey(KeyCode.D))
		{
			this.transform.Translate(CameraState.CurrentCamera.transform.right);
		}

//		else
//		{
//			if(Input.GetKey(KeyCode.A))
//			{
//				this.transform.Rotate(new Vector3(0,-rotateSpeed,0));
//			}
//			
//			if(Input.GetKey(KeyCode.D))
//			{
//				this.transform.Rotate(new Vector3(0,rotateSpeed,0));
//			}
//		}
	}
}
