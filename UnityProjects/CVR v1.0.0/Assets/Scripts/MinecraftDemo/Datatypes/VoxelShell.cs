using UnityEngine;
using System.Collections;

public class VoxelShell {

	//1 -	1   - types
	//2 -	2   -
	//3	-	4   -
	//4	-	8   -
	//5	-	16  -
	//6	-	32  -
	//7	-	64  - locked
	//8	-	128 - filled
	static byte[] vs_flags = new byte[]{(byte)0, (byte)1, (byte)2, (byte)4, (byte)8, (byte)16, (byte)32, (byte)64, (byte) 128, (byte)255};	

	byte voxInfo;

	public VoxelPosByte vp;

	public bool filled {
		get 
		{
			return (voxInfo & vs_flags[8]) == vs_flags[8];
		}
		set
		{
			if(value == true)
			{
				if((voxInfo & vs_flags[8]) == vs_flags[0]) 
					voxInfo = (byte)(voxInfo | vs_flags[8]);
			}else 
			{
				if((voxInfo & vs_flags[8]) == vs_flags[8]) 
					voxInfo = (byte)(voxInfo ^ vs_flags[8]);
			}
		}
	}
	public bool locked {
		get 
		{
			return (voxInfo & vs_flags[7]) == vs_flags[7];
		}
		set
		{
			if(value == true)
			{
				if((voxInfo & vs_flags[7]) == vs_flags[0]) 
					voxInfo = (byte)(voxInfo | vs_flags[7]);
			}else 
			{
				if((voxInfo & vs_flags[7]) == vs_flags[7]) 
					voxInfo = (byte)(voxInfo ^ vs_flags[7]);
			}
		}
	}
	public int voxelType {
		get
		{
			int v = (int)(voxInfo & ((vs_flags[8] | vs_flags[7]) ^ vs_flags[9]));
			return v;

		}
		set
		{
			if(value > 63 || value < 0)
			{
				Debug.Log("Error: voxelType set value("+value+") out of range 0 - 63");
			}
			else 
			{
				//Save the locked/filled bits then set the new vox type value
				voxInfo = (byte)(value | (voxInfo & (vs_flags[8] | vs_flags[7])));
			}
		}
	}

	public VoxelSystemChunkGreedy parentChunk;
	public float Mass = 1.0f;	
	public VoxelShell(){}
	public VoxelShell(ref VoxelSystemChunkGreedy _parentchunk)
	{
		parentChunk = _parentchunk;
	}


}
