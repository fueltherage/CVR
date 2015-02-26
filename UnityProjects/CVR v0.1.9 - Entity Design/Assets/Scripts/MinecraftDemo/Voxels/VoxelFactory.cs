using UnityEngine;
using System.Collections;


public static class VoxelFactory {
	public static void GenerateVoxel(int VoxelNum,ref VoxelShell _voxelShell, Vector3 offsets,  float _spacing)
	{	
		//Work in progress, this will have unique initialization for each voxel type, for now they have different materials based on their type.
		switch(VoxelNum)
		{
		case 0:
			_voxelShell.filled = false;
			break;
		case 1:
			_voxelShell.filled = true;	
			break;		
		case 2:	
			_voxelShell.filled = true;	
			break;
		case 3:
			_voxelShell.filled = true;
			break;
		case 4:
			_voxelShell.filled = true;
			break;
		case 5:
			_voxelShell.filled = true;
			break;
		case 6:	
			_voxelShell.filled = true;
			break;
		case 7:	
			_voxelShell.filled = true;
			break;
		case 8:	
			_voxelShell.filled = true;
			break;
		case 9:
			_voxelShell.filled = true;
			break;	
		case 10:		        
			_voxelShell.filled = true;
			break;
		}
		_voxelShell.voxel.VoxelType = VoxelNum;//Set the type
		//_voxelShell.parentChunk.needsUpdating = true;
	}

}
