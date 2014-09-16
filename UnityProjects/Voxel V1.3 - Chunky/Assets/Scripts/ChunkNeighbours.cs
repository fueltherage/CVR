using UnityEngine;
using System.Collections;

public class ChunkNeighbours {
	public GameObject xpos;
	public GameObject xneg;
	public GameObject ypos;
	public GameObject yneg;
	public GameObject zpos;
	public GameObject zneg;

	public VoxelChunk xpos_vcs;
	public VoxelChunk xneg_vcs;
	public VoxelChunk ypos_vcs;
	public VoxelChunk yneg_vcs;
	public VoxelChunk zpos_vcs;
	public VoxelChunk zneg_vcs;

	public ChunkNeighbours(ref GameObject _xpos,
	                       ref GameObject _xneg,
	                       ref GameObject _ypos,
	                       ref GameObject _yneg,
	                       ref GameObject _zpos,
	                       ref GameObject _zneg ){
		xpos = _xpos;
		xneg = _xneg;
		ypos = _ypos;
		yneg = _yneg;
		zpos = _zpos;
		zneg = _zneg; 
		UpdateVoxelChunkRefs();
	}
	public ChunkNeighbours()
	{
	}
	public void UpdateVoxelChunkRefs()
	{
		xpos_vcs = xpos.GetComponent<VoxelChunk>();
		if(xpos_vcs == null) xpos_vcs = xpos.GetComponent<VoxelChunkEmpty>();
		
		xneg_vcs = xneg.GetComponent<VoxelChunk>();
		if(xneg_vcs == null) xneg_vcs = xneg.GetComponent<VoxelChunkEmpty>();
		
		ypos_vcs = ypos.GetComponent<VoxelChunk>();
		if(ypos_vcs == null) ypos_vcs = ypos.GetComponent<VoxelChunkEmpty>();
		
		yneg_vcs = yneg.GetComponent<VoxelChunk>();
		if(yneg_vcs == null) yneg_vcs = yneg.GetComponent<VoxelChunkEmpty>();
		
		zpos_vcs = zpos.GetComponent<VoxelChunk>();
		if(zpos_vcs == null) zpos_vcs = zpos.GetComponent<VoxelChunkEmpty>();
		
		zneg_vcs = zneg.GetComponent<VoxelChunk>();
		if(zneg_vcs == null) zneg_vcs = zneg.GetComponent<VoxelChunkEmpty>();
	}
}
