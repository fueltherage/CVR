using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class QuadFace_NoBottom : MeshData_Vox {
	Neighbours neighbours;
	Vector3 offset;
	float spacing;
	Vector2 pos;
	int faceCount;
	int textureWidth = 2;
	VoxelSystem vs;

	static float d = 32.0f;
	static float w = 32.0f;
	static float h = 32.0f;
	static Vector2[] Cube_UVs = { new Vector2 { x = 0,   y = 0}, 
								  new Vector2 { x = 0,   y = 1/h},
								  new Vector2 { x = 1/w, y = 1/h}, 
								  new Vector2 { x = 1/w, y = 0} };
	public QuadFace_NoBottom()
	{

	}

	public QuadFace_NoBottom(Neighbours _neighbours,Vector3 _offset,  float _spacing, Vector2 _position)
	{
		pos = _position;
		offset = _offset;
		neighbours = _neighbours;
		spacing = _spacing;
	}
	public QuadFace_NoBottom(Neighbours _neighbours,Vector3 _offset,  float _spacing, Vector2 _position, VoxelSystem _vs)
	{
		pos = _position;
		offset = _offset;
		neighbours = _neighbours;
		spacing = _spacing;
		vs = _vs;
	}

    //Tested, works as intended
	public override int GenerateMesh(int _faceCount, VoxelPos _vp, VoxelSystemChunk _Chunk)
	{


		Vector2 temp = new Vector2(_vp.x/d, _vp.z/d);


		Triangles.Clear();
		UVs.Clear();
		Verts.Clear ();

		float VSpace = vs.VoxelSpacing;
		VoxelPos ChunkSize = new VoxelPos(vs.ChunkSizeX,
		                                  vs.ChunkSizeY,
		                                  vs.ChunkSizeZ);
		
		VoxelPos SystemSize = new VoxelPos(vs.XSize,
		                                   vs.YSize,
		                                   vs.ZSize);
		
		VoxelPos cPos = _Chunk.chunkPos;
		if(!neighbours.xpos.filled)
		{
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + _vp.y * spacing, offset.z + _vp.z * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + _vp.y * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + (_vp.z+1) * spacing));
			Verts.Add (new Vector3(offset.x + (_vp.x+1) * spacing, offset.y + (_vp.y+1) * spacing, offset.z + _vp.z * spacing));			
			
			Triangles.Add(new Triangle((_faceCount*4)+1, _faceCount*4,(_faceCount*4)+3));		
			Triangles.Add(new Triangle((_faceCount*4)+2,(_faceCount*4)+1,(_faceCount*4)+3));			

			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),		//0.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
			
			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),	    //0.0	
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));  //1.0
			
			UVs.Add(new Vector2((_vp.x +1+ cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  	//1.0
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));	//1.0
			
			UVs.Add(new Vector2((_vp.x + 1 +cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),   //1.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0

//			UVs.Add(Cube_UVs[0] + temp);
//			UVs.Add(Cube_UVs[1] + temp);
//			UVs.Add(Cube_UVs[2] + temp);
//			UVs.Add(Cube_UVs[3] + temp);

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

			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),		//0.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
			
			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),	    //0.0	
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));  //1.0
			
			UVs.Add(new Vector2((_vp.x +1+ cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  	//1.0
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));	//1.0
			
			UVs.Add(new Vector2((_vp.x + 1 +cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),   //1.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
//			UVs.Add(Cube_UVs[0]+ temp);
//			UVs.Add(Cube_UVs[1]+ temp);
//			UVs.Add(Cube_UVs[2]+ temp);
//			UVs.Add(Cube_UVs[3]+ temp);
//			
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

			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),		//0.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
			
			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),	    //0.0	
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));  //1.0
			
			UVs.Add(new Vector2((_vp.x +1+ cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  	//1.0
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));	//1.0
			
			UVs.Add(new Vector2((_vp.x + 1 +cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),   //1.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
			
//			UVs.Add(Cube_UVs[0]+ temp);
//			UVs.Add(Cube_UVs[1]+ temp);
//			UVs.Add(Cube_UVs[2]+ temp);
//			UVs.Add(Cube_UVs[3]+ temp);
			
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

			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),		//0.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
			
			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),	    //0.0	
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));  //1.0
			
			UVs.Add(new Vector2((_vp.x +1+ cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  	//1.0
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));	//1.0
			
			UVs.Add(new Vector2((_vp.x + 1 +cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),   //1.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0

			
//			UVs.Add(Cube_UVs[0]+ temp);
//			UVs.Add(Cube_UVs[1]+ temp);
//
//			UVs.Add(Cube_UVs[2]+ temp);
//			UVs.Add(Cube_UVs[3]+ temp);
			
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

			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),		//0.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
			
			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),	    //0.0	
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));  //1.0
			
			UVs.Add(new Vector2((_vp.x +1+ cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  	//1.0
			                    (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));	//1.0
			
			UVs.Add(new Vector2((_vp.x +1+cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),   //1.0
			                    (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
			
//			UVs.Add(Cube_UVs[0]+ temp);
//			UVs.Add(Cube_UVs[1]+ temp);
//			UVs.Add(Cube_UVs[2]+ temp);
//			UVs.Add(Cube_UVs[3]+ temp);
			
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
			
			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),		//0.0
	                     		(_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0

			UVs.Add(new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),	    //0.0	
	                     		(_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));  //1.0

			UVs.Add(new Vector2((_vp.x +1+ cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  	//1.0
	                     		(_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));	//1.0

			UVs.Add(new Vector2((_vp.x + 1 +cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),   //1.0
	                     		(_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)));    //0.0
			
//			UVs.Add(Cube_UVs[0]+ temp);
//			UVs.Add(Cube_UVs[1]+ temp);
//			UVs.Add(Cube_UVs[2]+ temp);
//			UVs.Add(Cube_UVs[3]+ temp);
			
			_faceCount++;  
		}


//		UVs[0] = new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),			   //0.0
//		                     (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace));           //0.0//		
//		UVs[1] = new Vector2((_vp.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),			   //0.0	
//		                     (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)); //1.0
//
//		UVs[2] = new Vector2((_vp.x +1+ cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  //1.0
//		                     (_vp.z +1+ cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace));//1.0
//
//		UVs[3] = new Vector2((_vp.x + 1 +cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  //1.0
//		                     (_vp.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace));           //0.0
		return _faceCount;
	}
}
