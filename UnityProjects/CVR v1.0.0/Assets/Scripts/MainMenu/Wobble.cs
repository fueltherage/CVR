using UnityEngine;
using System.Collections;

public class Wobble : MonoBehaviour {

	public float changeInterval = 1;
	public float TorqueForce = 1;
	public float elapsedTime=0;
	public float RandomRange = 1000;

	void Start () {
		GetComponent<Rigidbody>().inertiaTensor.Set(0.5f,0.5f,0.5f);
		AddRandomTorque();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.fixedDeltaTime;
		if(elapsedTime >= changeInterval)
		{
			AddRandomTorque();
			elapsedTime =0;
		}	
	}
	void AddRandomTorque()
	{
		GetComponent<Rigidbody>().AddTorque((Random.Range(-RandomRange,RandomRange)/RandomRange)*TorqueForce, 
		                    (Random.Range(-RandomRange,RandomRange)/RandomRange)*TorqueForce, 
		                    (Random.Range(-RandomRange,RandomRange)/RandomRange)*TorqueForce);
	}
}
