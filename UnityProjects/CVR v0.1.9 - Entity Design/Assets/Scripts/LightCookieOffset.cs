using UnityEngine;
using System.Collections;

public class LightCookieOffset : MonoBehaviour {

	public Vector3 offsetSpeed = new Vector3(0.0f,1.0f,0.0f);
	// Use this for initialization
	Light light;
	void Start () {
		light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {			
		float offsetReset = light.cookieSize * 1.55f;
		if(transform.position.y >= offsetReset) transform.Translate(new Vector3(0.0f,-transform.position.y, 0.0f), Space.World);
		else this.transform.Translate(offsetSpeed, Space.World);
	
	}
}
