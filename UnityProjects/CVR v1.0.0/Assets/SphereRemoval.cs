using UnityEngine;
using System.Collections;

public class SphereRemoval : MonoBehaviour {

	VoxelSystemGreedy vs;
	VoxSystemChunkManager vcm;
	bool init = false;
	Node node;
	public int radius;
	// Use this for initialization
	void Start () {
		node = GetComponent<Node>();
		vs = node.vs;
		vcm = node.vs.GetComponent<VoxSystemChunkManager>();
		node.f_updateCalls += RemoveArea;
	}
	
	// Update is called once per frame
	void Update () {
//		if(!init)
//		{
//			if(vs.Initialized)
//			{
//				RemoveArea();
//				init=true;
//			}
//		}
	}
	void RemoveArea()
	{
		vcm.RemoveVoxelAoE(this.transform.position, radius);
	}
}
