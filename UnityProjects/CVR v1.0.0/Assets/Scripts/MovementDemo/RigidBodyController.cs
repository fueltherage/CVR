using UnityEngine;
using System.Collections;


public class RigidBodyController : MonoBehaviour {

	public float Force = 10;
	public float Impulse = 10;
	public float maxSpeed = 10;
	public Vector3 velocity;
	public float ForceDampen = 2;
	Rigidbody rigidbody;
	float currentForce;
	bool gravity = false;
	// Use this for initialization
	void Start () {
		rigidbody = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		velocity = GetComponent<Rigidbody>().velocity;


		if(!Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftShift))
		{
			rigidbody.AddForce(-new Vector3(0,1,0) * Impulse, ForceMode.Impulse);			
		}

        if(Input.GetKey(KeyCode.Space))
		{
			rigidbody.AddForce(new Vector3(0,1,0) * Impulse, ForceMode.Impulse);
		}

		if(Input.GetKey (KeyCode.A))
		{
			rigidbody.AddForce(-CameraState.CurrentCamera.transform.right * Impulse, ForceMode.Impulse );
		}

		if(Input.GetKey (KeyCode.D))
		{
			rigidbody.AddForce(CameraState.CurrentCamera.transform.right* Impulse, ForceMode.Impulse);
		}

		if(Input.GetKey (KeyCode.W))
		{
			rigidbody.AddForce(CameraState.CurrentCamera.transform.forward* Impulse, ForceMode.Impulse);
		}

		if(Input.GetKey (KeyCode.S))
		{
			rigidbody.AddForce(-CameraState.CurrentCamera.transform.forward* Impulse, ForceMode.Impulse );
		}

		if(GetComponent<Rigidbody>().velocity.magnitude >= maxSpeed) GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * maxSpeed;

	}
    public void UpSpeed()
    {
        Impulse += 1;
    }
    public void DownSpeed()
    {
        Impulse -= 1;
    }
}
