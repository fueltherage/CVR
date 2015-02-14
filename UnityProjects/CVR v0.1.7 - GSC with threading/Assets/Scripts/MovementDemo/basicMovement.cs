using UnityEngine;
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
			Vector3 v = new Vector3(Camera.main.transform.forward.x,0, Camera.main.transform.forward.z);
			v.Normalize();
			this.transform.Translate(v);
		}
		if(Input.GetKey(KeyCode.S))
		{
			Vector3 v = new Vector3(Camera.main.transform.forward.x,0, Camera.main.transform.forward.z);
			v.Normalize();
			this.transform.Translate(-v);
		}

		if(Input.GetKey(KeyCode.A))
		{
			this.transform.Rotate(new Vector3(0,-rotateSpeed,0));
		}if(Input.GetKey(KeyCode.D))
		{
			this.transform.Rotate(new Vector3(0,rotateSpeed,0));
		}




	}
}
