using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelSystem : MonoBehaviour {
	//Notes
	/* 
	 * 
	*/
	public  int XSize = 6;
	public  int YSize = 6;
	public  int ZSize = 6;
	public  float VoxelSpacing = 1f;
	public float x_offset;
	public float y_offset;
	public float z_offset;

	List<Index> TriangleIndex;
	List<int> Triangles;
	List<Vector2> UVs;
	List<Vector3> Verts;
	List<Vector3> Normals;
	
	Voxel[,,] blocks;
	Vert[,,]VertGrid;

	// Use this for initialization
	void Start () {
		//Shift the game oject over so the center is always the origin.
		this.transform.Translate (-VoxelSpacing / 2.0f, -VoxelSpacing / 2.0f, -VoxelSpacing / 2.0f);
	
		x_offset = - XSize*VoxelSpacing/2.0f;//Starting position of vertices
		y_offset = - YSize*VoxelSpacing/2.0f;
		z_offset = - ZSize*VoxelSpacing/2.0f;
		// - Size/2.0f is to offset the Vertices by half of the Size so that they are centered around the Vox origin


		//Subscribe to Voxel Events
		VoxelEvents.onVoxelAdded += AddVoxel;
		VoxelEvents.onVoxelRemoved += RemoveVoxel;

		//Initializing Voxel/Vertex array
		blocks = new Voxel[XSize,YSize,ZSize];

		for (int x =0; x < XSize; x++){
			for (int y =0; y < XSize; y++){
				for (int z =0; z < XSize; z++){
					blocks[x,y,z] = new Voxel();
				}
			}
		}

		VertGrid = new Vert[XSize+1,YSize+1,ZSize+1];

		for (int x =0; x <= XSize; x++){
			for (int y =0; y <= XSize; y++){
				for (int z =0; z <= XSize; z++){
					Vector3 v = new Vector3(x * VoxelSpacing + x_offset,
					                        y * VoxelSpacing + y_offset,
					                        z * VoxelSpacing + z_offset);
					Index i = new Index(0);
					VertGrid[x,y,z] = new Vert(ref i,ref v);
				}
			}
		}


		for (int x = 0; x < XSize; ++x) {
			for (int y = 0; y < YSize; ++y) {
				for (int z = 0; z < ZSize; ++z) {
					if(x==0||y==0||z==0||x==XSize-1||y==YSize-1||z==ZSize-1)
					{					
						blocks[x,y,z].locked = true;
						blocks[x,y,z].filled = false;
						//Lock the outer layer
					}
					else
					{ 
						Neighbours n = new Neighbours(ref blocks[x,y+1,z], ref blocks[x,y-1,z], ref blocks[x-1,y,z],
						                              ref blocks[x+1,y,z], ref blocks[x,y,z-1] ,ref blocks[x,y,z+1]);

						MeshVerts mv = new MeshVerts(ref VertGrid[x,y,z],ref VertGrid[x+1,y,z],ref VertGrid[x+1,y+1,z],ref VertGrid[x+1,y,z+1],
						                             ref VertGrid[x+1,y+1,z+1],ref VertGrid[x,y+1,z+1],ref VertGrid[x,y+1,z],ref VertGrid[x,y,z+1]);
						blocks[x,y,z].SetMeshInfo(n,mv);
						blocks[x,y,z].locked = false;
					}
				}				
			}
		}
		blocks [XSize/2,YSize/2,ZSize/2].filled = true;


		Triangles = new List<int> ();
		UVs = new List<Vector2> ();
		Verts = new List<Vector3> ();	
		TriangleIndex = new List<Index>();
		Normals = new List<Vector3>();
		UpdateMesh ();


	}

	public void AddVoxel(VoxelPos voxel)
	{
		if (!blocks [voxel.x, voxel.y, voxel.z].locked) {
			blocks [voxel.x, voxel.y, voxel.z].filled = true;
			UpdateMesh ();
		} else {
			Debug.Log(".");
		}	
	}
	public void RemoveVoxel(VoxelPos voxel)
	{
		blocks [voxel.x, voxel.y, voxel.z].filled = false;
		UpdateMesh ();
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateMesh()
	{
		Mesh SystemMesh = GenerateMeshData ();		
		gameObject.GetComponent<MeshFilter> ().mesh = SystemMesh;

		//Nullify the mesh so it initiates a reset;
		MeshCollider meshCollider = GetComponent<MeshCollider> ();
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = SystemMesh;
	}
	public void UpdateVertexIndex()
	{
		int index =0;
		for (int x =0; x < XSize; x++){
			for (int y =0; y < XSize; y++){
				for (int z =0; z < XSize; z++){
					if(VertGrid[x,y,z].used)
					{
						VertGrid[x,y,z].index.value = index;
						Verts.Add(VertGrid[x,y,z].Vertex);
						index++;
						UVs.Add(Vector2.zero);
					}
				}
			}
		}
	}

	Mesh GenerateMeshData ()
	{
		//Flag all vertices as unused
//		for (int x =0; x <= XSize; x++){
//			for (int y =0; y <= XSize; y++){
//				for (int z =0; z <= XSize; z++){
//					VertGrid[x,y,z].used = false;
//				}
//			}
//		}

		//This is inefficient, i want to only remove the mesh data of voxels that have changed
		//and replace it with updated mesh data. 
		Triangles.Clear();
		UVs.Clear();
		Verts.Clear();
		Normals.Clear();

		for (int x = 1; x < XSize-1; ++x) {
			for (int y = 1; y < YSize-1; ++y) {
				for (int z = 1; z < ZSize-1; ++z) {
					if(blocks[x,y,z].filled)
					{
						MeshData temp = blocks[x,y,z].GenerateMesh();

						for (int i =0; i < temp.Triangles.Count;i++)
						{
							TriangleIndex.Add(temp.Triangles[i]);
						}
						for (int i =0; i < temp.UVs.Count;i++)
						{
							UVs.Add (temp.UVs[i]);
						}
						for (int i =0; i < temp.Normals.Count;i++)
						{
							Normals.Add (temp.Normals[i]);
						}
					}
				}				
			}
		}
		//Reindex verts 
		UpdateVertexIndex();

		for (int i =0; i < TriangleIndex.Count; i++)
		{
			Triangles.Add(TriangleIndex[i].value);
		}

		Mesh mesh = new Mesh ();
		mesh.vertices = Verts.ToArray();
		mesh.triangles = Triangles.ToArray();
		mesh.uv = UVs.ToArray ();
		mesh.normals = Normals.ToArray();

//		for(int i =0; i<mesh.vertices.Length;i++)
//		{
//			Debug.Log(mesh.vertices[i]);
//		}
//		for(int i =0; i<mesh.triangles.Length;i++)
//		{
//			Debug.Log(mesh.triangles[i]);
//		}
//		for(int i =0; i<mesh.uv.Length;i++)
//		{
//			Debug.Log(mesh.uv[i]);
//		}

		mesh.Optimize ();

		return mesh;
	}	
}
