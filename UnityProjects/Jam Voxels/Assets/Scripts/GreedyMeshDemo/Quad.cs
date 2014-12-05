using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Quad {
	public int submeshIndex;
	public Triangle[] triangles;
	public Vector2[] UVs;
	public Vector3[] verts;
	public Vector3 normal;

	//public Quad(){}
	public Quad(int _submeshIndex ,float _Width, float _Height, Vector3 _position, float _VoxelSpacing, VoxelPos normDirection, ref int _faceCount, VoxelPos _StartPos, ref VoxelSystemChunkGreedy _Chunk)
	{
		submeshIndex = _submeshIndex;
		normal = new Vector3(normDirection.x, normDirection.y, normDirection.z);
		triangles = new Triangle[2];


		triangles[0].verts = new int[3];
		triangles[1].verts = new int[3];

		UVs = new Vector2[4];
		verts = new Vector3[4];
		if(normDirection.x == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z);

			triangles[0].verts[0] = 1 + _faceCount*4;
			triangles[0].verts[1] = 0 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;


			triangles[1].verts[0] = 2 + _faceCount*4;
			triangles[1].verts[1] = 0 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;		

		}
		else if(normDirection.x == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z);
			
			triangles[0].verts[0] = 0 + _faceCount*4;
			triangles[0].verts[1] = 1 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;

			
			triangles[1].verts[0] = 0 + _faceCount*4;
			triangles[1].verts[1] = 2 + _faceCount*4;
			triangles[1].verts[2] = 3 + _faceCount*4;
		}
		else if(normDirection.y == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						    _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0].verts[0] = 0 + _faceCount*4;
			triangles[0].verts[1] = 1 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 0 + _faceCount*4;
			triangles[1].verts[1] = 2 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;
		}
		else if(normDirection.y == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						    _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0].verts[0] = 1 + _faceCount*4;
			triangles[0].verts[1] = 0 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 2 + _faceCount*4;
			triangles[1].verts[1] = 0 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;
		}
		else if(normDirection.z == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, 						  _position.y + _Width * _VoxelSpacing, _position.z );
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y + _Width * _VoxelSpacing, _position.z );
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0].verts[0] = 0 + _faceCount*4;
			triangles[0].verts[1] = 1 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 0 + _faceCount*4;
			triangles[1].verts[1] = 2 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;

		}else if(normDirection.z == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, 						  _position.y + _Width * _VoxelSpacing, _position.z );
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y + _Width * _VoxelSpacing, _position.z );
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0].verts[0] = 1 + _faceCount*4;
			triangles[0].verts[1] = 0 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 2 + _faceCount*4;
			triangles[1].verts[1] = 0 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;
		}
		float VSpace = _Chunk.systemParent.VoxelSpacing;
		VoxelPos ChunkSize = new VoxelPos(_Chunk.systemParent.ChunkSizeX,
		                             _Chunk.systemParent.ChunkSizeY,
		                             _Chunk.systemParent.ChunkSizeZ);
		VoxelPos SystemSize = new VoxelPos(_Chunk.systemParent.XSize,
		                                   _Chunk.systemParent.YSize,
		                                   _Chunk.systemParent.ZSize);
		float imageHeight = 100.0f/200.0f;
		float imageWidth = 100.0f/200.0f;
		VoxelPos cPos = _Chunk.chunkPos;

		UVs[0] = new Vector2((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),			   //0.0
		                     (_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace));           //0.0
	
		UVs[3] = new Vector2((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  //1.0
							 (_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace));           //0.0


		UVs[2] = new Vector2((_StartPos.x + _Height  + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),  //1.0
		                     (_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace));//1.0
	
		UVs[1] = new Vector2((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace),			   //0.0	
		                     (_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)); //1.0
	
		_faceCount++;

	}
	public Quad(int _submeshIndex ,float _Width, float _Height, Vector3 _position, float _VoxelSpacing, VoxelPos normDirection, ref int _faceCount)
	{
		submeshIndex = _submeshIndex;
		normal = new Vector3(normDirection.x, normDirection.y, normDirection.z);
		triangles = new Triangle[2];
		//		triangles[0] = new Triangle();
		//		triangles[1] = new Triangle();
		
		triangles[0].verts = new int[3];
		triangles[1].verts = new int[3];
		
		UVs = new Vector2[4];
		verts = new Vector3[4];
		if(normDirection.x == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z);
			
			triangles[0].verts[0] = 1 + _faceCount*4;
			triangles[0].verts[1] = 0 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 2 + _faceCount*4;
			triangles[1].verts[1] = 0 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;		
			
		}
		else if(normDirection.x == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z);
			
			triangles[0].verts[0] = 0 + _faceCount*4;
			triangles[0].verts[1] = 1 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 0 + _faceCount*4;
			triangles[1].verts[1] = 2 + _faceCount*4;
			triangles[1].verts[2] = 3 + _faceCount*4;
		}
		else if(normDirection.y == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						    _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0].verts[0] = 0 + _faceCount*4;
			triangles[0].verts[1] = 1 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 0 + _faceCount*4;
			triangles[1].verts[1] = 2 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;
		}
		else if(normDirection.y == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						    _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0].verts[0] = 1 + _faceCount*4;
			triangles[0].verts[1] = 0 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 2 + _faceCount*4;
			triangles[1].verts[1] = 0 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;
		}
		else if(normDirection.z == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, 						  _position.y + _Width * _VoxelSpacing, _position.z );
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y + _Width * _VoxelSpacing, _position.z );
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0].verts[0] = 0 + _faceCount*4;
			triangles[0].verts[1] = 1 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 0 + _faceCount*4;
			triangles[1].verts[1] = 2 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;
			
		}else if(normDirection.z == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, 						  _position.y + _Width * _VoxelSpacing, _position.z );
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y + _Width * _VoxelSpacing, _position.z );
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0].verts[0] = 1 + _faceCount*4;
			triangles[0].verts[1] = 0 + _faceCount*4;
			triangles[0].verts[2] = 2 + _faceCount*4;
			
			
			triangles[1].verts[0] = 2 + _faceCount*4;
			triangles[1].verts[1] = 0 + _faceCount*4;					
			triangles[1].verts[2] = 3 + _faceCount*4;
		}
		UVs[0] = new Vector2(0.0f ,				 0.0f );
		UVs[1] = new Vector2(1.0f*(float)_Width, 0.0f);
		UVs[2] = new Vector2(1.0f*(float)_Width, 1.0f*(float)_Height);
		UVs[3] = new Vector2(0.0f,				 1.0f*(float)_Height);
		_faceCount++;
		
	}


}
