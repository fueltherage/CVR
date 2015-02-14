using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

	public GameObject target;
	public float catchupSpeed = 0.1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 difference;
		difference.x = Mathf.Lerp(transform.position.x, target.transform.position.x, catchupSpeed);
		difference.y = Mathf.Lerp(transform.position.y, target.transform.position.y, catchupSpeed);
		difference.z = Mathf.Lerp(transform.position.z, target.transform.position.z, catchupSpeed);
		transform.position = difference;
	
	}
}
