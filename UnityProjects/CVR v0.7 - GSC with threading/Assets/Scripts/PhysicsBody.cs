using UnityEngine;
using System.Collections;

public class PhysicsBody : MonoBehaviour {
	Vector3 velocity;
	Vector3 acceleration;
	Vector3 rotVel;
	Vector3 rotAcc;
	VoxelSystemGreedy vs;


	// Use this for initialization
	void Start () {
		velocity = new Vector3();
		acceleration= new Vector3();
		rotVel= new Vector3();
		rotVel.x =1.0f;
		rotAcc = new Vector3();
		vs = GetComponent<VoxelSystemGreedy>();
	
	}	
	// Update is called once per frame
	void Update () {
		Vector3 displacement = velocity * Time.deltaTime + 0.5f * acceleration * Time.deltaTime * Time.deltaTime;
		velocity += acceleration * Time.deltaTime;
		this.transform.Translate(displacement);


		Vector3 rotDisplacment = rotAcc * Time.deltaTime + 0.5f * rotAcc * Time.deltaTime * Time.deltaTime;
		rotVel += rotAcc * Time.deltaTime;
	
		//q(t + ∆t) = q(t) + ∆t/2 ωq(t)
		Quaternion rotVelQ = new Quaternion(rotVel.x, rotVel.y, rotVel.z,0);

		Quaternion result;
		result.x = this.transform.rotation.x + Time.deltaTime / 2.0f * rotVelQ.x * this.transform.rotation.x;
		result.y = this.transform.rotation.y + Time.deltaTime / 2.0f * rotVelQ.y * this.transform.rotation.y;
		result.z = this.transform.rotation.z + Time.deltaTime / 2.0f * rotVelQ.z * this.transform.rotation.z;
		result.w = this.transform.rotation.w + Time.deltaTime / 2.0f * rotVelQ.w * this.transform.rotation.w;
		this.transform.rotation = result;

	}
	void CalculateCOM()
	{
	}
	void CalculateIntertia()
	{
	}
	void AddTorque()
	{
	}
	void AddForce(Vector3 _direction, float force)
	{

	}
	void AddForce(Vector3 _position, Vector3 _direction, float force)
	{
		
	}
}
