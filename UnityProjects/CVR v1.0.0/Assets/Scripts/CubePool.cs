using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CubePool : MonoBehaviour {    

	public GameObject CubeVoxelGO;
	public int PoolSize = 500;
    public float cubeLifeTime = 2;    
    public Vector3 DifNormalRandom_ForceMultiplier = new Vector3(1.0f, 1.0f, 1.0f);
    public static Vector3 s_DifNormalRandom_ForceMultiplier = new Vector3(1.0f, 1.0f, 1.0f);   
    public static bool init = false;
    public int SpawnEvery = 1;
    
    static float s_cubeLifeTime = 1;
    static ObjectPool<GameObject> CubeObjectPool;

   
    static List<CubeTimer> DeathTimers;
    
    static Mesh cubeMesh;
    static int cubeSpawnCount;
    static int s_spawnEvery;
	// Use this for initialization
	void Start () {
        s_DifNormalRandom_ForceMultiplier = DifNormalRandom_ForceMultiplier;

        Init_Mesh();
        s_spawnEvery = SpawnEvery;
        CubeObjectPool = new ObjectPool<GameObject>(PoolSize);
        DeathTimers = new List<CubeTimer>();
        s_cubeLifeTime = cubeLifeTime;
        InitiatlizePoolObjects();
	}	
	
    void FixedUpdate()
    {
        if (DeathTimers.Count > 0) 
        for (int i = 0; i < DeathTimers.Count; i++)
			{                
                DeathTimers[i].timeToLive -= Time.fixedDeltaTime;
                if (DeathTimers[i].timeToLive <= 0)
                {
                    DeathTimers[i]._obj._Object.SetActive(false);
                    DeathTimers[i]._obj.ReturnToPool();
                    DeathTimers.Remove(DeathTimers[i]);
                    --i;
                }
			}          
        
    }

    // Update is called once per frame
	void Update () {
        s_DifNormalRandom_ForceMultiplier = DifNormalRandom_ForceMultiplier;
        SpawnEvery = s_spawnEvery;
        if (DeathTimers.Count < PoolSize / 0.25f)
        {
            s_spawnEvery = 1;
        }
        else if (DeathTimers.Count < PoolSize * 0.5f)
        {
            s_spawnEvery = 2;
        }
        else if (DeathTimers.Count < PoolSize * 0.75f)
        {
            s_spawnEvery = 3;
        }
        else
        {
            s_spawnEvery = 4;
        }
	}

    /// <summary>
    /// Spawns a cube at _worldPos with given orientation
    /// </summary>
    /// <param name="_worldPos"></param>
    /// <param name="orientation"></param>
    void InitiatlizePoolObjects()
    {
        
        for (int i = 0; i < PoolSize; i++)
		{
            CubeObjectPool.Pool[i]._Object = Instantiate(CubeVoxelGO, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            CubeObjectPool.Pool[i]._Object.name = "CubeDebris " + (i+1);
            CubeObjectPool.Pool[i]._Object.transform.parent = this.transform;//Hierarchy hiding 
            CubeObjectPool.Pool[i]._Object.SetActive(false);
            CubeObjectPool.Pool[i]._Object.GetComponent<MeshFilter>().mesh = (Mesh)Instantiate(cubeMesh);  
		}
        init = true;
        
    }
    void Init_Mesh()
    {       
        cubeMesh = new Mesh();
        Vector3[] cubeVerts = new Vector3[4 * 6];
        int[] tris = new int[6 * 6];
        Vector2[] initUV = new Vector2[4*6]; // Used only as init info for the Tangent solver
        int _faceCount = 0;

        ////////////////////////////////////////////////
        //x+ 
        //////////////////////////////////////////////// What's better, setting it by hand or new Vector...
        cubeVerts[0].x = 0.5f; cubeVerts[0].y = -0.5f; cubeVerts[0].z = -0.5f;
        cubeVerts[1].x = 0.5f; cubeVerts[1].y = 0.5f; cubeVerts[1].z = -0.5f;
        cubeVerts[2].x = 0.5f; cubeVerts[2].y = 0.5f; cubeVerts[2].z = 0.5f;
        cubeVerts[3].x = 0.5f; cubeVerts[3].y = -0.5f; cubeVerts[3].z = 0.5f;

        initUV[0].x = 0.0f; initUV[0].y = 0.0f;
        initUV[1].x = 0.0f; initUV[1].y = 1.0f;
        initUV[2].x = 1.0f; initUV[2].y = 1.0f;
        initUV[3].x = 1.0f; initUV[3].y = 0.0f; 

        tris[0] = 0 + _faceCount * 4;
        tris[1] = 1 + _faceCount * 4;
        tris[2] = 2 + _faceCount * 4;
        tris[3] = 0 + _faceCount * 4;
        tris[4] = 2 + _faceCount * 4;
        tris[5] = 3 + _faceCount * 4;
        _faceCount++;
        ////////////////////////////////////////////////
        //x-
        ////////////////////////////////////////////////
        cubeVerts[4] = new Vector3(-0.5f, -0.5f, -0.5f);
        cubeVerts[5] = new Vector3(-0.5f,  0.5f, -0.5f);
        cubeVerts[6] = new Vector3( -0.5f,  0.5f,  0.5f);
        cubeVerts[7] = new Vector3( -0.5f, -0.5f,  0.5f);

        initUV[4] = new Vector2(0.0f,0.0f);
        initUV[5] = new Vector2(1.0f,0.0f);
        initUV[6] = new Vector2(1.0f,1.0f);
        initUV[7] = new Vector2(0.0f,1.0f);

        tris[6] = 1 + _faceCount * 4;
        tris[7] = 0 + _faceCount * 4;
        tris[8] = 2 + _faceCount * 4;
        tris[9] = 2 + _faceCount * 4;
        tris[10] = 0 + _faceCount * 4;
        tris[11] = 3 + _faceCount * 4;
        _faceCount++;
        ////////////////////////////////////////////////
        //+y
        ////////////////////////////////////////////////
        cubeVerts[8] = new Vector3(-0.5f, 0.5f, -0.5f);
        cubeVerts[9] = new Vector3(-0.5f, 0.5f, 0.5f);
        cubeVerts[10] = new Vector3(0.5f, 0.5f, 0.5f);
        cubeVerts[11] = new Vector3(0.5f, 0.5f, -0.5f);

        initUV[8] = new Vector2(0.0f,0.0f);
        initUV[9] = new Vector2(0.0f,1.0f);
        initUV[10] = new Vector2(1.0f,1.0f);
        initUV[11] = new Vector2(1.0f,0.0f);

        tris[12] = 0 + _faceCount * 4;
        tris[13] = 1 + _faceCount * 4;
        tris[14] = 2 + _faceCount * 4;
        tris[15] = 0 + _faceCount * 4;
        tris[16] = 2 + _faceCount * 4;
        tris[17] = 3 + _faceCount * 4;
        _faceCount++;
        ////////////////////////////////////////////////
        //-y
        ////////////////////////////////////////////////
        cubeVerts[12] = new Vector3(-0.5f, -0.5f, -0.5f);
        cubeVerts[13] = new Vector3(-0.5f, -0.5f, 0.5f);
        cubeVerts[14] = new Vector3(0.5f, -0.5f, 0.5f);
        cubeVerts[15] = new Vector3(0.5f, -0.5f, -0.5f);

        initUV[12] = new Vector2(0.0f,0.0f);
        initUV[13] = new Vector2(0.0f,1.0f);
        initUV[14] = new Vector2(1.0f,1.0f);
        initUV[15] = new Vector2(1.0f,0.0f);

        tris[18] = 1 + _faceCount * 4;
        tris[19] = 0 + _faceCount * 4;
        tris[20] = 2 + _faceCount * 4;
        tris[21] = 2 + _faceCount * 4;
        tris[22] = 0 + _faceCount * 4;
        tris[23] = 3 + _faceCount * 4;
        _faceCount++;
        ////////////////////////////////////////////////
        //+z 
        ////////////////////////////////////////////////
        cubeVerts[16] = new Vector3(-0.5f, -0.5f, 0.5f);
        cubeVerts[17] = new Vector3(-0.5f, 0.5f, 0.5f);
        cubeVerts[18] = new Vector3(0.5f, 0.5f, 0.5f);
        cubeVerts[19] = new Vector3(0.5f, -0.5f, 0.5f);

        initUV[16] = new Vector2(0.0f,0.0f);
        initUV[17] = new Vector2(0.0f,1.0f);
        initUV[18] = new Vector2(1.0f,1.0f);
        initUV[19] = new Vector2(1.0f,0.0f);

        tris[24] = 1 + _faceCount * 4;
        tris[25] = 0 + _faceCount * 4;
        tris[26] = 2 + _faceCount * 4;
        tris[27] = 2 + _faceCount * 4;
        tris[28] = 0 + _faceCount * 4;
        tris[29] = 3 + _faceCount * 4;
        _faceCount++;

        ////////////////////////////////////////////////
        //-z
        ////////////////////////////////////////////////
        cubeVerts[20] = new Vector3(-0.5f, -0.5f, -0.5f);
        cubeVerts[21] = new Vector3(-0.5f, 0.5f, -0.5f);
        cubeVerts[22] = new Vector3(0.5f, 0.5f, -0.5f);
        cubeVerts[23] = new Vector3(0.5f, -0.5f, -0.5f);

        initUV[20] = new Vector2(0.0f,0.0f);
        initUV[21] = new Vector2(0.0f,1.0f);
        initUV[22] = new Vector2(1.0f,1.0f);
        initUV[23] = new Vector2(1.0f,0.0f);

        tris[30] = 0 + _faceCount * 4;
        tris[31] = 1 + _faceCount * 4;
        tris[32] = 2 + _faceCount * 4;
        tris[33] = 0 + _faceCount * 4;
        tris[34] = 2 + _faceCount * 4;
        tris[35] = 3 + _faceCount * 4;


        for (int i = 0; i < cubeVerts.Length; i++)
        {
            cubeVerts[i] *= 0.95f;
        }
        cubeMesh.vertices = cubeVerts;
        cubeMesh.triangles = tris;
        cubeMesh.RecalculateNormals();
        cubeMesh.Optimize();
        cubeMesh.uv = initUV;
        
        MeshUtils.TangentSolver(cubeMesh);


    }
    public static void SpawnCubeAtLocation(Vector3 _worldPos, VoxelSystemGreedy system, VoxelPos _vPos, int Type)
    {
        
        if (init)
        {
            if (cubeSpawnCount >= s_spawnEvery)
            {
                cubeSpawnCount = 1;
                PoolObject<GameObject> cube_PO = CubeObjectPool.GetFreeObject();
                if (cube_PO == null)
                {
                    FreeOldestCube();
                    return;
                }
                cube_PO._Object.SetActive(true);

                //Create a death timer
                CubeTimer timer = new CubeTimer();
                timer.timeToLive = s_cubeLifeTime;
                timer._obj = cube_PO;
                DeathTimers.Add(timer);

                //Set the position, rotation, and scale of debris cube
                cube_PO._Object.transform.position = _worldPos;
                cube_PO._Object.transform.eulerAngles = system.transform.eulerAngles;
                cube_PO._Object.transform.localScale = new Vector3(system.VoxelSpacing, system.VoxelSpacing, system.VoxelSpacing);
                cube_PO._Object.GetComponent<Renderer>().material = system.factory.VoxelMats[Type];
                MeshFilter mesh = cube_PO._Object.GetComponent<MeshFilter>();


                GenerateCubeUV(mesh, system, _vPos, UVType.FullProjection);

                //for (int i = 0; i < mesh.mesh.uv.Length; i++)
                //{
                //    Debug.Log(mesh.mesh.uv[i]);   
                //}

                ResetRBSpeed(cube_PO._Object);
            }
            else cubeSpawnCount++;

        }
        else Debug.Log("Cube Pool not initialized");
    }
    public enum UVType { FullProjection, SpriteSheet }
    static void GenerateCubeUV(MeshFilter _meshF , VoxelSystemGreedy system, VoxelPos _pos, UVType type)
    {      


       
        VoxelPos vp = new VoxelPos(_pos.x % system.ChunkSizeX,
                                   _pos.y % system.ChunkSizeY,
                                   _pos.z % system.ChunkSizeZ);
        VoxelPos cp = new VoxelPos(_pos.x / system.ChunkSizeX,
                                   _pos.y / system.ChunkSizeY,
                                   _pos.z / system.ChunkSizeX);

        int voxelType=0;
       
        voxelType = system.chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z].voxelType;        
        Material mat = system.chunks_vcs[cp.x, cp.y, cp.z].factory.VoxelMats[voxelType];
        Vector3 pos = new Vector3(_pos.x, _pos.y, _pos.z);

        //The 3D demsion of the voxel system    
        Vector3 SystemSize = new Vector3(system.XSize * system.ChunkSizeX,
                                           system.YSize * system.ChunkSizeY,
                                           system.ZSize * system.ChunkSizeX);
        float Xnum;//Amount of images along the x and y axis
		float Ynum;//
		float Ximage; //1.0f / Xnum;
		float Yimage;//1.0f / Ynum;
        Vector3 ImageManip;//Used to manipulate the UV to work with sprite sheets
		if(system.UniqueSides)
		{
			
			Xnum = 4.0f;
			Ximage = 1.0f / Xnum;
			Ynum = 4.0f;
			Yimage = 1.0f / Ynum;
            ImageManip = new Vector3(1.0f,1.0f,1.0f);

		}
		else 
		{
			Xnum = 1.0f;
			Ximage = 0.0f;
			Ynum = 1.0f;
			Yimage = 0.0f;
            ImageManip = new Vector3(1.0f,1.0f,1.0f);
		}
       

       

        Vector2[] UVs= new Vector2[6*4];
        ////////////////////////////////////////////////
        //+x 
        ////////////////////////////////////////////////        
        UVs[0].x = pos.z / SystemSize.z / Xnum/ system.UVRatio.z; //0.0
        UVs[0].y = (pos.y / SystemSize.y / Ynum + Yimage)/ system.UVRatio.y; //0.0
        
        UVs[1].x = pos.z / SystemSize.z / Xnum / system.UVRatio.z;		//0.0 
        UVs[1].y = ((pos.y + 1.0f) / SystemSize.y / Ynum + Yimage)/ system.UVRatio.y; //1.0

        UVs[2].x = (pos.z + 1.0f) / SystemSize.z / Xnum / system.UVRatio.z; //1.0		     
        UVs[2].y = ((pos.y + 1.0f) / SystemSize.y / Ynum + Yimage) / system.UVRatio.y; //1.0

        UVs[3].x = (pos.z + 1.0f) / SystemSize.z / Xnum / system.UVRatio.z;//1.0;		
        UVs[3].y = (pos.y / SystemSize.y / Ynum + Yimage)/ system.UVRatio.y; 	     //0.0  
        ////////////////////////////////////////////////
        // -x 
        ////////////////////////////////////////////////
        UVs[4].x = (pos.z / SystemSize.z / Xnum + Ximage) / system.UVRatio.z;           //0.0    
        UVs[4].y = (pos.y / SystemSize.y / Ynum + Yimage) / system.UVRatio.y;			 //0.0

        UVs[5].x = (pos.z / SystemSize.z / Xnum + Ximage) / system.UVRatio.z;           //0.0 
        UVs[5].y = ((pos.y + 1.0f) / SystemSize.y / Ynum+ Yimage) / system.UVRatio.y;  //1.0           

        UVs[6].x = ((pos.z + 1.0f) / SystemSize.z / Xnum + Ximage) / system.UVRatio.z;  //1.0	 
        UVs[6].y = ((pos.y + 1.0f) / SystemSize.y / Ynum+ Yimage) / system.UVRatio.y; //1.0

        UVs[7].x = ((pos.z + 1.0f) / SystemSize.z / Xnum + Ximage) / system.UVRatio.z;  //1.0	
        UVs[7].y = (pos.y / SystemSize.y / Ynum + Yimage) / system.UVRatio.y;			 //0.0	         
        ////////////////////////////////////////////////
        //+y 
        ////////////////////////////////////////////////
        UVs[8].x = (pos.x / SystemSize.x / Xnum + Ximage) / system.UVRatio.x;			  //0.0
        UVs[8].y = (pos.z / SystemSize.z / Ynum * ImageManip.y + Yimage * 3) / system.UVRatio.z;            //0.0   

        UVs[9].x = (pos.x / SystemSize.x / Xnum + Ximage) / system.UVRatio.x;			  //0.0	
        UVs[9].y = ((pos.z + 1.0f) / SystemSize.z / Ynum * ImageManip.y + Yimage * 3) / system.UVRatio.z;  //1.0  

        UVs[10].x = ((pos.x + 1.0f) / SystemSize.x / Xnum + Ximage) / system.UVRatio.x; //1.0
        UVs[10].y = ((pos.z + 1.0f) / SystemSize.z / Ynum * ImageManip.y + Yimage * 3) / system.UVRatio.z;  //1.0

        UVs[11].x = ((pos.x + 1.0f) / SystemSize.x / Xnum + Ximage) / system.UVRatio.x;  //1.0
        UVs[11].y = (pos.z / SystemSize.z / Ynum * ImageManip.y + Yimage * 3) / system.UVRatio.z;           //0.0   
        ////////////////////////////////////////////////
        //-y
        ////////////////////////////////////////////////
        UVs[12].x = (pos.x / SystemSize.x / Xnum ) / system.UVRatio.x;			  //0.0
        UVs[12].y = (pos.z / SystemSize.z / Ynum * ImageManip.y + Yimage * 3) / system.UVRatio.z;            //0.0  

        UVs[13].x = (pos.x / SystemSize.x / Xnum ) / system.UVRatio.x;			  //0.0	
        UVs[13].y = ((pos.z + 1.0f) / SystemSize.z / Ynum * ImageManip.y + Yimage * 3) / system.UVRatio.z;  //1.0   

        UVs[14].x = ((pos.x + 1.0f) / SystemSize.x / Xnum ) / system.UVRatio.x; //1.0
        UVs[14].y = ((pos.z + 1.0f) / SystemSize.z / Ynum * ImageManip.y + Yimage * 3) / system.UVRatio.z;  //1.0

        UVs[15].x = ((pos.x + 1.0f) / SystemSize.x / Xnum ) / system.UVRatio.x;  //1.0
        UVs[15].y = (pos.z / SystemSize.z / Ynum * ImageManip.y + Yimage * 3) / system.UVRatio.z;           //0.0  

        ////////////////////////////////////////////////
        //+z
        ////////////////////////////////////////////////
        UVs[16].x = (pos.x / SystemSize.x / Xnum + Ximage) / system.UVRatio.x;//0.0
        UVs[16].y = (pos.y / SystemSize.y / Ynum + Yimage * 2) / system.UVRatio.y;  //0.0

        UVs[17].x = (pos.x / SystemSize.x / Xnum + Ximage) / system.UVRatio.x; //0.0	
        UVs[17].y = ((pos.y + 1) / SystemSize.y / Ynum + Yimage * 2) / system.UVRatio.y;  //1.0	

        UVs[18].x = ((pos.x + 1) / SystemSize.x / Xnum + Ximage) / system.UVRatio.x; //1.0
        UVs[18].y = ((pos.y + 1) / SystemSize.y / Ynum + Yimage * 2) / system.UVRatio.y;  //1.0     

        UVs[19].x = ((pos.x + 1) / SystemSize.x / Xnum + Ximage) / system.UVRatio.x;  //1.0
        UVs[19].y = (pos.y / SystemSize.y / Ynum + Yimage * 2) / system.UVRatio.y; //0.0  
        ////////////////////////////////////////////////
        //-z
        ////////////////////////////////////////////////
        UVs[20].x = (pos.x / SystemSize.x / Xnum ) / system.UVRatio.x;//0.0
        UVs[20].y = (pos.y / SystemSize.y / Ynum + Yimage * 2) / system.UVRatio.y;  //0.0        

        UVs[21].x = (pos.x / SystemSize.x / Xnum ) / system.UVRatio.x; //0.0	
        UVs[21].y = ((pos.y + 1) / SystemSize.y / Ynum + Yimage * 2) / system.UVRatio.y;  //1.0	        

        UVs[22].x = ((pos.x + 1) / SystemSize.x / Xnum ) / system.UVRatio.x; //1.0
        UVs[22].y = ((pos.y + 1) / SystemSize.y / Ynum + Yimage * 2) / system.UVRatio.y;  //1.0     

        UVs[23].x = ((pos.x + 1) / SystemSize.x / Xnum ) / system.UVRatio.x;  //1.0
        UVs[23].y = (pos.y / SystemSize.y / Ynum + Yimage * 2) / system.UVRatio.y; //0.0


        
        _meshF.mesh.uv = UVs;
        _meshF.gameObject.GetComponent<Renderer>().material = mat;
    }
    class CubeTimer
    {
        public float timeToLive;
        public PoolObject<GameObject> _obj;
    }
    static void ResetRBSpeed(GameObject go)
    {
        Vector3 norm = MouseSelectionTool_Greedy.MouseClickNormal;
        Rigidbody rb = go.GetComponent<Rigidbody>();
        Vector3 dif = go.transform.position - MouseSelectionTool_Greedy.MouseClickWorldPos;
        dif /= dif.magnitude;//Normalize the direction 
        //Reset the cube's state
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 randomForce = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        //Applies a force to the cubed based on position in respect to where the ray hit the voxel system, the normal of the surface, and a random force with bias agaist gravity(y value is higher)
        rb.AddForce(((dif * s_DifNormalRandom_ForceMultiplier.x) + (norm * s_DifNormalRandom_ForceMultiplier.y) + randomForce * s_DifNormalRandom_ForceMultiplier.z) * 100);

        //Apply a random torque force 
        Vector3 randomRot = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f) , Random.Range(-1.0f, 1.0f));
        rb.AddTorque(randomRot * Random.Range(-10, 10));
    }
    /// <summary>
    /// Frees the cube that's first in the death timer's list, which is coincidentally the oldest
    /// </summary>
    static void FreeOldestCube()
    {
        ResetRBSpeed(DeathTimers[0]._obj._Object);
        //DeathTimers[0]._obj._Object.SetActive(false);
        DeathTimers[0]._obj.ReturnToPool();       
        DeathTimers.Remove(DeathTimers[0]);
    }

}

