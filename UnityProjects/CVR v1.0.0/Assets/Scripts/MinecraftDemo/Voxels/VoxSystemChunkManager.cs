using UnityEngine;
using System.Collections;



public class VoxSystemChunkManager : MonoBehaviour {

	VoxelSystemGreedy vSystem;
    
    
	// Use this for initialization
	void Start () {    
        vSystem = GetComponent<VoxelSystemGreedy>();        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public Vector3 WorldToVoxelAdd(RaycastHit Pos)
    {
        Vector3 difference = Pos.point - vSystem.transform.position;
        difference += Pos.normal /2.0f;
        difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= vSystem.offset;
        difference -= vSystem.chunks_vcs[0, 0, 0].offset;
        difference /= vSystem.VoxelSpacing;
        return difference;
    }
    public Vector3 WorldToVoxelAdd(Vector3 Pos)
    {
        Vector3 difference = Pos - vSystem.transform.position;       
        difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= vSystem.offset;
        difference -= vSystem.chunks_vcs[0, 0, 0].offset;
        difference /= vSystem.VoxelSpacing;
        return difference;
    }
    public Vector3 WorldToVoxelRemove(RaycastHit Pos)
    {
        Vector3 difference = Pos.point - vSystem.transform.position;
        difference -= Pos.normal / 2.0f;
        difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= vSystem.offset;
        difference -= vSystem.chunks_vcs[0, 0, 0].offset;
        difference /= vSystem.VoxelSpacing;
        return difference;
    }
    public Vector3 WorldToVoxelRemove(Vector3 Pos)
    {
        Vector3 difference = Pos - vSystem.transform.position;
        difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= vSystem.offset;
        difference -= vSystem.chunks_vcs[0, 0, 0].offset;
        difference /= vSystem.VoxelSpacing;
        return difference;
    }

    public void QuickAdd(RaycastHit Pos, int type, bool update)
    {
        vSystem.QuickAdd(new VoxelPos(WorldToVoxelAdd(Pos)), type, update);
    }
    public void QuickRemove(RaycastHit Pos, bool update)
    { 
        vSystem.QuickRemove(new VoxelPos(WorldToVoxelRemove(Pos)), update);
    }
    public void AddVoxel(Vector3 Pos, bool update)
	{
        vSystem.AddVoxel(new VoxelPos(WorldToVoxelAdd(Pos)), update);
	}
    public void AddVoxel(Vector3 Pos, bool update, int type)
    {        
        vSystem.AddVoxel(new VoxelPos(WorldToVoxelAdd(Pos)), update, type);
    }
	public void RemoveVoxel(Vector3 Pos, bool update)
    { 
		vSystem.RemoveVoxel(new VoxelPos(WorldToVoxelRemove(Pos)), update);
    }
	public void AddVoxel(VoxelPos Pos, bool update,int type)
	{
		vSystem.AddVoxel(Pos,update,type);
	}
    public void AddVoxel(VoxelPos Pos, bool update)
    {
		vSystem.AddVoxel(Pos,update);
    }
	public void RemoveVoxel(VoxelPos Pos, bool update)
    {
		vSystem.RemoveVoxel(Pos,update);
    }
	public void RemoveVoxelAoE(RaycastHit Pos, int radius, bool PureVoxel)
	{
		Vector3 difference = Pos.point;
		difference -= Pos.normal * vSystem.VoxelSpacing / 2.0f;   
		RemoveVoxelAoE(difference, radius, PureVoxel);
	}
    public void AddVoxelAoE(RaycastHit Pos, int radius, bool PureVoxel, int type)
    {
        Vector3 difference = WorldToVoxelAdd(Pos);        
        Vector3 offset;
      
        if (PureVoxel)
            radius = (int)(radius / vSystem.VoxelSpacing);

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    offset.x =x;
                    offset.y =y;
                    offset.z =z;

                    if (offset.magnitude <= radius)
                    {
						AddVoxel(new VoxelPos(difference + (offset  / 1.42f)), true, type);
					}
				}   
            }   
        }
    }
    public void RemoveVoxelAoE(Vector3 Pos, int radius, bool PureVoxel)
    {  
        Vector3 offset;
        Vector3 difference = WorldToVoxelRemove(Pos);
        
        if (PureVoxel)
            radius = (int)(radius / vSystem.VoxelSpacing);
     

        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    offset.x =x;
                    offset.y =y;
                    offset.z =z;
                    if (offset.magnitude <= radius)
                    {
                        RemoveVoxel(new VoxelPos(difference + (offset / 1.42f)), true);
					}
				}   
            }   
        }
    }
    public void AddVoxelAoE(Vector3 Pos, int radius,int type, bool PureVoxel)
    {     
        Vector3 offset;    
        Vector3 difference = WorldToVoxelAdd(Pos);
   
        if (PureVoxel)
            radius = (int)(radius / vSystem.VoxelSpacing);


        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                for (int z = -radius; z <= radius; z++)
                {
                    offset.x =x;
                    offset.y =y;
                    offset.z =z;
                    
                    if(offset.magnitude <= radius)
                    {
                        AddVoxel(new VoxelPos(difference + (offset / 1.42f)), true, type);
                    }
                }   
            }   
        }
    }

    public void AddVoxel(RaycastHit Pos, bool update)
    {       
		vSystem.AddVoxel(new VoxelPos(WorldToVoxelAdd(Pos)), update);
    }
	public void RemoveVoxel(RaycastHit Pos, bool update)
    {
		vSystem.RemoveVoxel(new VoxelPos(WorldToVoxelRemove(Pos)), update);
    }
}
