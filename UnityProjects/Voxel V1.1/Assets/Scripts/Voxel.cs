using UnityEngine;

using System.Collections;


public class Voxel{
	public bool filled;
	public bool locked;

	public Voxel()
	{


	}
	public virtual MeshData GenerateMesh(VoxelNeighbours _v)// Vertices info needs to be passed also.
	{
		MeshData meshData = new MeshData ();
		return meshData;
	}	
}
