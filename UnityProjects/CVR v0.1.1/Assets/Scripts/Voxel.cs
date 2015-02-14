using UnityEngine;

using System.Collections;


public class Voxel{
	public bool filled;
	public bool locked;
	public bool needsMeshUpdate;
	Neighbours nVoxels;
	MeshData VoxelMesh;

	public Voxel()
	{

	}
	public virtual MeshData GenerateMesh()
	{
		if(VoxelMesh.generated && !needsMeshUpdate)
		{
			return VoxelMesh;
		}
		else
		{
			VoxelMesh.GenerateMeshData();
			needsMeshUpdate = false;
			return VoxelMesh;
		}
	}
	public void SetMeshInfo(Neighbours _vVoxels, MeshVerts _verts)
	{
		needsMeshUpdate = true;
		nVoxels = _vVoxels;
		VoxelMesh = new CubeMesh(_vVoxels,_verts);
	}
}
