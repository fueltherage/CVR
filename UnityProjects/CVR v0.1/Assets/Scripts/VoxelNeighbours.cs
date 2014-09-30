using UnityEngine;
using System.Collections;

public class VoxelNeighbours{


	public bool px, nx, py, ny, pz, nz;

	public VoxelNeighbours()
	{

	}
	public VoxelNeighbours(bool _px, bool _nx, bool _py, bool _ny,bool _pz, bool _nz)
	{
		px = _px;
		nx = _nx;
		py = _py;
		ny = _ny;
		pz = _pz;
		nz = _nz;
	}
	public void NegateAll()
	{
		px = false;
		nx = false; 
		py = false;
		ny = false;
		pz = false;
		nz = false;
	}
}
