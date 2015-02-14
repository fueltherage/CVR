using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelChunk: MonoBehaviour {
	//Notes
	/* 
	 * 
	*/
	public int XSize = 10;
	public int YSize = 10;
	public int ZSize = 10;
	public float VoxelSpacing = 1f;
	public Vector3 offset;  
    public VoxelShell[,,] blocks;
    //public Transform VoxelStartingSpot;


    public bool Initialized = false;

	public VoxMaterialFactory factory;
	protected Dictionary<int, int> SubmeshIndex;
	protected Dictionary<int, int> MaterialIndex;

	protected int SubmeshCount;
	protected int currentSelectedVoxel=1;
	public bool needsUpdating = true;

    


    protected List<Triangle> Triangles;
    
    protected List<int> TrIndex; //index for submeshing
    protected List<Vector2> UVs;
    protected List<Vector3> Verts;
    protected Mesh vmesh;
	public bool Generating;
	
	// Use this for initialization
	void Start () {
		Init();
	}
	public virtual void Init (){   
		//thisChunk = gameObject.GetComponent<VoxelChunk>();
		
        //Initialize the mesh lists;
        Triangles = new List<Triangle>();
        UVs = new List<Vector2>();
        TrIndex = new List<int>(); //index for submeshing
        Verts = new List<Vector3>();
        vmesh = new Mesh();
      
		//Shift the game object so the center is always the origin.
		this.transform.Translate (-VoxelSpacing / 2.0f, -VoxelSpacing / 2.0f, -VoxelSpacing / 2.0f);
	
		offset.x = - XSize*VoxelSpacing/2.0f;//Starting position of vertices
		offset.y = - YSize*VoxelSpacing/2.0f;
		offset.z = - ZSize*VoxelSpacing/2.0f;
		// - Size/2.0f is to offset the Vertices by half of the Size so that they are centered around the origin

        //VoxelStartingSpot = transform.FindChild("VoxelStartingSpot");
        //VoxelStartingSpot.transform.Translate(offset);

		//Subscribe to Voxel Events
        SubscribeToVoxelEvents();

		//Initializing Voxel/VoxelShell array
        //InitShells();

        //Initialize Dictionaries
        InitDics();  
 
        //Give the voxelShells neighbour refrences
        GenerateNeighbours();

		//Center Voxel filled for testing
		VoxelFactory.GenerateVoxel(1,ref blocks[XSize/2,YSize/2,ZSize/2],offset,VoxelSpacing);

		UpdateMesh ();

        Initialized = true;
	}
    protected void GenerateNeighbours()
    {
        for (int x = 0; x < XSize; ++x){
            for (int y = 0; y < YSize; ++y){
                for (int z = 0; z < ZSize; ++z) {
                    if (x == 0 || y == 0 || z == 0 || x == XSize - 1 || y == YSize - 1 || z == ZSize - 1)
                    {
                        blocks[x, y, z].locked = true;
                        blocks[x, y, z].filled = false;
                        //Lock the outer layer
                    }
                    else{
                        blocks[x, y, z].neighbours = new Neighbours(ref blocks[x + 1, y, z], ref blocks[x - 1, y, z], ref blocks[x , y + 1, z],
                                                                    ref blocks[x, y - 1, z], ref blocks[x, y, z + 1], ref blocks[x, y, z - 1]);
                        blocks[x, y, z].locked = false;
                        //VoxelFactory.GenerateVoxel(1,ref blocks[x,y,z],offset,VoxelSpacing);
                    }
                }
			}
        }
    }
    protected void SubscribeToVoxelEvents()
    {
        VoxelEvents.onVoxelAdded += AddVoxel;
        VoxelEvents.onVoxelRemoved += RemoveVoxel;
        VoxelEvents.onVoxelSwitch += SelectedVoxel;
    }
    protected void InitDics()
    {
        SubmeshIndex = new Dictionary<int, int>();
        MaterialIndex = new Dictionary<int, int>();
        SubmeshCount = 0;
    }
   
	public void SelectedVoxel(int _currentSelectedVoxel)
	{
		currentSelectedVoxel = _currentSelectedVoxel;
	}
	public void AddVoxel(VoxelPos voxel, bool update)
	{
		//Debug.Log(voxel);
		if (!blocks [voxel.x, voxel.y, voxel.z].locked) {
			if(blocks [voxel.x, voxel.y, voxel.z].voxel.VoxelType!= currentSelectedVoxel)
			{
				VoxelFactory.GenerateVoxel(currentSelectedVoxel, ref blocks [voxel.x, voxel.y, voxel.z], offset, VoxelSpacing);
				if(update)
				UpdateMesh ();
			}
		} 	
	}
	public void RemoveVoxel(VoxelPos voxel, bool update)
	{
		VoxelFactory.GenerateVoxel(0,ref blocks [voxel.x, voxel.y, voxel.z], offset, VoxelSpacing);
		if(update)
		UpdateMesh ();
	}	
	// Update is called once per frame
	void Update () {

	}

	public virtual void UpdateMesh()
	{
		Generating = true;
		GenerateMesh();	
	}

	protected virtual void GenerateMesh ()
	{
        Triangles.Clear();
        Verts.Clear();
        UVs.Clear();
        TrIndex.Clear();
		vmesh.Clear();	

		int faceCount = 0;
		for (int x = 0; x < XSize; ++x) {
			for (int y = 0; y < YSize; ++y) {
				for (int z = 0; z < ZSize; ++z) {
					if(blocks[x,y,z].filled && !blocks[x,y,z].locked)
					{
						faceCount = blocks[x,y,z].GenerateMesh(faceCount);					
						for (int i = 0; i < blocks[x,y,z].voxel.meshData.Verts.Count; ++i)
						{
							Verts.Add(blocks[x,y,z].voxel.meshData.Verts[i]);
							UVs.Add(blocks[x,y,z].voxel.meshData.UVs[i]);
						}
						for (int i = 0; i < blocks[x,y,z].voxel.meshData.Triangles.Count; ++i)
						{
							Triangles.Add(blocks[x,y,z].voxel.meshData.Triangles[i]);
							TrIndex.Add(SubmeshIndexChecker(blocks[x,y,z].voxel.VoxelType));
						}
					}
				}				
			}
		}

        vmesh.vertices = Verts.ToArray();
		//mesh.triangles = GetRawTriangles(ref Triangles);
        vmesh.subMeshCount = SubmeshCount;

		for (int i = 0; i < SubmeshCount; i++) 
		{
            vmesh.SetTriangles(GetSubMeshTriangles(i, ref Triangles, ref TrIndex), i);
		}

		List<Material> mat= new List<Material>();
		for(int i = 0; i < SubmeshCount; i++)
		{
			mat.Add (factory.VoxelMats[MaterialIndex[i]]);
		}
		renderer.materials = mat.ToArray();

        vmesh.uv = UVs.ToArray();
        vmesh.RecalculateNormals();

		//Use TangentSolver if shader requires it
        TangentSolver(vmesh);
        vmesh.Optimize();

		gameObject.GetComponent<MeshFilter> ().mesh = vmesh;
		
		//Nullify the mesh so it initiates a reset... :(
		MeshCollider meshCollider = GetComponent<MeshCollider> ();
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = vmesh;   

		Generating = false;

	}	

	//This method converts a list of triangles to a one dementional int array
	protected int [] GetRawTriangles(ref List<Triangle> _triangles)
	{
		int[] tri = new int[_triangles.Count * 3];
		for (int i = 0; i < _triangles.Count; i++) {
			tri[(i*3)] = _triangles[i].verts[0];
			tri[(i*3)+1] = _triangles[i].verts[1];
			tri[(i*3)+2] = _triangles[i].verts[2];
		}
		return tri;
	}

	//Checks to see if the voxelType's material has been registered to this chunk
	protected int SubmeshIndexChecker(int _voxelType)
	{
		//Return if it's already registered
		if(SubmeshIndex.ContainsKey(_voxelType))
		{
			return SubmeshIndex[_voxelType];
		}else{//Register it! 
			SubmeshIndex.Add(_voxelType, SubmeshCount);
			MaterialIndex.Add(SubmeshCount, _voxelType);
			SubmeshCount++;
			return SubmeshIndex[_voxelType];
		}
	}

	//This method returns all the triangles that that are registered to _submesh using _trIndex as refrence
	protected int [] GetSubMeshTriangles(int _submesh, ref List<Triangle> _triangles, ref List<int> _trIndex)
	{
		List<int>rawTri = new List<int>();
		for (int i = 0; i < _triangles.Count; i++)
		{
			if(_trIndex.Count ==0)
			{
				Debug.Log("wtf");
			}
			if(_submesh == _trIndex[i])
			{
				rawTri.Add (_triangles[i].verts[0]);
				rawTri.Add (_triangles[i].verts[1]);
				rawTri.Add (_triangles[i].verts[2]);
			}
		}
		return rawTri.ToArray();
	}


	//Got TangerSolver from internet, still dont know how it does what it does but it fixes the shader not having tangents.
	protected static void TangentSolver(Mesh theMesh)
	{
		int vertexCount = theMesh.vertexCount;
		Vector3[] vertices = theMesh.vertices;
		Vector3[] normals = theMesh.normals;
		Vector2[] texcoords = theMesh.uv;
		int[] triangles = theMesh.triangles;
		int triangleCount = triangles.Length / 3;
		Vector4[] tangents = new Vector4[vertexCount];
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];
		int tri = 0;
		for (int i = 0; i < (triangleCount); i++)
		{
			int i1 = triangles[tri];
			int i2 = triangles[tri + 1];
			int i3 = triangles[tri + 2];
			
			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];
			
			Vector2 w1 = texcoords[i1];
			Vector2 w2 = texcoords[i2];
			Vector2 w3 = texcoords[i3];
			
			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;
			
			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;
			
			float r = 1.0f / (s1 * t2 - s2 * t1);
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
			
			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;
			
			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
			
			tri += 3;
		}
		
		for (int i = 0; i < (vertexCount); i++)
		{
			Vector3 n = normals[i];
			Vector3 t = tan1[i];
			
			// Gram-Schmidt orthogonalize
			Vector3.OrthoNormalize(ref n, ref t);
			
			tangents[i].x = t.x;
			tangents[i].y = t.y;
			tangents[i].z = t.z;
			
			// Calculate handedness
			tangents[i].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0) ? -1.0f : 1.0f;
		}
		theMesh.tangents = tangents;
	}
}
