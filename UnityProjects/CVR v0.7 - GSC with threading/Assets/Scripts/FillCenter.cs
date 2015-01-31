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
		{
			if(vs.Initialized)
			{
				vs.AddVoxel(new VoxelPos(vs.XSize/2 * vs.ChunkSizeX + vs.ChunkSizeX/2,
				                         vs.YSize/2 * vs.ChunkSizeY + vs.ChunkSizeY/2,
				                         vs.ZSize/2 * vs.ChunkSizeZ + vs.ChunkSizeZ/2),true);
				init = true;
				Destroy(GetComponent<FillCenter>());

			}
		}
		
	}
}
