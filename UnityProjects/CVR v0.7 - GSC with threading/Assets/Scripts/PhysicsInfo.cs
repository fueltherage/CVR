using UnityEngine;
using System.Collections;

public class PhysicsInfo : MonoBehaviour {

	VoxelSystemGreedy vs;
	Vector3 CenterOfMass;
	void Start () {
		vs = transform.FindChild("VoxelSystemGreedy").gameObject.GetComponent<VoxelSystemGreedy>();
	}
	void Update()
	{

	}
	void UpdateCOM()
	{

		for (int x = 0; x < vs.XSize; x++){
			for (int y = 0; y < vs.YSize; y++){
				for (int z = 0; z < vs.ZSize; z++){
					for (int xc = 0; xc < vs.ChunkSizeX; xc++){
						for (int yc = 0; yc < vs.ChunkSizeY; yc++){
							for (int zc = 0; zc < vs.ChunkSizeZ; zc++){
								if(vs.chunks_vcs[x,y,z].blocks[xc,yc,zc].filled)
								{

								}
							}
						}
					}
				}
			}
		}

	}



}
