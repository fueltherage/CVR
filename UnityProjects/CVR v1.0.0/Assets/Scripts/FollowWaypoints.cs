using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowWaypoints : MonoBehaviour {

    SaveLoadVoxels slv;
    public List<GameObject> waypoints;
    public int StartingIndex = 0;
    public float force = 1;
    public float rotationForce = 0;
    public Vector3 RotationAxis;
    
    int current;
    Vector3 target;
    Rigidbody rb;
    float delay = 2.0f;
    
    float range = 10;
    

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        current = StartingIndex;
        target = waypoints[current].transform.position + new Vector3(Random.Range(-range, range), Random.Range(-range, range), Random.Range(-range, range));
        slv = GetComponent<SaveLoadVoxels>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (slv.loaded)
        {
            if (Time.timeSinceLevelLoad > 2.0f)
            {
                Vector3 direction = target - this.transform.position;
                rb.AddForce(direction.normalized * force);
               //rb.AddTorque(RotationAxis * rotationForce);
            }
        }
   
	}
    void OnTriggerEnter(Collider c)
    {
        if (c.name == waypoints[current].name)
        {
            current++;
            if (current >= waypoints.Count) current = 0;
            target = waypoints[current].transform.position + new Vector3(Random.Range(-range,range),Random.Range(-range,range),Random.Range(-range,range));
        }
    }
}
