using UnityEngine;
using System.Collections;

public class RigidBodyController : MonoBehaviour {

	public float Force = 10;
	public float Impulse = 10;
	public float maxSpeed = 10;
	public Vector3 velocity;
	public float ForceDampen = 2;
	float currentForce;
	bool gravity = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKeyDown(KeyCode.T))
		{   gravity = !gravity;
			rigidbody.useGravity = gravity;
		}
		velocity = rigidbody.velocity;
		
		if(Input.GetKey (KeyCode.A))
		{
			this.rigidbody.AddForce(new Vector3(-1,0,0) * Impulse, ForceMode.Impulse );
		}

		if(Input.GetKey (KeyCode.D))
		{
			this.rigidbody.AddForce(new Vector3(1,0,0)* Impulse, ForceMode.Impulse);
		}

		if(Input.GetKey (KeyCode.W))
		{
			this.rigidbody.AddForce(new Vector3(0,0,1)* Impulse, ForceMode.Impulse);
		}

		if(Input.GetKey (KeyCode.S))
		{
			this.rigidbody.AddForce(new Vector3(0,0,-1)* Impulse, ForceMode.Impulse );
		}

		if(rigidbody.velocity.magnitude >= maxSpeed) rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;

	}
}
