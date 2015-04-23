using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowWaypoints : MonoBehaviour {

	public GameObject StartingWaypoint;

    MoveToTarget mtt; 
	public GameObject currentTarget;
    float delay = 2.0f;   
    float range = 10.0f;

    public Vector3 WpTarget_v3; 

	// Use this for initialization
	void Start () {

   
        mtt = GetComponent<MoveToTarget>();
        mtt.moveType = MoveToTarget.MovementType.WorldPos;
        WpTarget_v3 = StartingWaypoint.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, -1.0f), Random.Range(-1.0f, -1.0f))*range;
		currentTarget = StartingWaypoint;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
         
	}
    void OnTriggerEnter(Collider c)
    {
        if (c.name == currentTarget.name)
        {

			WpTarget_v3 = c.gameObject.GetComponent<Waypoint>().GetNextTarget(ref currentTarget);
		}
    }
    
}
