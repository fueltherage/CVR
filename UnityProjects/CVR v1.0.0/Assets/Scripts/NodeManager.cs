﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour {

	// Use this for initialization
	public VoxelSystemGreedy vs;
	Node[] nodes;
	void Start () {
		nodes = GetComponentsInChildren<Node>();
		UpdateNodeSystemRefs();
      

	}
	public void UpdateNodeSystemRefs()
	{
		for(int i =0; i<nodes.Length;i++)
		{
			nodes[i].vs = vs;
		}
	}
	public void UpdateNodes()
	{
		for(int i =0; i<nodes.Length;i++)
		{
			nodes[i].first_updateCalls();
		}
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].second_updateCalls();
        }
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].third_updateCalls();
        }
        vs.UpdateMeshes();
	}

	
	// Update is called once per frame
	void Update () {
		if(VoxelThreads.Current.ActiveThreads == 0)
		{
			UpdateNodes ();
            Destroy(this);
		}
	}
}
