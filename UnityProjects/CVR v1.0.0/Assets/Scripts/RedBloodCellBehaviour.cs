using UnityEngine;
using System.Collections;

public class RedBloodCellBehaviour : MonoBehaviour {

    MoveToTarget mtt;
    FollowWaypoints FW;
	// Use this for initialization
	void Start () {
        mtt = GetComponent<MoveToTarget>();
        FW = GetComponent<FollowWaypoints>();
        mtt.moving = true;
	}
	
	// Update is called once per frame
	void Update () {
        mtt.vecTarget = FW.WpTarget_v3;
	
	}
}
