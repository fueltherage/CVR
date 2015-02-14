using UnityEngine;
using System.Collections;

public class Neighbours {
	public VoxelShell xpos;
	public VoxelShell xneg;
	public VoxelShell ypos;
	public VoxelShell yneg;
	public VoxelShell zpos;
	public VoxelShell zneg;
	//These flag a direction's quad being generated to allow skipping during loops; 
	public bool xposQuad;
	public bool xnegQuad;
	public bool yposQuad;
	public bool ynegQuad;
	public bool zposQuad;
	public bool znegQuad;
	public Neighbours(ref VoxelShell _xpos, ref VoxelShell _xneg, ref VoxelShell _ypos, ref VoxelShell _yneg, ref VoxelShell _zpos, ref VoxelShell _zneg)
	{
		xpos = _xpos;
		xneg = _xneg; 
		ypos = _ypos; 
		yneg = _yneg;
		zpos = _zpos;
		zneg = _zneg;
		ResetFlags();
	}
	public Neighbours()
	{
	}
	public void ResetFlags()
	{
		xposQuad = false;
		xnegQuad = false;
		yposQuad = false;
		ynegQuad = false;
		zposQuad = false;
		znegQuad = false;
	}
	public void UpdateNeighbours()
	{	
		if(xpos.parentChunk != null && xpos.filled) 
		xpos.parentChunk.needsUpdating = true;
		if(xneg.parentChunk != null && xneg.filled)
		xneg.parentChunk.needsUpdating = true;
		if(ypos.parentChunk != null && ypos.filled)
		ypos.parentChunk.needsUpdating = true;
		if(yneg.parentChunk != null && yneg.filled)
		yneg.parentChunk.needsUpdating = true;
		if(zpos.parentChunk != null && zpos.filled)
		zpos.parentChunk.needsUpdating = true;
		if(zneg.parentChunk != null && zneg.filled)
		zneg.parentChunk.needsUpdating = true;
	}
}
