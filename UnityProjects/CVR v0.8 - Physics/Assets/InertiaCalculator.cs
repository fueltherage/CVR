using UnityEngine;
using System.Collections;

public class InertiaCalculator : MonoBehaviour {

    public Vector3 Inertia;
	// Use this for initialization
	void Awake () {
        if(rigidbody != null)rigidbody.inertiaTensor = Inertia;
	}
	
	// Update is called once per frame
	void Update () {

        if(rigidbody != null)rigidbody.inertiaTensor = Inertia;
	}
}
