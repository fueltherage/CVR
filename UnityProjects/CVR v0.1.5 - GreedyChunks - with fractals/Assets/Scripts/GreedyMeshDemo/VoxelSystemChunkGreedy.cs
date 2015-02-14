using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelSystemChunkGreedy : VoxelChunkGreedy {

	public ChunkNeighbours neighbours;
	public VoxelPos voxPos = new VoxelPos(0,0,0);
	List<Material> mat;
	void Start()
	{
	}
	void Update()
	{
		if(needsUpdating) UpdateMesh();
		needsUpdating = false;
	}
	public override void Init()
	{
		thisChunk = gameObject.GetComponent<VoxelSystemChunk>();
		offset.x = - XSize*VoxelSpacing/2.0f;//Starting position of vertices
		offset.y = - YSize*VoxelSpacing/2.0f;
		offset.z = - ZSize*VoxelSpacing/2.0f;

        InitShells();
        InitDics();
		Triangles = new List<Triangle>();
		Quads = new List<Quad>();
		TrIndex = new List<int>(); //index for submeshing
		UVs = new List<Vector2> ();
		Verts = new List<Vector3> ();
		vmesh = new Mesh ();

		Initialized = true;
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
	protected override Mesh GenerateMesh ()
	{
		//Clear all this shit :(
		vmesh.Clear();
		Triangles.Clear();
		Quads.Clear();
		TrIndex.Clear(); //index for submeshing
		UVs.Clear();
		Verts.Clear();
		int faceCount = 0;
		
		//temp vars
		int xtemp, ytemp, ztemp, type, width, height;
		bool QuadDone = false;
		VoxelPos bookmark = new VoxelPos();

		
		for (int x = 0; x < XSize; x++)		
			for (int y = 0; y < YSize; y++)			
				for (int z = 0; z < ZSize; z++)				
					blocks[x,y,z].neighbours.ResetFlags();
		
		
		//xnegLoop
		{
			for (int x = 0; x < XSize; ++x) {
				for (int y = 0; y < YSize; ++y) {
					for (int z = 0; z < XSize; ++z) {
						if(blocks[x,y,z].filled && !blocks[x,y,z].locked && !blocks[x, y, z].neighbours.xneg.filled)
						{
							if(!blocks[x,y,z].neighbours.xnegQuad)
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
								
										if(ztemp < ZSize-1)
										{//Check the adjacent voxel only if were not at the end		
											if(blocks[xtemp, ytemp, ztemp+1].filled && !blocks[xtemp, ytemp, ztemp].neighbours.xneg.neighbours.zpos.filled)
											{
												
												if(blocks[xtemp, ytemp, ztemp+1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp].neighbours.xnegQuad)
												{
													width++;
													ztemp++;
													if(ztemp == ZSize)//Bookmark it when we get to the end 
													{
														//Bookmark this location to return to, when the current quad is complete.
														//If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
														if(ztemp == ZSize)
														{											
															bookmark.z = 0;
															bookmark.y = ytemp + 1;	
														}
														else
														{
															bookmark.z = ztemp;
															bookmark.y = ytemp;;
														}
														bookmark.x = xtemp;			        
														break;
													}
												}else
												{
													//Bookmark it when the type isnt the same
													ztemp++;
													if(ztemp == ZSize-1)
													{											
														bookmark.z = 0;
														bookmark.y = ytemp + 1;
													}
													else
													{
														bookmark.z = ztemp;
														bookmark.y = ytemp;;
													}
													bookmark.x = xtemp;		
													break;
												}
											}
										}else
										{
											//bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
											if(ztemp == ZSize-1)
											{											
												bookmark.z = 0;
												bookmark.y = ytemp + 1;
											}
											else
											{
												bookmark.z = ztemp;
												bookmark.y = ytemp;;
											}
											bookmark.x = xtemp;		
											break;
										}
									}while(true);
									
									ztemp = z;//reset ztemp position 
									
									//Check if rows above are the same type/filled
									//Check the voxel directly above, maybe we can go up
									if(ytemp < YSize -1)
									if(blocks[xtemp, ytemp+1, ztemp].filled || blocks[xtemp, ytemp+1, ztemp].voxel.VoxelType != type)
									{
										//Move the cursor up a row 
										ytemp++;
									}else 
									{
										//forget about going up, gtfo and moveon									
										QuadDone = true;
										continue;
									}
									
									//Iterate through voxels along the z axis until you find a voxel that is empty or different type							
									do
									{
										for(int zc = ztemp; zc < ztemp + width; zc++)
										{
											//if there is a fault, move on and forget about that row
											if(!blocks[xtemp, ytemp, zc].filled || blocks[xtemp, ytemp, zc].voxel.VoxelType != type || blocks[xtemp, ytemp, zc].neighbours.xneg.filled)
											{
												QuadDone = true;
												break;	
											}
										}
										if(!QuadDone)
										{
											height++;
											ytemp++;
											
										}else break;
									}while(true);
								}while(!QuadDone);
								
								for(int w = z; w < z+width; w++){
									for (int h = y; h < y+height; h++) {
										blocks[x, h, w].neighbours.xnegQuad = true;
									}	
								}
								
								Quads.Add(new Quad(SubmeshIndexChecker(type), 
								                   width, 
								                   height, 
								                   new Vector3(offset.x + x * VoxelSpacing,
								            offset.y + y * VoxelSpacing,
								            offset.z + z * VoxelSpacing ),
								                   VoxelSpacing,
								                   new VoxelPos(-1,0,0),
								                   ref faceCount));
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
								y = bookmark.y;
								z = bookmark.z-1;//the -1 is to compensate for the loop adding 1 on the next iteration
								//infinite loop failsafe for testing only
								//loopCount++;
								//if(loopCount>= loopCutter) return mesh;
							}
						}
					}
				}
			}
		}
		//xposLoop
		{
			for (int x = 1; x < XSize-1; ++x) {
				for (int y = 1; y < YSize-1; ++y) {
					for (int z = 1; z < XSize-1; ++z) {
						if(blocks[x,y,z].filled && !blocks[x,y,z].locked && !blocks[x+1, y, z].filled)
						{
							if(!blocks[x,y,z].neighbours.xposQuad)
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
										
										if(blocks[xtemp, ytemp, ztemp+1].filled && !blocks[xtemp+1, ytemp, ztemp+1].filled)
										{
											
											if(blocks[xtemp, ytemp, ztemp+1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp+1].neighbours.xposQuad)
											{
												width++;
												ztemp++;
												if(ztemp == ZSize-1)//Bookmark it when we get to the end 
												{
													//Bookmark this location to return to, when the current quad is complete.
													//If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
													BookMarkXAxis(ref bookmark, xtemp, ytemp, ztemp);												        
													break;
												}
											}else
											{
												//Bookmark it when the type isnt the same
												ztemp++;
												BookMarkXAxis(ref bookmark, xtemp, ytemp, ztemp);	
												break;
											}
										}else
										{
											//bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
											BookMarkXAxis(ref bookmark, xtemp, ytemp, ztemp);	
											break;
										}
									}while(true);
									
									ztemp = z;//reset ztemp position 
									
									//Check if rows above are the same type/filled
									//Check the voxel directly above, maybe we can go up
									if(ytemp < YSize -1)
										if(blocks[xtemp, ytemp+1, ztemp].filled || blocks[xtemp, ytemp+1, ztemp].voxel.VoxelType != type)
									{
										//Move the cursor up a row 
										ytemp++;
									}else 
									{
										//forget about going up, gtfo and moveon									
										QuadDone = true;
										continue;
									}
									
									//Iterate through voxels along the z axis until you find a voxel that is empty or different type							
									do
									{
										for(int zc = ztemp; zc < ztemp + width; zc++)
										{
											//if there is a fault, move on and forget about that row
											if(!blocks[xtemp, ytemp, zc].filled || blocks[xtemp, ytemp, zc].voxel.VoxelType != type || blocks[xtemp + 1, ytemp, zc].filled)
											{
												QuadDone = true;
												break;	
											}
										}
										if(!QuadDone)
										{
											//If you made it thought the last for-loop unscathed then you just leveled up a row :)
											height++;
											ytemp++;
											
										}else break;
									}while(true);
								}while(!QuadDone);
								
								for(int w = z; w < z+width; w++){
									for (int h = y; h < y+height; h++) {
										blocks[x, h, w].neighbours.xposQuad = true;
									}	
								}
								
								Quads.Add(new Quad(SubmeshIndexChecker(type), 
								                   width, 
								                   height, 
								                   new Vector3(offset.x + x + 1 * VoxelSpacing,
								            offset.y + y * VoxelSpacing,
								            offset.z + z * VoxelSpacing ),
								                   VoxelSpacing,
								                   new VoxelPos(1,0,0),
								                   ref faceCount));
								
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
								y = bookmark.y;
								z = bookmark.z-1;
								//infinite loop failsafe for testing only
								//loopCount++;
								//if(loopCount>= loopCutter){ return mesh; Debug.Log("EndlessLoop");}
							}
						}
					}
				}
			}
		}
		//yposLoop
		{
			for (int y = 1; y < YSize-1; ++y) {
				for (int x = 1; x < XSize-1; ++x) {
					for (int z = 1; z < XSize-1; ++z) {
						if(blocks[x,y,z].filled && !blocks[x,y,z].locked && !blocks[x, y+1, z].filled)
						{
							if(!blocks[x,y,z].neighbours.yposQuad)
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
										
										if(blocks[xtemp, ytemp, ztemp+1].filled && !blocks[xtemp, ytemp+1, ztemp+1].filled)
										{
											
											if(blocks[xtemp, ytemp, ztemp+1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp+1].neighbours.yposQuad)
											{
												width++;
												ztemp++;
												if(ztemp == ZSize-1)//Bookmark it when we get to the end 
												{
													//Bookmark this location to return to, when the current quad is complete.
													//If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
													BookMarkYAxis(ref bookmark, xtemp, ytemp, ztemp);												        
													break;
												}
											}else
											{
												//Bookmark it when the type isnt the same
												ztemp++;
												BookMarkYAxis(ref bookmark, xtemp, ytemp, ztemp);	
												break;
											}
										}else
										{
											//bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
											BookMarkYAxis(ref bookmark, xtemp, ytemp, ztemp);	
											break;
										}
									}while(true);
									
									ztemp = z;//reset ztemp position 
									
									//Check if rows above are the same type/filled
									//Check the voxel directly above, maybe we can go up
									if(xtemp < XSize -1)
										if(blocks[xtemp+1, ytemp, ztemp].filled || blocks[xtemp+1, ytemp, ztemp].voxel.VoxelType != type)
									{
										//Move the cursor up a row 
										xtemp++;
									}else 
									{
										//forget about going up, gtfo and moveon									
										QuadDone = true;
										continue;
									}
									
									//Iterate through voxels along the z axis until you find a voxel that is empty or different type							
									do
									{
										for(int zc = ztemp; zc < ztemp + width; zc++)
										{
											//if there is a fault, move on and forget about that row
											if(!blocks[xtemp, ytemp, zc].filled || blocks[xtemp, ytemp, zc].voxel.VoxelType != type || blocks[xtemp, ytemp + 1, zc].filled)
											{
												QuadDone = true;
												break;	
											}
										}
										if(!QuadDone)
										{
											//If you made it thought the last for-loop unscathed then you just leveled up a row :)
											height++;
											xtemp++;
											
										}else break;
									}while(true);
								}while(!QuadDone);
								
								for(int w = z; w < z+width; w++){
									for (int h = x; h < x+height; h++) {
										blocks[h, y, w].neighbours.yposQuad = true;
									}	
								}
								
								Quads.Add(new Quad(SubmeshIndexChecker(type), 
								                   width, 
								                   height, 
								                   new Vector3(offset.x + x * VoxelSpacing,
								            offset.y + y + 1 * VoxelSpacing,
								            offset.z + z * VoxelSpacing ),
								                   VoxelSpacing,
								                   new VoxelPos(0,1,0),
								                   ref faceCount));
								
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
								y = bookmark.y;
								z = bookmark.z-1;
								//infinite loop failsafe for testing only
								//loopCount++;
								//if(loopCount>= loopCutter){ return mesh; Debug.Log("EndlessLoop");}
							}
						}
					}
				}
			}
		}
		//ynegLoop
		{
			for (int y = 1; y < YSize-1; ++y) {
				for (int x = 1; x < XSize-1; ++x) {
					for (int z = 1; z < XSize-1; ++z) {
						if(blocks[x,y,z].filled && !blocks[x,y,z].locked && !blocks[x, y-1, z].filled)
						{
							if(!blocks[x,y,z].neighbours.ynegQuad)
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
										
										if(blocks[xtemp, ytemp, ztemp+1].filled && !blocks[xtemp, ytemp-1, ztemp+1].filled)
										{
											
											if(blocks[xtemp, ytemp, ztemp+1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp].neighbours.ynegQuad)
											{
												width++;
												ztemp++;
												if(ztemp == ZSize-1)//Bookmark it when we get to the end 
												{
													//Bookmark this location to return to, when the current quad is complete.
													//If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
													BookMarkYAxis(ref bookmark, xtemp, ytemp, ztemp);												        
													break;
												}
											}else
											{
												//Bookmark it when the type isnt the same
												ztemp++;
												BookMarkYAxis(ref bookmark, xtemp, ytemp, ztemp);	
												break;
											}
										}else
										{
											//bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
											BookMarkYAxis(ref bookmark, xtemp, ytemp, ztemp);	
											break;
										}
									}while(true);
									
									ztemp = z;//reset ztemp position 
									
									//Check if rows above are the same type/filled
									//Check the voxel directly above, maybe we can go up
									if(xtemp < XSize -1)
										if(blocks[xtemp+1, ytemp, ztemp].filled || blocks[xtemp+1, ytemp, ztemp].voxel.VoxelType != type)
									{
										//Move the cursor up a row 
										xtemp++;
									}else 
									{
										//forget about going up, gtfo and moveon									
										QuadDone = true;
										continue;
									}
									
									//Iterate through voxels along the z axis until you find a voxel that is empty or different type							
									do
									{
										for(int zc = ztemp; zc < ztemp + width; zc++)
										{
											//if there is a fault, move on and forget about that row
											if(!blocks[xtemp, ytemp, zc].filled || blocks[xtemp, ytemp, zc].voxel.VoxelType != type || blocks[xtemp, ytemp - 1, zc].filled)
											{
												QuadDone = true;
												break;	
											}
										}
										if(!QuadDone)
										{
											//If you made it thought the last for-loop unscathed then you just leveled up a row :)
											height++;
											xtemp++;
											
										}else break;
									}while(true);
								}while(!QuadDone);
								
								for(int w = z; w < z+width; w++){
									for (int h = x; h < x+height; h++) {
										blocks[h, y, w].neighbours.ynegQuad = true;
									}	
								}
								
								Quads.Add(new Quad(SubmeshIndexChecker(type), 
								                   width, 
								                   height, 
								                   new Vector3(offset.x + x * VoxelSpacing,
								            offset.y + y * VoxelSpacing,
								            offset.z + z * VoxelSpacing ),
								                   VoxelSpacing,
								                   new VoxelPos(0,-1,0),
								                   ref faceCount));
								
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
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
		//znegLoop
		{
			for (int z = 1; z < ZSize-1; ++z) {
				for (int x = 1; x < XSize-1; ++x) {
					for (int y = 1; y < YSize-1; ++y) {
						if(blocks[x,y,z].filled && !blocks[x,y,z].locked && !blocks[x, y, z-1].filled)
						{
							if(!blocks[x,y,z].neighbours.znegQuad)
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
										
										if(blocks[xtemp, ytemp+1, ztemp].filled && !blocks[xtemp, ytemp+1, ztemp-1].filled)
										{
											
											if(blocks[xtemp, ytemp+1, ztemp].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp].neighbours.znegQuad)
											{
												width++;
												ytemp++;
												if(ytemp == YSize-1)//Bookmark it when we get to the end 
												{
													//Bookmark this location to return to, when the current quad is complete.
													//If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
													BookMarkZAxis(ref bookmark, xtemp, ytemp, ztemp);												        
													break;
												}
											}else
											{
												//Bookmark it when the type isnt the same
												ytemp++;
												BookMarkZAxis(ref bookmark, xtemp, ytemp, ztemp);	
												break;
											}
										}else
										{
											//bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
											BookMarkZAxis(ref bookmark, xtemp, ytemp, ztemp);	
											break;
										}
									}while(true);
									
									ytemp = y;//reset ztemp position 
									
									//Check if rows above are the same type/filled
									//Check the voxel directly above, maybe we can go up
									if(xtemp < XSize -1)
										if(blocks[xtemp+1, ytemp, ztemp].filled || blocks[xtemp+1, ytemp, ztemp].voxel.VoxelType != type)
									{
										//Move the cursor up a row 
										xtemp++;
									}else 
									{
										//forget about going up, gtfo and moveon									
										QuadDone = true;
										continue;
									}
									
									//Iterate through voxels along the z axis until you find a voxel that is empty or different type							
									do
									{
										for(int yc = ytemp; yc < ytemp + width; yc++)
										{
											//if there is a fault, move on and forget about that row
											if(!blocks[xtemp, yc, ztemp].filled || blocks[xtemp, yc, ztemp].voxel.VoxelType != type || blocks[xtemp, yc, ztemp - 1].filled)
											{
												QuadDone = true;
												break;	
											}
										}
										if(!QuadDone)
										{
											//If you made it thought the last for-loop unscathed then you just leveled up a row :)
											height++;
											xtemp++;
											
										}else break;
									}while(true);
								}while(!QuadDone);
								
								for(int w = y; w < y+width; w++){
									for (int h = x; h < x+height; h++) {
										blocks[h, w, z].neighbours.znegQuad = true;
									}	
								}
								
								Quads.Add(new Quad(SubmeshIndexChecker(type), 
								                   width, 
								                   height, 
								                   new Vector3(offset.x + x * VoxelSpacing,
								            offset.y + y * VoxelSpacing,
								            offset.z + z * VoxelSpacing ),
								                   VoxelSpacing,
								                   new VoxelPos(0,0,-1),
								                   ref faceCount));
								
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
								y = bookmark.y - 1;
								z = bookmark.z;
								//infinite loop failsafe for testing only
								//loopCount++;
								//if(loopCount>= loopCutter){ return mesh; Debug.Log("EndlessLoop");}
							}
						}
					}
				}
			}
		}
		//zposLoop
		{
			for (int z = 1; z < ZSize-1; ++z) {
				for (int x = 1; x < XSize-1; ++x) {
					for (int y = 1; y < YSize-1; ++y) {
						if(blocks[x,y,z].filled && !blocks[x,y,z].locked && !blocks[x, y, z+1].filled)
						{
							if(!blocks[x,y,z].neighbours.zposQuad)
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
										
										if(blocks[xtemp, ytemp+1, ztemp].filled && !blocks[xtemp, ytemp+1, ztemp+1].filled)
										{
											
											if(blocks[xtemp, ytemp+1, ztemp].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp].neighbours.zposQuad)
											{
												width++;
												ytemp++;
												if(ytemp == YSize-1)//Bookmark it when we get to the end 
												{
													//Bookmark this location to return to, when the current quad is complete.
													//If the bookmark goes out of bounds then return it to the beginning of the row above. <<not perfect wip
													BookMarkZAxis(ref bookmark, xtemp, ytemp, ztemp);												        
													break;
												}
											}else
											{
												//Bookmark it when the type isnt the same
												ytemp++;
												BookMarkZAxis(ref bookmark, xtemp, ytemp, ztemp);	
												break;
											}
										}else
										{
											//bookmark it even if its empty or filled, we need somewhere to go you know when the quad is finished!
											BookMarkZAxis(ref bookmark, xtemp, ytemp, ztemp);	
											break;
										}
									}while(true);
									
									ytemp = y;//reset ztemp position 
									
									//Check if rows above are the same type/filled
									//Check the voxel directly above, maybe we can go up
									if(xtemp < XSize -1)
										if(blocks[xtemp+1, ytemp, ztemp].filled || blocks[xtemp+1, ytemp, ztemp].voxel.VoxelType != type)
									{
										//Move the cursor up a row 
										xtemp++;
									}else 
									{
										//forget about going up, gtfo and moveon									
										QuadDone = true;
										continue;
									}
									
									//Iterate through voxels along the z axis until you find a voxel that is empty or different type							
									do
									{
										for(int yc = ytemp; yc < ytemp + width; yc++)
										{
											//if there is a fault, move on and forget about that row
											if(!blocks[xtemp, yc, ztemp].filled || blocks[xtemp, yc, ztemp].voxel.VoxelType != type  || blocks[xtemp, yc, ztemp + 1].filled)
											{
												QuadDone = true;
												break;	
											}
										}
										if(!QuadDone)
										{
											//If you made it thought the last for-loop unscathed then you just leveled up a row :)
											height++;
											xtemp++;
											
										}else break;
									}while(true);
								}while(!QuadDone);
								
								for(int w = y; w < y+width; w++){
									for (int h = x; h < x+height; h++) {
										blocks[h, w, z].neighbours.zposQuad = true;
									}	
								}
								
								Quads.Add(new Quad(SubmeshIndexChecker(type), 
								                   width, 
								                   height, 
								                   new Vector3(offset.x + x * VoxelSpacing,
								            offset.y + y * VoxelSpacing,
								            offset.z + z + 1 * VoxelSpacing ),
								                   VoxelSpacing,
								                   new VoxelPos(0,0,1),
								                   ref faceCount));
								
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								TrIndex.Add(Quads[Quads.Count-1].submeshIndex);
								//Debug.Log(bookmark.ToString());
								x = bookmark.x;
								y = bookmark.y - 1;
								z = bookmark.z;
								//infinite loop failsafe for testing only
								//loopCount++;
								//if(loopCount>= loopCutter){ return mesh; Debug.Log("EndlessLoop");}
							}
						}
					}
				}
			}
		}
		
		//Gather all the mesh data
		foreach (Quad q in Quads)
		{
			for(int i =0; i<4;i++)
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
			vmesh.SetTriangles(GetSubMeshTriangles(i,ref Triangles,ref TrIndex), i);
		}
		
		//Build the chunks material array
		mat = new List<Material>();
		for(int i = 0; i < SubmeshCount; i++)
		{
			mat.Add (factory.VoxelMats[MaterialIndex[i]]);
		}
		renderer.materials = mat.ToArray();
		
		vmesh.uv = UVs.ToArray ();
		vmesh.RecalculateNormals ();
		
		//Use TangentSolver if shader requires it
		//TangentSolver(mesh);
		
		vmesh.Optimize ();
		return vmesh;
	}	
}
