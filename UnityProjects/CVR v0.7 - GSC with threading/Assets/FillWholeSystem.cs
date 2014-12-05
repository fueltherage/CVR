using UnityEngine;
using System.Collections;

public class FillWholeSystem : MonoBehaviour {
	VoxelSystemGreedy vc;

	bool init = false;
	// Use this for initialization
	void Start () {
		vc = this.gameObject.GetComponent<VoxelSystemGreedy>();	
	}
	
	// Update is called once per frame
	void Update () {
		if(vc.Initialized && !init)
		{
			init = true;
			Fill();
		}
	}
	void Fill()
	{
		
	
		for (int x = 0; x < vc.XSize; x++){
			for (int y = 0; y < vc.YSize; y++){
				for (int z = 0; z < vc.ZSize; z++){
					for (int xc = 0; xc < vc.ChunkSizeX; xc++){
						for (int yc = 0; yc < vc.ChunkSizeY; yc++){
							for (int zc = 0; zc < vc.ChunkSizeZ; zc++){															
								VoxelFactory.GenerateVoxel(1, ref vc.chunks_vcs[x,y,z].blocks[xc, yc, zc], vc.offset, vc.VoxelSpacing);								

							}
						}
					}
				}
			}
		}
		vc.UpdateMeshes();
	}
}

