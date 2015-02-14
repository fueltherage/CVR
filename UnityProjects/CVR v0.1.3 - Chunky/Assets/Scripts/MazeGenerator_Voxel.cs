using UnityEngine;
using System.Collections;

public class MazeGenerator_Voxel : MonoBehaviour {

	public GameObject SystemObject;
	VoxelChunkSingle VoxelMesh;

	bool[,,]mazeBlocks;
	bool generated = false;
	VoxelPos c;
	// Use this for initialization
	void Start () {

	}
	void UpdateVoxelSystem()
	{
		for(int x =0; x< VoxelMesh.XSize; x++)
		{
			for(int y =0; y< VoxelMesh.YSize; y++)
			{
				for(int z =0; z< VoxelMesh.ZSize; z++)
				{
					if(mazeBlocks[x,y,z])
						VoxelEvents.VoxelAdded(new VoxelPos(x,y,z));
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && !generated)
		{
			VoxelMesh = SystemObject.GetComponent<VoxelChunkSingle>();
			mazeBlocks = new bool[VoxelMesh.XSize,VoxelMesh.YSize,VoxelMesh.ZSize];
			for(int x = 0; x < VoxelMesh.XSize; x++)
			{
				for (int z = 0; z < VoxelMesh.ZSize; z++)
				{
					mazeBlocks[x,1,z] = true;
					mazeBlocks[x,2,z] = true;
				}
			}
			mazeBlocks[1,2,1] = false;
			mazeBlocks[2,2,1] = false;
			mazeBlocks[2,2,2] = false;
			mazeBlocks[1,2,2] = false;
			c = new VoxelPos(2,1,2);
			
			
			
			UpdateVoxelSystem();
		}	
	}
	bool GeneratePath()
	{
		int xp;
		int xn;
		int zp;
		int zn;
		if(c.x+2 >VoxelMesh.XSize)
		{
			if(mazeBlocks[c.x+2,c.y,c.z]&& mazeBlocks[c.x+1,c.y,c.z])
			{
				mazeBlocks[c.x+1,c.y,c.z] = false;
				mazeBlocks[c.x+2,c.y,c.z] = false;
				c.x += 2;
			}
		}
		return true;
	}

}
