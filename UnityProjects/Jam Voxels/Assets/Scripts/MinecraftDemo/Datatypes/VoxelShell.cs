using UnityEngine;
using System.Collections;

public class VoxelShell {
	public Voxel voxel;
	public Neighbours neighbours;
	public VoxelPos vp;
	public bool filled;
	public bool locked;
	public VoxelSystemChunkGreedy parentChunk;
	public VoxelSystemChunk parentChunkS;

	public VoxelShell(){}
	public VoxelShell(ref VoxelSystemChunkGreedy _parentchunk)
	{
		parentChunk = _parentchunk;
	}
	public VoxelShell(ref VoxelSystemChunk _parentchunk)
	{
		parentChunkS = _parentchunk;
	}

	public int GenerateMesh(int _faceCount)
	{	
		return voxel.meshData.GenerateMesh(_faceCount, vp, parentChunkS);
	}

}
