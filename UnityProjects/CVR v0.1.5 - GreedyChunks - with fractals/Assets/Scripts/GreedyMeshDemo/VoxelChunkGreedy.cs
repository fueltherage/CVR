using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelChunkGreedy : VoxelChunk {
	//Notes
	/* 
	 * 
	*/	
	// Use this for initializatio

	protected List<Quad> Quads;



	void Start () {
		Init();
	}
	public override void Init (){   
		thisChunk = gameObject.GetComponent<VoxelChunkGreedy>();

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
        InitShells();

        //Initialize Dictionaries
        InitDics();  
 
        //Give the voxelShells neighbour refrences
        GenerateNeighbours();

		Triangles = new List<Triangle>();
		Quads = new List<Quad>();
		TrIndex = new List<int>(); //index for submeshing
		UVs = new List<Vector2> ();
		Verts = new List<Vector3> ();
		vmesh = new Mesh ();

		//Center Voxel filled for testing
		//VoxelFactory.GenerateVoxel(1,ref blocks[XSize/2,YSize/2,ZSize/2],offset,VoxelSpacing);
		//VoxelFactory.GenerateVoxel(10,ref blocks[1,1,1],offset,VoxelSpacing);
		//VoxelFactory.GenerateVoxel(10,ref blocks[1,2,1],offset,VoxelSpacing);
		UpdateMesh ();

        Initialized = true;
} 



    //float w = 2;
    //float elapsedTime=0.0f;
	// Update is called once per frame
	void Update () {
        //for (int x = 1; x < XSize-1; x++) {
        //    for (int y = 1; y < YSize-1; y++) {
        //        for (int z = 1; z < ZSize-1; z++) {
        //            if( Mathf.Sin(w*(x + Time.time)) + Mathf.Cos(w*(y+ Time.time)) + Mathf.Sin(w*(z+ Time.time)) > 0)					
        //                VoxelFactory.GenerateVoxel(1,ref blocks[x,y,z],offset,VoxelSpacing);
        //            else VoxelFactory.GenerateVoxel(0,ref blocks[x,y,z],offset,VoxelSpacing);
        //        }
        //    }
        //}
        //elapsedTime += Time.deltaTime;
        //if(elapsedTime>=0.05f)
        //{
        //    elapsedTime = 0;
        //    UpdateMesh();
        //}
        

	}
	protected override Mesh GenerateMesh ()
	{
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
	

		for (int x = 1; x < XSize-1; x++)		
			for (int y = 1; y < YSize-1; y++)			
				for (int z = 1; z < ZSize-1; z++)				
					blocks[x,y,z].neighbours.ResetFlags();
	

		//xnegLoop
		{
			for (int x = 1; x < XSize-1; ++x) {
				for (int y = 1; y < YSize-1; ++y) {
					for (int z = 1; z < XSize-1; ++z) {
						if(blocks[x,y,z].filled && !blocks[x,y,z].locked && !blocks[x-1, y, z].filled)
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
										//Check the adjacent voxel
										
										if(blocks[xtemp, ytemp, ztemp+1].filled && !blocks[xtemp-1, ytemp, ztemp+1].filled)
										{

											if(blocks[xtemp, ytemp, ztemp+1].voxel.VoxelType == type && !blocks[xtemp, ytemp, ztemp].neighbours.xnegQuad)
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
											if(!blocks[xtemp, ytemp, zc].filled || blocks[xtemp, ytemp, zc].voxel.VoxelType != type || blocks[xtemp - 1, ytemp, zc].filled)
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
								z = bookmark.z-1;
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
		List<Material> mat = new List<Material>();
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
	//FixBookmarks for each axis
	protected void BookMarkXAxis( ref VoxelPos _bookmark, int _x, int _y, int _z)
	{

		if(_z >= ZSize-1){
			_bookmark.z = 1;
			_bookmark.y = _y + 1;
		}
		else
		{
			_bookmark.z = _z;
			_bookmark.y = _y;
		}
		_bookmark.x = _x;
	}
	protected void BookMarkYAxis( ref VoxelPos _bookmark, int _x, int _y, int _z)
	{
		
		if(_z >= ZSize-1){
			_bookmark.z = 1;
			_bookmark.x = _x + 1;
		}
		else
		{
			_bookmark.z = _z;
			_bookmark.x = _x;
		}
		_bookmark.y = _y;
	}
	protected void BookMarkZAxis( ref VoxelPos _bookmark, int _x, int _y, int _z)
	{
		
		if(_y >= YSize-1){
			_bookmark.y = 1;
			_bookmark.x = _x + 1;
		}
		else
		{
			_bookmark.y = _y;
			_bookmark.x = _x;
		}
		_bookmark.z = _z;
	}

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

	protected int SubmeshIndexChecker(int _voxelType)
	{
		if(SubmeshIndex.ContainsKey(_voxelType))
		{
			return SubmeshIndex[_voxelType];
		}else{
			SubmeshIndex.Add(_voxelType, SubmeshCount);
			MaterialIndex.Add(SubmeshCount, _voxelType);
			SubmeshCount++;
			return SubmeshIndex[_voxelType];
		}
	}

	protected int [] GetSubMeshTriangles(int _submesh, ref List<Triangle> _triangles, ref List<int> _trIndex)
	{
		List<int>rawTri = new List<int>();
		for (int i = 0; i < _triangles.Count; i++)
		{
			if(_submesh == _trIndex[i])
			{
				rawTri.Add (_triangles[i].verts[0]);
				rawTri.Add (_triangles[i].verts[1]);
				rawTri.Add (_triangles[i].verts[2]);
			}
		}
		return rawTri.ToArray();
	}

    //Got TangerSolver from internet, still dont know what it does.
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
