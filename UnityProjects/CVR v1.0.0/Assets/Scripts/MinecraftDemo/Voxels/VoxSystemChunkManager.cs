﻿using UnityEngine;
using System.Collections;



public class VoxSystemChunkManager : MonoBehaviour {

	public VoxelSystemGreedy vSystem;
    Vector3 systemOffset;
    bool init = false;
    
    
	// Use this for initialization
	void Start () {    
        vSystem = GetComponent<VoxelSystemGreedy>();        
	}
	
	// Update is called once per frame
	void Update () {
        if(!init)
        {
            if (vSystem.Initialized)
            { 
                systemOffset = vSystem.offset;
                
                if (vSystem.XSize == 1) systemOffset.x = 0;
                if (vSystem.YSize == 1) systemOffset.y = 0;
                if (vSystem.ZSize == 1) systemOffset.z = 0;
            }
        }
	
	}
    public Vector3 WorldToVoxelAdd(RaycastHit Pos)
    {       

        Vector3 difference = Pos.point - vSystem.transform.position;
        difference += Pos.normal / 2.0f * vSystem.VoxelSpacing;
        WorldToVoxel(ref difference);        
        return difference;
    }
    public Vector3 WorldToVoxelAdd(Vector3 Pos)
    {
        Vector3 difference = Pos - vSystem.transform.position;
        WorldToVoxel(ref difference);
        return difference;
    }
    public Vector3 WorldToVoxelRemove(RaycastHit Pos)
    {       
        Vector3 difference = Pos.point - vSystem.transform.position;
        difference -= Pos.normal / 2.0f * vSystem.VoxelSpacing;
        WorldToVoxel(ref difference);        
        return difference;
    }
    public Vector3 WorldToVoxelRemove(Vector3 Pos)
    {        
        Vector3 difference = Pos - vSystem.transform.position;
        WorldToVoxel(ref difference);
        return difference;
    }
    void WorldToVoxel(ref Vector3 point)
    {
        point = transform.worldToLocalMatrix.MultiplyVector(point);
        point -= systemOffset;
        point -= vSystem.chunks_vcs[0, 0, 0].offset;
        point /= vSystem.VoxelSpacing;
    }

    public void QuickAdd(RaycastHit Pos, bool update,int type)
    {
        vSystem.QuickAdd(new VoxelPos(WorldToVoxelAdd(Pos)), type, update);
    }
    public void QuickAdd(Vector3 Pos, bool update, int type)
    {
        vSystem.QuickAdd(new VoxelPos(WorldToVoxelAdd(Pos)), type, update);
    }
    public void QuickAdd(VoxelPos Pos, bool update, int type)
    {
        vSystem.QuickAdd(Pos, type, update);
    }
    public void QuickRemove(RaycastHit Pos, bool update)
    { 
        vSystem.QuickRemove(new VoxelPos(WorldToVoxelRemove(Pos)), update);
    }
    public void QuickRemove(Vector3 Pos, bool update)
    {
        vSystem.QuickRemove(new VoxelPos(WorldToVoxelRemove(Pos)), update);
    }
    public void QuickRemove(VoxelPos Pos, bool update)
    {
        vSystem.QuickRemove(Pos, update);
    }
    //-------------------------------------------------------------------
    public void AddVoxel(Vector3 Pos, bool update)
	{
        vSystem.AddVoxel(new VoxelPos(WorldToVoxelAdd(Pos)), update);
	}
    public void AddVoxel(Vector3 Pos, bool update, int type)
    {        
        vSystem.AddVoxel(new VoxelPos(WorldToVoxelAdd(Pos)), update, type);    
    }
	
	public void AddVoxel(VoxelPos Pos, bool update,int type)
	{
		vSystem.AddVoxel(Pos,update,type);
	}
    public void AddVoxel(VoxelPos Pos, bool update)
    {
		vSystem.AddVoxel(Pos,update);
    }
    //-------------------------------------------------------------------

    public void RemoveVoxel(Vector3 Pos, bool update)
    {
        vSystem.RemoveVoxel(new VoxelPos(WorldToVoxelRemove(Pos)), update);
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

                    if (offset.magnitude < radius)
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
                    if (offset.magnitude < radius)
                    {
                        RemoveVoxel(new VoxelPos(difference + (offset / 1.42f)), true);
					}
				}   
            }   
        }
    }
    public void QuickRemoveVoxelAoE(Vector3 Pos, int radius, bool PureVoxel)
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
                    offset.x = x;
                    offset.y = y;
                    offset.z = z;
                    if (offset.magnitude < radius)
                    {
                        QuickRemove(new VoxelPos(difference + (offset / 1.42f)), true);
                    }
                }
            }
        }
    }
    public void QuickAddVoxelAoE(Vector3 Pos, int radius, int type, bool PureVoxel)
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
                    offset.x = x;
                    offset.y = y;
                    offset.z = z;

                    if (offset.magnitude < radius)
                    {
                        QuickAdd(new VoxelPos(difference + (offset / 1.42f)), true, type);
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

    public void AddVoxel(RaycastHit Pos, bool update, int type)
    {       
		vSystem.AddVoxel(new VoxelPos(WorldToVoxelAdd(Pos)), update, type);
    }
	public void RemoveVoxel(RaycastHit Pos, bool update)
    {
		vSystem.RemoveVoxel(new VoxelPos(WorldToVoxelRemove(Pos)), update);
    }
}
