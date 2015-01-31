using UnityEngine;
using System.Collections;

public class PhysicsRelay : MonoBehaviour {

	Rigidbody rid;
	PhysicsInfo info;
	// Use this for initialization
	void Start () {
		rid = transform.parent.gameObject.GetComponent<Rigidbody>();
		info =  transform.parent.gameObject.GetComponent<PhysicsInfo>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter(Collision hit)
	{
		if(hit.gameObject.tag == "Chunk")
		{

		}
	}
}
