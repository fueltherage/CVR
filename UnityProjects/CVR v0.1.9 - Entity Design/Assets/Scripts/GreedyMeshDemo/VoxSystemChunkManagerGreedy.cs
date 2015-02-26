using UnityEngine;
using System.Collections;

public class VoxSystemChunkManagerGreedy : MonoBehaviour {

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
        difference -= chunk.offset;
        difference /= chunk.VoxelSpacing;
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
    public void AddVoxel(RaycastHit Pos, bool update)
    {
        Vector3 difference = Pos.point - chunk.transform.position;
        difference -= chunk.offset;
        difference /= chunk.VoxelSpacing;
        difference += Pos.normal * chunk.VoxelSpacing / 2.0f;
		difference -= chunk.chunks_vcs[0,0,0].offset;

		chunk.AddVoxel(new VoxelPos(difference), update);
    }
	public void RemoveVoxel(RaycastHit Pos, bool update)
    {
        Vector3 difference = Pos.point - chunk.transform.position;
        difference -= chunk.offset;
        difference /= chunk.VoxelSpacing;
        difference -= Pos.normal * chunk.VoxelSpacing / 2.0f;   
		difference -= chunk.chunks_vcs[0,0,0].offset;

		chunk.RemoveVoxel(new VoxelPos(difference), update);
    }
}
