using UnityEngine;
using System.Collections;

public class DebugMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject.GetComponent<MeshRenderer>());
		Destroy(gameObject.GetComponent<MeshFilter>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
