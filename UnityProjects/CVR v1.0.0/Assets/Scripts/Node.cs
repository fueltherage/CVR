using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {
   
	public VoxelSystemGreedy vs;
	public System.Action first_updateCalls;
	public System.Action second_updateCalls;
    public System.Action third_updateCalls;
	// Use this for initialization
	void Start () {
        first_updateCalls += go;
        second_updateCalls += go;
        third_updateCalls += go;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void go()
	{
	}
}
