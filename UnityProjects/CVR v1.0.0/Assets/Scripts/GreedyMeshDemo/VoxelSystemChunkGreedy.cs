using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelSystemChunkGreedy : VoxelChunk{


	public bool CreateConvexCollider = false;
	public bool MeshBaked = false;
	public bool MeshBaking = false;
	public bool UniqueSides = false;

	public ChunkNeighbours neighbours;
	public VoxelPos chunkPos = new VoxelPos(0,0,0);   
	public VoxelSystemGreedy systemParent;

	public float chunkMass =0;
	public Vector3 centerOfMass;

	protected VoxelSystemChunkGreedy thisChunk;
	protected voxList<JamQuad> Quads;


    

	VoxelPos bookmark;
	GameObject ConvexCollider;
	MeshCollider CC;
	MeshCollider meshCollider;
	MeshFilter meshFilter;

    byte[,,] genFlags;

    //byte flag notes
    /*
     * 1  negx
     * 2  posx
     * 4  negy
     * 8  posy
     * 16 negz
     * 32 posz
     * 
     */
    byte[] flags = new byte[] { (byte)1, (byte)2, (byte)4, (byte)8, (byte)16, (byte)32 };


	void Start()
	{

	}
	void Update()
	{
 
	}
	public override void Init()
	{
        if(CreateConvexCollider)
        {
    		ConvexCollider = new GameObject(gameObject.name + " CC");
    		CC = ConvexCollider.AddComponent<MeshCollider>();
    		ConvexCollider.transform.parent = gameObject.transform.parent.parent;
    		ConvexCollider.layer = LayerMask.NameToLayer("Ignore Raycast");
    		CC.convex = true;
    		ConvexCollider.transform.localPosition = this.transform.localPosition;
    		ConvexCollider.transform.localRotation = this.transform.localRotation;
    		ConvexCollider.transform.localScale = this.transform.localScale;
        }
		meshCollider = GetComponent<MeshCollider>();
		meshFilter = GetComponent<MeshFilter>();

		thisChunk = gameObject.GetComponent<VoxelSystemChunkGreedy>();
		offset.x = - XSize*VoxelSpacing/2.0f;//Starting position of vertices
		offset.y = - YSize*VoxelSpacing/2.0f;
		offset.z = - ZSize*VoxelSpacing/2.0f;

		InitShells();
		InitDics();
        for (int i = 1; i <= 1; i++)
        {
            SubmeshIndexChecker(i);
        }
        

        

		Triangles = new voxList<int[]>();
		UVs = new voxList<Vector2>();
		TrIndex = new voxList<int>(); //triangle material index for submeshing
		Verts = new voxList<Vector3>();
		vmesh = new Mesh();
		Quads = new voxList<JamQuad>();
        genFlags = new byte[XSize, YSize, ZSize];
		
        
		//needsUpdating = true;
		Initialized = true;
	}

	public void UpdateMesh()
	{
		GenerateQuad();
	}

	private void ClearAllGeoObjects()
	{
		Triangles.Clear();
		Verts.Clear();
		UVs.Clear();
		TrIndex.Clear();
		Quads.Clear();

        MaterialIndex.Clear();
        SubmeshCount = 0;
        SubmeshIndex.Clear();

	}
    private void ResetGenFlags()
    {        
		for (int x = 0; x < XSize; ++x)
			for (int y = 0; y < YSize; ++y)
                for (int z = 0; z < ZSize; ++z)
                {
                    genFlags[x, y, z] = 0;
                }
    }
	protected void GenerateQuad()
	{
        lock (thisChunk)
        {
            Generating = true;

            ClearAllGeoObjects();
            int faceCount = 0;

            //temp vars
            int xtemp, ytemp, ztemp, type, width, height;
            bool QuadDone = false;
            bookmark = new VoxelPos();



            if (systemParent.rigidBody != null)
            {
                chunkMass = 0;
                centerOfMass = Vector3.zero;

                for (int x = 0; x < XSize; ++x)
                    for (int y = 0; y < YSize; ++y)
                        for (int z = 0; z < ZSize; ++z)
                        {

                            if (blocks[x, y, z].filled)
                            {
                                chunkMass += blocks[x, y, x].voxel.Mass;
                            }

                        }

                for (int x = 0; x < XSize; ++x)
                    for (int y = 0; y < YSize; ++y)
                        for (int z = 0; z < ZSize; ++z)
                        {
                            if (blocks[x, y, z].filled)
                            {
                                centerOfMass.x += (x + offset.x) * blocks[x, y, z].voxel.Mass / chunkMass;
                                centerOfMass.y += (y + offset.y) * blocks[x, y, z].voxel.Mass / chunkMass;
                                centerOfMass.z += (z + offset.z) * blocks[x, y, z].voxel.Mass / chunkMass;
                            }
                        }
            }

            ResetGenFlags();




            //Notes
  
            //Greedy JamQuad Mesh Generation
            /*Greedy meshing  works by checking ajacent voxels to for similar states and uses this info to generate a quad for the combined
             * 1. If the side is empty
             * 2. If the voxel is the same type
             * If both of these conditions are met the quad grows in width 
             * Repeat until conditions is false
             * then check voxel above the starting point
             */
            
            #region xnegLoop

            ResetBookmark();

            for (int x = 0; x < XSize; ++x)
            {
                for (int y = 0; y < YSize; ++y)
                {
                    for (int z = 0; z < ZSize; ++z)
                    {
                        if (blocks[x, y, z].filled && !systemParent.Neighbouring(ref blocks[x, y, z], VoxelSystemGreedy.Direction.negX).filled)
                        {
                            if (!((genFlags[x, y, z] & flags[0]) == flags[0]))
                            {
                                //set variables to default values
                                width = 1;
                                height = 1;
                                xtemp = x;
                                ytemp = y;
                                ztemp = z;
                                QuadDone = false;
                                type = blocks[xtemp, ytemp, ztemp].voxel.VoxelType;

                                do
                                {

                                    do
                                    {//First Row 
                                        //Check the adjacent voxel

                                        if (ztemp + 1 == ZSize) break;

                                        VoxelShell negX = systemParent.Neighbouring(ref blocks[xtemp, ytemp, ztemp], VoxelSystemGreedy.Direction.negX);
                                        bool temp = negX.parentChunk != null;
                                        if (temp) temp = systemParent.Neighbouring(ref negX, VoxelSystemGreedy.Direction.posZ).filled;
                                        if (blocks[xtemp, ytemp, ztemp + 1].filled && !temp)
                                        {
                                            //If the voxel is the same type and the posZ voxel isnt already generated
                                            if (blocks[xtemp, ytemp, ztemp + 1].voxel.VoxelType == type && !((genFlags[xtemp, ytemp, ztemp + 1] & flags[0]) == flags[0]))
                                            {
                                                width++;
                                                ztemp++;
                                                if (ztemp == ZSize - 1)
                                                {//Bookmark it when we get to the end												
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip                                                  
                                                    bookmark.z = 0;
                                                    bookmark.y = ytemp + 1;
                                                    bookmark.x = xtemp;
                                                    break;
                                                }
                                            }
                                            else
                                            { //Bookmark it when the type isnt the same
                                                ztemp++;
                                                if (ztemp >= ZSize)
                                                {
                                                    bookmark.z = 0;
                                                    bookmark.y = ytemp + 1;
                                                }
                                                else
                                                {
                                                    bookmark.z = ztemp;
                                                    bookmark.y = ytemp;
                                                }
                                                bookmark.x = xtemp;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //bookmark it even if its empty or z +1 is filled, we need somewhere to go when the quad is finished!
                                            if (ztemp >= ZSize)
                                            {
                                                bookmark.z = 0;
                                                bookmark.y = ytemp + 1;
                                            }
                                            else
                                            {
                                                bookmark.z = ztemp;
                                                bookmark.y = ytemp;
                                            }
                                            bookmark.x = xtemp;
                                            break;
                                        }
                                    } while (true);

                                    ztemp = z;//reset ztemp position 

                                    //Check if rows above are the same type/filled
                                    //Check the voxel directly above, maybe we can go up
                                    if (ytemp < YSize - 1)
                                        if (blocks[xtemp, ytemp + 1, ztemp].filled
                                           && blocks[xtemp, ytemp + 1, ztemp].voxel.VoxelType == type)
                                        {
                                            //Move the cursor up a row 
                                            ytemp++;
                                        }
                                        else
                                        {
                                            //forget about going up, gtfo and moveon									
                                            QuadDone = true;
                                            continue;
                                        }
                                    //Iterate through voxels along the z axis until you find a voxel that is empty or different type

                                    do
                                    {
                                        for (int zc = ztemp; zc < ztemp + width; zc++)
                                        {
                                            //if there is a fault, move on and forget about that row
                                            if (!blocks[xtemp, ytemp, zc].filled
                                                || blocks[xtemp, ytemp, zc].voxel.VoxelType != type
                                                || systemParent.Neighbouring(ref blocks[xtemp, ytemp, zc], VoxelSystemGreedy.Direction.negX).filled
                                                || ((genFlags[xtemp, ytemp, zc] & flags[0]) == flags[0]))
                                            {
                                                QuadDone = true;
                                                break;
                                            }
                                        }
                                        if (!QuadDone)
                                        {
                                            ytemp++;
                                            if (ytemp == YSize)
                                            {
                                                if (y < YSize - 1) height++;
                                                QuadDone = true;
                                                break;
                                            }
                                            else height++;
                                        }
                                        else break;
                                    } while (true);
                                } while (!QuadDone);

                                for (int w = z; w < z + width; w++)
                                {
                                    for (int h = y; h < y + height; h++)
                                    {
                                        //if(!blocks[x, h, w].locked)
                                        genFlags[x, h, w] += flags[0];
                                    }
                                }
                                if (Quads.vcount <= Quads.vList.Count)
                                {
                                    Quads.Add(new JamQuad(SubmeshIndexChecker(type),
                                                       width,
                                                       height,
                                                       new Vector3(offset.x + x * VoxelSpacing,
                                                                   offset.y + y * VoxelSpacing,
                                                                   offset.z + z * VoxelSpacing),
                                                       VoxelSpacing,
                                                       new VoxelPos(-1, 0, 0),
                                                       ref faceCount,
                                                       new VoxelPos(x, y, z),
                                                       ref thisChunk,
                                                      systemParent.UVRatio,
                                                      UniqueSides));
                                }
                                else
                                {
                                    Quads.vList[Quads.vcount].GenerateQuad(SubmeshIndexChecker(type),
                                                                           width,
                                                                           height,
                                                                           new Vector3(offset.x + x * VoxelSpacing,
                                                                                       offset.y + y * VoxelSpacing,
                                                                                       offset.z + z * VoxelSpacing),
                                                                           VoxelSpacing,
                                                                           new VoxelPos(-1, 0, 0),
                                                                           ref faceCount,
                                                                           new VoxelPos(x, y, z),
                                                                           ref thisChunk,
                                                                           systemParent.UVRatio,
                                                                           UniqueSides);
                                    Quads.vcount++;
                                }
                                TrIndex.Add(Quads.vList[Quads.vcount - 1].submeshIndex);

                                x = bookmark.x;
                                if (x >= XSize) x = XSize - 1;
                                y = bookmark.y;
                                if (y >= YSize) y = YSize - 1;
                                z = bookmark.z - 1;
                                //infinite loop failsafe for testing only
                                //loopCount++;
                                //if(loopCount>= loopCutter) return mesh;
                            }
                        }
                    }
                }
            }

            #endregion
            #region xposLoop

            ResetBookmark();
            for (int x = 0; x < XSize; ++x)
            {
                for (int y = 0; y < YSize; ++y)
                {
                    for (int z = 0; z < ZSize; ++z)
                    {
                        if (blocks[x, y, z].filled && !systemParent.Neighbouring(ref blocks[x, y, z], VoxelSystemGreedy.Direction.posX).filled)
                        {
                            if (!((genFlags[x, y, z] & flags[1]) == flags[1]))
                            {
                                //set variables to default values
                                width = 1;
                                height = 1;
                                xtemp = x;
                                ytemp = y;
                                ztemp = z;
                                QuadDone = false;
                                type = blocks[xtemp, ytemp, ztemp].voxel.VoxelType;

                                do
                                {
                                    //First Row 
                                    do
                                    {
                                        //Check the adjacent voxel
                                        if (ztemp + 1 == ZSize) break;
                                        VoxelShell xpos = systemParent.Neighbouring(ref blocks[xtemp, ytemp, ztemp], VoxelSystemGreedy.Direction.posX);
                                        bool temp = xpos.parentChunk != null;
                                        if (temp) temp = systemParent.Neighbouring(ref xpos, VoxelSystemGreedy.Direction.posZ).filled;
                                        if (blocks[xtemp, ytemp, ztemp + 1].filled && !temp)
                                        {
                                            if (blocks[xtemp, ytemp, ztemp + 1].voxel.VoxelType == type && !((genFlags[xtemp, ytemp, ztemp + 1] & flags[1]) == flags[1]))
                                            {
                                                width++;
                                                ztemp++;
                                                if (ztemp == ZSize - 1)//Bookmark it when we get to the end 
                                                {
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip                                                    
                                                    bookmark.z = 0;
                                                    bookmark.y = ytemp + 1;
                                                    bookmark.x = xtemp;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Bookmark it when the type isnt the same
                                                ztemp++;
                                                if (ztemp >= ZSize)
                                                {
                                                    bookmark.z = 0;
                                                    bookmark.y = ytemp + 1;
                                                }
                                                else
                                                {
                                                    bookmark.z = ztemp;
                                                    bookmark.y = ytemp;
                                                }
                                                bookmark.x = xtemp;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
                                            if (ztemp >= ZSize)
                                            {
                                                bookmark.z = 0;
                                                bookmark.y = ytemp + 1;
                                            }
                                            else
                                            {
                                                bookmark.z = ztemp;
                                                bookmark.y = ytemp;
                                            }
                                            bookmark.x = xtemp;
                                            break;
                                        }
                                    } while (true);

                                    ztemp = z;//reset ztemp position 

                                    //Check if rows above are the same type/filled
                                    //Check the voxel directly above, maybe we can go up
                                    if (ytemp < YSize - 1)
                                        if (blocks[xtemp, ytemp + 1, ztemp].filled
                                            && blocks[xtemp, ytemp + 1, ztemp].voxel.VoxelType == type)
                                        {
                                            //Move the cursor up a row 
                                            ytemp++;
                                        }
                                        else
                                        {
                                            //forget about going up, gtfo and moveon									
                                            QuadDone = true;
                                            continue;
                                        }

                                    //Iterate through voxels along the z axis until you find a voxel that is empty or different type							
                                    do
                                    {
                                        for (int zc = ztemp; zc < ztemp + width; zc++)
                                        {
                                            //if there is a fault, move on and forget about that row
                                            if (!blocks[xtemp, ytemp, zc].filled
                                                || blocks[xtemp, ytemp, zc].voxel.VoxelType != type
                                                || systemParent.Neighbouring(ref blocks[xtemp, ytemp, zc], VoxelSystemGreedy.Direction.posX).filled
                                                || ((genFlags[xtemp, ytemp, zc] & flags[1]) == flags[1]))
                                            {
                                                QuadDone = true;
                                                break;
                                            }
                                        }
                                        if (!QuadDone)
                                        {
                                            //If you made it thought the last for-loop unscathed then you just leveled up a row :)

                                            ytemp++;
                                            if (ytemp == YSize)
                                            {
                                                if (y < ytemp - 1) height++;
                                                QuadDone = true;
                                                break;
                                            }
                                            else height++;

                                        }
                                        else break;
                                    } while (true);
                                } while (!QuadDone);

                                for (int w = z; w < z + width; w++)
                                {
                                    for (int h = y; h < y + height; h++)
                                    {
                                        //if (!blocks[x, h, w].locked)
                                        genFlags[x, h, w] += flags[1];
                                    }
                                }
                                if (Quads.vcount <= Quads.vList.Count)
                                {
                                    Quads.Add(new JamQuad(SubmeshIndexChecker(type),
                                                       width,
                                                       height,
                                                          new Vector3(offset.x + x * VoxelSpacing + VoxelSpacing,
                                                                      offset.y + y * VoxelSpacing,
                                                                      offset.z + z * VoxelSpacing),
                                                       VoxelSpacing,
                                                       new VoxelPos(1, 0, 0),
                                                       ref faceCount,
                                                       new VoxelPos(x, y, z),
                                                       ref thisChunk,
                                                      systemParent.UVRatio,
                                                      UniqueSides));
                                }
                                else
                                {
                                    Quads.vList[Quads.vcount].GenerateQuad(SubmeshIndexChecker(type),
                                                                           width,
                                                                           height,
                                                                           new Vector3(offset.x + x * VoxelSpacing + VoxelSpacing,
                                                                           offset.y + y * VoxelSpacing,
                                                                           offset.z + z * VoxelSpacing),
                                                                           VoxelSpacing,
                                                                           new VoxelPos(1, 0, 0),
                                                                           ref faceCount,
                                                                           new VoxelPos(x, y, z),
                                                                           ref thisChunk,
                                                                           systemParent.UVRatio,
                                                                           UniqueSides);
                                    Quads.vcount++;
                                }

                                TrIndex.Add(Quads.vList[Quads.vcount - 1].submeshIndex);

                                x = bookmark.x;
                                y = bookmark.y;
                                if (y >= YSize) y = YSize - 1;
                                z = bookmark.z - 1;


                            }
                        }
                    }
                }
            }

            #endregion

            #region ynegLoop

            ResetBookmark();
            for (int y = 0; y < YSize; ++y)
            {
                for (int x = 0; x < XSize; ++x)
                {
                    for (int z = 0; z < ZSize; ++z)
                    {
                        if (blocks[x, y, z].filled && !systemParent.Neighbouring(ref blocks[x, y, z], VoxelSystemGreedy.Direction.negY).filled)
                        {
                            if (!((genFlags[x, y, z] & flags[2]) == flags[2]))
                            {
                                //set variables to default values
                                width = 1;
                                height = 1;
                                xtemp = x;
                                ytemp = y;
                                ztemp = z;
                                QuadDone = false;
                                type = blocks[xtemp, ytemp, ztemp].voxel.VoxelType;

                                do
                                {
                                    //First Row 
                                    do
                                    {
                                        //Check the adjacent voxel
                                        if (ztemp + 1 == ZSize) break;
                                        VoxelShell yneg = systemParent.Neighbouring(ref blocks[xtemp, ytemp, ztemp], VoxelSystemGreedy.Direction.negY);
                                        bool temp = yneg.parentChunk != null;
                                        if (temp) temp = systemParent.Neighbouring(ref yneg, VoxelSystemGreedy.Direction.posZ).filled;
                                        if (blocks[xtemp, ytemp, ztemp + 1].filled && !temp)
                                        {

                                            if (blocks[xtemp, ytemp, ztemp + 1].voxel.VoxelType == type && !((genFlags[xtemp, ytemp, ztemp + 1] & flags[2]) == flags[2]))
                                            {
                                                width++;
                                                ztemp++;
                                                if (ztemp == ZSize - 1)//Bookmark it when we get to the end 
                                                {
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
                                                    bookmark.z = 0;
                                                    bookmark.x = xtemp + 1;
                                                    bookmark.y = ytemp;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Bookmark it when the type isnt the same
                                                ztemp++;
                                                if (ztemp >= ZSize)
                                                {
                                                    bookmark.z = 0;
                                                    bookmark.x = xtemp + 1;
                                                }
                                                else
                                                {
                                                    bookmark.z = ztemp;
                                                    bookmark.x = xtemp;
                                                }
                                                bookmark.y = y;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
                                            if (ztemp >= ZSize)
                                            {
                                                bookmark.z = 0;
                                                bookmark.x = xtemp + 1;
                                            }
                                            else
                                            {
                                                bookmark.z = ztemp;
                                                bookmark.x = xtemp;
                                            }
                                            bookmark.y = y;
                                            break;
                                        }
                                    } while (true);

                                    ztemp = z;//reset ztemp position 

                                    //Check if rows above are the same type/filled
                                    //Check the voxel directly above, maybe we can go up
                                    if (xtemp < XSize - 1)
                                        if (blocks[xtemp + 1, ytemp, ztemp].filled
                                            && blocks[xtemp + 1, ytemp, ztemp].voxel.VoxelType == type)
                                        {
                                            //Move the cursor up a row 
                                            xtemp++;
                                        }
                                        else
                                        {
                                            //forget about going up, gtfo and moveon
                                            QuadDone = true;
                                            continue;
                                        }

                                    //Iterate through voxels along the z axis until you find a voxel that is empty or different type							
                                    do
                                    {
                                        for (int zc = ztemp; zc < ztemp + width; zc++)
                                        {
                                            //if there is a fault, move on and forget about that row
                                            if (!blocks[xtemp, ytemp, zc].filled
                                                || blocks[xtemp, ytemp, zc].voxel.VoxelType != type
                                                || systemParent.Neighbouring(ref blocks[xtemp, ytemp, zc], VoxelSystemGreedy.Direction.negY).filled
                                                || ((genFlags[xtemp, ytemp, zc] & flags[2]) == flags[2]))
                                            {
                                                QuadDone = true;
                                                break;
                                            }
                                        }
                                        if (!QuadDone)
                                        {
                                            //If you made it thought the last for-loop unscathed then you just leveled up a row :)
                                            xtemp++;
                                            if (xtemp == XSize)
                                            {
                                                if (x < XSize - 1) height++;
                                                QuadDone = true;
                                                break;
                                            }
                                            else height++;
                                        }
                                        else break;
                                    } while (true);
                                } while (!QuadDone);

                                for (int w = z; w < z + width; w++)
                                {
                                    for (int h = x; h < x + height; h++)
                                    {
                                        //if(!blocks[h, y, w].locked)
                                        genFlags[h, y, w] += flags[2];
                                        //blocks[h, y, w].neighbours.ynegQuad = true;
                                    }
                                }
                                if (Quads.vcount <= Quads.vList.Count)
                                {
                                    Quads.Add(new JamQuad(SubmeshIndexChecker(type),
                                                       width,
                                                       height,
                                                       new Vector3(offset.x + x * VoxelSpacing,
                                                offset.y + y * VoxelSpacing,
                                                offset.z + z * VoxelSpacing),
                                                       VoxelSpacing,
                                                       new VoxelPos(0, -1, 0),
                                                       ref faceCount,
                                                       new VoxelPos(x, y, z),
                                                          ref thisChunk,
                                                      systemParent.UVRatio,
                                                      UniqueSides));
                                }
                                else
                                {
                                    Quads.vList[Quads.vcount].GenerateQuad(SubmeshIndexChecker(type),
                                                                           width,
                                                                           height,
                                                                           new Vector3(offset.x + x * VoxelSpacing,
                                                offset.y + y * VoxelSpacing,
                                                offset.z + z * VoxelSpacing),
                                                                           VoxelSpacing,
                                                                           new VoxelPos(0, -1, 0),
                                                                           ref faceCount,
                                                                           new VoxelPos(x, y, z),
                                                                           ref thisChunk,
                                                                       systemParent.UVRatio,
                                                                       UniqueSides);
                                    Quads.vcount++;
                                }
                                TrIndex.Add(Quads.vList[Quads.vcount - 1].submeshIndex);

                                x = bookmark.x;
                                if (x >= XSize) x = XSize - 1;
                                y = bookmark.y;
                                z = bookmark.z - 1;

                                //infinite loop failsafe for testing only
                                //loopCount++;
                                //if(loopCount>= loopCutter){ return mesh; Debug.Log("EndlessLoop");}
                            }
                        }
                    }
                }
            }

            #endregion
            #region yposLoop

            ResetBookmark();
            for (int y = 0; y < YSize; ++y)
            {
                for (int x = 0; x < XSize; ++x)
                {
                    for (int z = 0; z < ZSize; ++z)
                    {
                        if (blocks[x, y, z].filled && !systemParent.Neighbouring(ref blocks[x, y, z], VoxelSystemGreedy.Direction.posY).filled)
                        {
                            if (!((genFlags[x, y, z] & flags[3]) == flags[3]))
                            {
                                //set variables to default values
                                width = 1;
                                height = 1;
                                xtemp = x;
                                ytemp = y;
                                ztemp = z;
                                QuadDone = false;
                                type = blocks[xtemp, ytemp, ztemp].voxel.VoxelType;

                                do
                                {
                                    //First Row 
                                    do
                                    {
                                        if (ztemp + 1 == ZSize) break;
                                        //Check the adjacent voxel
                                        VoxelShell ypos = systemParent.Neighbouring(ref blocks[xtemp, ytemp, ztemp], VoxelSystemGreedy.Direction.posY);
                                        bool temp = ypos.parentChunk != null;
                                        if(temp) temp = systemParent.Neighbouring(ref ypos, VoxelSystemGreedy.Direction.posZ).filled;
                                        if (blocks[xtemp, ytemp, ztemp + 1].filled && !temp)
                                        {

                                            if (blocks[xtemp, ytemp, ztemp + 1].voxel.VoxelType == type && !((genFlags[xtemp, ytemp, ztemp + 1] & flags[3]) == flags[3]))
                                            {
                                                width++;
                                                ztemp++;
                                                if (ztemp == ZSize - 1)//Bookmark it when we get to the end 
                                                {
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip                                                    
                                                    bookmark.z = 0;
                                                    bookmark.x = xtemp + 1;
                                                    bookmark.y = ytemp;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Bookmark it when the type isnt the same
                                                ++ztemp;
                                                if (ztemp >= ZSize)
                                                {
                                                    bookmark.z = 0;
                                                    bookmark.x = xtemp + 1;
                                                }
                                                else
                                                {
                                                    bookmark.z = ztemp;
                                                    bookmark.x = xtemp;
                                                }
                                                bookmark.y = ytemp;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
                                            if (ztemp >= ZSize)
                                            {
                                                bookmark.z = 0;
                                                bookmark.x = xtemp + 1;
                                            }
                                            else
                                            {
                                                bookmark.z = ztemp;
                                                bookmark.x = xtemp;
                                            }
                                            bookmark.y = ytemp;
                                            break;
                                        }
                                    } while (true);

                                    ztemp = z;//reset ztemp position 

                                    //Check if rows above are the same type/filled
                                    //Check the voxel directly above, maybe we can go up
                                    if (xtemp < XSize - 1)
                                        if (blocks[xtemp + 1, ytemp, ztemp].filled
                                            && blocks[xtemp + 1, ytemp, ztemp].voxel.VoxelType == type)
                                        {
                                            //Move the cursor up a row 
                                            xtemp++;
                                        }
                                        else
                                        {
                                            //forget about going up, gtfo and moveon									
                                            QuadDone = true;
                                            continue;
                                        }

                                    //Iterate through voxels along the z axis until you find a voxel that is empty or different type							
                                    do
                                    {
                                        for (int zc = ztemp; zc < ztemp + width; zc++)
                                        {
                                            //if there is a fault, move on and forget about that row
                                            if (!blocks[xtemp, ytemp, zc].filled
                                                || blocks[xtemp, ytemp, zc].voxel.VoxelType != type
                                                || systemParent.Neighbouring(ref blocks[xtemp, ytemp, zc], VoxelSystemGreedy.Direction.posY).filled
                                                || ((genFlags[xtemp, ytemp, zc] & flags[3]) == flags[3]))
                                            {
                                                QuadDone = true;
                                                break;
                                            }
                                        }
                                        if (!QuadDone)
                                        {
                                            //If you made it thought the last for-loop unscathed then you just leveled up a row :)											
                                            xtemp++;
                                            if (xtemp == XSize)
                                            {
                                                if (x < XSize - 1) height++;
                                                QuadDone = true;
                                                break;
                                            }
                                            else height++;
                                        }
                                        else break;
                                    } while (true);
                                } while (!QuadDone);

                                for (int w = z; w < z + width; w++)
                                {
                                    for (int h = x; h < x + height; h++)
                                    {
                                        //if (!blocks[h, y, w].locked)
                                        genFlags[h, y, w] += flags[3];
                                        //blocks[h, y, w].neighbours.yposQuad = true;

                                    }
                                }
                                if (Quads.vcount <= Quads.vList.Count)
                                {
                                    Quads.Add(new JamQuad(SubmeshIndexChecker(type),
                                                       width,
                                                       height,
                                                       new Vector3(offset.x + x * VoxelSpacing,
                                                            offset.y + y * VoxelSpacing + VoxelSpacing,
                                                            offset.z + z * VoxelSpacing),
                                                       VoxelSpacing,
                                                       new VoxelPos(0, 1, 0),
                                                       ref faceCount,
                                                       new VoxelPos(x, y, z),
                                                       ref thisChunk,
                                                          systemParent.UVRatio,
                                                          UniqueSides));
                                }
                                else
                                {
                                    Quads.vList[Quads.vcount].GenerateQuad(SubmeshIndexChecker(type),
                                                                           width,
                                                                           height,
                                                                           new Vector3(offset.x + x * VoxelSpacing,
                                                                            offset.y + y * VoxelSpacing + VoxelSpacing,
                                                                            offset.z + z * VoxelSpacing),
                                                                           VoxelSpacing,
                                                                           new VoxelPos(0, 1, 0),
                                                                           ref faceCount,
                                                                           new VoxelPos(x, y, z),
                                                                           ref thisChunk,
                                                                           systemParent.UVRatio,
                                                                           UniqueSides);
                                    Quads.vcount++;
                                }
                                TrIndex.Add(Quads.vList[Quads.vcount - 1].submeshIndex);

                                x = bookmark.x;
                                if (x >= XSize) x = XSize - 1;
                                y = bookmark.y;//								
                                z = bookmark.z - 1;

                            }
                        }
                    }
                }
            }

            #endregion
            #region znegLoop

            ResetBookmark();
            for (int z = 0; z < ZSize; ++z)
            {
                for (int x = 0; x < XSize; ++x)
                {
                    for (int y = 0; y < YSize; ++y)
                    {
                        if (blocks[x, y, z].filled && !systemParent.Neighbouring(ref blocks[x, y, z], VoxelSystemGreedy.Direction.negZ).filled)
                        {
                            if (!((genFlags[x, y, z] & flags[4]) == flags[4]))
                            {
                                //set variables to default values
                                width = 1;
                                height = 1;
                                xtemp = x;
                                ytemp = y;
                                ztemp = z;
                                QuadDone = false;
                                type = blocks[xtemp, ytemp, ztemp].voxel.VoxelType;

                                do
                                {
                                    //First Row 
                                    do
                                    {
                                        //Check the adjacent voxel
                                        if (ytemp + 1 == YSize) break;
                                        VoxelShell zneg = systemParent.Neighbouring(ref blocks[xtemp, ytemp, ztemp], VoxelSystemGreedy.Direction.negZ);
                                        bool temp = zneg.parentChunk != null;
                                        if(temp) temp = systemParent.Neighbouring(ref zneg, VoxelSystemGreedy.Direction.posY).filled;
                                        if (blocks[xtemp, ytemp + 1, ztemp].filled && !temp)
                                        {

                                            if (blocks[xtemp, ytemp + 1, ztemp].voxel.VoxelType == type && !((genFlags[xtemp, ytemp + 1, ztemp] & flags[4]) == flags[4]))
                                            {
                                                width++;
                                                ytemp++;
                                                if (ytemp == YSize - 1)//Bookmark it when we get to the end 
                                                {
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
                                                    bookmark.y = 0;
                                                    bookmark.x = xtemp + 1;
                                                    bookmark.z = ztemp;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Bookmark it when the type isnt the same
                                                ytemp++;
                                                if (ytemp >= YSize)
                                                {
                                                    bookmark.y = 0;
                                                    bookmark.x = xtemp + 1;
                                                }
                                                else
                                                {
                                                    bookmark.y = ytemp;
                                                    bookmark.x = xtemp;
                                                }
                                                bookmark.z = ztemp;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
                                            if (ytemp >= YSize)
                                            {
                                                bookmark.y = 0;
                                                bookmark.x = xtemp + 1;
                                            }
                                            else
                                            {
                                                bookmark.y = ytemp;
                                                bookmark.x = xtemp;
                                            }
                                            bookmark.z = ztemp;
                                            break;
                                        }
                                    } while (true);

                                    ytemp = y;//reset ytemp position 

                                    //Check if rows above are the same type/filled
                                    //Check the voxel directly above, maybe we can go up
                                    if (xtemp < XSize - 1)
                                        if (blocks[xtemp + 1, ytemp, ztemp].filled
                                            && blocks[xtemp + 1, ytemp, ztemp].voxel.VoxelType == type)
                                        {
                                            //Move the cursor up a row 
                                            xtemp++;
                                        }
                                        else
                                        {
                                            //forget about going up, gtfo and moveon									
                                            QuadDone = true;
                                            continue;
                                        }

                                    //Iterate through voxels along the z axis until you find a voxel that is empty or different type							
                                    do
                                    {
                                        for (int yc = ytemp; yc < ytemp + width; yc++)
                                        {
                                            //if there is a fault, move on and forget about that row
                                            if (!blocks[xtemp, yc, ztemp].filled
                                                || blocks[xtemp, yc, ztemp].voxel.VoxelType != type
                                                || systemParent.Neighbouring(ref blocks[xtemp, yc, ztemp], VoxelSystemGreedy.Direction.negZ).filled
                                                || ((genFlags[xtemp, yc, ztemp] & flags[4]) == flags[4]))
                                            {
                                                QuadDone = true;
                                                break;
                                            }
                                        }
                                        if (!QuadDone)
                                        {
                                            //If you made it thought the last for-loop unscathed then you just leveled up a row :)

                                            xtemp++;
                                            if (xtemp == XSize)
                                            {
                                                if (x < XSize - 1) height++;
                                                QuadDone = true;
                                                break;
                                            }
                                            else height++;
                                        }
                                        else break;
                                    } while (true);
                                } while (!QuadDone);

                                for (int w = y; w < y + width; w++)
                                {
                                    for (int h = x; h < x + height; h++)
                                    {
                                        //if(!blocks[h, w, z].locked)
                                        genFlags[h, w, z] += flags[4];
                                        //blocks[h, w, z].neighbours.znegQuad = true;
                                    }
                                }
                                if (Quads.vcount <= Quads.vList.Count)
                                {
                                    Quads.Add(new JamQuad(SubmeshIndexChecker(type),
                                                       width,
                                                       height,
                                                       new Vector3(offset.x + x * VoxelSpacing,
                                                offset.y + y * VoxelSpacing,
                                                offset.z + z * VoxelSpacing),
                                                       VoxelSpacing,
                                                       new VoxelPos(0, 0, -1),
                                                       ref faceCount,
                                                       new VoxelPos(x, y, z),
                                                          ref thisChunk,
                                                      systemParent.UVRatio,
                                                      UniqueSides));
                                }
                                else
                                {
                                    Quads.vList[Quads.vcount].GenerateQuad(SubmeshIndexChecker(type),
                                                                           width,
                                                                           height,
                                                                           new Vector3(offset.x + x * VoxelSpacing,
                                                                        offset.y + y * VoxelSpacing,
                                                                        offset.z + z * VoxelSpacing),
                                                                           VoxelSpacing,
                                                                           new VoxelPos(0, 0, -1),
                                                                           ref faceCount,
                                                                           new VoxelPos(x, y, z),
                                                                           ref thisChunk,
                                                                       systemParent.UVRatio,
                                                                       UniqueSides);
                                    Quads.vcount++;
                                }
                                TrIndex.Add(Quads.vList[Quads.vcount - 1].submeshIndex);

                                x = bookmark.x;
                                y = bookmark.y - 1;
                                z = bookmark.z;
                                if (x >= XSize) x = XSize - 1;

                                //infinite loop failsafe for testing only
                                //loopCount++;
                                //if(loopCount>= loopCutter){ return mesh; Debug.Log("EndlessLoop");}
                            }
                        }
                    }
                }
            }

            #endregion
            #region zposLoop

            ResetBookmark();
            for (int z = 0; z < ZSize; ++z)
            {
                for (int x = 0; x < XSize; ++x)
                {
                    for (int y = 0; y < YSize; ++y)
                    {
                        if (blocks[x, y, z].filled && !systemParent.Neighbouring(ref blocks[x, y, z], VoxelSystemGreedy.Direction.posZ).filled)
                        {
                            if (!((genFlags[x, y, z] & flags[5]) == flags[5]))
                            {
                                //set variables to default values
                                width = 1;
                                height = 1;
                                xtemp = x;
                                ytemp = y;
                                ztemp = z;
                                QuadDone = false;
                                type = blocks[xtemp, ytemp, ztemp].voxel.VoxelType;

                                do
                                {
                                    //First Row 
                                    do
                                    {
                                        //Check the adjacent voxel
                                        if (ytemp + 1 == YSize) break;

                                        VoxelShell zpos = systemParent.Neighbouring(ref blocks[xtemp, ytemp, ztemp], VoxelSystemGreedy.Direction.posZ);
                                        bool temp = zpos.parentChunk != null;
                                        if(temp) temp = systemParent.Neighbouring(ref zpos, VoxelSystemGreedy.Direction.posY).filled;
                                        if (blocks[xtemp, ytemp + 1, ztemp].filled && !temp)
                                        {

                                            if (blocks[xtemp, ytemp + 1, ztemp].voxel.VoxelType == type && !((genFlags[xtemp, ytemp + 1, ztemp] & flags[5]) == flags[5]))
                                            {
                                                width++;
                                                ytemp++;
                                                if (ytemp == YSize - 1)//Bookmark it when we get to the end 
                                                {
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip

                                                    bookmark.y = 0;
                                                    bookmark.x = xtemp + 1;
                                                    bookmark.z = ztemp;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Bookmark it when the type isnt the same
                                                ytemp++;
                                                if (ytemp >= YSize)
                                                {
                                                    bookmark.y = 0;
                                                    bookmark.x = xtemp + 1;
                                                }
                                                else
                                                {
                                                    bookmark.y = ytemp;
                                                    bookmark.x = xtemp;
                                                }
                                                bookmark.z = ztemp;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
                                            if (ytemp >= YSize)
                                            {
                                                bookmark.y = 0;
                                                bookmark.x = xtemp + 1;
                                            }
                                            else
                                            {
                                                bookmark.y = ytemp;
                                                bookmark.x = xtemp;
                                            }
                                            bookmark.z = ztemp;
                                            break;
                                        }
                                    } while (true);

                                    ytemp = y;//reset ytemp position 

                                    //Check if rows above are the same type/filled
                                    //Check the voxel directly above, maybe we can go up
                                    if (xtemp < XSize - 1)
                                        if (blocks[xtemp + 1, ytemp, ztemp].filled
                                            && blocks[xtemp + 1, ytemp, ztemp].voxel.VoxelType == type)
                                        {
                                            //Move the cursor up a row 
                                            xtemp++;
                                        }
                                        else
                                        {
                                            //forget about going up, gtfo and moveon									
                                            QuadDone = true;
                                            continue;
                                        }

                                    //Iterate through voxels along the z axis until you find a voxel that is empty or different type							
                                    do
                                    {
                                        for (int yc = ytemp; yc < ytemp + width; yc++)
                                        {
                                            //if there is a fault, move on and forget about that row
                                            if (!blocks[xtemp, yc, ztemp].filled
                                                || blocks[xtemp, yc, ztemp].voxel.VoxelType != type
                                                || systemParent.Neighbouring(ref blocks[xtemp, yc, ztemp], VoxelSystemGreedy.Direction.posZ).filled
                                                || ((genFlags[xtemp, yc, ztemp] & flags[5]) == flags[5]))
                                            {
                                                QuadDone = true;
                                                break;
                                            }
                                        }
                                        if (!QuadDone)
                                        {
                                            //If you made it thought the last for-loop unscathed then you just leveled up a row :)                                         
                                            xtemp++;
                                            if (xtemp == XSize)
                                            {
                                                if (x < XSize - 1) height++;
                                                QuadDone = true;
                                                break;
                                            }
                                            else height++;


                                        }
                                        else break;
                                    } while (true);
                                } while (!QuadDone);

                                for (int w = y; w < y + width; w++)
                                {
                                    for (int h = x; h < x + height; h++)
                                    {
                                        //if(!blocks[h, w, z].locked)
                                        genFlags[h, w, z] += flags[5];
                                        //blocks[h, w, z].neighbours.zposQuad = true;
                                    }
                                }
                                if (Quads.vcount <= Quads.vList.Count)
                                {
                                    Quads.Add(new JamQuad(SubmeshIndexChecker(type),
                                                       width,
                                                       height,
                                                       new Vector3(offset.x + x * VoxelSpacing,
                                                                   offset.y + y * VoxelSpacing,
                                                                   offset.z + z * VoxelSpacing + VoxelSpacing),
                                                       VoxelSpacing,
                                                       new VoxelPos(0, 0, 1),
                                                       ref faceCount,
                                                       new VoxelPos(x, y, z),
                                                          ref thisChunk,
                                                      systemParent.UVRatio,
                                                      UniqueSides));
                                }
                                else
                                {
                                    Quads.vList[Quads.vcount].GenerateQuad(SubmeshIndexChecker(type),
                                                                           width,
                                                                           height,
                                                                           new Vector3(offset.x + x * VoxelSpacing,
                                                                            offset.y + y * VoxelSpacing,
                                                                            offset.z + z * VoxelSpacing + VoxelSpacing),
                                                                           VoxelSpacing,
                                                                           new VoxelPos(0, 0, 1),
                                                                           ref faceCount,
                                                                           new VoxelPos(x, y, z),
                                                                           ref thisChunk,
                                                                       systemParent.UVRatio,
                                                                       UniqueSides);
                                    Quads.vcount++;
                                }
                                TrIndex.Add(Quads.vList[Quads.vcount - 1].submeshIndex);

                                x = bookmark.x;
                                if (x >= XSize) x = XSize - 1;
                                y = bookmark.y - 1;
                                z = bookmark.z;
                            }
                        }
                        //Inertia Calculation
                    }
                }
            }

            #endregion


            MeshBaked = true;
            Generating = false;
        }


	}
	public void GenerateThisMesh()
	{
		queuedForUpdate = false;
		GenerateMesh ();
	}
	protected void InitShells()
	{
		blocks = new VoxelShell[XSize, YSize, ZSize];
		for (int x = 0; x < XSize; ++x)
		{
			for (int y = 0; y < YSize; ++y)
			{
				for (int z = 0; z < ZSize; ++z)
				{
					blocks[x, y, z] = new VoxelShell(ref thisChunk);
					blocks[x, y, z].voxel = new Voxel();
					blocks[x, y, z].vp = new VoxelPos(x, y, z);
				}
			}
		}
	}
    public void HardMeshUpdate()
    {
        GenerateQuad();
        GenerateMesh();
    }
	
	protected void GenerateMesh()
	{

        lock (thisChunk)
        {
            if (Generating)
            {
                Debug.Log("Still Generating mesh");
                return;
            }
            MeshBaking = true;
            Profiler.BeginSample("MeshClear");
            vmesh.Clear();
            Profiler.EndSample();

            Profiler.BeginSample("Quad Unboxing");
            for (int q = 0; q < Quads.vcount; ++q)
            {
                for (int i = 0; i < 4; ++i)
                {
                    Verts.Add(Quads.vList[q].verts[i]);
                    UVs.Add(Quads.vList[q].UVs[i]);
                }
                Triangles.Add(Quads.vList[q].triangles);
            }
            Profiler.EndSample();

            vmesh.vertices = Verts.ToArray();
            vmesh.subMeshCount = SubmeshCount;

            //Set each triangle for each submesh    		
            Profiler.BeginSample("Triangle Assignement");
            for (int i = 0; i < SubmeshCount; ++i)
            {

                vmesh.SetTriangles(GetSubMeshTriangles(i, ref Triangles, ref TrIndex), i);
            }
            Profiler.EndSample();

            Profiler.BeginSample("MaterialAssignment");
            //Build the chunks material array
            Material[] mat = new Material[SubmeshCount];
            for (int i = 0; i < SubmeshCount; ++i)
            {
                mat[i] = factory.VoxelMats[MaterialIndex[i]];
            }
            Profiler.EndSample();
			if(gameObject != null)
            GetComponent<Renderer>().materials = mat;
            vmesh.uv = UVs.ToArray();
            vmesh.RecalculateNormals();

            Profiler.BeginSample("Tangent Solver");
            //Use TangentSolver if shader requires it
            MeshUtils.TangentSolver(vmesh);
            Profiler.EndSample();
            Profiler.BeginSample("Assignments");
            //vmesh.Optimize();
            meshFilter.mesh = vmesh;

            //Nullify the mesh so it initiates a reset... :(            
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = vmesh;

            if (CreateConvexCollider)
            {
                CC.sharedMesh = null;
                CC.sharedMesh = vmesh;
            }
            Profiler.EndSample();
            //This stop the error leeking temporarily 
            if (systemParent.rigidBody != null)
                systemParent.calc.CalcIntertia();

            MeshBaking = false;
            MeshBaked = true;


        }
    }
	
	public bool Equals( ref object _chunk )
	{
		if (_chunk == null) return false;
		VoxelSystemChunkGreedy temp = _chunk as VoxelSystemChunkGreedy;
		if (temp == null) return false;
		else return Equals(temp);
	}
	public bool Equals(VoxelSystemChunkGreedy _chunk)
	{
		if (_chunk == null) return false;
		return chunkPos.Equals(_chunk.chunkPos);
	}    

	void ResetBookmark ()
	{
		bookmark.x =0;
		bookmark.y =0;
		bookmark.z =0;
	} 
    //public void CalculateNeighbours()
    //{
    //    for (int x = 0; x < XSize; ++x) {
    //        for (int y = 0; y < YSize; ++y) {
    //            for (int z = 0; z < ZSize; ++z) {
    //                //Debug.Log("x: "+x+" y:"+y+" z:"+z);
    //                Neighbours temp_neighbours = new Neighbours();
    //                if(x+1 == XSize)
    //                {
    //                    temp_neighbours.xpos = neighbours.xpos_vcs.blocks[0,y,z];
    //                }
    //                else temp_neighbours.xpos = blocks[x+1,y,z];
					
    //                if(x == 0)	
    //                {
    //                    temp_neighbours.xneg = neighbours.xneg_vcs.blocks[XSize-1,y,z];	
    //                }
    //                else temp_neighbours.xneg = blocks[x-1,y,z];
					
    //                if(y+1 == YSize) 
    //                {
    //                    temp_neighbours.ypos = neighbours.ypos_vcs.blocks[x,0,z];
    //                }
    //                else temp_neighbours.ypos = blocks[x,y+1,z];
					
    //                if(y == 0) 
    //                {
    //                    temp_neighbours.yneg = neighbours.yneg_vcs.blocks[x,YSize-1,z];
    //                }
    //                else temp_neighbours.yneg = blocks[x,y-1,z];
					
    //                if(z+1 == ZSize)
    //                {
    //                    temp_neighbours.zpos = neighbours.zpos_vcs.blocks[x,y,0];
    //                }
    //                else temp_neighbours.zpos = blocks[x,y,z+1];
					
    //                if(z == 0) 
    //                {
    //                    temp_neighbours.zneg = neighbours.zneg_vcs.blocks[x,y,ZSize-1];
    //                }
    //                else temp_neighbours.zneg = blocks[x,y,z-1];
					
    //                blocks[x,y,z].neighbours = temp_neighbours;					
					
    //                //blocks[x,y,z].locked = false;
    //            }				
    //        }
    //    }
    //}
}
