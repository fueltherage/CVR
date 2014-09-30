using UnityEngine;
using System.Collections;

public class CubeMesh: MeshData {
	//Note Add triangles in CCW order
	public CubeMesh()
	{}
	public CubeMesh(Neighbours _neighbours, MeshVerts _verts)
	{
		n = _neighbours;
		VoxelVerts = _verts;

	}
	public override void GenerateMeshData ()
	{
		Verts.Clear();
		Triangles.Clear();
		UVs.Clear();
		if(!n.top.filled)
		{
//			Verts.Add (VoxelVerts.Vert010); //0
//			Verts.Add (VoxelVerts.Vert110); //1
//			Verts.Add (VoxelVerts.Vert111); //2
//			Verts.Add (VoxelVerts.Vert011); //3


			Triangles.Add(VoxelVerts.Vert110.index);
			Triangles.Add(VoxelVerts.Vert010.index); 
			Triangles.Add(VoxelVerts.Vert111.index);
			

			Triangles.Add(VoxelVerts.Vert111.index);
			Triangles.Add(VoxelVerts.Vert010.index);
			Triangles.Add(VoxelVerts.Vert011.index);

			normal = Vector3.Cross ( VoxelVerts.Vert110.Vertex - VoxelVerts.Vert010.Vertex,
	                                 VoxelVerts.Vert110.Vertex - VoxelVerts.Vert111.Vertex);
			
			Normals.Add (normal);
			Normals.Add (normal);

//			UVs.Add(new Vector2(0,0));	
//			UVs.Add(new Vector2(1,0));
//			UVs.Add(new Vector2(1,1));
//			UVs.Add(new Vector2(0,1));	




			VoxelVerts.Vert010.used = true; 
			VoxelVerts.Vert110.used = true;
			VoxelVerts.Vert111.used = true; 
			VoxelVerts.Vert011.used = true; 

		}
		if(!n.bot.filled)
		{

			
		
			Triangles.Add(VoxelVerts.Vert001.index);
			Triangles.Add(VoxelVerts.Vert000.index); 
			Triangles.Add(VoxelVerts.Vert100.index);
			

			Triangles.Add(VoxelVerts.Vert001.index);
			Triangles.Add(VoxelVerts.Vert100.index);
			Triangles.Add(VoxelVerts.Vert101.index);


			Vector3 normal = Vector3.Cross ( VoxelVerts.Vert001.Vertex - VoxelVerts.Vert000.Vertex,
			                                 VoxelVerts.Vert100.Vertex - VoxelVerts.Vert100.Vertex);
			
			Normals.Add (normal);
			Normals.Add (normal);

//			UVs.Add(new Vector2(0,0));	
//			UVs.Add(new Vector2(1,0));
//			UVs.Add(new Vector2(1,1));
//			UVs.Add(new Vector2(0,1));	


			VoxelVerts.Vert000.used = true; 
			VoxelVerts.Vert100.used = true; 
			VoxelVerts.Vert101.used = true; 
			VoxelVerts.Vert001.used = true; 


		}
		if(!n.left.filled)
		{
//			Verts.Add (VoxelVerts.Vert000); //0
//			Verts.Add (VoxelVerts.Vert001); //1
//			Verts.Add (VoxelVerts.Vert011); //2
//			Verts.Add (VoxelVerts.Vert010); //3


			Triangles.Add(VoxelVerts.Vert000.index);
			Triangles.Add(VoxelVerts.Vert001.index); 
			Triangles.Add(VoxelVerts.Vert010.index);
			

			Triangles.Add(VoxelVerts.Vert010.index);
			Triangles.Add(VoxelVerts.Vert001.index);
			Triangles.Add(VoxelVerts.Vert011.index);


			Vector3 normal = Vector3.Cross (VoxelVerts.Vert000.Vertex - VoxelVerts.Vert010.Vertex,
			                                VoxelVerts.Vert000.Vertex - VoxelVerts.Vert001.Vertex);
			
			Normals.Add (normal);
			Normals.Add (normal);

//			UVs.Add(new Vector2(0,0));	
//			UVs.Add(new Vector2(1,0));
//			UVs.Add(new Vector2(1,1));
//			UVs.Add(new Vector2(0,1));	


			VoxelVerts.Vert000.used = true; 
			VoxelVerts.Vert001.used = true; 
			VoxelVerts.Vert011.used = true; 
			VoxelVerts.Vert010.used = true; 
			
		}
		if(!n.right.filled)
		{
//			Verts.Add (VoxelVerts.Vert100); //0
//			Verts.Add (VoxelVerts.Vert101); //1
//			Verts.Add (VoxelVerts.Vert111); //2
//			Verts.Add (VoxelVerts.Vert110); //3


			Vector3 normal = Vector3.Cross ( VoxelVerts.Vert100.Vertex - VoxelVerts.Vert110.Vertex,
			                                 VoxelVerts.Vert100.Vertex - VoxelVerts.Vert101.Vertex);

			Normals.Add (normal);
			Normals.Add (normal);


			Triangles.Add(VoxelVerts.Vert100.index);
			Triangles.Add(VoxelVerts.Vert110.index); 
			Triangles.Add(VoxelVerts.Vert101.index);


			Triangles.Add(VoxelVerts.Vert111.index);
			Triangles.Add(VoxelVerts.Vert101.index);
			Triangles.Add(VoxelVerts.Vert110.index);
			
//			UVs.Add(new Vector2(0,0));	
//			UVs.Add(new Vector2(1,0));
//			UVs.Add(new Vector2(1,1));
//			UVs.Add(new Vector2(0,1));		


			VoxelVerts.Vert100.used = true; 
			VoxelVerts.Vert101.used = true; 
			VoxelVerts.Vert111.used = true; 
			VoxelVerts.Vert110.used = true; 

	
		}
		if(!n.front.filled)
		{
//			Verts.Add (VoxelVerts.Vert000); //0
//			Verts.Add (VoxelVerts.Vert100); //1
//			Verts.Add (VoxelVerts.Vert110); //2
//			Verts.Add (VoxelVerts.Vert010); //3

			Vector3 normal = Vector3.Cross ( VoxelVerts.Vert100.Vertex - VoxelVerts.Vert000.Vertex,
			                                 VoxelVerts.Vert100.Vertex - VoxelVerts.Vert110.Vertex);
			
			Normals.Add (normal);
			Normals.Add (normal);


			Triangles.Add(VoxelVerts.Vert100.index);
			Triangles.Add(VoxelVerts.Vert000.index); 
			Triangles.Add(VoxelVerts.Vert110.index);

			Triangles.Add(VoxelVerts.Vert110.index);
			Triangles.Add(VoxelVerts.Vert000.index);
			Triangles.Add(VoxelVerts.Vert010.index);

//			UVs.Add(new Vector2(0,0));	
//			UVs.Add(new Vector2(1,0));
//			UVs.Add(new Vector2(1,1));
//			UVs.Add(new Vector2(0,1));	


			VoxelVerts.Vert000.used = true; 
			VoxelVerts.Vert100.used = true; 
			VoxelVerts.Vert110.used = true; 
			VoxelVerts.Vert010.used = true; 

		}
		if(!n.back.filled)
		{
//			Verts.Add (VoxelVerts.Vert001); //0
//			Verts.Add (VoxelVerts.Vert101); //1
//			Verts.Add (VoxelVerts.Vert111); //2
//			Verts.Add (VoxelVerts.Vert011); //3

			Vector3 normal = Vector3.Cross (VoxelVerts.Vert001.Vertex - VoxelVerts.Vert011.Vertex,
			                                VoxelVerts.Vert001.Vertex - VoxelVerts.Vert111.Vertex );
			                               
			
			Normals.Add (normal);
			Normals.Add (normal);


			Triangles.Add(VoxelVerts.Vert001.index);
			Triangles.Add(VoxelVerts.Vert111.index);
			Triangles.Add(VoxelVerts.Vert011.index);


			Triangles.Add(VoxelVerts.Vert001.index); 
			Triangles.Add(VoxelVerts.Vert101.index);
			Triangles.Add(VoxelVerts.Vert111.index);



//			UVs.Add(new Vector2(0,0));	
//			UVs.Add(new Vector2(1,0));
//			UVs.Add(new Vector2(1,1));
//			UVs.Add(new Vector2(0,1));		

			VoxelVerts.Vert001.used = true; 
			VoxelVerts.Vert101.used = true; 
			VoxelVerts.Vert111.used = true; 
			VoxelVerts.Vert011.used = true; 
		}
		foreach(Vert v in Verts)
		{
			v.used = true;
		}
		generated = true;
	}
}
