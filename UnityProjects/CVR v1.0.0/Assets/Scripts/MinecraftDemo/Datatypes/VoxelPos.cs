using UnityEngine;
using System.Collections;

public struct VoxelPosByte {
	public byte x, y, z;
	public VoxelPosByte(byte _x, byte _y, byte _z)
	{
		x = _x;
		y = _y;
		z = _z;
	}
	public VoxelPosByte(float _x, float _y, float _z)
	{
		x = (byte)_x;
		y = (byte)_y;
		z = (byte)_z;
	}
	public VoxelPosByte(Vector3 _v)
    {
        x = (byte)_v.x;
        y = (byte)_v.y;
        z = (byte)_v.z;
    }
	public VoxelPosByte(double _x, double _y, double _z)
    {
        x = (byte)_x;
        y = (byte)_y;
        z = (byte)_z;
    }
	public void Set(byte _x, byte _y, byte _z)
	{
		x = _x;
		y = _y;
		z = _z;
	}
	public void Set(int _x, int _y, int _z)
	{
		x = (byte)_x;
		y = (byte)_y;
		z = (byte)_z;
	}

	public override string ToString ()
	{
		return string.Format ("X:"+x+" Y:"+y+" Z:"+z);
	}
}
public struct VoxelPos
{
	public int x, y, z;
	public VoxelPos(int _x, int _y, int _z)
	{
		x = _x;
		y = _y;
		z = _z;
	}
	public VoxelPos(float _x, float _y, float _z)
	{
		x = (int)_x;
		y = (int)_y;
		z = (int)_z;
	}
	public VoxelPos(Vector3 _v)
	{
		x = (int)_v.x;
		y = (int)_v.y;
		z = (int)_v.z;
	}
	public VoxelPos(double _x, double _y, double _z)
	{
		x = (int)_x;
		y = (int)_y;
		z = (int)_z;
	}
	public void Set(int _x, int _y, int _z)
	{
		x = _x;
		y = _y;
		z = _z;
	}
	public static VoxelPos operator - (VoxelPos v1, VoxelPos v2)
	{
		VoxelPos newVox = new VoxelPos (v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
		return newVox;
	}
	public override string ToString ()
	{
		return string.Format ("X:"+x+" Y:"+y+" Z:"+z);
	}
	public bool Equals(VoxelPos _vp)
	{
		if(_vp.x==x&&_vp.y == y&&_vp.z==z)return true;
		else return false;
	}
}
