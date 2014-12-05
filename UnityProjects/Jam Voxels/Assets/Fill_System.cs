﻿using UnityEngine;
using System.Collections;

public class Fill_System : MonoBehaviour {

	VoxelSystem vc;
	bool init= false;
	// Use this for initialization
	void Start () {
		vc = GetComponent<VoxelSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!init)
		{
			if(vc.Initialized)
			for (int x = 0; x < vc.XSize; x++){
				for (int y = 0; y < vc.YSize; y++){
					for (int z = 0; z < vc.ZSize; z++){
						for (int xc = 0; xc < vc.ChunkSizeX; xc++){
							for (int yc = 0; yc < vc.ChunkSizeY; yc++){
								for (int zc = 0; zc < vc.ChunkSizeZ; zc++)
								{
									vc.AddVoxel(xc + x * vc.ChunkSizeX,yc + y * vc.ChunkSizeY, zc + z * vc.ChunkSizeZ,false,1);
								}
							}
						}
					}
				}
			}
			init = true;
			vc.UpdateMeshes();
		}
	}
}
