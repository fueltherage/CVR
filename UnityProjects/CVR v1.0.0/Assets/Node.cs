using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {

	public VoxelSystemGreedy vs;
	public System.Action f_updateCalls;
	public System.Action s_updateCalls;
	// Use this for initialization
	void Start () {
		f_updateCalls += go;
		s_updateCalls += go;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void UpdateNode()
	{
		VoxelThreads.QueueOnMainThread(f_updateCalls);
		VoxelThreads.QueueOnMainThread(s_updateCalls);
	}
	void go()
	{
	}
}
