using UnityEngine;

using System.Collections;


public class Voxel{
	//Stats
	public float Health;
	public float DamageResistance;
	public float Energy;
	public bool filled;
	public bool locked;
	public Neighbours neighbours;
	public MeshData_Vox meshData;
	public VoxelPos vp;
	public int VoxelType;
	public Voxel()
	{
	}
	public Voxel(Neighbours _n,VoxelPos _vp)
	{
		neighbours = _n;
		vp = _vp;
	}
	public void LoadMesh(MeshData_Vox _mesh)
	{
		meshData = _mesh;
	}
	public Voxel(Neighbours _n, MeshData_Vox _mesh, VoxelPos _vp)
	{
		neighbours = _n;
		meshData = _mesh;
		vp = _vp;
	}
	public virtual int GenerateMesh(int _faceCount)
	{	
		return meshData.GenerateMesh(_faceCount, vp);
	}

}
