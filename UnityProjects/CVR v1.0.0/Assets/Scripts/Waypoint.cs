using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour {

    public List <GameObject> next;
	public float range = 10.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public Vector3 GetNextTarget(ref GameObject _targetRef)
	{
		_targetRef = next[Random.Range(0, next.Count)];
		return _targetRef.transform.position + new Vector3(Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f))*range;
	}
}
