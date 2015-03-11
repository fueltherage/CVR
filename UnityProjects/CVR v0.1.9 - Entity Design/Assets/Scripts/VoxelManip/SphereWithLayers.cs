using UnityEngine;
using System.Collections;

public class SphereWithLayers : MonoBehaviour {

	// Use this for initialization
    public int layers;
    public float radius = 5.0f;
	public float LayerSize = 4.0f;
    VoxelSystemGreedy vc;
    Vector3 offset;
    bool init = false;

	void Start () {
        vc = GetComponent<VoxelSystemGreedy>();
        offset = new Vector3(vc.XSize*vc.ChunkSizeX,
                             vc.YSize*vc.ChunkSizeY,
                             vc.ZSize*vc.ChunkSizeZ);
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
                                        p -= offset/2.0f;
                                        int VoxType=0;
										if(p.magnitude < LayerSize)
                                            VoxType = 1;
										else if(p.magnitude < LayerSize * 2)
                                            VoxType = 2;
										else if(p.magnitude < LayerSize * 3)
                                            VoxType = 3;
                                       
                                        if(p.magnitude < radius/vc.VoxelSpacing){
											VoxelFactory.GenerateVoxel(VoxType, ref vc.chunks_vcs[x,y,z].blocks[xc, yc, zc], vc.offset, vc.VoxelSpacing);

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
