using UnityEngine;
using System.Collections;

public class RandomFill : MonoBehaviour {

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
        }
    }
    void Fill()
    {
        Debug.Log("Random Filling");
        for (int x = 0; x < vc.XSize; x++){
            for (int y = 0; y < vc.YSize; y++){
                for (int z = 0; z < vc.ZSize; z++){
                    for (int xc = 0; xc < vc.ChunkSizeX; xc++){
                        for (int yc = 0; yc < vc.ChunkSizeY; yc++){
                            for (int zc = 0; zc < vc.ChunkSizeZ; zc++){                                                         
                                vsum.AddVoxel(new VoxelPos(x*vc.ChunkSizeX + xc, 
                                                           y*vc.ChunkSizeY + yc, 
                                                           z*vc.ChunkSizeZ + zc),false,Random.Range(1,5));
                            }
                        }
                    }
                }
            }
        }
        vc.UpdateMeshes();
    }
}
