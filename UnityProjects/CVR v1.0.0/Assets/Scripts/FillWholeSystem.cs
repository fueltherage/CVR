﻿using UnityEngine;
using System.Collections;

public class FillWholeSystem : MonoBehaviour {
	VoxelSystemGreedy vc;
	VoxSystemChunkManager vsum;
    public int type = 1;
	bool init = false;
    public bool update = true;
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
								vsum.QuickAdd(new VoxelPos(x * vc.ChunkSizeX + xc, 
								                           y * vc.ChunkSizeY + yc,
                                                           z * vc.ChunkSizeZ + zc), false, type);
							}
						}
					}
				}
			}
		}
        if (update)
		vc.UpdateMeshes();
        Destroy(this);
	}
}

