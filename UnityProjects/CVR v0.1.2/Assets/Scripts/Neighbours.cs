using UnityEngine;
using System.Collections;

public class Neighbours {
	public VoxelShell top;
	public VoxelShell bot;
	public VoxelShell left;
	public VoxelShell right;
	public VoxelShell front;
	public VoxelShell back;
	public Neighbours(ref VoxelShell _top, ref VoxelShell _bot, ref VoxelShell _left, ref VoxelShell _right, ref VoxelShell _front, ref VoxelShell _back)
	{
		top = _top;
		bot = _bot; 
		left = _left; 
		right = _right;
		front = _front;
		back = _back;
	}

}
