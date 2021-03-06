﻿using UnityEngine;
using System.Collections;

public class SphereRemoval : MonoBehaviour {

	VoxelSystemGreedy vs;
	VoxSystemChunkManager vcm;
	bool init = false;
	Node node;
	public int radius;
    public bool pureVoxel = true;
	// Use this for initialization
	void Start () {
		node = GetComponent<Node>();
		vs = node.vs;
		vcm = node.vs.GetComponent<VoxSystemChunkManager>();
		node.first_updateCalls += RemoveArea;
	}
	
	// Update is called once per frame
	void Update () {
//		if(!init)
//		{
//			if(vs.Initialized)
//			{
//				RemoveArea();
//				init=true;
//			}
//		}
	}
	void RemoveArea()
	{
        //vcm.AddVoxelAoE(this.transform.position, radius + (int)(2 * vs.VoxelSpacing), 4, true);
        vcm.QuickRemoveVoxelAoE(this.transform.position, radius, pureVoxel);
        //vcm.RemoveVoxelAoE(this.transform.position, radius, true);
	}
}
