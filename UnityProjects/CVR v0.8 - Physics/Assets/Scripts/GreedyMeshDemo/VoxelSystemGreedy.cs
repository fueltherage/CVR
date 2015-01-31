using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelSystemGreedy : MonoBehaviour {
	public  int XSize = 10;
	public  int YSize = 2;
	public  int ZSize = 10;

	public float VoxelSpacing;
	public int ChunkSizeX = 5;
	public int ChunkSizeY = 5;
	public int ChunkSizeZ = 5;
	public GameObject[,,] chunks;
	public VoxelSystemChunkGreedy[,,] chunks_vcs;
	public GameObject VoxelChunkGO;
	public GameObject EmptyChunk;
	public Vector3 offset;
	public VoxMaterialFactory factory;
	public bool Initialized = false;
	public bool CreateConvexColliders = false;
	public VoxelSystemUpdateManager VSUM;

	int currentSelectedVoxel=1;

	void Start () {
	
		//Create a new empty chunk for this system and sync it's size
		EmptyChunk = Instantiate(EmptyChunk, this.transform.position, Quaternion.identity) as GameObject;
		EmptyChunk.name = "EmptyChunk";
		VSUM = GetComponent<VoxelSystemUpdateManager>();
		VoxelChunkEmpty EmptyChunkSync = EmptyChunk.GetComponent<VoxelChunkEmpty>();
		EmptyChunkSync.XSize = ChunkSizeX;
		EmptyChunkSync.YSize = ChunkSizeY;
		EmptyChunkSync.ZSize = ChunkSizeZ;

		VoxelEvents.onVoxelAdded += AddVoxel;
		VoxelEvents.onVoxelRemoved += RemoveVoxel;
		VoxelEvents.onVoxelSwitch += SelectedVoxel;

		chunks = new GameObject[XSize, YSize, ZSize];
	
		offset.x = - XSize*ChunkSizeX*VoxelSpacing/2.0f;
		offset.y = - YSize*ChunkSizeY*VoxelSpacing/2.0f;
		offset.z = - ZSize*ChunkSizeZ*VoxelSpacing/2.0f;

		for (int x =0; x < XSize; x++){
			for (int y =0; y < YSize; y++){
				for (int z =0; z < ZSize; z++){
					chunks[x,y,z] = Instantiate(VoxelChunkGO, this.transform.position, Quaternion.identity) as GameObject;
					chunks[x,y,z].transform.parent = this.transform;
					chunks[x,y,z].transform.Translate(new Vector3(offset.x + x  * ChunkSizeX * VoxelSpacing + ChunkSizeX /2.0f,
                                                                  offset.y + y  * ChunkSizeY * VoxelSpacing + ChunkSizeY /2.0f,
                                                                  offset.z + z  * ChunkSizeZ * VoxelSpacing + ChunkSizeZ /2.0f));
					chunks[x,y,z].name = "Chunk "+x+" "+y+ " "+z;
				}
			}
		}

		chunks_vcs = new VoxelSystemChunkGreedy[XSize, YSize, ZSize];

		//Generate the neighbours for each chunk
		for (int x =0; x < XSize; x++){
			for (int y =0; y < YSize; y++){
				for (int z =0; z < ZSize; z++){

					ChunkNeighbours temp_neighbours = new ChunkNeighbours();

					if(x+1 == XSize)temp_neighbours.xpos = EmptyChunk;
					else temp_neighbours.xpos = chunks[x+1,y,z];

					if(x-1 < 0)temp_neighbours.xneg = EmptyChunk;
					else temp_neighbours.xneg = chunks[x-1,y,z];

					if(y+1 == YSize) temp_neighbours.ypos = EmptyChunk;
					else temp_neighbours.ypos = chunks[x,y+1,z];

					if(y-1 < 0)temp_neighbours.yneg = EmptyChunk;
					else temp_neighbours.yneg = chunks[x,y-1,z];

					if(z+1 == ZSize)temp_neighbours.zpos = EmptyChunk;
					else temp_neighbours.zpos = chunks[x,y,z+1];

					if(z-1 < 0)temp_neighbours.zneg = EmptyChunk;
					else temp_neighbours.zneg = chunks[x,y,z-1];

					temp_neighbours.UpdateVoxelChunkRefs();

					chunks_vcs[x,y,z] = chunks[x,y,z].GetComponent<VoxelSystemChunkGreedy>();
					chunks_vcs[x,y,z].neighbours = temp_neighbours;	
					chunks_vcs[x,y,z].VoxelSpacing = VoxelSpacing;
					chunks_vcs[x,y,z].XSize = ChunkSizeX;
					chunks_vcs[x,y,z].YSize = ChunkSizeY;
					chunks_vcs[x,y,z].ZSize = ChunkSizeZ;
					chunks_vcs[x,y,z].factory = factory;
					chunks_vcs[x,y,z].chunkPos = new VoxelPos(x,y,z);
					chunks_vcs[x,y,z].systemParent = this;
				}
			}
		}
		
		//Make sure each chunk is initialized
		EmptyChunkSync.Init();
		EmptyChunkSync.gameObject.transform.parent = this.transform;
		for (int x =0; x < XSize; x++){
			for (int y =0; y < YSize; y++){
				for (int z =0; z < ZSize; z++){
					chunks_vcs[x,y,z].Init();
				}
			}
		}
		//The reason there needs to be a repeat loop is because all the chunks needs to be completely initialized
		//before neighbours are calculated to avoid nulls value refrences.
		for (int x =0; x < XSize; x++){
			for (int y =0; y < YSize; y++){
				for (int z =0; z < ZSize; z++){
					chunks_vcs[x,y,z].CalculateNeighbours();
				}
			}
		}

		Initialized = true;
	}
	public void SelectedVoxel(int _currentSelectedVoxel)
	{
		currentSelectedVoxel = _currentSelectedVoxel;
	}
	public void AddVoxel(VoxelPos voxel, bool update,int type)
	{
		AddVoxel (voxel.x, voxel.y, voxel.z, update, type);
	}
	public void AddVoxel(VoxelPos voxel, bool update)
	{
		AddVoxel (voxel.x, voxel.y, voxel.z, update);
	}
	public void AddVoxel(int x, int y, int z, bool update)
	{
		AddVoxel(x, y, z, update, currentSelectedVoxel);
	}
	public void AddVoxel(int x, int y, int z, bool update, int type)
	{

		//Calculate the position
		VoxelPos voxPos = new VoxelPos(x % ChunkSizeX,
									   y % ChunkSizeY,
									   z % ChunkSizeZ);

		VoxelPos chunkPos = new VoxelPos(x / ChunkSizeX,
										 y / ChunkSizeY,
										 z / ChunkSizeZ);	

		if(voxPos.x >=0 && voxPos.y >=0 && voxPos.z >=0)
		
		if((chunkPos.x < XSize && chunkPos.y < YSize && chunkPos.z < ZSize)
		   && (chunkPos.x >= 0 && chunkPos.y >= 0 && chunkPos.z >= 0))
		{				
			if(chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].voxel.VoxelType!= currentSelectedVoxel)
			{
				VoxelFactory.GenerateVoxel(type, ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z],
													 chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].offset, VoxelSpacing);	
			}else chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].filled = true;


			if(update)
			{
				if(!chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].Generating)
				{
					System.Action up= go;
					System.Action gen= go;
					chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].neighbours.GetNeighbourDelegate(ref up,ref gen);
					if(up==null)
					VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);
					else VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z],up,gen);
				}
			}

		}

	}
	public void RemoveVoxel(VoxelPos voxel, bool update)
	{
		//Calculate the position
		VoxelPos voxPos = new VoxelPos(voxel.x % ChunkSizeX,
									   voxel.y % ChunkSizeY,
									   voxel.z % ChunkSizeZ);

		VoxelPos chunkPos = new VoxelPos(voxel.x / ChunkSizeX,
										 voxel.y / ChunkSizeY,
										 voxel.z / ChunkSizeZ);

		if(voxPos.x >=0 && voxPos.y >=0 && voxPos.z >=0)		
		if((chunkPos.x < XSize && chunkPos.y < YSize && chunkPos.z < ZSize)
		   && (chunkPos.x >= 0 && chunkPos.y >= 0 && chunkPos.z >= 0))
		{

			VoxelFactory.GenerateVoxel(0, ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z],
									   chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].offset, VoxelSpacing);

			if(update)
			{

                chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].neighbours.UpdateNeighbours();
                VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);


//				System.Action up = go;
//				System.Action gen = go;
//				chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].neighbours.GetNeighbourDelegate(ref up,ref gen);
//				
//				if(up==null)
//					VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);
//				else VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z],up,gen);
			}
		}
	}
	void go(){}
	public void UpdateMeshes()
	{
		for(int x = 0; x < XSize; x++)
		{
			for(int y = 0; y < YSize; y++)
			{
				for(int z = 0; z < ZSize; z++)
				{
					VSUM.QueueChunkForUpdate(ref chunks_vcs[x,y,z]);
				}
			}
		}
	}	
	// Update is called once per frame
	void Update () {
        
	}
}
