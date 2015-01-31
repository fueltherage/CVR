using UnityEngine;
using System.Collections;

public class ShootBall : MonoBehaviour {
	public GameObject ball;
	public float force;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			GameObject newBall = Instantiate(ball, this.transform.position, transform.rotation) as GameObject;
			newBall.rigidbody.velocity = this.transform.forward*force;
		}
	}
}
