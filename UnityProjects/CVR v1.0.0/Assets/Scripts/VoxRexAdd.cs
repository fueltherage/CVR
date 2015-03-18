using UnityEngine;
using System.Collections;

public class VoxRexAdd : MonoBehaviour {

	VoxelSystemGreedy vs;
	VoxSystemChunkManager vcm;
	bool init = false;
	Node node;
	public int xR,yR,zR;
	public int type = 1;

	// Use this for initialization
	void Start () {
		node = GetComponent<Node>();
		vs = node.vs;
		vcm = node.vs.GetComponent<VoxSystemChunkManager>();
		node.s_updateCalls += AddArea;
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
	void AddArea()
	{
		for(int x = -xR; x < xR; x++)
			for(int y = -yR; y < yR; y++)
				for(int z = -zR; z < zR; z++)
			{
				vcm.AddVoxel(this.transform.position + new Vector3(x,y,z),false,type);
			}	
	}
}
