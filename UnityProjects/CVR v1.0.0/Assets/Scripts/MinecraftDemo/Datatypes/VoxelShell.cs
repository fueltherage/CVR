using UnityEngine;
using System.Collections;

public class VoxelShell {
	public Voxel voxel;
	public Neighbours neighbours;
	public VoxelPos vp;
	public bool filled;
	public bool locked;
	public VoxelSystemChunkGreedy parentChunk;

	public VoxelShell(){}
	public VoxelShell(ref VoxelSystemChunkGreedy _parentchunk)
	{
		parentChunk = _parentchunk;
	}


}
