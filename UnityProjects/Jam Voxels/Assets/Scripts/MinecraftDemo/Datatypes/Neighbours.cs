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
	public void ForceUpdateNeighbours()
{
		if(xpos.filled )
		if(xpos.parentChunkS != xpos.neighbours.xneg.parentChunkS )
		if(xpos.parentChunkS!= null )
		{
			
			xpos.parentChunkS.needsUpdating= true;
			xpos.parentChunkS.UpdateMesh();
			
		}

		if(xneg.filled )
		if(xneg.parentChunkS != xneg.neighbours.xpos.parentChunkS )
		if(xneg.parentChunkS != null)
		{
			
			xneg.parentChunkS.needsUpdating = true;
			xneg.parentChunkS.UpdateMesh();
			
		}

		if(ypos.filled )
		if(ypos.parentChunkS != ypos.neighbours.yneg.parentChunkS )
		if(ypos.parentChunkS != null)
		{
			
			ypos.parentChunkS.needsUpdating = true;
			ypos.parentChunkS.UpdateMesh();		
			
		}

		if(yneg.filled )
		if(yneg.parentChunkS != yneg.neighbours.ypos.parentChunkS )
		if(yneg.parentChunkS != null)
		{
		
			yneg.parentChunkS.needsUpdating = true;
			yneg.parentChunkS.UpdateMesh();
			
		}

		if(zpos.filled )
		if(zpos.parentChunkS != zpos.neighbours.zneg.parentChunkS )
		if(zpos.parentChunkS != null )
		{
			
			zpos.parentChunkS.needsUpdating = true;
			zpos.parentChunkS.UpdateMesh();
			
		}

		if(zneg.filled )
		if(zneg.parentChunkS != zneg.neighbours.zpos.parentChunkS )
		if(zneg.parentChunkS != null )
		{
		
			zneg.parentChunkS.needsUpdating = true;
			zneg.parentChunkS.UpdateMesh();
			
		}
	}
	public void FlagNeightboursForUpdate()
	{
		xpos.parentChunkS.needsUpdating = true;
		xneg.parentChunkS.needsUpdating = true;
		ypos.parentChunkS.needsUpdating = true;
		yneg.parentChunkS.needsUpdating = true;
		zneg.parentChunkS.needsUpdating = true;
		zpos.parentChunkS.needsUpdating = true;
	}
	public void UpdateNeighbours()
	{	
		if(xpos.filled )
		if(xpos.parentChunk != xpos.neighbours.xneg.parentChunk )
		if(xpos.parentChunk!= null )
		{
			if(!xpos.parentChunk.Generating)
			{
				xpos.parentChunk.needsUpdating= true;
				xpos.parentChunk.systemParent.VSUM.QueueChunkForUpdate(ref xpos.parentChunk);
			}
		}

		if(xneg.filled )
		if(xneg.parentChunk != xneg.neighbours.xpos.parentChunk )
		if(xneg.parentChunk != null)
		{
			if(!xneg.parentChunk.Generating)
			{
				xneg.parentChunk.needsUpdating = true;
				xneg.parentChunk.systemParent.VSUM.QueueChunkForUpdate(ref xneg.parentChunk);
			}
		}

		if(ypos.filled )
		if(ypos.parentChunk != ypos.neighbours.yneg.parentChunk )
		if(ypos.parentChunk != null)
		{
			if(!ypos.parentChunk.Generating)
			{
				ypos.parentChunk.needsUpdating = true;
				ypos.parentChunk.systemParent.VSUM.QueueChunkForUpdate(ref ypos.parentChunk);			
			}
		}

		if(yneg.filled )
		if(yneg.parentChunk != yneg.neighbours.ypos.parentChunk )
		if(yneg.parentChunk != null)
		{
			if(!yneg.parentChunk.Generating)
			{
				yneg.parentChunk.needsUpdating = true;
				yneg.parentChunk.systemParent.VSUM.QueueChunkForUpdate(ref yneg.parentChunk);
			}
		}

		if(zpos.filled )
		if(zpos.parentChunk != zpos.neighbours.zneg.parentChunk )
		if(zpos.parentChunk != null )
		{
			if(!zpos.parentChunk.Generating)
			{
				zpos.parentChunk.needsUpdating = true;
				zpos.parentChunk.systemParent.VSUM.QueueChunkForUpdate(ref zpos.parentChunk);
			}
		}

		if(zneg.filled )
		if(zneg.parentChunk != zneg.neighbours.zpos.parentChunk )
		if(zneg.parentChunk != null )
		{
			if(!zneg.parentChunk.Generating)
			{
				zneg.parentChunk.needsUpdating = true;
				zneg.parentChunk.systemParent.VSUM.QueueChunkForUpdate(ref zneg.parentChunk);
			}
		}
	}

	public void GetNeighbourDelegate(ref System.Action updateStuff)
	{	

		if(xpos.filled )
			if(xpos.parentChunk != xpos.neighbours.xneg.parentChunk )
				if(xpos.parentChunk!= null )
			{
				if(!xpos.parentChunk.Generating)
				{
					xpos.parentChunk.needsUpdating= true;

					updateStuff += xpos.parentChunk.UpdateMesh;
				}
			}
		
		if(xneg.filled )
			if(xneg.parentChunk != xneg.neighbours.xpos.parentChunk )
				if(xneg.parentChunk != null)
			{
				if(!xneg.parentChunk.Generating)
				{
					xneg.parentChunk.needsUpdating = true;
					updateStuff += xneg.parentChunk.UpdateMesh;
				}
			}
		
		if(ypos.filled )
			if(ypos.parentChunk != ypos.neighbours.yneg.parentChunk )
				if(ypos.parentChunk != null)
			{
				if(!ypos.parentChunk.Generating)
				{
					ypos.parentChunk.needsUpdating = true;
					updateStuff += ypos.parentChunk.UpdateMesh;			
				}
			}
		
		if(yneg.filled )
			if(yneg.parentChunk != yneg.neighbours.ypos.parentChunk )
				if(yneg.parentChunk != null)
			{
				if(!yneg.parentChunk.Generating)
				{
					yneg.parentChunk.needsUpdating = true;
					updateStuff += yneg.parentChunk.UpdateMesh;
				}
			}
		
		if(zpos.filled )
			if(zpos.parentChunk != zpos.neighbours.zneg.parentChunk )
				if(zpos.parentChunk != null )
			{
				if(!zpos.parentChunk.Generating)
				{
					zpos.parentChunk.needsUpdating = true;
					updateStuff += zpos.parentChunk.UpdateMesh;
				}
			}
		
		if(zneg.filled )
			if(zneg.parentChunk != zneg.neighbours.zpos.parentChunk )
				if(zneg.parentChunk != null )
			{
				if(!zneg.parentChunk.Generating)
				{
					zneg.parentChunk.needsUpdating = true;
					updateStuff += zneg.parentChunk.UpdateMesh;
				}
			}

	}

}
