using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshData{

	public List<Index> Triangles = new List<Index> ();
	public List<Vector2> UVs = new List<Vector2> ();
	public List<Vert> Verts = new List<Vert> ();
	public List<Vector3> Normals = new List<Vector3>();

	public Neighbours n;
	public MeshVerts VoxelVerts;
	public bool generated;
	protected Vector3 normal;

	public MeshData()
	{}
	public MeshData(Neighbours _neightbours, MeshVerts _meshVerts)
	{
		n = _neightbours;
		VoxelVerts = _meshVerts;
	}
	public virtual void GenerateMeshData()
	{

	}

}
