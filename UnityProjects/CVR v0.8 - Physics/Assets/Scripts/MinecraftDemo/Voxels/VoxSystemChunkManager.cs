using UnityEngine;
using System.Collections;



public class VoxSystemChunkManager : MonoBehaviour {

	VoxelSystemGreedy chunk;


    
    
	// Use this for initialization
	void Start () {    
        chunk = GetComponent<VoxelSystemGreedy>();        
	}
	
	// Update is called once per frame
	void Update () {
	
	} 

	public void RemoveVoxel(Vector3 Pos, bool update)
    {
       //Remove the voxel that Pos is in.
        Vector3 difference = Pos - chunk.transform.position;
		difference = transform.worldToLocalMatrix.MultiplyVector(difference);
		difference -= chunk.offset/2.0f;
		difference /= chunk.VoxelSpacing;        
		difference -= chunk.chunks_vcs[0,0,0].offset;
		chunk.RemoveVoxel(new VoxelPos(difference.x,difference.y,difference.z), update);
    }
	public void AddVoxel(VoxelPos Pos, bool update,int type)
	{
		chunk.AddVoxel(Pos,update,type);
	}
    public void AddVoxel(VoxelPos Pos, bool update)
    {
		chunk.AddVoxel(Pos,update);
    }
	public void RemoveVoxel(VoxelPos Pos, bool update)
    {
		chunk.RemoveVoxel(Pos,update);
    }
	public void RemoveVoxelAoE(RaycastHit Pos, int radius)
	{
		Vector3 difference = Pos.point;

		difference -= Pos.normal / 2.0f;   

		for (int x = -radius; x < radius; x++) {
			for (int y = -radius; y < radius; y++) {
				for (int z = -radius; z < radius; z++) {
					Vector3 offset = new Vector3(x,y,z);

					if(offset.magnitude < radius)
					{
						RemoveVoxel(difference + offset/1.5f, true);
					}
				}	
			}	
		}


	}



    public void AddVoxel(RaycastHit Pos, bool update)
    {
        Vector3 difference = Pos.point - chunk.transform.position;
		difference += Pos.normal * chunk.VoxelSpacing / 2.0f;
		difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= chunk.offset/2.0f;
        difference /= chunk.VoxelSpacing;        
		difference -= chunk.chunks_vcs[0,0,0].offset;
		chunk.AddVoxel(new VoxelPos(difference), update);
    }
	public void RemoveVoxel(RaycastHit Pos, bool update)
    {
        Vector3 difference = Pos.point - chunk.transform.position;
		difference -= Pos.normal / 2.0f;   
		difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= chunk.offset/2.0f;
        difference /= chunk.VoxelSpacing;       
		difference -= chunk.chunks_vcs[0,0,0].offset;
	
		chunk.RemoveVoxel(new VoxelPos(difference), update);
    }
}
