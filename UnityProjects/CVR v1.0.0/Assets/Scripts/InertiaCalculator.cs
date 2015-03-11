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
        if(GetComponent<Rigidbody>() != null)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
            GetComponent<Rigidbody>().centerOfMass = Vector3.zero;
            GetComponent<Rigidbody>().inertiaTensor = Inertia;
        }
    }
	void Update () {


	}
}
