using UnityEngine;
using System.Collections;

public class Triangle {
	public int[] verts;
	public Triangle()
	{
		verts = new int[3];
	}
	public Triangle(int v1, int v2, int v3)
	{
		verts = new int[3];
		verts[0] = v1;
		verts[1] = v2;
		verts[2] = v3;
	}

}
