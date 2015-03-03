using UnityEngine;
using System.Collections;

public class LightCookieOffset : MonoBehaviour {

	public Vector3 offsetSpeed = new Vector3(0.0f,1.0f,0.0f);
	// Use this for initialization
	public float offsetReset = 100;
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(transform.position.y >= offsetReset) transform.Translate(new Vector3(0.0f,-transform.position.y, 0.0f));
		else this.transform.Translate(offsetSpeed);
	
	}
}
