using UnityEngine;

using System.Collections;


public class Voxel{
	//Stats
	public float Health;
	public float DamageResistance;
	public float Energy;



	public MeshData_Vox meshData;
	public int VoxelType;
	public Voxel()
	{
	}
	public Voxel(MeshData_Vox _mesh)
	{
		meshData = _mesh;
	}
	public void LoadMesh(MeshData_Vox _mesh)
	{
		meshData = _mesh;
	}



}
