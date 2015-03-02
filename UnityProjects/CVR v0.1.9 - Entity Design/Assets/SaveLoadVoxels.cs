using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SaveLoadVoxels : MonoBehaviour {
	public string Name = "";

	VoxelSystemGreedy vs; 
	bool loading = false;
	bool saving = false;
	int VoxNum =1;
	int count;

	// Use this for initialization
	void Start () {
		vs = GetComponent<VoxelSystemGreedy>();
	
	}
	void OnGUI()
	{
		Color col = new Color();
		col = (saving) ? col = Color.red : col = Color.blue;
		GUI.color = col;
		GUI.TextField( new Rect(10, 40, 100, 25), (Mathf.RoundToInt((float)(count / VoxNum)*100)).ToString());
	}

	VoxelPos VoxelNum(VoxelPos c, VoxelPos s)
	{
		return new VoxelPos(s.x * vs.ChunkSizeX + c.x,
		                    s.y * vs.ChunkSizeX + c.y,
		                    s.z * vs.ChunkSizeX + c.z);

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Y))
		{
			if(saving)
			{
				Debug.Log ("Saving Already");
			}
			else{
				//VoxelThreads.RunAsync(SaveSystem);
				SaveSystem();
			}
		}
		if(Input.GetKeyDown(KeyCode.T))
		{
			LoadSystem();
		}
	
	}
	public void LoadSystem()
	{
		loading = true;
		if(Name == "")
		{
			Debug.LogError("<color=red>Error:</color> Missing file name, please provide name in script inspector.");
			return;
		}
		string s = "";
		try
		{
			s = System.IO.File.ReadAllText("./Assets/SavedVoxels/"+Name+".txt");
		}catch(UnityException e)
		{
			Debug.LogError("<color=red>Error:</color> File not found at location: /Assets/SavedVoxels/"+Name+".txt");
			return;
		}

		JSONNode node = JSONNode.Parse(s);

		
		vs.XSize = node[Name]["i"]["x"].AsInt;
		vs.YSize = node[Name]["i"]["y"].AsInt;
		vs.ZSize = node[Name]["i"]["z"].AsInt;
		vs.ChunkSizeX = node[Name]["i"]["xc"].AsInt;
		vs.ChunkSizeY = node[Name]["i"]["yc"].AsInt;
		vs.ChunkSizeZ = node[Name]["i"]["zc"].AsInt;
		vs.VoxelSpacing = node[Name]["i"]["vs"].AsFloat;

		//vs.Init();

		for (int x = 0; x < vs.XSize; x++)
			for (int y = 0; y < vs.YSize; y++)
				for (int z = 0; z < vs.ZSize; z++)
					for (int xc = 0; xc < vs.ChunkSizeX; xc++)
						for (int yc = 0; yc < vs.ChunkSizeY; yc++)
							for (int zc = 0; zc < vs.ChunkSizeZ; zc++){	
							vs.RemoveVoxel(new VoxelPos(x*vs.ChunkSizeX + xc, 
							                            y*vs.ChunkSizeX + yc, 
							                            z*vs.ChunkSizeX + zc),
							               				false);
							}

		int max = node[Name]["c"].AsArray.Count;
		VoxNum = max;
		if(VoxNum == 0)
		{
			Debug.Log("Saving Empty System, Plz fix");VoxNum = 1;
		}
		count = 0;
		for (int i = 0; i < max; i++) {
			vs.AddVoxel(new VoxelPos(node[Name]["c"][i]["p"][0].AsInt,
			                         node[Name]["c"][i]["p"][1].AsInt,
			                         node[Name]["c"][i]["p"][2].AsInt),
			            			 false,
			            			 node[Name]["c"][i]["t"].AsInt);
			++count;

		}

		vs.UpdateMeshes();
		Debug.Log ("<color=green>VoxSaveFile "+Name+" loaded successfully. "+count+" voxels loaded.</color>");
		loading = false;
	}
	struct SaveGarbage
	{
		public int type;
		public bool saved;
	}
	interface SavedInfo
	{

	}
	struct SaveArea : SavedInfo
	{
		public SaveArea(int _type, VoxelPos _p1, VoxelPos _p2){type = _type;pos1 = _p1; pos2 = _p2;}
		public int type;
		public VoxelPos pos1;
		public VoxelPos pos2;
	}
	struct SaveSingle : SavedInfo
	{
		public SaveSingle(int _type, VoxelPos _p1){type = _type;pos1 = _p1;}
		public int type;
		public VoxelPos pos1;
	}

	public void SaveSystem()//(bool SaveWorldSpace)
	{	
		saving = true;
		JSONClass node = new JSONClass();


		//if(Name == "") Name = vs.name;


		node[Name]["i"][0].AsInt = vs.XSize;
		node[Name]["i"][1].AsInt = vs.YSize;
		node[Name]["i"][2].AsInt = vs.ZSize;
		node[Name]["i"][3].AsInt = vs.ChunkSizeX;
		node[Name]["i"][4].AsInt = vs.ChunkSizeY;
		node[Name]["i"][5].AsInt = vs.ChunkSizeZ;
		node[Name]["i"][6].AsFloat = vs.VoxelSpacing;

		/* Might not need to save this
		if(SaveWorldSpace)
		{
			node["info"]["wp"][0].AsFloat = vs.transform.position.x;
			node["info"]["wp"][1].AsFloat = vs.transform.position.y;
			node["info"]["wp"][2].AsFloat = vs.transform.position.z;
			node["info"]["wr"][0].AsFloat = vs.transform.rotation.x;
			node["info"]["wr"][1].AsFloat = vs.transform.rotation.y;
			node["info"]["wr"][2].AsFloat = vs.transform.rotation.z;
			node["info"]["wr"][3].AsFloat = vs.transform.rotation.w;
		}*/

		//Loop through all the voxels in the system, Save them if they're not 0
		count = 0;
		VoxNum = vs.ChunkSizeX*vs.ChunkSizeY*vs.ChunkSizeZ*vs.XSize*vs.YSize*vs.ZSize;
		int voxX = vs.XSize*vs.ChunkSizeX;
		int voxY = vs.YSize*vs.ChunkSizeY;
		int voxZ = vs.ZSize*vs.ChunkSizeZ;


		//Convert a 3x3x3x3x3x3 to 3x3x3 for easibility 
		SaveGarbage[,,] g = new SaveGarbage[voxX,voxY,voxZ];
		for (int x = 0; x < vs.XSize; x++)
			for (int y = 0; y < vs.YSize; y++)
				for (int z = 0; z < vs.ZSize; z++)
					for (int xc = 0; xc < vs.ChunkSizeX; xc++)
						for (int yc = 0; yc < vs.ChunkSizeY; yc++)
							for (int zc = 0; zc < vs.ChunkSizeZ; zc++){															
								g[ x * vs.ChunkSizeX + xc, y * vs.ChunkSizeY + yc, z * vs.ChunkSizeZ + zc ].type 
								= vs.chunks_vcs[x,y,z].blocks[xc,yc,zc].voxel.VoxelType;																
							}


		int xtemp, ytemp, ztemp, type, x_length, y_length, z_length; 
		VoxelPos Bookmark = new VoxelPos();
		List<SavedInfo> saveStream = new List<SavedInfo>();
		for (int x = 0; x < voxX; x++) {
			for (int y = 0; y < voxY; y++) {
				for (int z = 0; z < voxZ; z++) {

					if(g[x,y,z].type > 0 && !g[x,y,z].saved)
					{
						x_length = 1;
						y_length = 1;
						z_length = 1;
						xtemp = x;
						ytemp = y;
						ztemp = z;
						bool Done = false;

						do
						{
							if(ztemp + 1 == voxX)break;
							if((g[xtemp,ytemp,ztemp+1].type == g[xtemp,ytemp,ztemp].type) && !g[xtemp,ytemp,ztemp+1].saved)
							{
								++z_length;
								++ztemp;
								if(ztemp == voxZ)
								{
									Bookmark.x = xtemp;
									Bookmark.y = ytemp + 1;
									Bookmark.z = 0;
									break;
								}
							}else{
								++ztemp;
								if (ztemp >= voxZ){
									Bookmark.z = 0;
									Bookmark.y = ytemp + 1;
								}else{
									Bookmark.z = ztemp;
									Bookmark.y = ytemp;
								}
								Bookmark.x = xtemp;
								break;
							}
						}while(true);

						ztemp = z;

						if(ytemp < voxY -1)
						{
							ytemp++;
							do{
								for (int zc = ztemp; zc < ztemp + z_length; zc++){
									if (g[xtemp, ytemp, zc].type != g[x,y,z].type || g[xtemp, ytemp, zc].saved){
										Done = true;
										break;
									}
								}
								if(!Done)
								{
									ytemp++;
									if(ytemp == voxY){ 												
										if(y < voxY-1)y_length++;
										Done = true;
										break;
									}else y_length++;
								}else break;																
							} while (true);
						}
						Done = false;
						xtemp = x;
						ytemp = y;
						ztemp = z;

						if(xtemp < voxX -1)
						{
							xtemp++;
							do{
								for (int zc = ztemp; zc < ztemp + z_length; zc++){
									for (int yc = ytemp; yc < ytemp + y_length; yc++){
										if (g[xtemp, yc, zc].type != g[x,y,z].type|| g[xtemp, yc, zc].saved){
											Done = true;
											break;
										}
									}
								}
								if(!Done)
								{
									xtemp++;
									if(xtemp == voxX){ 												
										if(x < voxX-1)x_length++;
										break;
									}else x_length++;
								}else break;																
							} while (true);
						}
						//Mark the area as saved
						for (int w = x; w < x + x_length; w++){
							for (int h = y; h < y + y_length; h++){
								for (int l = z; l < z + z_length; l++){									
									g[w, h, l].saved = true;
								}
							}
						}
						if( x_length ==1 && y_length == 1 && z_length == 1)
						{
							SaveSingle info = new SaveSingle(g[x,y,z].type, new VoxelPos(x,y,z));
							saveStream.Add(info);
						}else
						{
							SaveArea info = new SaveArea(g[x,y,z].type, new VoxelPos(x,y,z), new VoxelPos(x+x_length, y+y_length, z+z_length));
							saveStream.Add(info);
						}
						x = Bookmark.x;
						y = Bookmark.y;
						z = Bookmark.z;
					}
				}				
			}
		}
		count = 0;
		int count2 = 0;
		foreach(SavedInfo i in saveStream)
		{
			if(i is SaveArea)
			{
				node[name]["c"]["a"][count][0][0].AsInt = ((SaveArea)i).pos1.x;
				node[name]["c"]["a"][count][0][1].AsInt = ((SaveArea)i).pos1.y;		
				node[name]["c"]["a"][count][0][2].AsInt = ((SaveArea)i).pos1.z;
				node[name]["c"]["a"][count][1][0].AsInt = ((SaveArea)i).pos2.x;
				node[name]["c"]["a"][count][1][1].AsInt = ((SaveArea)i).pos2.y;
				node[name]["c"]["a"][count][1][2].AsInt = ((SaveArea)i).pos2.z;
				node[name]["c"]["a"][count][3].AsInt = ((SaveArea)i).type;
				count++;
			}else{
				node[name]["c"]["s"][count2][0][0].AsInt = ((SaveSingle)i).pos1.x;
				node[name]["c"]["s"][count2][0][1].AsInt = ((SaveSingle)i).pos1.y;		
				node[name]["c"]["s"][count2][0][2].AsInt = ((SaveSingle)i).pos1.z;
				node[name]["c"]["s"][count2][1].AsInt = ((SaveSingle)i).type;
				count2++;
			}
		}

		System.IO.File.WriteAllText("./Assets/SavedVoxels/"+Name+".txt",node.ToString());
		//Debug.Log("String Char Count "+node.ToString().Length+" "+node.ToString());
		saving = false;

	}



}
