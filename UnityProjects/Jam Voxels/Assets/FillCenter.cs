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
			VoxelFactory.GenerateVoxel(1,ref vs.chunks_vcs[vs.XSize/2,vs.YSize/2, vs.ZSize/2].blocks[vs.ChunkSizeX/2, vs.ChunkSizeY/2, vs.ChunkSizeZ/2],vs.offset,vs.VoxelSpacing);
			init = true;
			this.enabled = false;
		}
		
	}
}
