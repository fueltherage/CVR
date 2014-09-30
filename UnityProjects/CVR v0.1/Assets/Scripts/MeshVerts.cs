using UnityEngine;
using System.Collections;

public class MeshVerts {
			    //  xyz
	public Vert Vert000;
	public Vert Vert100;
	public Vert Vert110;
	public Vert Vert101;
	public Vert Vert111;
	public Vert Vert011;
	public Vert Vert010;
	public Vert Vert001;
	public MeshVerts()
	{}
	public MeshVerts(ref Vert _Vert000,ref Vert _Vert100,ref Vert _Vert110,ref Vert _Vert101,
	                 ref Vert _Vert111,ref Vert _Vert011,ref Vert _Vert010,ref Vert _Vert001)
	{
		Vert000 = _Vert000;
		Vert100 = _Vert100;
		Vert110 = _Vert110;
		Vert101 = _Vert101;
		Vert111 = _Vert111;
		Vert011 = _Vert011;
		Vert010 = _Vert010;
		Vert001 = _Vert001;
	}
}
