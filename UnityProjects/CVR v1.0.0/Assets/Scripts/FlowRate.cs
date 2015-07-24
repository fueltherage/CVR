using UnityEngine;
using System.Collections;

public class FlowRate : MonoBehaviour {

	public float speed = 1.0f;
	public float amplitude = 0.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		MoveToTarget.forceMultiplier = amplitude * Mathf.Sin(Time.timeSinceLevelLoad*speed) + 1.0f;
	}
}
