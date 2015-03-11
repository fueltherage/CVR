using UnityEngine;
using System.Collections;

public class FillCenter : MonoBehaviour {
	VoxelSystemGreedy vs;
	bool init=false;
	// Use this for initialization
	void Start () {
		vs = gameObject.GetComponent<VoxelSystemGreedy>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!init)
		if(vs.Initialized)
		{
			vs.AddVoxel(new VoxelPos((vs.ChunkSizeX*vs.XSize)/2.0f,
			                         (vs.ChunkSizeY*vs.YSize)/2.0f,
						  			 (vs.ChunkSizeZ*vs.ZSize)/2.0f),true);
			init = true;
			Destroy(this);
		}
		
	}
}
