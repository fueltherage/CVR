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
	public void UpdateNeighbours()
	{	if(xpos.parentChunk!= null)
		xpos.parentChunk.needsUpdating = true;
		if(xneg.parentChunk!= null)
		xneg.parentChunk.needsUpdating = true;
		if(ypos.parentChunk!= null)
		ypos.parentChunk.needsUpdating = true;
		if(yneg.parentChunk!= null)
		yneg.parentChunk.needsUpdating = true;
		if(zpos.parentChunk!= null)
		zpos.parentChunk.needsUpdating = true;
		if(zneg.parentChunk!= null)
		zneg.parentChunk.needsUpdating = true;
	}

}
