using UnityEngine;
using System.Collections;


public static class VoxelFactory {
	public static Voxel GenerateVoxel(int VoxelNum,ref Voxel _voxel, Vector3 offsets,  float _spacing)
	{	
		switch(VoxelNum)
		{
		case 0:
			_voxel.meshData = new MeshData_Vox();
			break;
		case 1:		

			_voxel.LoadMesh(new CubeMesh_Vox(_voxel.neighbours, offsets, _spacing));              
			break;		
		case 2:		

			_voxel.LoadMesh(new CubeMesh_Vox(_voxel.neighbours, offsets, _spacing));              
			break;
		}
	
	_voxel.VoxelType = VoxelNum;
	return _voxel;

	}

}
