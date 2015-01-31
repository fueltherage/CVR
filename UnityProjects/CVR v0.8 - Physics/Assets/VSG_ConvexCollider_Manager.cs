using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class VSG_ConvexCollider_Manager : MonoBehaviour {

	//THis Script is reliant on its parent being a VoxelSystemGreedy
	List<GameObject> ConvexColliders;
	GameObject entity; 
	VoxelSystemGreedy vs;
	bool init = false;
	// Use this for initialization
	void Start () {
		ConvexColliders = new List<GameObject>();
		vs = transform.parent.GetComponent<VoxelSystemGreedy>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!init)
		{
			for (int x = 0; x < vs.XSize; x++) {
				for (int y = 0; y < vs.YSize; y++) {
					for (int z = 0; x < vs.ZSize; z++) {
						GameObject cc = new GameObject(vs.chunks[x,y,z].name+ " ConvexCollider");
						MeshCollider meshCol = 	cc.AddComponent<MeshCollider>();	
						ConvexColliderUtils ccUtil = cc.AddComponent<ConvexColliderUtils>();
						ccUtil.chunk = vs.chunks_vcs[x,y,z];
						meshCol.convex = true;
						ConvexColliders.Add(cc);
					}					
				}
			}
		}	
	}
}
