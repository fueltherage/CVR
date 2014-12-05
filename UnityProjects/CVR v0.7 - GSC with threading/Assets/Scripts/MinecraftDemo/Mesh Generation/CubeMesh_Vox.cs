using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CubeMesh_Vox : MeshData_Vox {
	Neighbours neighbours;
	Vector3 offset;
	float spacing;
	int faceCount;

	static Vector2[] Cube_UVs = { new Vector2 { x = 0, y = 0}, 
								  new Vector2 { x = 1, y = 0},
								  new Vector2 { x = 1, y = 1}, 
								  new Vector2 { x = 0, y = 1}};
	public CubeMesh_Vox()
	{

	}

	public CubeMesh_Vox(Neighbours _neighbours,Vector3 _offset,  float _spacing)
	{
		offset = _offset;
		neighbours = _neighbours;
		spacing = _spacing;
	}

    //Tested, works as intended
	public override int GenerateMesh(int _faceCount, VoxelPos _vp)
	{
		Triangles.Clear();
		UVs.Clear();
		Verts.Clear ();
		if(!neighbours.xpos.filled)
		{
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + _vp.y * spacing, offset.z + _vp.z * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + _vp.y * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + _vp.z * spacing));			
			
			Triangles.Add(new Triangle((_faceCount*4)+1, _faceCount*4,(_faceCount*4)+3));		
			Triangles.Add(new Triangle((_faceCount*4)+2,(_faceCount*4)+1,(_faceCount*4)+3));			
			
			UVs.Add(Cube_UVs[0]);
			UVs.Add(Cube_UVs[1]);
			UVs.Add(Cube_UVs[2]);
			UVs.Add(Cube_UVs[3]);

			_faceCount++; 
		}
		if(!neighbours.xneg.filled)
		{
			Verts.Add (new Vector3(offset.x + _vp.x * spacing, offset.y + _vp.y * spacing, offset.z + _vp.z * spacing));
			Verts.Add (new Vector3(offset.x + _vp.x * spacing, offset.y + _vp.y * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + _vp.x * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + _vp.x * spacing, offset.y + (_vp.y+1) * spacing, offset.z + _vp.z * spacing));

			Triangles.Add(new Triangle((_faceCount*4)+3, _faceCount*4,   (_faceCount*4)+1));		
			Triangles.Add(new Triangle((_faceCount*4)+1,(_faceCount*4)+2,(_faceCount*4)+3));		
			
			UVs.Add(Cube_UVs[0]);
			UVs.Add(Cube_UVs[1]);
			UVs.Add(Cube_UVs[2]);
			UVs.Add(Cube_UVs[3]);
			
			_faceCount++;    
		}
		if(!neighbours.ypos.filled)
		{
			Verts.Add (new Vector3(offset.x + (_vp.x) * spacing, offset.y + (_vp.y + 1) * spacing, offset.z + (_vp.z) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x) * spacing, offset.y + (_vp.y + 1) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z) * spacing));

			Triangles.Add(new Triangle((_faceCount*4)+3, _faceCount*4,   (_faceCount*4)+1));		
			Triangles.Add(new Triangle((_faceCount*4)+1,(_faceCount*4)+2,(_faceCount*4)+3));	


			
			UVs.Add(Cube_UVs[0]);
			UVs.Add(Cube_UVs[1]);
			UVs.Add(Cube_UVs[2]);
			UVs.Add(Cube_UVs[3]);
			
			_faceCount++;  
		}
		if(!neighbours.yneg.filled)
		{
			Verts.Add (new Vector3(offset.x + (_vp.x) * spacing, offset.y + (_vp.y) * spacing, offset.z + (_vp.z) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x) * spacing, offset.y + (_vp.y) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y) * spacing, offset.z + (_vp.z) * spacing));

			Triangles.Add(new Triangle((_faceCount*4),  (_faceCount*4)+3,(_faceCount*4)+1));		
			Triangles.Add(new Triangle((_faceCount*4)+2,(_faceCount*4)+1,(_faceCount*4)+3));	
			

			
			UVs.Add(Cube_UVs[0]);
			UVs.Add(Cube_UVs[1]);
			UVs.Add(Cube_UVs[2]);
			UVs.Add(Cube_UVs[3]);
			
			_faceCount++;  
		}
		if(!neighbours.zpos.filled)
		{
			Verts.Add (new Vector3(offset.x + (_vp.x) * spacing, offset.y + (_vp.y) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y) * spacing, offset.z + (_vp.z+1) * spacing));

			Triangles.Add(new Triangle((_faceCount*4),  (_faceCount*4)+3,(_faceCount*4)+1));		
			Triangles.Add(new Triangle((_faceCount*4)+2,(_faceCount*4)+1,(_faceCount*4)+3));	

			
			UVs.Add(Cube_UVs[0]);
			UVs.Add(Cube_UVs[1]);
			UVs.Add(Cube_UVs[2]);
			UVs.Add(Cube_UVs[3]);
			
			_faceCount++;  
		}
		if(!neighbours.zneg.filled)
		{
			Verts.Add (new Vector3(offset.x + (_vp.x) * spacing, offset.y + (_vp.y) * spacing, offset.z + (_vp.z) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y) * spacing, offset.z + (_vp.z) * spacing));	

			Triangles.Add(new Triangle((_faceCount*4)+3, _faceCount*4,(_faceCount*4)+1));		
			Triangles.Add(new Triangle((_faceCount*4)+1,(_faceCount*4)+2,(_faceCount*4)+3));	
			

			
			UVs.Add(Cube_UVs[0]);
			UVs.Add(Cube_UVs[1]);
			UVs.Add(Cube_UVs[2]);
			UVs.Add(Cube_UVs[3]);
			
			_faceCount++;  
		}

		return _faceCount;
	}
}
