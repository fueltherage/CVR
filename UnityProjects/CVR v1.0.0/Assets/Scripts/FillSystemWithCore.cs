using UnityEngine;
using System.Collections;

public class FillSystemWithCore : MonoBehaviour {
	VoxelSystemGreedy vc;
	VoxSystemChunkManager vsum;
	
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
		for (int x = 0; x < vc.XSize; x++){
			for (int y = 0; y < vc.YSize; y++){
				for (int z = 0; z < vc.ZSize; z++){
					for (int xc = 0; xc < vc.ChunkSizeX; xc++){
						for (int yc = 0; yc < vc.ChunkSizeY; yc++){
							for (int zc = 0; zc < vc.ChunkSizeZ; zc++){
								if((x==0&&xc==0)||(xc==vc.ChunkSizeX-1&&x==vc.XSize-1)||
								   (y==0&&yc==0)||(yc==vc.ChunkSizeY-1&&y==vc.YSize-1)||
								   (z==0&&zc==0)||(zc==vc.ChunkSizeZ-1&&z==vc.ZSize-1))
								{
									vsum.AddVoxel(new VoxelPos(x*vc.ChunkSizeX + xc, 
									                           y*vc.ChunkSizeX + yc, 
									                           z*vc.ChunkSizeX + zc),false,1);
								}else vsum.AddVoxel(new VoxelPos(x*vc.ChunkSizeX + xc, 
								                                 y*vc.ChunkSizeX + yc, 
								                                 z*vc.ChunkSizeX + zc),false,2);
							}
						}
					}
				}
			}
		}
		vc.UpdateMeshes();
	}
}
