using UnityEngine;
using System.Collections;

public class FillSystemRemoveCenter : MonoBehaviour {


	VoxelSystemGreedy vc;
	VoxSystemChunkManager vsum;
	public int type = 1; 
	public int xR,yR,zR;
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
			//Destroy(this);
		}
	}
	void Fill()
	{
		Debug.Log("Filling");
		for (int x = 0; x < vc.XSize; x++){
			for (int y = 0; y < vc.YSize; y++){
				for (int z = 0; z < vc.ZSize; z++){
					for (int xc = 0; xc < vc.ChunkSizeX; xc++){
						for (int yc = 0; yc < vc.ChunkSizeY; yc++){
							for (int zc = 0; zc < vc.ChunkSizeZ; zc++){															
								vsum.AddVoxel(new VoxelPos(x*vc.ChunkSizeX + xc, 
								                           y*vc.ChunkSizeY + yc, 
								                           z*vc.ChunkSizeZ + zc),false,type);
							}
						}
					}
				}
			}
		}
		for (int x = -xR; x <= xR; x++){
			for (int y = -yR; y <= yR; y++){
				for (int z = -zR; z <= zR; z++){
					vsum.RemoveVoxel(new VoxelPos((vc.ChunkSizeX*vc.XSize)/2.0f + x,
						                          (vc.ChunkSizeY*vc.YSize)/2.0f + y,
					                              (vc.ChunkSizeZ*vc.ZSize)/2.0f + z), false);		
				}
			}
		}
		vc.UpdateMeshes();
	}
}
		
