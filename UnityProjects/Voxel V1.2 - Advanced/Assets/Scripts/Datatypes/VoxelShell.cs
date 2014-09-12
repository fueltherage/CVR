using UnityEngine;
using System.Collections;

public class VoxelShell {
	public Voxel voxel;
	public Neighbours neighbours;
	public VoxelPos vp;
	public bool filled;
	public bool locked;
	public VoxelShell()
	{

	}
	public virtual int GenerateMesh(int _faceCount)
	{	
		return voxel.meshData.GenerateMesh(_faceCount, vp);
	}
}
