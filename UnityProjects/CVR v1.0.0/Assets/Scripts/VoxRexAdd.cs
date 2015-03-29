using UnityEngine;
using System.Collections;

public class VoxRexAdd : MonoBehaviour {

	
	VoxSystemChunkManager vcm;
    public int order = 1;
	bool init = false;
	Node node;
	public int xR,yR,zR;
	public int type = 1;

	// Use this for initialization
	void Start () {
		node = GetComponent<Node>();
		
		vcm = node.vs.GetComponent<VoxSystemChunkManager>();
		node.third_updateCalls += AddArea;
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
                
				vcm.QuickAdd(this.transform.position + new Vector3(x,y,z),type,true);
                //vcm.AddVoxel(this.transform.position + new Vector3(x, y, z), true, type);
			}	
	}
}
