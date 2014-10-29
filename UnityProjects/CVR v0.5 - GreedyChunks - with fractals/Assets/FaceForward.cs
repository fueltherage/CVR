using UnityEngine;
using System.Collections;

public class FaceForward : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(this.transform.position + this.rigidbody.velocity);
	}
}
