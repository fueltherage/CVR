using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelChunk: MonoBehaviour {
	//Notes
	/* 
	 * Triangles need to be optimized into a 1D array 
	*/
	public int XSize = 10;
	public int YSize = 10;
	public int ZSize = 10;
	public float VoxelSpacing;
	public Vector3 offset;  
    public VoxelShell[,,] blocks;
    //public Transform VoxelStartingSpot;


    public bool Initialized = false;

	public VoxMaterialFactory factory;
	public Dictionary<int, int> SubmeshIndex;
	public Dictionary<int, int> MaterialIndex;

	protected int SubmeshCount;
	protected int currentSelectedVoxel=1;
	//public bool needsUpdating = true;
	public bool queuedForUpdate = false;


	protected voxList<int[]> Triangles;    
	protected voxList<int> TrIndex; //index for submeshing
	protected voxList<Vector2> UVs;
    protected voxList<Vector3> Verts;
    protected Mesh vmesh;
	public bool Generating;


	
	// Use this for initialization
	void Start () {
		Init();
	}
	public virtual void Init (){   
		//thisChunk = gameObject.GetComponent<VoxelChunk>();
		
        //Initialize the mesh lists;
		Triangles = new voxList<int[]>();
		UVs = new voxList<Vector2>();
		TrIndex = new voxList<int>(); //index for submeshing
		Verts = new voxList<Vector3>();
        vmesh = new Mesh();
      
		//Shift the game object so the center is always the origin.
		//this.transform.Translate (-VoxelSpacing / 2.0f, -VoxelSpacing / 2.0f, -VoxelSpacing / 2.0f);
	
		offset.x = - XSize*VoxelSpacing/2.0f;//Starting position of vertices
		offset.y = - YSize*VoxelSpacing/2.0f;
		offset.z = - ZSize*VoxelSpacing/2.0f;
		// - Size/2.0f is to offset the Vertices by half of the Size so that they are centered around the origin

        //VoxelStartingSpot = transform.FindChild("VoxelStartingSpot");
        //VoxelStartingSpot.transform.Translate(offset);

		//Subscribe to Voxel Events
        //SubscribeToVoxelEvents();

		//Initializing Voxel/VoxelShell array
        //InitShells();




        //Initialize Dictionaries
        InitDics();  
 
        //Give the voxelShells neighbour refrences
        //GenerateNeighbours();

		//Center Voxel filled for testing
		//VoxelFactory.GenerateVoxel(1,ref blocks[XSize/2,YSize/2,ZSize/2],offset,VoxelSpacing);

		//UpdateMesh ();

        Initialized = true;
	}
    
    //protected void SubscribeToVoxelEvents()
    //{
    //    VoxelEvents.onVoxelAdded += AddVoxel;
    //    VoxelEvents.onVoxelRemoved += RemoveVoxel;
    //    VoxelEvents.onVoxelSwitch += SelectedVoxel;
    //}
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
    //public void AddVoxel(VoxelPos voxel, bool update)
    //{
    //    //Debug.Log(voxel);
    //    if (!blocks [voxel.x, voxel.y, voxel.z].locked) {
    //        if(blocks [voxel.x, voxel.y, voxel.z].voxel.VoxelType!= currentSelectedVoxel)
    //        {
    //            VoxelFactory.GenerateVoxel(currentSelectedVoxel, ref blocks [voxel.x, voxel.y, voxel.z]);
    //            if(update)
    //            //UpdateMesh ();
    //        }
    //    } 	
    //}
    //public void RemoveVoxel(VoxelPos voxel, bool update)
    //{
    //    VoxelFactory.GenerateVoxel(0,ref blocks [voxel.x, voxel.y, voxel.z]);
    //    if(update)
    //    //UpdateMesh ();
    //}	
	// Update is called once per frame
	void Update () {

	}

//    public virtual void UpdateMesh()
//    {
//        Generating = true;
////		GenerateMesh();	
//    }



	//This method converts a list of triangles to a one dementional int array
	protected int [] GetRawTriangles(ref voxList<Triangle> _triangles)
	{
		int[] tri = new int[_triangles.vcount * 3];
		for (int i = 0; i < _triangles.vcount; i++) {
			tri[(i*3)] = _triangles.vList[i].verts[0];
			tri[(i*3)+1] = _triangles.vList[i].verts[1];
			tri[(i*3)+2] = _triangles.vList[i].verts[2];
		}
		return tri;
	}

	//Checks to see if the voxelType's material has been registered to this chunk
	public int SubmeshIndexChecker(int _voxelType)
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
	protected int [] GetSubMeshTriangles(int _submesh, ref voxList<int[]> _triangles, ref voxList<int> _trIndex)
	{

		List<int> rawTri = new List<int>();
		try
		{
			for (int i = 0; i < _triangles.vcount; i++)
			{
				if(_trIndex.vcount == 0)
				{
					Debug.Log("trIndex is empty");
				}
				if(_submesh == _trIndex.vList[i])
				{
					rawTri.Add (_triangles.vList[i][0]);
					rawTri.Add (_triangles.vList[i][1]);
					rawTri.Add (_triangles.vList[i][2]);
					rawTri.Add (_triangles.vList[i][3]);
					rawTri.Add (_triangles.vList[i][4]);
					rawTri.Add (_triangles.vList[i][5]);
				}
			}
		}
        catch(System.Exception e)
        {
            Debug.Log ("Another threading error");
        }
		return rawTri.ToArray();
		
	}


	
}
