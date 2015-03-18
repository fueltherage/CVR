using UnityEngine;
using System.Collections;

public class SphereWithLayers : MonoBehaviour {

	// Use this for initialization
    public int layers;
    public float radius = 5.0f;
	float LayerSize;
    VoxelSystemGreedy vc;
    Vector3 offset;
    bool init = false;

	void Start () {
        vc = GetComponent<VoxelSystemGreedy>();
        offset = new Vector3(vc.XSize*vc.ChunkSizeX,
                             vc.YSize*vc.ChunkSizeY,
                             vc.ZSize*vc.ChunkSizeZ);
		LayerSize = radius / (float)layers;
	}
	
	// Update is called once per frame
	void Update () {
        if(!init)
        {

            if(vc.Initialized)
            {
                Debug.Log("Sphere");    
                for (int x = 0; x < vc.XSize; x++){
                    for (int y = 0; y < vc.YSize; y++){
                        for (int z = 0; z < vc.ZSize; z++){
                            for (int xc = 0; xc < vc.ChunkSizeX; xc++){
                                for (int yc = 0; yc < vc.ChunkSizeY; yc++){
                                    for (int zc = 0; zc < vc.ChunkSizeZ; zc++){
										Vector3 p = new Vector3(x*vc.ChunkSizeX + xc, y*vc.ChunkSizeY + yc, z*vc.ChunkSizeX + zc);
										p += vc.offset;
                                        int VoxType=0;
										for(int i = 1; i<= layers; i++)
										{
											if(p.magnitude < LayerSize * i)
											{
												VoxType = i;
												break;
											}
										}
                                       
                                        if(p.magnitude < radius/vc.VoxelSpacing){
											VoxelFactory.GenerateVoxel(VoxType, ref vc.chunks_vcs[x,y,z].blocks[xc, yc, zc]);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                init = true;
            }
            vc.UpdateMeshes();
        }
	
	}
}
