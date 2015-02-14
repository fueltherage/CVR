using UnityEngine;
using System.Collections;


public static class VoxelFactory {
	public static void GenerateVoxel(int VoxelNum,ref VoxelShell _voxelShell, Vector3 offsets,  float _spacing)
	{	
		switch(VoxelNum)
		{
		case 0:
			_voxelShell.filled = false;
			//_voxelShell.voxel.meshData = new MeshData_Vox();
			break;
		case 1:		
			_voxelShell.voxel = new Voxel();
			_voxelShell.voxel.LoadMesh(new CubeMesh_Vox(_voxelShell.neighbours, offsets, _spacing));
			_voxelShell.filled = true;

		
			break;		
		case 2:		
			_voxelShell.voxel = new Voxel();
			_voxelShell.voxel.LoadMesh(new CubeMesh_Vox(_voxelShell.neighbours, offsets, _spacing)); 
			_voxelShell.filled = true;
	
			break;
		case 3:		
			_voxelShell.voxel = new Voxel();
			_voxelShell.voxel.LoadMesh(new CubeMesh_Vox(_voxelShell.neighbours, offsets, _spacing));              
			_voxelShell.filled = true;

			break;
		case 4:		
			_voxelShell.voxel = new Voxel();
			_voxelShell.voxel.LoadMesh(new CubeMesh_Vox(_voxelShell.neighbours, offsets, _spacing));              
			_voxelShell.filled = true;
			break;
		case 5:		
			_voxelShell.voxel = new Voxel();
			_voxelShell.voxel.LoadMesh(new CubeMesh_Vox(_voxelShell.neighbours, offsets, _spacing));              
			_voxelShell.filled = true;
			break;
		case 6:		
			_voxelShell.voxel = new Voxel();
			_voxelShell.voxel.LoadMesh(new CubeMesh_Vox(_voxelShell.neighbours, offsets, _spacing));              
			_voxelShell.filled = true;
			break;
		case 7:		
			_voxelShell.voxel = new Voxel();
			_voxelShell.voxel.LoadMesh(new CubeMesh_Vox(_voxelShell.neighbours, offsets, _spacing));              
			_voxelShell.filled = true;
			break;
		case 8:		
			_voxelShell.voxel = new Voxel();
			_voxelShell.voxel.LoadMesh(new CubeMesh_Vox(_voxelShell.neighbours, offsets, _spacing));              
			_voxelShell.filled = true;
			break;
		}

		_voxelShell.voxel.VoxelType = VoxelNum;

	}

}
