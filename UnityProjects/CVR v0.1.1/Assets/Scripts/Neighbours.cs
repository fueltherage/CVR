using UnityEngine;
using System.Collections;

public class Neighbours {
	public Voxel top;
	public Voxel bot;
	public Voxel left;
	public Voxel right;
	public Voxel front;
	public Voxel back;
	public Neighbours(ref Voxel _top, ref Voxel _bot, ref Voxel _left, ref Voxel _right, ref Voxel _front, ref Voxel _back)
	{
		top = _top;
		bot = _bot; 
		left = _left; 
		right = _right;
		front = _front;
		back = _back;
	}

}
