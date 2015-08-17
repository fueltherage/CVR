using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct JamQuad {
	public int submeshIndex;
	public int[] triangles;
	public Vector2[] UVs;
	public Vector3[] verts;
	public Vector3 normal;
	public bool init;
	//public JamQuad(){}
	public JamQuad(int _submeshIndex ,
	               float _Width, 
	               float _Height, 
	               Vector3 _position, 
	               float _VoxelSpacing, 
	               VoxelPos normDirection, 
	               ref int _faceCount, 
	               VoxelPos _StartPos, 
	               ref VoxelSystemChunkGreedy _Chunk,
	               Vector3 UVRatio,
	               bool UniqueSides)
	{
		submeshIndex = _submeshIndex;
		normal = new Vector3(normDirection.x, normDirection.y, normDirection.z) ;
		triangles = new int[6];				
		UVs = new Vector2[4];
		verts = new Vector3[4];
		
		//Used in UV calc
        float VSpace =1.0f ;
		VoxelPos ChunkSize = new VoxelPos(_Chunk.systemParent.ChunkSizeX,
		                                  _Chunk.systemParent.ChunkSizeY,
		                                  _Chunk.systemParent.ChunkSizeZ);
		VoxelPos SystemSize = new VoxelPos(_Chunk.systemParent.XSize,
		                                   _Chunk.systemParent.YSize,
		                                   _Chunk.systemParent.ZSize);
		
		VoxelPos cPos = _Chunk.chunkPos;
		
		
		//Notes Image is in a 2x2 format 64x64px
		//See ship example for texture layout
		float Xnum;//Amount of images along the x and y axis
		float Ynum;//
		float Ximage; //1.0f / Xnum;
		float Yimage;//1.0f / Ynum;
        Vector3 ImageManip;
		if(UniqueSides)
		{
			
			Xnum = 4.0f;
			Ximage = 1.0f / Xnum;
			Ynum = 4.0f;
			Yimage = 1.0f / Ynum;
            ImageManip = new Vector3(1.0f,1.0f,1.0f);

		}
		else 
		{
			Xnum = 1.0f;
			Ximage = 0.0f;
			Ynum = 1.0f;
			Yimage = 0.0f;
            ImageManip = new Vector3(1.0f,1.0f,1.0f);
		}


		
		if(normDirection.x == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z);
			
			triangles[0] = 1 + _faceCount*4;
			triangles[1] = 0 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;			
			triangles[3] = 2 + _faceCount*4;
			triangles[4] = 0 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;	

			UVs[0].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace) / Ynum +Yimage )/ UVRatio.y; //0.0
			UVs[0].x = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace) / Xnum  )/ UVRatio.z; //0.0			          

            UVs[1].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage) / UVRatio.y; 	     //0.0
			UVs[1].x = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace) / Xnum )/ UVRatio.z;//1.0;			             

            UVs[2].y = ((_StartPos.y + _Height + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage) / UVRatio.y; //1.0
			UVs[2].x = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace) / Xnum )/ UVRatio.z; //1.0		

            UVs[3].y = ((_StartPos.y + _Height + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage) / UVRatio.y; //1.0
			UVs[3].x = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace) / Xnum)/ UVRatio.z;		//0.0 

		}
		else if(normDirection.x == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z);
			
			triangles[0] = 0 + _faceCount*4;
			triangles[1] = 1 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;
			triangles[3] = 0 + _faceCount*4;
			triangles[4] = 2 + _faceCount*4;
			triangles[5] = 3 + _faceCount*4;

            UVs[0].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage) / UVRatio.y;			 //0.0
			UVs[0].x = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Xnum + Ximage )/ UVRatio.z;           //0.0

            UVs[1].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage) / UVRatio.y;			 //0.0	
			UVs[1].x = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace) /Xnum + Ximage )/ UVRatio.z;  //1.0			

            UVs[2].y = ((_StartPos.y + _Height + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage) / UVRatio.y; //1.0
			UVs[2].x =((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Xnum + Ximage)/ UVRatio.z;  //1.0	


            UVs[3].y = ((_StartPos.y + _Height + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage) / UVRatio.y;  //1.0
			UVs[3].x = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/ Xnum + Ximage)/ UVRatio.z;           //0.0
			
		}
		else if(normDirection.y == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y, _position.z);
			
			triangles[0] = 0 + _faceCount*4;
			triangles[1] = 1 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;
			triangles[3] = 0 + _faceCount*4;
			triangles[4] = 2 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;
			
			UVs[0].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x;			 //0.0
            UVs[0].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace) / (ChunkSize.z * SystemSize.z * VSpace) / (Ynum * ImageManip.y) + Yimage * 3) / UVRatio.z;           //0.0

            UVs[1].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace) / Xnum + Ximage) / UVRatio.x;			//0.0	
            UVs[1].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace) / (ChunkSize.z * SystemSize.z * VSpace) / (Ynum * ImageManip.y) + Yimage * 3) / UVRatio.z; //1.0

            UVs[2].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace) / Xnum + Ximage) / UVRatio.x; //1.0
            UVs[2].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace) / (ChunkSize.z * SystemSize.z * VSpace) / (Ynum * ImageManip.y) + Yimage * 3) / UVRatio.z; //1.0			

            UVs[3].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace) / Xnum + Ximage) / UVRatio.x;  //1.0
            UVs[3].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace) / (ChunkSize.z * SystemSize.z * VSpace) / (Ynum * ImageManip.y) + Yimage * 3) / UVRatio.z;           //0.0
			
			
		}
		else if(normDirection.y == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						    _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0] = 1 + _faceCount*4;
			triangles[1] = 0 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;
			triangles[3] = 2 + _faceCount*4;
			triangles[4] = 0 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;
			
			UVs[0].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum )/ UVRatio.x;			  //0.0
			UVs[0].y =((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/ (Ynum*ImageManip.y) + Yimage * 3 )/ UVRatio.z;            //0.0
			
			UVs[1].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x;			  //0.0	
            UVs[1].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace) / (ChunkSize.z * SystemSize.z * VSpace) / (Ynum * ImageManip.y) + Yimage * 3) / UVRatio.z;  //1.0
			
			
			UVs[2].x = ((_StartPos.x + _Height  + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x; //1.0
            UVs[2].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace) / (ChunkSize.z * SystemSize.z * VSpace) / (Ynum * ImageManip.y) + Yimage * 3) / UVRatio.z;  //1.0
			
			UVs[3].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x;  //1.0
            UVs[3].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace) / (ChunkSize.z * SystemSize.z * VSpace) / (Ynum * ImageManip.y) + Yimage * 3) / UVRatio.z;           //0.0
			
			
		}
		else if(normDirection.z == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, 						  _position.y + _Width * _VoxelSpacing, _position.z );
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y + _Width * _VoxelSpacing, _position.z );
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0] = 1 + _faceCount*4;
			triangles[1] = 0 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;			
			triangles[3] = 2 + _faceCount*4;
			triangles[4] = 0 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;
			
			UVs[0].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace) / Xnum + Ximage)/ UVRatio.x;			  //0.0
            UVs[0].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2) / UVRatio.y;           //0.0
			
			UVs[1].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x;			  //0.0	
            UVs[1].y = ((_StartPos.y + _Width + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2) / UVRatio.y;  //1.0			
			
			UVs[2].x = ((_StartPos.x + _Height  + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace)/Xnum +  Ximage)/ UVRatio.x; //1.0
			UVs[2].y = ((_StartPos.y + _Width + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace)/Ynum + Yimage * 2)/ UVRatio.y;  //1.0
			
			UVs[3].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x;  //1.0
            UVs[3].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2) / UVRatio.y;           //0.0		

        }
        else if (normDirection.z == -1)
        {
            verts[0] = _position;
            verts[1] = new Vector3(_position.x, _position.y + _Width * _VoxelSpacing, _position.z);
            verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y + _Width * _VoxelSpacing, _position.z);
            verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y, _position.z);

            triangles[0] = 0 + _faceCount * 4;
            triangles[1] = 1 + _faceCount * 4;
            triangles[2] = 2 + _faceCount * 4;
            triangles[3] = 0 + _faceCount * 4;
            triangles[4] = 2 + _faceCount * 4;
            triangles[5] = 3 + _faceCount * 4;

            UVs[0].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace) / Xnum) / UVRatio.x;			  //0.0
            UVs[0].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2) / UVRatio.y;           //0.0

            UVs[1].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace) / Xnum ) / UVRatio.x;			  //0.0	
            UVs[1].y = ((_StartPos.y + _Width + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2) / UVRatio.y;  //1.0			

            UVs[2].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace) / Xnum) / UVRatio.x; //1.0
            UVs[2].y = ((_StartPos.y + _Width + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2) / UVRatio.y;  //1.0

            UVs[3].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace) / (ChunkSize.x * SystemSize.x * VSpace) / Xnum) / UVRatio.x;  //1.0
            UVs[3].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace) / (ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2) / UVRatio.y;           //0.0




        }
		_faceCount++;
        

		init = true;
	}


	public void GenerateQuad(int _submeshIndex ,
	                         float _Width, 
	                         float _Height, 
	                         Vector3 _position, 
	                         float _VoxelSpacing, 
	                         VoxelPos normDirection, 
	                         ref int _faceCount, 
	                         VoxelPos _StartPos, 
	                         ref VoxelSystemChunkGreedy _Chunk,
	                         Vector3 UVRatio,
	                         bool UniqueSides)
	{
		submeshIndex = _submeshIndex;
		normal = new Vector3(normDirection.x, normDirection.y, normDirection.z) ;

		
		//Used in UV calc
		float VSpace = 1;
		VoxelPos ChunkSize = new VoxelPos(_Chunk.systemParent.ChunkSizeX,
		                                  _Chunk.systemParent.ChunkSizeY,
		                                  _Chunk.systemParent.ChunkSizeZ);
		VoxelPos SystemSize = new VoxelPos(_Chunk.systemParent.XSize,
		                                   _Chunk.systemParent.YSize,
		                                   _Chunk.systemParent.ZSize);
		
		VoxelPos cPos = _Chunk.chunkPos;
		
		
		//Notes Image is in a 2x2 format 64x64px
		//top left is top/bot
		//bot left is side
		//top right is front
		//bot right is back
		float Xnum;//Amount of images along the x and y axis
		float Ynum;//
		float Ximage; //1.0f / Xnum;
		float Yimage;//1.0f / Ynum;
		
		if(UniqueSides)
		{

			Xnum = 4.0f;
			Ximage = 1.0f / Xnum;
			Ynum = 4.0f;
			Yimage = 1.0f / Ynum;
		}
		else 
		{
			Xnum = 1.0f;
			Ximage = 0.0f;
			Ynum = 1.0f;
			Yimage = 0.0f;
		}

		
		if(normDirection.x == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z);
			
			triangles[0] = 1 + _faceCount*4;
			triangles[1] = 0 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;			
			triangles[3] = 2 + _faceCount*4;
			triangles[4] = 0 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;	
			
			UVs[0].x = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)           / (ChunkSize.y * SystemSize.y * VSpace) / Xnum + Ximage )/ UVRatio.y; //0.0
			UVs[0].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)           / (ChunkSize.z * SystemSize.z * VSpace) / Ynum + Yimage )/ UVRatio.z; //0.0			          
			
			UVs[1].x = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)           / (ChunkSize.y * SystemSize.y * VSpace) / Xnum + Ximage )/ UVRatio.y; 	     //0.0
			UVs[1].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)  / (ChunkSize.z * SystemSize.z * VSpace) / Ynum + Yimage )/ UVRatio.z;//1.0;			             
			
			UVs[2].x = ((_StartPos.y + _Height  + cPos.y * ChunkSize.y * VSpace)/ (ChunkSize.y * SystemSize.y * VSpace) / Xnum + Ximage) / UVRatio.y; //1.0
			UVs[2].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)  / (ChunkSize.z * SystemSize.z * VSpace) / Ynum + Yimage) / UVRatio.z; //1.0		
			
			UVs[3].x = ((_StartPos.y + _Height + cPos.y * ChunkSize.y * VSpace) /(ChunkSize.y * SystemSize.y * VSpace) / Xnum + Ximage)/ UVRatio.y; //1.0
			UVs[3].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)           /(ChunkSize.z * SystemSize.z * VSpace) / Ynum + Yimage)/ UVRatio.z;		//0.0 
			
		}
		else if(normDirection.x == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x, _position.y + _Height * _VoxelSpacing, _position.z);
			
			triangles[0] = 0 + _faceCount*4;
			triangles[1] = 1 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;
			triangles[3] = 0 + _faceCount*4;
			triangles[4] = 2 + _faceCount*4;
			triangles[5] = 3 + _faceCount*4; 
			
			UVs[0].x = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace)/Xnum)/ UVRatio.y;			 //0.0
			UVs[0].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Ynum + Yimage)/ UVRatio.z;           //0.0
			
			UVs[1].x = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace)/Xnum)/ UVRatio.y;			 //0.0	
			UVs[1].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Ynum + Yimage)/ UVRatio.z;  //1.0			
			
			UVs[2].x =((_StartPos.y + _Height  + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace)/Xnum)/ UVRatio.y; //1.0
			UVs[2].y =((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Ynum +Yimage)/ UVRatio.z;  //1.0	
			
			
			UVs[3].x = ((_StartPos.y + _Height + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace)/Xnum)/ UVRatio.y;  //1.0
			UVs[3].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Ynum +Yimage)/ UVRatio.z;           //0.0
			
		}
		else if(normDirection.y == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						   _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y, _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y, _position.z);
			
			triangles[0] = 0 + _faceCount*4;
			triangles[1] = 1 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;
			triangles[3] = 0 + _faceCount*4;
			triangles[4] = 2 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;
			
			UVs[0].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x;			 //0.0
			UVs[0].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Ynum + Yimage*3)/ UVRatio.z;           //0.0
			
			UVs[1].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x;			//0.0	
			UVs[1].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Ynum + Yimage*3)/ UVRatio.z; //1.0
			
			UVs[2].x =  ((_StartPos.x + _Height  + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x; //1.0
			UVs[2].y =  ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Ynum + Yimage*3)/ UVRatio.z; //1.0			
			
			UVs[3].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x;  //1.0
			UVs[3].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/Ynum + Yimage*3)/ UVRatio.z;           //0.0
			
			
		}
		else if(normDirection.y == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, _position.y, 						    _position.z + _Width * _VoxelSpacing);
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z + _Width * _VoxelSpacing);
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0] = 1 + _faceCount*4;
			triangles[1] = 0 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;
			triangles[3] = 2 + _faceCount*4;
			triangles[4] = 0 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;
			
			UVs[0].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x;			  //0.0
			UVs[0].y =((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/ Ynum + Yimage * 3)/ UVRatio.z;            //0.0
			
			UVs[1].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x;			  //0.0	
			UVs[1].y =((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/ Ynum + Yimage * 3)/ UVRatio.z;  //1.0
			
			
			UVs[2].x = ((_StartPos.x + _Height  + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x; //1.0
			UVs[2].y =((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/ Ynum + Yimage * 3)/ UVRatio.z;  //1.0
			
			UVs[3].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x;  //1.0
			UVs[3].y =  ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)/(ChunkSize.z * SystemSize.z * VSpace)/ Ynum + Yimage * 3)/ UVRatio.z;           //0.0
			
			
		}
		else if(normDirection.z == -1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, 						  _position.y + _Width * _VoxelSpacing, _position.z );
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y + _Width * _VoxelSpacing, _position.z );
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0] = 0 + _faceCount*4;
			triangles[1] = 1 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;
			triangles[3] = 0 + _faceCount*4;
			triangles[4] = 2 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;
			
			UVs[0].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x;			  //0.0
			UVs[0].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace)/Ynum + Yimage * 2)/ UVRatio.y;           //0.0
			
			UVs[1].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x;			  //0.0	
			UVs[1].y =((_StartPos.y + _Width + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 3)/ UVRatio.y;  //1.0			
			
			UVs[2].x = ((_StartPos.x + _Height  + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x; //1.0
			UVs[2].y =  ((_StartPos.y + _Width + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 3 )/ UVRatio.y;  //1.0
			
			UVs[3].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum)/ UVRatio.x;  //1.0
			UVs[3].y = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2)/ UVRatio.y;           //0.0
			
			
			
			
		}else if(normDirection.z == 1)
		{
			verts[0] = _position;
			verts[1] = new Vector3(_position.x, 						  _position.y + _Width * _VoxelSpacing, _position.z );
			verts[2] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y + _Width * _VoxelSpacing, _position.z );
			verts[3] = new Vector3(_position.x + _Height * _VoxelSpacing, _position.y , _position.z);
			
			triangles[0] = 1 + _faceCount*4;
			triangles[1] = 0 + _faceCount*4;
			triangles[2] = 2 + _faceCount*4;			
			triangles[3] = 2 + _faceCount*4;
			triangles[4] = 0 + _faceCount*4;					
			triangles[5] = 3 + _faceCount*4;
			
			UVs[0].x = ((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace) / Xnum + Ximage)/ UVRatio.x;			  //0.0
			UVs[0].y =((_StartPos.y + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace) / Ynum + Yimage * 2)/ UVRatio.y;           //0.0
			
			UVs[1].x =((_StartPos.x + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x;			  //0.0	
			UVs[1].y =((_StartPos.y + _Width + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace)/Ynum+Yimage * 2)/ UVRatio.y;  //1.0			
			
			UVs[2].x = ((_StartPos.x + _Height  + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum+  Ximage)/ UVRatio.x; //1.0
			UVs[2].y =((_StartPos.y + _Width + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace)/Ynum+Yimage * 2)/ UVRatio.y;  //1.0
			
			UVs[3].x = ((_StartPos.x + _Height + cPos.x * ChunkSize.x * VSpace)/(ChunkSize.x * SystemSize.x * VSpace)/Xnum + Ximage)/ UVRatio.x;  //1.0
			UVs[3].y =((_StartPos.y + cPos.y * ChunkSize.y * VSpace)/(ChunkSize.y * SystemSize.y * VSpace)/Ynum+Yimage * 2)/ UVRatio.y;           //0.0		
			
		}
		_faceCount++;
	}
	/*
	public JamQuad(int _submeshIndex ,float _Width, float _Height, Vector3 _position, float _VoxelSpacing, VoxelPos normDirection, ref int _faceCount)
	{
		submeshIndex = _submeshIndex;
		normal = new Vector3(normDirection.x, normDirection.y, normDirection.z);
		triangles = new int[6];
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
			
			triangles[0] = 1 + _faceCount*4;
			triangles[1] = 0 + _faceCount*4;
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
		init = true;
	}
	*/


}
