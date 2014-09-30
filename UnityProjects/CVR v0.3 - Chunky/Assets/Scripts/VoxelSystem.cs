using UnityEngine;
using System.Collections;

public class VoxelSystem : MonoBehaviour {
	public  int XSize = 10;
	public  int YSize = 10;
	public  int ZSize = 10;

	public float VoxelSpacing = 1;
	public int ChunkSizeX = 5;
	public int ChunkSizeY = 5;
	public int ChunkSizeZ = 5;
	public GameObject[,,] chunks;
	public VoxelChunk[,,] chunks_vcs;
	public GameObject VoxelChunkGO;
	public GameObject EmptyChunk;
	public Vector3 offset;
	public VoxMaterialFactory factory;

	int currentSelectedVoxel=0;
	ChunkNeighbours[,,] neighbours;



	void Start () {


		VoxelEvents.onVoxelAdded += AddVoxel;
		VoxelEvents.onVoxelRemoved += RemoveVoxel;
		VoxelEvents.onVoxelSwitch += SelectedVoxel;
		
		factory = GameObject.Find("VoxelMaterials").GetComponent<VoxMaterialFactory>();

		chunks = new GameObject[XSize, YSize, ZSize];
	
		offset.x = - XSize*ChunkSizeX*VoxelSpacing/2.0f;
		offset.y = - YSize*ChunkSizeY*VoxelSpacing/2.0f;
		offset.z = - ZSize*ChunkSizeZ*VoxelSpacing/2.0f;

		for (int x =0; x < XSize; x++){
			for (int y =0; y < YSize; y++){
				for (int z =0; z < ZSize; z++){
					chunks[x,y,z] = Instantiate(VoxelChunkGO, Vector3.zero, Quaternion.identity) as GameObject;
					chunks[x,y,z].transform.parent = this.transform;
					chunks[x,y,z].transform.Translate(new Vector3(offset.x + x * ChunkSizeX * VoxelSpacing,
					                                              offset.y + y * ChunkSizeY * VoxelSpacing,
					                                              offset.z + z * ChunkSizeZ * VoxelSpacing));
				}
			}
		}

		neighbours = new ChunkNeighbours[XSize, YSize, ZSize];
		chunks_vcs = new VoxelChunk[XSize, YSize, ZSize];

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

					chunks_vcs[x,y,z] = chunks[x,y,z].GetComponent<VoxelChunk>();
					chunks_vcs[x,y,z].neighbours = temp_neighbours;
				}
			}
		}
		EmptyChunk.GetComponent<VoxelChunkEmpty>().Init();

		for (int x =0; x < XSize; x++){
			for (int y =0; y < YSize; y++){
				for (int z =0; z < ZSize; z++){
					chunks_vcs[x,y,z].Init();
				}
			}
		}
		for (int x =0; x < XSize; x++){
			for (int y =0; y < YSize; y++){
				for (int z =0; z < ZSize; z++){
					chunks_vcs[x,y,z].CalculateNeighbours();
				}
			}
		}

		//put a voxel in the middle
		VoxelFactory.GenerateVoxel(1,
		                           ref chunks_vcs[XSize/2, YSize/2, ZSize/2].blocks[ChunkSizeX/2,ChunkSizeY/2,ChunkSizeZ/2],
		                           chunks_vcs[XSize/2, YSize/2, ZSize/2].offset,
		                           VoxelSpacing);

		chunks_vcs[XSize/2, YSize/2, ZSize/2].UpdateMesh();

	}
	public void SelectedVoxel(int _currentSelectedVoxel)
	{
		currentSelectedVoxel = _currentSelectedVoxel;
	}
	public void AddVoxel(VoxelPos voxel)
	{
		//Debug.Log(voxel);
		VoxelPos voxPos = new VoxelPos(voxel.x % ChunkSizeX,
	                                   voxel.y % ChunkSizeY,
	                                   voxel.z % ChunkSizeX);

		VoxelPos chunkPos = new VoxelPos(voxel.x/XSize,
		                                 voxel.y/YSize,
		                                 voxel.z/ZSize);

		if(chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].voxel.VoxelType!= currentSelectedVoxel)
		{
			VoxelFactory.GenerateVoxel(currentSelectedVoxel, ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z],chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].offset, VoxelSpacing);	
		}else chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].filled = true;

		chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].UpdateMesh ();
		 	
	}
	public void RemoveVoxel(VoxelPos voxel)
	{
		VoxelPos voxPos = new VoxelPos(voxel.x % ChunkSizeX,
		                               voxel.y % ChunkSizeY,
		                               voxel.z % ChunkSizeX);
		VoxelPos chunkPos = new VoxelPos(voxel.x/XSize,
		                                 voxel.y/YSize,
		                                 voxel.z/ZSize);
		VoxelFactory.GenerateVoxel(0, ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z],chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].offset, VoxelSpacing);
		chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].UpdateMesh ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
