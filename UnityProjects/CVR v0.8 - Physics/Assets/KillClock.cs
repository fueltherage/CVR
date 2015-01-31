using UnityEngine;
using System.Collections;

public class KillClock : MonoBehaviour {
	public float KillTimer = 1.0f;
	// Use this for initialization
	void Start () {
		Destroy(this.gameObject, KillTimer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
