﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelSystemChunkGreedy : VoxelChunkGreedy {

	public ChunkNeighbours neighbours;
	public VoxelPos chunkPos = new VoxelPos(0,0,0) ;
	VoxelPos bookmark;
	void Start()
	{
	}
	void Update()
	{
		if (needsUpdating)
		{
			UpdateMesh();
			needsUpdating = false;
		}
	}
	public override void Init()
	{
		thisChunk = gameObject.GetComponent<VoxelSystemChunkGreedy>();
		offset.x = - XSize*VoxelSpacing/2.0f;//Starting position of vertices
		offset.y = - YSize*VoxelSpacing/2.0f;
		offset.z = - ZSize*VoxelSpacing/2.0f;

		InitShells();
		InitDics();

		Triangles = new List<Triangle>();
		UVs = new List<Vector2>();
		TrIndex = new List<int>(); //index for submeshing
		Verts = new List<Vector3>();
		vmesh = new Mesh();
		Quads = new List<Quad>();

		Initialized = true;
	}
	protected override Mesh GenerateMesh()
	{			
		Triangles.Clear();
		Verts.Clear();
		UVs.Clear();
		TrIndex.Clear();
		vmesh.Clear();
		Quads.Clear();

		int faceCount = 0;

		//temp vars
		int xtemp, ytemp, ztemp, type, width, height;
		bool QuadDone = false;
		bookmark = new VoxelPos();


		for (int x = 0; x < XSize; x++)
			for (int y = 0; y < YSize; y++)
				for (int z = 0; z < ZSize; z++)
					blocks[x, y, z].neighbours.ResetFlags();

		//Notes
		/*
		 * In reguards to the two different ways of checking adjacent voxel values
		 * ill be using neighbours when checking if an object is filled because it'll allow me to check adjacent chunks    
		 */

		#region xnegLoop

		{
			ResetBookmark();
			for (int x = 0; x < XSize; ++x)
			{
				for (int y = 0; y < YSize; ++y)
				{
					for (int z = 0; z < XSize; ++z)
					{
						if (blocks[x, y, z].filled && !blocks[x, y, z].locked && !blocks[x, y, z].neighbours.xneg.filled)
						{
							if (!blocks[x, y, z].neighbours.xnegQuad)
							{
								//set variables to default values
								width = 1;
								height = 1;
								xtemp = x;
								ytemp = y;
								ztemp = z;
								QuadDone = false;
								type = blocks[xtemp, ytemp, ztemp].voxel.VoxelType;

								do{
									//First Row 
									do{
										//Check the adjacent voxel
										if(ztemp+1 == ZSize)break;
										if (blocks[xtemp, ytemp, ztemp + 1].filled && !blocks[xtemp, ytemp, ztemp].neighbours.xneg.neighbours.zpos.filled)
										{

											if (blocks[xtemp, ytemp, ztemp + 1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp+1].neighbours.xnegQuad)
											{
												width++;
												ztemp++;
												if (ztemp == ZSize - 1){//Bookmark it when we get to the end												
													//Bookmark this location to return to, when the current quad is complete.
													//If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip                                                  
													bookmark.z = 0;
													bookmark.y = ytemp + 1;
													bookmark.x = xtemp;
													break;
												}
											}else{ //Bookmark it when the type isnt the same
												ztemp++;
												if (ztemp >= ZSize){
													bookmark.z = 0;
													bookmark.y = ytemp + 1;
												}else{
													bookmark.z = ztemp;
													bookmark.y = ytemp;
												}
												bookmark.x = xtemp;
												break;
											}
										}else{
											//bookmark it even if its empty or z +1 is filled, we need somewhere to go when the quad is finished!
											if (ztemp >= ZSize){
												bookmark.z = 0;
												bookmark.y = ytemp + 1;
											}else{
												bookmark.z = ztemp;
												bookmark.y = ytemp;
											}
											bookmark.x = xtemp;
											break;
										}
									}while (true);

									ztemp = z;//reset ztemp position 

									//Check if rows above are the same type/filled
									//Check the voxel directly above, maybe we can go up
									if (ytemp < YSize-1)
										if (blocks[xtemp, ytemp + 1, ztemp].filled 
											&& blocks[xtemp, ytemp + 1, ztemp].voxel.VoxelType == type){
											//Move the cursor up a row 
											ytemp++;
										}else{
											//forget about going up, gtfo and moveon									
											QuadDone = true;
											continue;
										}
									//Iterate through voxels along the z axis until you find a voxel that is empty or different type

									do{
										for (int zc = ztemp; zc < ztemp + width; zc++){
											//if there is a fault, move on and forget about that row
											if (!blocks[xtemp, ytemp, zc].filled 
											  || blocks[xtemp, ytemp, zc].voxel.VoxelType != type 
											  || blocks[xtemp, ytemp, zc].neighbours.xneg.filled 
											  || blocks[xtemp, ytemp, zc].neighbours.xnegQuad){
												QuadDone = true;
												break;
											}
										}
										if (!QuadDone){
											ytemp++;
											if(ytemp == YSize){ 												
												if(y < YSize-1)height++;
												QuadDone = true;
												break;
											}
											else height++;
										}
										else break;
									} while (true);
								} while (!QuadDone);

								for (int w = z; w < z + width; w++){
									for (int h = y; h < y + height; h++){
										if(!blocks[x, h, w].locked)
										blocks[x, h, w].neighbours.xnegQuad = true;
									}
								}

								Quads.Add(new Quad(SubmeshIndexChecker(type),
												   width,
												   height,
												   new Vector3(offset.x + x * VoxelSpacing,
															   offset.y + y * VoxelSpacing,
															   offset.z + z * VoxelSpacing),
													VoxelSpacing,
													new VoxelPos(-1, 0, 0),
													ref faceCount));
								TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
								y = bookmark.y;
								if(y>=YSize) y=YSize-1;
								z = bookmark.z - 1;
								//infinite loop failsafe for testing only
								//loopCount++;
								//if(loopCount>= loopCutter) return mesh;
							}
						}
					}
				}
			}
		}
		#endregion

		#region xposLoop
		{
			ResetBookmark();
			for (int x = 0; x < XSize; ++x)
			{
				for (int y = 0; y < YSize; ++y)
				{
					for (int z = 0; z < XSize; ++z)
					{
						if (blocks[x, y, z].filled && !blocks[x, y, z].locked && !blocks[x, y, z].neighbours.xpos.filled)
						{
							if (!blocks[x, y, z].neighbours.xposQuad)
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
										if (blocks[xtemp, ytemp, ztemp + 1].filled && !blocks[xtemp, ytemp, ztemp].neighbours.xpos.neighbours.zpos.filled)
										{
											if (blocks[xtemp, ytemp, ztemp + 1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp + 1].neighbours.xposQuad)
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
												|| blocks[xtemp, ytemp, zc].neighbours.xpos.filled
												|| blocks[xtemp, ytemp, zc].neighbours.xposQuad)
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
												if (y < ytemp-1) height++;
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
										if (!blocks[x, h, w].locked)
											blocks[x, h, w].neighbours.xposQuad = true;
									}
								}

								Quads.Add(new Quad(SubmeshIndexChecker(type),
												   width,
												   height,
												   new Vector3(offset.x + x + 1 * VoxelSpacing,
															   offset.y + y * VoxelSpacing,
															   offset.z + z * VoxelSpacing),
												   VoxelSpacing,
												   new VoxelPos(1, 0, 0),
												   ref faceCount));

								TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
								y = bookmark.y;
								if (y >= YSize) y = YSize - 1;
								z = bookmark.z - 1;


							}
						}
					}
				}
			}
		}
		#endregion

		#region yposLoop
		{
			ResetBookmark();
			for (int y = 0; y < YSize; ++y)
			{
				for (int x = 0; x < XSize; ++x)
				{
					for (int z = 0; z < ZSize; ++z)
					{
						if (blocks[x, y, z].filled && !blocks[x, y, z].locked && !blocks[x, y, z].neighbours.ypos.filled)
						{
							if (!blocks[x, y, z].neighbours.yposQuad)
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
										if (blocks[xtemp, ytemp, ztemp + 1].filled && !blocks[xtemp, ytemp, ztemp].neighbours.ypos.neighbours.zpos.filled)
										{

											if (blocks[xtemp, ytemp, ztemp + 1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp + 1].neighbours.yposQuad)
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
												|| blocks[xtemp, ytemp, zc].neighbours.ypos.filled
												|| blocks[xtemp, ytemp, zc].neighbours.yposQuad)
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
												if (x < XSize-1) height++;
												QuadDone = true;
												break;
											}else height++;
										}else break;
									} while (true);
								} while (!QuadDone);

								for (int w = z; w < z + width; w++)
								{
									for (int h = x; h < x + height; h++)
									{										
										if (!blocks[h, y, w].locked)
											blocks[h, y, w].neighbours.yposQuad = true;

									}
								}

								Quads.Add(new Quad(SubmeshIndexChecker(type),
												   width,
												   height,
												   new Vector3(offset.x + x * VoxelSpacing,
															   offset.y + y + 1 * VoxelSpacing,
															   offset.z + z * VoxelSpacing),
												   VoxelSpacing,
												   new VoxelPos(0, 1, 0),
												   ref faceCount));

								TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
								if (x >= XSize)x = XSize - 1;
								y = bookmark.y;//								
								z = bookmark.z - 1;						

							}
						}
					}
				}
			}
		}
		#endregion
        #region ynegLoop
        {
			ResetBookmark();
            for (int y = 0; y < YSize; ++y)
            {
                for (int x = 0; x < XSize ; ++x)
                {
                    for (int z = 0; z < ZSize ; ++z)
                    {
                        if (blocks[x, y, z].filled && !blocks[x, y, z].locked && !blocks[x, y, z].neighbours.yneg.filled)
                        {
                            if (!blocks[x, y, z].neighbours.ynegQuad)
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
										if(ztemp+1 == ZSize)break;

                                        if (blocks[xtemp, ytemp, ztemp + 1].filled && !blocks[xtemp, ytemp, ztemp].neighbours.yneg.neighbours.zpos.filled)
                                        {

                                            if (blocks[xtemp, ytemp, ztemp + 1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp].neighbours.ynegQuad)
                                            {
                                                width++;
                                                ztemp++;
                                                if (ztemp == ZSize - 1)//Bookmark it when we get to the end 
                                                {
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
													bookmark.z = 0;
													bookmark.x = xtemp + 1;													
													bookmark.y = y;
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
                                        if (blocks[xtemp + 1, ytemp, ztemp].filled && blocks[xtemp + 1, ytemp, ztemp].voxel.VoxelType == type)
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
											    || blocks[xtemp, ytemp, zc].neighbours.yneg.filled
											    || blocks[xtemp, ytemp, zc].neighbours.ynegQuad)
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
												if (x < XSize-1) height++;
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
										if(!blocks[h, y, w].locked)
                                        blocks[h, y, w].neighbours.ynegQuad = true;
                                    }
                                }

                                Quads.Add(new Quad(SubmeshIndexChecker(type),
                                                   width,
                                                   height,
                                                   new Vector3(offset.x + x * VoxelSpacing,
                                                               offset.y + y * VoxelSpacing,
                                                               offset.z + z * VoxelSpacing),
                                                   VoxelSpacing,
                                                   new VoxelPos(0, -1, 0),
                                                   ref faceCount));

                                TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
                                TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
                                //Debug.Log(bookmark.ToString());
                                x = bookmark.x;
								if(x>=XSize) x=XSize-1;
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
        }
        #endregion
        #region znegLoop
        {
			ResetBookmark();
            for (int z =0; z < ZSize; ++z)
            {
                for (int x = 0; x < XSize; ++x)
                {
                    for (int y = 0; y < YSize; ++y)
                    {
                        if (blocks[x, y, z].filled && !blocks[x, y, z].locked && !blocks[x, y, z].neighbours.zneg.filled)
                        {
                            if (!blocks[x, y, z].neighbours.znegQuad)
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
										if(ytemp+1 == YSize)break;
                                        if (blocks[xtemp, ytemp + 1, ztemp].filled && !blocks[xtemp, ytemp, ztemp].neighbours.ypos.neighbours.zneg.filled)
                                        {

                                            if (blocks[xtemp, ytemp + 1, ztemp].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp].neighbours.znegQuad)
                                            {
                                                width++;
                                                ytemp++;
                                                if (ytemp == YSize - 1)//Bookmark it when we get to the end 
                                                {
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
													bookmark.y = 0;
													bookmark.x = xtemp + 1;													
													bookmark.z = z;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Bookmark it when the type isnt the same
                                                ytemp++;
												if(ytemp >= YSize){
													bookmark.y = 0;
													bookmark.x = xtemp + 1;
												}
												else
												{
													bookmark.y = ytemp;
													bookmark.x = xtemp;
												}
												bookmark.z = z;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
											if(ytemp >= YSize){
												bookmark.y = 0;
												bookmark.x = xtemp + 1;
											}
											else
											{
												bookmark.y = ytemp;
												bookmark.x = xtemp;
											}
											bookmark.z = z;
                                            break;
                                        }
                                    } while (true);

                                    ytemp = y;//reset ytemp position 

                                    //Check if rows above are the same type/filled
                                    //Check the voxel directly above, maybe we can go up
                                    if (xtemp < XSize - 1)
                                        if (blocks[xtemp + 1, ytemp, ztemp].filled || blocks[xtemp + 1, ytemp, ztemp].voxel.VoxelType != type)
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
											    || blocks[xtemp, yc, ztemp].neighbours.zneg.filled
											    || blocks[xtemp, yc, ztemp].neighbours.znegQuad)
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
												if (x < XSize-1) height++;
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
										if(!blocks[h, w, z].locked)
                                        blocks[h, w, z].neighbours.znegQuad = true;
                                    }
                                }

                                Quads.Add(new Quad(SubmeshIndexChecker(type),
                                                   width,
                                                   height,
                                                   new Vector3(offset.x + x * VoxelSpacing,
                                            offset.y + y * VoxelSpacing,
                                            offset.z + z * VoxelSpacing),
                                                   VoxelSpacing,
                                                   new VoxelPos(0, 0, -1),
                                                   ref faceCount));

                                TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
                                TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
                                //Debug.Log(bookmark.ToString());
                                x = bookmark.x;
                                y = bookmark.y - 1;
                                z = bookmark.z;
								if(x>=XSize) x=XSize-1;

                                //infinite loop failsafe for testing only
                                //loopCount++;
                                //if(loopCount>= loopCutter){ return mesh; Debug.Log("EndlessLoop");}
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region zposLoop
        {
			ResetBookmark();
            for (int z = 0; z < ZSize; ++z)
            {
                for (int x = 0; x < XSize; ++x)
                {
                    for (int y = 0; y < YSize; ++y)
                    {
                        if (blocks[x, y, z].filled && !blocks[x, y, z].locked && !blocks[x, y, z].neighbours.zpos.filled)
                        {
                            if (!blocks[x, y, z].neighbours.zposQuad)
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
										if(ytemp+1 == YSize)break;
                                        if (blocks[xtemp, ytemp + 1, ztemp].filled && !blocks[xtemp, ytemp, ztemp].neighbours.ypos.neighbours.zpos.filled)
                                        {

                                            if (blocks[xtemp, ytemp + 1, ztemp].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp].neighbours.zposQuad)
                                            {
                                                width++;
                                                ytemp++;
                                                if (ytemp == YSize - 1)//Bookmark it when we get to the end 
                                                {
                                                    //Bookmark this location to return to, when the current quad is complete.
                                                    //If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip

													bookmark.y = 0;
													bookmark.x = xtemp + 1;													
													bookmark.z = z;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //Bookmark it when the type isnt the same
                                                ytemp++;
												if(ytemp >= YSize){
													bookmark.y = 0;
													bookmark.x = xtemp + 1;
												}
												else
												{
													bookmark.y = ytemp;
													bookmark.x = xtemp;
												}
												bookmark.z = z;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
											if(ytemp >= YSize){
												bookmark.y = 0;
												bookmark.x = xtemp + 1;
											}
											else
											{
												bookmark.y = ytemp;
												bookmark.x = xtemp;
											}
											bookmark.z = z;
                                            break;
                                        }
                                    } while (true);

                                    ytemp = y;//reset ytemp position 

                                    //Check if rows above are the same type/filled
                                    //Check the voxel directly above, maybe we can go up
                                    if (xtemp < XSize - 1)
                                        if (blocks[xtemp + 1, ytemp, ztemp].filled || blocks[xtemp + 1, ytemp, ztemp].voxel.VoxelType != type)
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
                                            if (!blocks[xtemp, yc, ztemp].filled || blocks[xtemp, yc, ztemp].voxel.VoxelType != type || blocks[xtemp, yc, ztemp].neighbours.zpos.filled)
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
												if (x < XSize-1) height++;
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
										if(!blocks[h, w, z].locked)
                                        blocks[h, w, z].neighbours.zposQuad = true;
                                    }
                                }

                                Quads.Add(new Quad(SubmeshIndexChecker(type),
                                                   width,
                                                   height,
                                                   new Vector3(offset.x + x * VoxelSpacing,
                                                                offset.y + y * VoxelSpacing,
                                                                offset.z + z + 1 * VoxelSpacing),
                                                   VoxelSpacing,
                                                   new VoxelPos(0, 0, 1),
                                                   ref faceCount));

                                TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
                                TrIndex.Add(Quads[Quads.Count - 1].submeshIndex);
                                //Debug.Log(bookmark.ToString());
                                x = bookmark.x;
								if(x>=XSize) x=XSize-1;
                                y = bookmark.y - 1;
                                z = bookmark.z;
                                
                            }
                        }
                    }
                }
            }
        }
        #endregion

		//Gather all the mesh data
		foreach (Quad q in Quads)
		{
			for (int i = 0; i < 4; i++)
			{
				Verts.Add(q.verts[i]);
				UVs.Add(q.UVs[i]);
			}
			Triangles.Add(q.triangles[0]);
			Triangles.Add(q.triangles[1]);
		}

		vmesh.vertices = Verts.ToArray();
		vmesh.subMeshCount = SubmeshCount;

		//Set each triangle for each submesh
		for (int i = 0; i < SubmeshCount; i++)
		{
			vmesh.SetTriangles(GetSubMeshTriangles(i, ref Triangles, ref TrIndex), i);
		}

		//Build the chunks material array
		List<Material> mat = new List<Material>();
		for (int i = 0; i < SubmeshCount; i++)
		{
			mat.Add(factory.VoxelMats[MaterialIndex[i]]);
		}

		renderer.materials = mat.ToArray();
		vmesh.uv = UVs.ToArray();
		vmesh.RecalculateNormals();

		//Use TangentSolver if shader requires it
		TangentSolver(vmesh);
			
		vmesh.Optimize();

		return vmesh;
	}	

	void ResetBookmark ()
	{
		bookmark.x =0;
		bookmark.y =0;
		bookmark.z =0;
	}

	public void CalculateNeighbours()
	{
		for (int x = 0; x < XSize; ++x) {
			for (int y = 0; y < YSize; ++y) {
				for (int z = 0; z < ZSize; ++z) {
					//Debug.Log("x: "+x+" y:"+y+" z:"+z);
					Neighbours temp_neighbours = new Neighbours();
					if(x+1 == XSize)
					{
						temp_neighbours.xpos = neighbours.xpos_vcs.blocks[0,y,z];
					}
					else temp_neighbours.xpos = blocks[x+1,y,z];
					
					if(x == 0)	
					{
						temp_neighbours.xneg = neighbours.xneg_vcs.blocks[XSize-1,y,z];	
					}
					else temp_neighbours.xneg = blocks[x-1,y,z];
					
					if(y+1 == YSize) 
					{
						temp_neighbours.ypos = neighbours.ypos_vcs.blocks[x,0,z];
					}
					else temp_neighbours.ypos = blocks[x,y+1,z];
					
					if(y == 0) 
					{
						temp_neighbours.yneg = neighbours.yneg_vcs.blocks[x,YSize-1,z];
					}
					else temp_neighbours.yneg = blocks[x,y-1,z];
					
					if(z+1 == ZSize)
					{
						temp_neighbours.zpos = neighbours.zpos_vcs.blocks[x,y,0];
					}
					else temp_neighbours.zpos = blocks[x,y,z+1];
					
					if(z == 0) 
					{
						temp_neighbours.zneg = neighbours.zneg_vcs.blocks[x,y,ZSize-1];
					}
					else temp_neighbours.zneg = blocks[x,y,z-1];
					
					blocks[x,y,z].neighbours = temp_neighbours;					
					
					blocks[x,y,z].locked = false;
				}				
			}
		}
	}
}
