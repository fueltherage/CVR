using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelSystemChunk : VoxelChunk {

	public ChunkNeighbours neighbours;
	public VoxelPos chunkPos = new VoxelPos(0,0,0) ;
	public VoxelSystem systemParent;
	VoxelSystemChunk thisChunk;
	void Start()
	{
	}
	void Update()
	{
		if(needsUpdating) UpdateMesh();
		needsUpdating = false;
	}
	public override void Init()
	{
		thisChunk = gameObject.GetComponent<VoxelSystemChunk>();
		offset.x = - XSize*VoxelSpacing/2.0f;//Starting position of vertices
		offset.y = - YSize*VoxelSpacing/2.0f;
		offset.z = - ZSize*VoxelSpacing/2.0f;

		Triangles = new List<Triangle>();
		UVs = new List<Vector2>();
		TrIndex = new List<int>(); //index for submeshing
		Verts = new List<Vector3>();
		vmesh = new Mesh();

        InitShells();

        InitDics();

		Initialized = true;
	}
	protected override void InitShells()
	{
		blocks = new VoxelShell[XSize, YSize, ZSize];
		for (int x = 0; x < XSize; x++)
		{
			for (int y = 0; y < YSize; y++)
			{
				for (int z = 0; z < ZSize; z++)
				{
					blocks[x, y, z] = new VoxelShell(ref thisChunk);
					blocks[x, y, z].voxel = new Voxel();
					blocks[x, y, z].vp = new VoxelPos(x, y, z);
				}
			}
		}
	}
	public void CalculateNeighbours()
	{
		for (int x = 0; x < XSize; ++x) {
			for (int y = 0; y < YSize; ++y) {
				for (int z = 0; z < ZSize; ++z) {
					//Debug.Log("x: "+x+" y:"+y+" z:"+z);
					Neighbours temp_neighbours = new Neighbours();
					if(x+1 == XSize)
					{
						temp_neighbours.xpos = neighbours.xpos_vcs.blocks[0,y,z];
					}
					else temp_neighbours.xpos = blocks[x+1,y,z];
					
					if(x == 0)	
					{
						temp_neighbours.xneg = neighbours.xneg_vcs.blocks[XSize-1,y,z];	
					}
					else temp_neighbours.xneg = blocks[x-1,y,z];
					
					if(y+1 == YSize) 
					{
						temp_neighbours.ypos = neighbours.ypos_vcs.blocks[x,0,z];
					}
					else temp_neighbours.ypos = blocks[x,y+1,z];
					
					if(y == 0) 
					{
						temp_neighbours.yneg = neighbours.yneg_vcs.blocks[x,YSize-1,z];
					}
					else temp_neighbours.yneg = blocks[x,y-1,z];
					
					if(z+1 == ZSize)
					{
						temp_neighbours.zpos = neighbours.zpos_vcs.blocks[x,y,0];
					}
					else temp_neighbours.zpos = blocks[x,y,z+1];
					
					if(z == 0) 
					{
						temp_neighbours.zneg = neighbours.zneg_vcs.blocks[x,y,ZSize-1];
					}
					else temp_neighbours.zneg = blocks[x,y,z-1];
					
					blocks[x,y,z].neighbours = temp_neighbours;					
					
					blocks[x,y,z].locked = false;
				}				
			}
		}
	}

}
