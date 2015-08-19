
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
[RequireComponent (typeof (VoxSystemChunkManager),typeof (VoxelSystemUpdateManager))]
public class VoxelSystemGreedy : MonoBehaviour {

	public  int XSize = 2;
	public  int YSize = 2;	
	public  int ZSize = 2;

	public float VoxelSpacing=1.0f;
	public int ChunkSizeX = 16;	
	public int ChunkSizeY = 16;	
	public int ChunkSizeZ = 16;

	public bool UniqueSides = false;
	public GameObject[,,] chunks;
	public VoxelSystemChunkGreedy[,,] chunks_vcs;
	public GameObject VoxelChunkGO;
	public GameObject EmptyChunk;
	public bool IsEmptyFilled =false;
    public bool SpawnPhysicsCubes = false;
	public Vector3 offset;	
	public VoxMaterialFactory factory;
	public bool Initialized = false;    
	public VoxelSystemUpdateManager VSUM;
    public InertiaCalculator calc;  
    public Rigidbody rigidBody;

    public Vector3 UVRatio;
    VoxelChunkEmpty EmptyChunkSync;
    
	int currentSelectedVoxel=1;
	GameObject KRB; //Unused RB only used to encapsulate the MeshColliders;
    bool CreateConvexColliders = false;   
	VoxelPos _tempVP;



	public int chunkMaskI;
	void Start () {
        if(!Initialized)Init();
		
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.U))
		{
			UpdateMeshes();
			
		}
	}
    public void Init()
    {
        //Create a new empty chunk for this system and sync it's size
        EmptyChunk = Instantiate(EmptyChunk, this.transform.position, Quaternion.identity) as GameObject;
        EmptyChunk.name = "EmptyChunk";
		EmptyChunk.transform.parent = this.transform;

        
        VSUM = GetComponent<VoxelSystemUpdateManager>();
		rigidBody = GetComponent<Rigidbody>();
		if(rigidBody!= null)
		calc = gameObject.AddComponent<InertiaCalculator>();

        EmptyChunkSync = EmptyChunk.GetComponent<VoxelChunkEmpty>();
        EmptyChunkSync.XSize = ChunkSizeX;
        EmptyChunkSync.YSize = ChunkSizeY;
        EmptyChunkSync.ZSize = ChunkSizeZ;


//      VoxelEvents.onVoxelAdded += AddVoxel;
//      VoxelEvents.onVoxelRemoved += RemoveVoxel;
        VoxelEvents.onVoxelSwitch += SelectedVoxelType;
        
        chunks = new GameObject[XSize, YSize, ZSize];
        
		offset.x = -(XSize*ChunkSizeX*VoxelSpacing/2.0f);
		offset.y = -(YSize*ChunkSizeY*VoxelSpacing/2.0f);
		offset.z = -(ZSize*ChunkSizeZ*VoxelSpacing/2.0f);

        
        
		Transform chunksParent;
		if(rigidBody == null)
		{
            chunksParent = this.transform;
           // chunkMask = LayerMask.GetMask("Rays");
            //chunkMaskI = LayerMask.NameToLayer("Rays");
		}else
		{ 
            //chunkMask = LayerMask.
            //chunkMaskI = LayerMask.NameToLayer("Rays");
            
            KRB = new GameObject();
			KRB.name = "KRB";
			KRB.transform.parent = this.transform;
            KRB.layer = chunkMaskI;
			KRB.tag = "Chunk";
			Rigidbody r = KRB.AddComponent<Rigidbody>();
            KRB.transform.localPosition = Vector3.zero;
            KRB.transform.localRotation = Quaternion.identity;
			r.isKinematic = true;
			r.useGravity = false;
			//r.constraints = RigidbodyConstraints.FreezeAll;		
			CreateConvexColliders = true;
			chunksParent = KRB.transform;

		}


        for (int x =0; x < XSize; x++){
            for (int y =0; y < YSize; y++){
                for (int z =0; z < ZSize; z++){
                    chunks[x,y,z] = Instantiate(VoxelChunkGO, this.transform.position, this.transform.rotation) as GameObject;
                    chunks[x, y, z].layer = chunkMaskI;
					chunks[x,y,z].transform.parent = chunksParent;
                    Vector3 chunkOffset = new Vector3(offset.x + x * ChunkSizeX * VoxelSpacing,
                                                      offset.y + y * ChunkSizeY * VoxelSpacing,
                                                      offset.z + z * ChunkSizeZ * VoxelSpacing);
                    if (XSize == 1) chunkOffset.x = 0;
                    if (YSize == 1) chunkOffset.y = 0;
                    if (ZSize == 1) chunkOffset.z = 0;

					chunks[x,y,z].transform.Translate(chunkOffset);
                    
                    chunks[x,y,z].name = "Chunk "+x+" "+y+ " "+z;
                }
            }
        }
        
        chunks_vcs = new VoxelSystemChunkGreedy[XSize, YSize, ZSize];
        
        //Generate the neighbours for each chunk
        for (int x =0; x < XSize; x++){
            for (int y =0; y < YSize; y++){
                for (int z =0; z < ZSize; z++){
                    
                    ChunkNeighbours temp_neighbours = new ChunkNeighbours();
                    
                    if(x+1 == XSize)temp_neighbours.xpos = EmptyChunk;
                    else temp_neighbours.xpos = chunks[x+1,y,z];
                    
                    if(x-1 < 0)temp_neighbours.xneg = EmptyChunk;
                    else temp_neighbours.xneg = chunks[x-1,y,z];
                    
                    if(y+1 == YSize) temp_neighbours.ypos = EmptyChunk;
                    else temp_neighbours.ypos = chunks[x,y+1,z];
                    
                    if(y-1 < 0)temp_neighbours.yneg = EmptyChunk;
                    else temp_neighbours.yneg = chunks[x,y-1,z];
                    
                    if(z+1 == ZSize)temp_neighbours.zpos = EmptyChunk;
                    else temp_neighbours.zpos = chunks[x,y,z+1];
                    
                    if(z-1 < 0)temp_neighbours.zneg = EmptyChunk;
                    else temp_neighbours.zneg = chunks[x,y,z-1];
                    
                    temp_neighbours.UpdateVoxelChunkRefs();
                    
                    chunks_vcs[x,y,z] = chunks[x,y,z].GetComponent<VoxelSystemChunkGreedy>();
                    chunks_vcs[x,y,z].neighbours = temp_neighbours; 
                    chunks_vcs[x,y,z].VoxelSpacing = VoxelSpacing;
                    chunks_vcs[x,y,z].XSize = ChunkSizeX;
                    chunks_vcs[x,y,z].YSize = ChunkSizeY;
                    chunks_vcs[x,y,z].ZSize = ChunkSizeZ;
                    chunks_vcs[x,y,z].factory = factory;

                    chunks_vcs[x,y,z].chunkPos = new VoxelPos(x,y,z);
                    chunks_vcs[x,y,z].systemParent = this;
                }
            }
        }
        
        //Make sure each chunk is initialized
        EmptyChunkSync.Init();
		EmptyChunkSync.emptyShell.filled = IsEmptyFilled;
        //EmptyChunkSync.gameObject.transform.parent = this.transform;

        for (int x =0; x < XSize; x++){
            for (int y =0; y < YSize; y++){
                for (int z =0; z < ZSize; z++){
                    chunks_vcs[x,y,z].CreateConvexCollider = CreateConvexColliders;
                    chunks_vcs[x,y,z].Init();
					chunks_vcs[x,y,z].UniqueSides = UniqueSides;

                }
            }
        }
        //All the chunks needs to be completely initialized
        //before neighbours are calculated to avoid nulls value refrences.
        //for (int x =0; x < XSize; x++){
        //    for (int y =0; y < YSize; y++){
        //        for (int z =0; z < ZSize; z++){
        //            chunks_vcs[x,y,z].CalculateNeighbours();
        //        }
        //    }
        //}
        if (UniqueSides) UVRatio = new Vector3(1.0f, 1.0f, 1.0f);
        else
        {
            float max = Mathf.Max(Mathf.Max(XSize * XSize, YSize * YSize), ZSize * ZSize);
            UVRatio = new Vector3(max / (XSize * XSize), max / (YSize * YSize), max / (ZSize * ZSize));
        }
       	
		_tempVP = new VoxelPos();

        Initialized = true;
    }
	//Event Method
	public void SelectedVoxelType(int _currentSelectedVoxel)
	{
		currentSelectedVoxel = _currentSelectedVoxel;
	}
	public void AddVoxel(VoxelPos voxel, bool update,int type)
	{
		AddVoxel (voxel.x, voxel.y, voxel.z, update, type);
	}
	public void AddVoxel(VoxelPos voxel, bool update)
	{
		AddVoxel (voxel.x, voxel.y, voxel.z, update);
	}
	public void AddVoxel(int x, int y, int z, bool update)
	{
		AddVoxel(x, y, z, update, currentSelectedVoxel);
	}
	public void AddVoxel(int x, int y, int z, bool update, int type)
	{

		//Calculate the position
		VoxelPos voxPos = new VoxelPos(x % ChunkSizeX,
									   y % ChunkSizeY,
									   z % ChunkSizeZ);

		VoxelPos chunkPos = new VoxelPos(x / ChunkSizeX,
										 y / ChunkSizeY,
										 z / ChunkSizeZ);	

		if(voxPos.x >=0 && voxPos.y >=0 && voxPos.z >=0)		
		if((chunkPos.x < XSize && chunkPos.y < YSize && chunkPos.z < ZSize)
	    && (chunkPos.x >= 0    && chunkPos.y >= 0    && chunkPos.z >= 0))
		{		
			if(!chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].locked)
			{
				if(chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].voxelType!= currentSelectedVoxel)
				{
					VoxelFactory.GenerateVoxel(type, ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z]);	
				}else chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].filled = true;


				if(update)
				{
                    //chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z].neighbours.UpdateNeighbours();
                    UpdateNeightbours(chunkPos,voxPos);
                    VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);
                    //if(!chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].Generating)
                    //{
                    //    System.Action up = go;
                    //    System.Action gen = go;
                    //    chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].neighbours.GetNeighbourDelegate(ref up,ref gen);
                    //    if(up==null)
                    //    VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);
                    //    else VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z],up,gen);
                    //}
				}
			}
		}
	}
    public void QuickAdd(VoxelPos voxel, int type, bool update)
    {
        VoxelPos voxPos = new VoxelPos(voxel.x % ChunkSizeX,
                                       voxel.y % ChunkSizeY,
                                       voxel.z % ChunkSizeZ);

        VoxelPos chunkPos = new VoxelPos(voxel.x / ChunkSizeX,
                                         voxel.y / ChunkSizeY,
                                         voxel.z / ChunkSizeZ);


        VoxelFactory.GenerateVoxel(type, ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z]);

        if (update)
        {

            //chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z].neighbours.UpdateNeighbours();
            UpdateNeightbours(chunkPos,voxPos);
            VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);

            //An attempt at syncing up chunk updates
            //if (!chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].Generating)
            //{
            //    System.Action up = go;
            //    System.Action gen = go;
            //    chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z].neighbours.GetNeighbourDelegate(ref up, ref gen);
            //    if (up == null)
            //        VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);
            //    else VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z], up, gen);
            //}
        }
    }
    public void QuickRemove(VoxelPos voxel, bool update)
    {
        VoxelPos voxPos = new VoxelPos(voxel.x % ChunkSizeX,
                                       voxel.y % ChunkSizeY,
                                       voxel.z % ChunkSizeZ);

        VoxelPos chunkPos = new VoxelPos(voxel.x / ChunkSizeX,
                                         voxel.y / ChunkSizeY,
                                         voxel.z / ChunkSizeZ);


        VoxelFactory.GenerateVoxel(0, ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z]);
        if (update)
        {

            UpdateNeightbours(chunkPos,voxPos);

            VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);

            ////An attempt at syncing up chunk updates
            //if (!chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].Generating)
            //{
            //    System.Action up = go;
            //    System.Action gen = go;
            //    chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z].neighbours.GetNeighbourDelegate(ref up, ref gen);
            //    if (up == null)
            //        VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);
            //    else VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z], up, gen);
            //}
        }
        
    }   
   
	public void RemoveVoxel(VoxelPos _voxel, bool update)
	{
		//Calculate the position
		VoxelPos voxPos = new VoxelPos(_voxel.x % ChunkSizeX,
									   _voxel.y % ChunkSizeY,
									   _voxel.z % ChunkSizeZ);

		VoxelPos chunkPos = new VoxelPos(_voxel.x / ChunkSizeX,
										 _voxel.y / ChunkSizeY,
										 _voxel.z / ChunkSizeZ);

		if(voxPos.x >=0 && voxPos.y >=0 && voxPos.z >=0)		
		if((chunkPos.x < XSize && chunkPos.y < YSize && chunkPos.z < ZSize)
	    && (chunkPos.x >= 0 && chunkPos.y >= 0 && chunkPos.z >= 0))
		{
			if(!chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].locked){

                

                if (SpawnPhysicsCubes && chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z].voxelType != 0)
                {
                    Vector3 worldPos = new Vector3(offset.x + chunks_vcs[0, 0, 0].offset.x + VoxelSpacing/2.0f + ( voxPos.x + chunkPos.x * ChunkSizeX) * VoxelSpacing,
                                                   offset.y + chunks_vcs[0, 0, 0].offset.y + VoxelSpacing/2.0f + ( voxPos.y + chunkPos.y * ChunkSizeY) * VoxelSpacing,
                                                   offset.z + chunks_vcs[0, 0, 0].offset.z + VoxelSpacing/2.0f + ( voxPos.z + chunkPos.z * ChunkSizeZ) * VoxelSpacing);
                    //Debug.Log(worldPos.ToString());
                    worldPos = transform.localToWorldMatrix.MultiplyPoint3x4(worldPos);
                    //Debug.Log("After   "+worldPos.ToString());
                    CubePool.SpawnCubeAtLocation(worldPos, this, _voxel, chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z].voxelType);
                }
                VoxelFactory.GenerateVoxel(0, ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x, voxPos.y, voxPos.z]);

				if(update)
				{
                    UpdateNeightbours(chunkPos,voxPos);
	                //chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].neighbours.UpdateNeighbours();
	                VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);

					//An attempt at syncing up chunk updates
                    //if(!chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].Generating)
                    //{
                    //    System.Action up = go;
                    //    System.Action gen = go;
                    //    chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z].blocks[voxPos.x,voxPos.y,voxPos.z].neighbours.GetNeighbourDelegate(ref up,ref gen);						
                    //    if(up==null)
                    //        VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z]);
                    //    else VSUM.QueueChunkForUpdate(ref chunks_vcs[chunkPos.x, chunkPos.y, chunkPos.z],up,gen);
                    //}
				}
			}
		}
	}
	public struct VoxelPos2
	{
		VoxelPos vp;
		VoxelPos cp;
		public VoxelPos2(VoxelPos _vp, VoxelPos _cp)
		{
			vp = _vp;
			cp = _cp;
		}
		public void Set(int vpX, int vpY, int vpZ, int cpX, int cpY, int cpZ)
		{
			vp.Set(vpX, vpY, vpZ);
			cp.Set(cpX, cpY, cpZ);
		}

	}
	public VoxelShell PosX(VoxelPosByte vp, VoxelPos cp)
	{
		if (vp.x + 1 == ChunkSizeX)//going outside of the chunk
		{
			if (cp.x < XSize - 1)//less then the max size
			{
				return chunks_vcs[cp.x+1, cp.y, cp.z].blocks[0, vp.y, vp.z];
			}
			else return EmptyChunkSync.emptyShell;
		}
		else
		{
			return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x+1, vp.y, vp.z];
		}
	}
	public VoxelShell NegX (VoxelPosByte vp, VoxelPos cp)
	{
		if (vp.x - 1 == -1)
		{
			if (cp.x > 0)
			{
				return chunks_vcs[cp.x-1, cp.y, cp.z].blocks[ChunkSizeX-1, vp.y, vp.z];
			}else return EmptyChunkSync.emptyShell;
		}
		else
		{
			return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x-1, vp.y, vp.z];
		}
	}
	public VoxelShell PosY (VoxelPosByte vp, VoxelPos cp)
	{
		if (vp.y + 1 == ChunkSizeY)//going outside of the chunk
		{
			if (cp.y < YSize - 1)//less then the max size
			{
				return chunks_vcs[cp.x, cp.y+1, cp.z].blocks[vp.x, 0, vp.z];
			}else return EmptyChunkSync.emptyShell;
		}
		else
		{
			return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y+1, vp.z];
		}
	}
	public VoxelShell NegY (VoxelPosByte vp, VoxelPos cp)
	{
		if (vp.y - 1 == -1)
		{
			if (cp.y > 0)
			{
				return chunks_vcs[cp.x, cp.y-1, cp.z].blocks[vp.x, ChunkSizeY-1, vp.z];
			}else return EmptyChunkSync.emptyShell;
		}
		else
		{
			return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y-1, vp.z];
		}

	}
	public VoxelShell PosZ (VoxelPosByte vp, VoxelPos cp)
	{
		if (vp.z + 1 == ChunkSizeZ)//going outside of the chunk
		{
			if (cp.z < ZSize - 1)//less then the max size
			{
				return chunks_vcs[cp.x, cp.y, cp.z+1].blocks[vp.x, vp.y, 0];
			} return EmptyChunkSync.emptyShell;
		}
		else
		{
			return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z+1];
		}
	}
	public VoxelShell NegZ (VoxelPosByte vp, VoxelPos cp)
	{
		if (vp.z - 1 == -1)
		{
			if (cp.z > 0)
			{
				return chunks_vcs[cp.x, cp.y, cp.z-1].blocks[vp.x, vp.y, ChunkSizeZ-1];
			}else return EmptyChunkSync.emptyShell;
		}
		else
		{
			return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z-1];
		}
	}
    public void UpdateNeightbours(VoxelPos cp, VoxelPos vp)
    {
        //+x
        if (vp.x + 1 == ChunkSizeX)//going outside of the chunk
        {
            if (cp.x < XSize - 1)//less then the max size
            {
                VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x+1, cp.y, cp.z].blocks[0, vp.y, vp.z].parentChunk);
            }
        }
        else
        {
			VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x+1, vp.y, vp.z].parentChunk);
        }

        //-x
		if (vp.x - 1 == -1)
		{
			if (cp.x > 0)
			{
				VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x-1, cp.y, cp.z].blocks[ChunkSizeX-1, vp.y, vp.z].parentChunk);
			}
		}
		else
		{
			VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x-1, vp.y, vp.z].parentChunk);
		}


        //+y
		if (vp.y + 1 == ChunkSizeY)//going outside of the chunk
		{
			if (cp.y < YSize - 1)//less then the max size
			{
				VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y+1, cp.z].blocks[vp.x, 0, vp.z].parentChunk);
			}
		}
		else
		{
			VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y+1, vp.z].parentChunk);
		}

        //-y
		if (vp.y - 1 == -1)
		{
			if (cp.y > 0)
			{
				VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y-1, cp.z].blocks[vp.x, ChunkSizeY-1, vp.z].parentChunk);
			}
		}
		else
		{
			VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y-1, vp.z].parentChunk);
		}


        //+z
		if (vp.z + 1 == ChunkSizeZ)//going outside of the chunk
		{
			if (cp.z < ZSize - 1)//less then the max size
			{
				VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y, cp.z+1].blocks[vp.x, vp.y, 0].parentChunk);
			}
		}
		else
		{
			VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z+1].parentChunk);
		}

        //-z
		if (vp.z - 1 == -1)
		{
			if (cp.z > 0)
			{
				VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y, cp.z-1].blocks[vp.x, vp.y, ChunkSizeZ-1].parentChunk);
			}
		}
		else
		{
			VSUM.QueueChunkForUpdate(ref chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z-1].parentChunk);
		}

        




        //VoxelShell v = Neighbouring(ref _v, Direction.negX);
        //if( v.parentChunk != null)
        //VSUM.QueueChunkForUpdate(ref v.parentChunk);

        //v = Neighbouring(ref _v, Direction.posX);
        //if (v.parentChunk != null)
        //VSUM.QueueChunkForUpdate(ref v.parentChunk);

        //v = Neighbouring(ref _v, Direction.negY);
        //if (v.parentChunk != null)
        //VSUM.QueueChunkForUpdate(ref v.parentChunk);

        //v = Neighbouring(ref _v, Direction.posY);
        //if (v.parentChunk != null)
        //VSUM.QueueChunkForUpdate(ref v.parentChunk);

        //v = Neighbouring(ref _v, Direction.negZ);
        //if (v.parentChunk != null)
        //VSUM.QueueChunkForUpdate(ref v.parentChunk);

        //v = Neighbouring(ref _v, Direction.posZ);
        //if (v.parentChunk != null)
        //VSUM.QueueChunkForUpdate(ref v.parentChunk);
    }
	void go(){}
	public void UpdateMeshes()
	{
		for(int x = 0; x < XSize; x++)
		{
			for(int y = 0; y < YSize; y++)
			{
				for(int z = 0; z < ZSize; z++)
				{
					VSUM.QueueChunkForUpdate(ref chunks_vcs[x,y,z]);
				}
			}
		}
	}
    public enum Direction { posX, negX, posY, negY, posZ, negZ};
    
//    public VoxelShell Neighbouring(ref VoxelShell _v, Direction _d)
//    {
//        VoxelPos vp;
//        VoxelPos cp;
//        vp.x = _v.vp.x;
//        vp.y = _v.vp.y;
//        vp.z = _v.vp.z;
//
//        cp.x = _v.parentChunk.chunkPos.x;
//		cp.y = _v.parentChunk.chunkPos.y;
//		cp.z = _v.parentChunk.chunkPos.z;
//
//        try
//        {
//
//            if (_d == Direction.posX)
//            {                
//				NegX(vp,cp);
//                return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z];
//            }
//            else if (_d == Direction.negX)
//            {
//                --vp.x;
//                if (vp.x < 0)
//                {
//                    --cp.x;
//                    vp.x = ChunkSizeX - 1;
//                    if (cp.x < 0)
//                    {
//                        return EmptyChunkSync.emptyShell;
//                    }
//                }
//                return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z];
//            }
//            else if (_d == Direction.posY)
//            {
//                ++vp.y;
//                if (vp.y >= ChunkSizeY)
//                {
//                    ++cp.y;
//                    vp.y = 0;
//                    if (cp.y >= YSize)
//                    {
//                        return EmptyChunkSync.emptyShell;
//                    }
//                }
//                return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z];
//
//            }
//            else if (_d == Direction.negY)
//            {
//                --vp.y;
//                if (vp.y < 0)
//                {
//                    --cp.y;
//                    vp.y = ChunkSizeY - 1;
//                    if (cp.y < 0)
//                    {
//                        return EmptyChunkSync.emptyShell;
//                    }
//                }
//                return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z];
//            }
//            else if (_d == Direction.posZ)
//            {
//                ++vp.z;
//                if (vp.z >= ChunkSizeZ)
//                {
//                    ++cp.z;
//                    vp.z = 0;
//                    if (cp.z >= ZSize)
//                    {
//                        return EmptyChunkSync.emptyShell;
//                    }
//                }
//                return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z];
//            }
//            else // (_d == Direction.negZ)
//            {
//                --vp.z;
//                if (vp.z < 0)
//                {
//                    --cp.z;
//                    vp.z = ChunkSizeZ - 1;
//                    if (cp.z < 0)
//                    {
//                        return EmptyChunkSync.emptyShell;
//                    }
//                }
//                return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z];
//            }
//        }
//        catch (System.IndexOutOfRangeException e)
//        {
//            return chunks_vcs[cp.x, cp.y, cp.z].blocks[vp.x, vp.y, vp.z];
//        }
//    }



}
