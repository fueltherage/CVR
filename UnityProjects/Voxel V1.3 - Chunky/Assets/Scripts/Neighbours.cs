using UnityEngine;
using System.Collections;

public class Neighbours {
	public VoxelShell xpos;
	public VoxelShell xneg;
	public VoxelShell ypos;
	public VoxelShell yneg;
	public VoxelShell zpos;
	public VoxelShell zneg;
	public Neighbours(ref VoxelShell _xpos, ref VoxelShell _xneg, ref VoxelShell _ypos, ref VoxelShell _yneg, ref VoxelShell _zpos, ref VoxelShell _zneg)
	{
		xpos = _xpos;
		xneg = _xneg; 
		ypos = _ypos; 
		yneg = _yneg;
		zpos = _zpos;
		zneg = _zneg;
	}
	public Neighbours()
	{
	}

}
