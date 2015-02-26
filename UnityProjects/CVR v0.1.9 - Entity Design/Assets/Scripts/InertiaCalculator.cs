using UnityEngine;
using System.Collections;

public class InertiaCalculator : MonoBehaviour {

    public Vector3 Inertia;
	// Use this for initialization
	void Awake () {
        CalcIntertia();
	}
	
	// Update is called once per frame
    public void CalcIntertia()
    {
        if(rigidbody != null)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            rigidbody.centerOfMass = Vector3.zero;
            rigidbody.inertiaTensor = Inertia;
        }
    }
	void Update () {


	}
}
