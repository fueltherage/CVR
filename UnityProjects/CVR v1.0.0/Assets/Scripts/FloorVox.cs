using UnityEngine;
using System.Collections;

public class FloorVox : MonoBehaviour {
	VoxelSystemGreedy vc;
	VoxSystemChunkManager vsum;
	public int LayersHigh = 1;
	public int Type =1;
	bool init = false;
	// Use this for initialization
	void Start () {
		vc = this.gameObject.GetComponent<VoxelSystemGreedy>();	
		vsum = GetComponent<VoxSystemChunkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(vc.Initialized && !init && VoxelThreads._initialized)
		{
			init = true;
			Fill();
			Destroy(this);
		}
	}
	void Fill()
	{
		Debug.Log("Filling");
		int height = 1 + LayersHigh/vc.ChunkSizeY;
		for (int x = 0; x < vc.XSize; x++){
			for (int y = 0; y < height; y++){
				for (int z = 0; z < vc.ZSize; z++){
					for (int xc = 0; xc < vc.ChunkSizeX; xc++){
						for (int yc = 0; yc < LayersHigh; yc++){
							for (int zc = 0; zc < vc.ChunkSizeZ; zc++){															
								vsum.AddVoxel(new VoxelPos(x*vc.ChunkSizeX + xc, 
								                           y*vc.ChunkSizeX + yc, 
								                           z*vc.ChunkSizeX + zc),false,Type);
							}
						}
					}
				}
			}
		}
		vc.UpdateMeshes();
	}
}
