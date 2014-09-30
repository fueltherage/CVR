using UnityEngine;
using System.Collections;

public class Vert{
	public Index index;
	public Vector3 Vertex;
	public bool used;
	public Vert(ref Index _index, ref Vector3 _Vertex)
	{
		index = _index;
		Vertex = _Vertex;
	}



}
