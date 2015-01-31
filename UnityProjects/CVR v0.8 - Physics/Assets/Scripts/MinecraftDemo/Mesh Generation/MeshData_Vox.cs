using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData_Vox{

	public List<Triangle> Triangles = new List<Triangle>();
	public List<Vector2> UVs = new List<Vector2> ();
	public List<Vector3> Verts = new List<Vector3> ();


	public MeshData_Vox()
	{
		
	}
	public virtual int GenerateMesh(int _faceCount, VoxelPos _vp)
	{
		return _faceCount;
	}
}
