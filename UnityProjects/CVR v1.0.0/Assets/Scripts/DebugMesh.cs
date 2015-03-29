using UnityEngine;
using System.Collections;

public class DebugMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy(gameObject.GetComponent<MeshRenderer>());
		Destroy(gameObject.GetComponent<MeshFilter>());
        Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
