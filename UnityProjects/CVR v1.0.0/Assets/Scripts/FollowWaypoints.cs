using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowWaypoints : MonoBehaviour {


    public List<GameObject> waypoints;
    public int StartingIndex = 0;
    MoveToTarget mtt; 
    public int current;
 
    Rigidbody rb;
    float delay = 2.0f;
    
    float range = 10.0f;

    public Vector3 WpTarget_v3; 

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        current = StartingIndex;
        mtt = GetComponent<MoveToTarget>();
        mtt.moveType = MoveToTarget.MovementType.WorldPos;
        WpTarget_v3 = waypoints[current].transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, -1.0f), Random.Range(-1.0f, -1.0f))*range;
      
	}
	
	// Update is called once per frame
	void FixedUpdate () {
         
	}
    void OnTriggerEnter(Collider c)
    {
        if (c.name == waypoints[current].name)
        {
            current++;
            if (current >= waypoints.Count) current = 0;
            WpTarget_v3 = waypoints[current].transform.position + new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f))*range;
        }
    }
    
}
