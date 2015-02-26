using UnityEngine;
using System.Collections;

public class VoxRamp : MonoBehaviour {
	VoxelSystemGreedy vc;
	VoxSystemChunkManager vsum;
	// Use this for initialization
	void Start () {
		vc = this.gameObject.GetComponent<VoxelSystemGreedy>();	
		vsum = GetComponent<VoxSystemChunkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(vc.Initialized && VoxelThreads._initialized)
		{
			Ramp();
			Destroy(this);
		}

	
	}
	void Ramp()
	{
		int count = vc.YSize * vc.ChunkSizeY;
		Debug.Log("Ramping");
		for (int x = 0; x < vc.XSize; x++){
			for (int y = 0; y < vc.YSize; y++){
				for (int z = 0; z < vc.ZSize; z++){
					for (int xc = 0; xc < vc.ChunkSizeX; xc++){
						for (int yc = 0; yc < vc.ChunkSizeY; yc++){
							for (int zc = 0; zc < vc.ChunkSizeZ; zc++){	
								VoxelPos vp = new VoxelPos(x*vc.ChunkSizeX + xc, 
								                           y*vc.ChunkSizeX + yc, 
								                           z*vc.ChunkSizeX + zc);
								if(vp.y <= count)
								vsum.AddVoxel(vp,false,1);
							}
						}
						count--;
					}
				}
			}

		}
		vc.UpdateMeshes();
}
}
