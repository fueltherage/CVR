using UnityEngine;
using System.Collections;

public class CubePool : MonoBehaviour {

	public GameObject CubeVoxelGO;
	public int PoolSize=1;

	GameObject[] pool;

	// Use this for initialization
	void Start () {
		pool = new GameObject[PoolSize];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	class Cube
	{
		public Vector3[] verts;
		public int[] tris;
		public Vector2[] UVs;
		public Cube(float _Spacing, Vector3 _WorldPos, ref VoxelSystemChunkGreedy _chunk)
		{
			verts = new Vector3[24];
			tris = new int[12*3];

			//XPos

			_Spacing *= 0.5f; 
			int _faceCount = 0;
			int offset = _faceCount * 4;
			verts[0 + offset] = new Vector3(_WorldPos.x + _Spacing, _WorldPos.y + _Spacing, _WorldPos.z + _Spacing);
			verts[1 + offset] = new Vector3(_WorldPos.x + _Spacing, _WorldPos.y - _Spacing, _WorldPos.z + _Spacing);
			verts[2 + offset] = new Vector3(_WorldPos.x + _Spacing, _WorldPos.y + _Spacing, _WorldPos.z - _Spacing);
			verts[3 + offset] = new Vector3(_WorldPos.x + _Spacing, _WorldPos.y - _Spacing, _WorldPos.z - _Spacing);
			
			tris[0 + offset] = 1 + _faceCount*4;
			tris[1 + offset] = 0 + _faceCount*4;
			tris[2 + offset] = 2 + _faceCount*4;			
			tris[3 + offset] = 2 + _faceCount*4;
			tris[4 + offset] = 0 + _faceCount*4;					
			tris[5 + offset] = 3 + _faceCount*4;	
			
//			UVs[0 + offset].x = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)           / (ChunkSize.y * SystemSize.y * VSpace) / Xnum + Ximage )/ UVRatio.y; //0.0
//			UVs[0 + offset].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)           / (ChunkSize.z * SystemSize.z * VSpace) / Ynum + Yimage )/ UVRatio.z; //0.0			          
//			
//			UVs[1 + offset].x = ((_StartPos.y + cPos.y * ChunkSize.y * VSpace)           / (ChunkSize.y * SystemSize.y * VSpace) / Xnum + Ximage )/ UVRatio.y; 	     //0.0
//			UVs[1 + offset].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)  / (ChunkSize.z * SystemSize.z * VSpace) / Ynum + Yimage )/ UVRatio.z;//1.0;			             
//			
//			UVs[2 + offset].x = ((_StartPos.y + _Height  + cPos.y * ChunkSize.y * VSpace)/ (ChunkSize.y * SystemSize.y * VSpace) / Xnum + Ximage) / UVRatio.y; //1.0
//			UVs[2 + offset].y = ((_StartPos.z + _Width + cPos.z * ChunkSize.z * VSpace)  / (ChunkSize.z * SystemSize.z * VSpace) / Ynum + Yimage) / UVRatio.z; //1.0		
//			
//			UVs[3 + offset].x = ((_StartPos.y + _Height + cPos.y * ChunkSize.y * VSpace) /(ChunkSize.y * SystemSize.y * VSpace) / Xnum + Ximage)/ UVRatio.y; //1.0
//			UVs[3 + offset].y = ((_StartPos.z + cPos.z * ChunkSize.z * VSpace)           /(ChunkSize.z * SystemSize.z * VSpace) / Ynum + Yimage)/ UVRatio.z;		//0.0 




		}



	}
}
