using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelEvents : MonoBehaviour {

	public delegate void VoxelEventHandler(VoxelPos voxel, bool update);
	public delegate void VoxelSelectionEventHandler(int currentSelectedVoxel);
	public static event VoxelSelectionEventHandler onVoxelSwitch;
	public static event VoxelEventHandler onVoxelAdded;
	public static event VoxelEventHandler onVoxelRemoved;



	public static void VoxelSwitched(int currentSelectedVoxel)
	{
		if (onVoxelSwitch != null) 
			onVoxelSwitch (currentSelectedVoxel);
		
	}
	public static void VoxelAdded(VoxelPos voxel,bool update)
	{
		if (onVoxelAdded != null) 
						onVoxelAdded (voxel,update);
				
	}
	public static void VoxelRemoved(VoxelPos voxel,bool update)
	{
		if (onVoxelRemoved != null)
						onVoxelRemoved (voxel,update);
	}
}
