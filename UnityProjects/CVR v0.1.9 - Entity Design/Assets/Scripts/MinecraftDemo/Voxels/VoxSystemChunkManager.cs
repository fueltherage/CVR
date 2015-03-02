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
    public void AddVoxel(Vector3 Pos, bool update)
    {
        Vector3 difference = Pos - vSystem.transform.position;
        difference = transform.worldToLocalMatrix.MultiplyVector(difference);
		difference -= vSystem.offset/2.0f;            
        difference -= vSystem.chunks_vcs[0,0,0].offset;
		difference /= vSystem.VoxelSpacing;   
        vSystem.AddVoxel(new VoxelPos(difference), update);
    }
	public void RemoveVoxel(Vector3 Pos, bool update)
    {
       //Remove the voxel that Pos is in.
        Vector3 difference = Pos - vSystem.transform.position;
		difference = transform.worldToLocalMatrix.MultiplyVector(difference);
		difference -= vSystem.offset/2.0f;
		difference -= vSystem.chunks_vcs[0,0,0].offset;
		difference /= vSystem.VoxelSpacing;    

		vSystem.RemoveVoxel(new VoxelPos(difference.x,difference.y,difference.z), update);
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
	public void RemoveVoxelAoE(RaycastHit Pos, int radius)
	{
		Vector3 difference = Pos.point;

		difference -= Pos.normal / 2.0f;   
        Vector3 offset;
        bool update;
		for (int x = -radius; x < radius; x++) {
			for (int y = -radius; y < radius; y++) {
				for (int z = -radius; z < radius; z++) {
                    offset.x =x;
                    offset.y =y;
                    offset.z =z;
                    update = radius == offset.magnitude;
					if(offset.magnitude < radius)
					{
						RemoveVoxel(difference + offset / 1.5f, true);
					}
				}	
			}	
		}
	}
    public void AddVoxelAoE(RaycastHit Pos, int radius)
    {
        Vector3 difference = Pos.point;
        
        difference -= Pos.normal / 2.0f;   
        Vector3 offset;
        bool update;
        for (int x = -radius; x < radius; x++) {
            for (int y = -radius; y < radius; y++) {
                for (int z = -radius; z < radius; z++) {
                    offset.x =x;
                    offset.y =y;
                    offset.z =z;
                    update = radius == offset.magnitude;
                    if(offset.magnitude <= radius)
                    {
                        AddVoxel(difference + offset / 1.5f, update);
                    }
                }   
            }   
        }
    }
    public void RemoveVoxelAoE(Vector3 Pos, int radius)
    {       

        Vector3 offset;
        bool update;

        Vector3 difference = Pos - vSystem.transform.position; 
        difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= vSystem.offset/2.0f;
        difference /= vSystem.VoxelSpacing;       
        difference -= vSystem.chunks_vcs[0,0,0].offset;

        for (int x = -radius; x < radius; x++) {
            for (int y = -radius; y < radius; y++) {
                for (int z = -radius; z < radius; z++) {
                    offset.x =x;
                    offset.y =y;
                    offset.z =z;
                    update = radius == offset.magnitude;
                    if(offset.magnitude <= radius)
                    {
                        RemoveVoxel(new VoxelPos(difference + offset), true);
                    }
                }   
            }   
        }
    }
    public void AddVoxelAoE(Vector3 Pos, int radius)
    {     
        Vector3 offset;
        bool update;

        Vector3 difference = Pos - vSystem.transform.position;
        difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= vSystem.offset/2.0f;
        difference /= vSystem.VoxelSpacing;        
        difference -= vSystem.chunks_vcs[0,0,0].offset;

        for (int x = -radius; x < radius; x++) {
            for (int y = -radius; y < radius; y++) {
                for (int z = -radius; z < radius; z++) {
                    offset.x =x;
                    offset.y =y;
                    offset.z =z;
                    update = radius == offset.magnitude;
                    if(offset.magnitude <= radius)
                    {
                        AddVoxel(new VoxelPos(difference + offset), true);
                    }
                }   
            }   
        }
    }

    public void AddVoxel(RaycastHit Pos, bool update)
    {
        Vector3 difference = Pos.point - vSystem.transform.position;
		difference += Pos.normal * vSystem.VoxelSpacing / 2.0f;
		difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= vSystem.offset / 2.0f;              
		difference -= vSystem.chunks_vcs[0,0,0].offset;
		difference /= vSystem.VoxelSpacing;  
		vSystem.AddVoxel(new VoxelPos(difference), update);
    }
	public void RemoveVoxel(RaycastHit Pos, bool update)
    {
        Vector3 difference = Pos.point - vSystem.transform.position;
        difference -= Pos.normal * vSystem.VoxelSpacing / 2.0f;   
		difference = transform.worldToLocalMatrix.MultiplyVector(difference);
        difference -= vSystem.offset / 2.0f;         
		difference -= vSystem.chunks_vcs[0,0,0].offset;
		difference /= vSystem.VoxelSpacing;      
	
		vSystem.RemoveVoxel(new VoxelPos(difference), update);
    }
}
