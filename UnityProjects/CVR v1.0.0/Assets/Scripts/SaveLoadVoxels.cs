using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System.Globalization;


public class SaveLoadVoxels : MonoBehaviour {
	public string voxName = "";
	public bool ClearBeforeLoading = false;
	public bool LoadOnStart = false;
    public bool loaded = false;
    public bool enableInput = false;
	VoxelSystemGreedy vs; 
	bool loading = false;
	bool saving = false;
	bool initLoad = false;
	int count;

	// Use this for initialization
	void Start () {
		vs = GetComponent<VoxelSystemGreedy>();	
	}
  

	VoxelPos VoxelNum(VoxelPos c, VoxelPos s)
	{
		return new VoxelPos(s.x * vs.ChunkSizeX + c.x,
		                    s.y * vs.ChunkSizeX + c.y,
		                    s.z * vs.ChunkSizeX + c.z);

	}
	
	// Update is called once per frame
	void Update () {
        if (enableInput)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (saving)
                {
                    Debug.Log("Saving Already");
                }
                else
                {
                    //VoxelThreads.RunAsync(SaveSystem);
                    SaveSystem();
                }
            }
        }
		if(Input.GetKeyDown(KeyCode.T))
		{
			LoadSystem();
		}
		if(!initLoad)
		if(vs.Initialized && LoadOnStart)
		{
			initLoad = true;
			LoadSystem();
		}

	
	}
	public void LoadSystem()
	{
		loading = true;
		if(voxName == "")
		{
			Debug.LogError("<color=red>Error:</color> Missing voxName, please provide voxName in script inspector.");
			return;
		}
		string s = "";
		try
		{
			//s = System.IO.File.ReadAllText("./Assets/SavedVoxels/"+voxName+".txt");            
            s = System.IO.File.ReadAllText(Application.dataPath +"\\SavedVoxels\\"+ voxName + ".txt");
		}catch(UnityException e)
		{
			Debug.LogError("<color=red>Error:</color> File not found at location: "+Application.dataPath+@"\"+voxName+".txt");
			return;
		}

		//JSONNode node = JSONNode.Parse(s);

		
        //vs.XSize = node[voxName]["i"][0].AsInt;
        //vs.YSize = node[voxName]["i"][1].AsInt;
        //vs.ZSize = node[voxName]["i"][2].AsInt;
        //vs.ChunkSizeX = node[voxName]["i"][3].AsInt;
        //vs.ChunkSizeY = node[voxName]["i"][4].AsInt;
        //vs.ChunkSizeZ = node[voxName]["i"][5].AsInt;
        //vs.VoxelSpacing = node[voxName]["i"][6].AsFloat;
        List<SaveArea> areaSaves = new List<SaveArea>();
        List<SaveSingle> singleSaves = new List<SaveSingle>();
        int c = 0;
        string buf="";
        int index = 0;
        while (true)//Initialiation info loaded
        {
            if (s[c] == 'a' || s[c] == 's')
            {                
                break;
            }
            while(true)
            {
                if (s[c] != '~')
                {
                    if (s[c] == 'a' || s[c] == 's')
                    {
                        break;
                    }
                    else
                    {
                        buf += s[c];
                        c++;
                    }
                }
                else
                { //When we reach a ~ , process the buffered info then switch to the next index.
                    switch (index)
                    {
                        case 0:
                            vs.XSize = int.Parse(buf, NumberStyles.Integer);
                            break;
                        case 1:
                            vs.YSize = int.Parse(buf, NumberStyles.Integer);
                            break;
                        case 2:
                            vs.ZSize = int.Parse(buf, NumberStyles.Integer);
                            break;
                        case 3:
                            vs.ChunkSizeX = int.Parse(buf, NumberStyles.Integer);
                            break;
                        case 4:
                            vs.ChunkSizeY = int.Parse(buf, NumberStyles.Integer);
                            break;
                        case 5:
                            vs.ChunkSizeZ = int.Parse(buf, NumberStyles.Integer);
                            break;
                        case 6:                            
                            vs.VoxelSpacing = float.Parse(buf, CultureInfo.CurrentCulture);
                            break;                            
                    }
                    c++;
                    index++;
                    buf = "";
                    break;
                }
                //if (index == 7) break;
            }
        }
        int count;
        bool saveArea = false;
        buf = "";
        if (s[c] == 'a')//area blocks
        {
            c++;
            saveArea = true;            
        }
        
        while (true)
        {
            if (s[c] != '|')
            {
                buf += s[c];
                c++;
            }
            else break;
        }
        count = int.Parse(buf, NumberStyles.Integer);
        buf = "";
        index = 0;
        SaveArea a;
        if (saveArea)
        {
            
            for (int i = 0; i < count; i++)
            {
                a = new SaveArea();
                if (s[c] == '|')//Skip the delimiter and reset the index;
                {
                    c++;                                      
                }
                index = 0;  
                while (true)
                {
                    if (s[c] != '~' && s[c] != 's' && s[c] != '|')                    
                    {                        
                        buf += s[c];
                        c++;
                    }
                    else
                    { 
                        switch (index)
                        {
                            case 0:
                                a.pos1.x = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 1:
                                a.pos1.y = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 2:
                                a.pos1.z = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 3:
                                a.pos2.x = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 4:
                                a.pos2.y = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 5:
                                a.pos2.z = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 6:
                                a.type = int.Parse(buf, NumberStyles.Integer);
                                areaSaves.Add(a);
                                break;
                        }
                        c++;
                        index++;
                        buf = "";
                        if (index == 7)
                        {
                            break;
                        }
                    }                    
                }

            }
        }
        index = 0;
        count = 0;
        buf="";
        if (s[c] == 's')
        {
            c++;
            while (true)
            {
                if (s[c] != '|') buf += s[c];
                else { break; }
            }
            count = int.Parse(buf, NumberStyles.Integer);
        }
        buf = "";
        SaveSingle ss;
        if (count > 0)
        {
            
            for (int i = 0; i < count; i++)
            {
                ss = new SaveSingle();
                if (s[c] == '|')//Skip the delimiter and reset the index;
                {
                    c++;
                    index = 0;
                }
                while (true)
                {
                    if (s[c] != '~')
                    {
                        buf += s[c];
                        c++;
                    }
                    else
                    {
                        switch (index)
                        {
                            case 0:
                                ss.pos1.x = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 1:
                                ss.pos1.y = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 2:
                                ss.pos1.z = int.Parse(buf, NumberStyles.Integer);
                                break;
                            case 3:
                                ss.type = int.Parse(buf, NumberStyles.Integer);
                                singleSaves.Add(ss);
                                break;                            
                        }
                        c++;
                        index++;
                        buf = "";
                        break;
                    }
                }
            }
        }


        if (ClearBeforeLoading)//Clears the system before adding anything
            for (int x = 0; x < vs.XSize; x++)
                for (int y = 0; y < vs.YSize; y++)
                    for (int z = 0; z < vs.ZSize; z++)
                        for (int xc = 0; xc < vs.ChunkSizeX; xc++)
                            for (int yc = 0; yc < vs.ChunkSizeY; yc++)
                                for (int zc = 0; zc < vs.ChunkSizeZ; zc++)
                                {
                                    vs.RemoveVoxel(new VoxelPos(x * vs.ChunkSizeX + xc,
                                                                y * vs.ChunkSizeY + yc,
                                                                z * vs.ChunkSizeZ + zc),
                                                                false);
                                }

        foreach (SaveArea sa in areaSaves)
        {
            for (int x =  sa.pos1.x; x < sa.pos1.x + sa.pos2.x; x++) {
                for (int y = sa.pos1.y; y < sa.pos1.y + sa.pos2.y; y++)
                {
                    for (int z = sa.pos1.z; z < sa.pos1.z + sa.pos2.z; z++)
                    {
                        vs.AddVoxel(new VoxelPos(x, y, z), false, sa.type);                        
                    }
                }
            }
        }
        foreach (SaveSingle singleS in singleSaves)
        {
            for (int i = 0; i < singleSaves.Count; i++)
            {
                vs.AddVoxel(new VoxelPos(singleS.pos1.x, singleS.pos1.y, singleS.pos1.z), false, singleS.type);
            }
        }



		//Areas JSON loading
        //count = 0;
        //for (int i = 0; i < node[voxName]["c"]["a"].AsArray.Count; i++) {
        //    int xStart =    node[voxName]["c"]["a"][i][0].AsInt;
        //    int yStart =    node[voxName]["c"]["a"][i][1].AsInt;
        //    int zStart =    node[voxName]["c"]["a"][i][2].AsInt;
        //    int xLength =   node[voxName]["c"]["a"][i][3].AsInt;
        //    int yLength =   node[voxName]["c"]["a"][i][4].AsInt;
        //    int zLength =   node[voxName]["c"]["a"][i][5].AsInt;
        //    int type =      node[voxName]["c"]["a"][i][6].AsInt;
        //    for (int x =  xStart; x < xStart + xLength; x++) {
        //        for (int y = yStart; y < yStart + yLength; y++) {
        //            for (int z = zStart; z < zStart + zLength; z++) {
        //                vs.AddVoxel(new VoxelPos(x, y, z), false, type);
        //                ++count;
        //            }
        //        }
        //    }
        //}
        ////Singles

        //for (int i = 0; i < node[voxName]["c"]["s"].AsArray.Count; i++) {
        //    vs.AddVoxel(new VoxelPos(node[voxName]["c"]["s"][i][0].AsInt
        //                             ,node[voxName]["c"]["s"][i][1].AsInt
        //                             ,node[voxName]["c"]["s"][i][2].AsInt)
        //                ,false, node[voxName]["c"]["s"][i][3].AsInt);
        //    ++count;
        //}


		vs.UpdateMeshes();


		//Debug.Log ("<color=green>VoxSaveFile "+voxName+" loaded successfully. "+count+" voxels loaded.</color>");
		loading = false;
        loaded = true;
	}
	struct SaveGarbage
	{
		public int type;
		public bool saved;
	}

	struct SaveArea 
	{
		public SaveArea(int _type, VoxelPos _p1, VoxelPos _p2){type = _type;pos1 = _p1; pos2 = _p2;}
		public int type;
		public VoxelPos pos1;
		public VoxelPos pos2;
	}
	struct SaveSingle
	{
		public SaveSingle(int _type, VoxelPos _p1){type = _type;pos1 = _p1;}
		public int type;
		public VoxelPos pos1;
	}

	public void SaveSystem()//(bool SaveWorldSpace)
	{	
		saving = true;
		JSONClass node = new JSONClass();


		//if(voxName == "") voxName = vs.voxName;


		node[voxName]["i"][0].AsInt = vs.XSize;
		node[voxName]["i"][1].AsInt = vs.YSize;
		node[voxName]["i"][2].AsInt = vs.ZSize;
		node[voxName]["i"][3].AsInt = vs.ChunkSizeX;
		node[voxName]["i"][4].AsInt = vs.ChunkSizeY;
		node[voxName]["i"][5].AsInt = vs.ChunkSizeZ;
		node[voxName]["i"][6].AsFloat = vs.VoxelSpacing;

        string initLine = vs.XSize.ToString() + "~" + vs.YSize.ToString() + "~" + vs.ZSize.ToString()
                        + "~" + vs.ChunkSizeX.ToString() + "~" + vs.ChunkSizeY.ToString() + "~" + vs.ChunkSizeZ.ToString()
                        + "~" + vs.VoxelSpacing.ToString();

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

		int voxX = vs.XSize*vs.ChunkSizeX;
		int voxY = vs.YSize*vs.ChunkSizeY;
		int voxZ = vs.ZSize*vs.ChunkSizeZ;


		//Convert a 3x3x3x3x3x3 to 9x9x9 for easibility 
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


		int xtemp, ytemp, ztemp, x_length, y_length, z_length; 
		VoxelPos Bookmark = new VoxelPos();
		List<SaveArea> saveStreamArea = new List<SaveArea>();
		List<SaveSingle> saveStreamSingle = new List<SaveSingle>();
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
							saveStreamSingle.Add(info);
						}else
						{
							SaveArea info = new SaveArea(g[x,y,z].type, new VoxelPos(x,y,z), new VoxelPos(x_length, y_length, z_length));
							saveStreamArea.Add(info);
						}
						x = Bookmark.x;
						y = Bookmark.y;
						z = Bookmark.z;
					}
				}				
			}
		}
		//count = 0;
       
        using (StreamWriter output = new StreamWriter("./Assets/SavedVoxels/" + voxName + ".txt", false))
        {
            output.Write(initLine);
            output.Write("a"+saveStreamArea.Count.ToString());
            
            foreach (SaveArea i in saveStreamArea)
            {

                //node[voxName]["c"]["a"][count][0].AsInt = i.pos1.x;
                //node[voxName]["c"]["a"][count][1].AsInt = i.pos1.y;
                //node[voxName]["c"]["a"][count][2].AsInt = i.pos1.z;
                //node[voxName]["c"]["a"][count][3].AsInt = i.pos2.x;
                //node[voxName]["c"]["a"][count][4].AsInt = i.pos2.y;
                //node[voxName]["c"]["a"][count][5].AsInt = i.pos2.z;
                //node[voxName]["c"]["a"][count][6].AsInt = i.type;
                //count++;

                output.Write("|"+ i.pos1.x.ToString() + "~" + i.pos1.y.ToString() + "~" + i.pos1.z.ToString()
                            + "~" + i.pos2.x.ToString() + "~" + i.pos2.y.ToString() + "~" + i.pos2.z.ToString()
                            + "~" + i.type.ToString());

                
            }
            //count = 0;
            output.Write("s"+saveStreamSingle.Count.ToString());
            foreach (SaveSingle i in saveStreamSingle)
            {
                //node[voxName]["c"]["s"][count][0].AsInt = i.pos1.x;
                //node[voxName]["c"]["s"][count][1].AsInt = i.pos1.y;
                //node[voxName]["c"]["s"][count][2].AsInt = i.pos1.z;
                //node[voxName]["c"]["s"][count][3].AsInt = i.type;
                //count++;

                output.Write("|"+i.pos1.x.ToString() + "~" + i.pos1.y.ToString() + "~" + i.pos1.z.ToString() + "~"+i.type.ToString());
            }
          

            //System.IO.File.WriteAllText("./Assets/SavedVoxels/"+ voxName +".txt",node.ToString());
            
            //Debug.Log("String Char Count "+node.ToString().Length+" "+node.ToString());
            
            output.Close();
        }
		saving = false;

	}



}
