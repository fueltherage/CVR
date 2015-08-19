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
            if (Input.GetKeyDown(KeyCode.T))
            {
                LoadSystem();
            }
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
        if (voxName == "")
        {
            Debug.LogError("<color=red>Error:</color> Missing voxName, please provide voxName in script inspector.");
            return;
        }
        string s = "";
        try
        {
            //s = System.IO.File.ReadAllText("./Assets/SavedVoxels/"+voxName+".txt");            
            s = System.IO.File.ReadAllText(Application.dataPath + "\\SavedVoxels\\" + voxName + ".txt");
        }
        catch (UnityException e)
        {
            Debug.LogError("<color=red>Error:</color> File not found at location: " + Application.dataPath + @"\" + voxName + ".txt");
            return;
        }

        List<SaveArea> areaSaves = new List<SaveArea>();
        List<SaveSingle> singleSaves = new List<SaveSingle>();
        int c = 0; // c is index of the string
        string buf = "";//Buffer for processing the string
        int index = 0;
        while (true)//Initialiation info loaded
        {
            if (s[c] == 'a' || s[c] == 's')
            {
                break;
            }
            while (true)
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
                    int parsed = int.Parse(buf, NumberStyles.Integer); 
                    
                    switch (index)
                    {
                        case 0:
                            vs.XSize = parsed;
                            break;
                        case 1:
                            vs.YSize = parsed;
                            break;
                        case 2:
                            vs.ZSize = parsed;
                            break;
                        case 3:
                            vs.ChunkSizeX = parsed;
                            break;
                        case 4:
                            vs.ChunkSizeY = parsed;
                            break;
                        case 5:
                            vs.ChunkSizeZ = parsed;
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

        while (true)//Read until |        
        {
            if (c < s.Length)
            {
                if (s[c] != '|')
                {
                    buf += s[c];
                    c++;

                }
                else break;
            }
            else break;

        }

        count = int.Parse(buf, NumberStyles.Integer);//How many voxel areas 
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
                    index = 0;
                }

                while (true)
                {  
                    
                    if (s[c] != '|' && s[c] != '~' && s[c] != 's')
                    {
                        buf += s[c];
                        c++;
                    }
                    else
                    {
                        int parsed = 0;
                        try
                        {
                            parsed = int.Parse(buf, NumberStyles.Integer);
                        }
                        catch (System.FormatException e)
                        {
                            Debug.Log(buf);
                        }
                        switch (index)
                        {
                            case 0:
                                a.pos1.x = parsed;
                                break;
                            case 1:
                                a.pos1.y = parsed;
                                break;
                            case 2:
                                a.pos1.z = parsed;
                                break;
                            case 3:
                                a.pos2.x = parsed;
                                break;
                            case 4:
                                a.pos2.y = parsed;
                                break;
                            case 5:
                                a.pos2.z = parsed;
                                break;
                            case 6:
                                a.type = parsed;
                                areaSaves.Add(a);
                                break;
                        }
                        if (s[c] == 's')
                        {
                            break;
                        }
                        c++;
                        index++;
                        buf = "";
                        if (index >= 7)
                        {
                            index = 0;
                            break;
                        }
                    }                    
                }
            }
        }
        index = 0;
        count = 0;
        buf = "";
        if (s[c] == 's')
        {
            c++;
            while (true)
            {//Read until |
                if (s[c] != '|')
                {
                    buf += s[c];
                    c++;
                }
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

                    //This end of file check is just a fix to keep from going outside of the array during if statements
                    if (c >= s.Length)
                    {//Im assuming the last number has finished buffering
                        int parsed = int.Parse(buf, NumberStyles.Integer);
                        ss.type = parsed;
                        singleSaves.Add(ss);
                        break;
                    }


                    if (s[c] != '|' && s[c] != '~')
                    {//Read until ~
                        buf += s[c];
                        c++;
                    }
                    else
                    {
                        int parsed = int.Parse(buf, NumberStyles.Integer);
                        switch (index)
                        {
                            case 0:
                                ss.pos1.x = parsed;
                                break;
                            case 1:
                                ss.pos1.y = parsed;
                                break;
                            case 2:
                                ss.pos1.z = parsed;
                                break;
                            case 3:
                                ss.type = parsed;
                                singleSaves.Add(ss);
                                break;
                        }
                        c++;
                        index++;
                        buf = "";
                        if (index == 4)
                        {
                            index = 0;
                            break;
                        }
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
            for (int x = sa.pos1.x; x < sa.pos1.x + sa.pos2.x; x++)
            {
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

	public void SaveSystem()
	{	
		saving = true;     

        string initLine = vs.XSize.ToString() + "~" + vs.YSize.ToString() + "~" + vs.ZSize.ToString()
                        + "~" + vs.ChunkSizeX.ToString() + "~" + vs.ChunkSizeY.ToString() + "~" + vs.ChunkSizeZ.ToString()
                        + "~" + vs.VoxelSpacing.ToString();		

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
								g[ x * vs.ChunkSizeX + xc,
                                   y * vs.ChunkSizeY + yc, 
                                   z * vs.ChunkSizeZ + zc ].type = vs.chunks_vcs[x,y,z].blocks[xc,yc,zc].voxelType;																
							}


		int xtemp, ytemp, ztemp, x_length, y_length, z_length; 
		VoxelPos Bookmark = new VoxelPos();
		List<SaveArea> saveStreamArea = new List<SaveArea>();
		List<SaveSingle> saveStreamSingle = new List<SaveSingle>();

        //Saving method uses greedy meshing logic to form saveAreas / saveSingle
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
							if((g[xtemp,ytemp,ztemp + 1].type == g[xtemp,ytemp,ztemp].type) && !g[xtemp,ytemp,ztemp + 1].saved)
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

               

                output.Write("|"+ i.pos1.x.ToString() + "~" + i.pos1.y.ToString() + "~" + i.pos1.z.ToString()
                            + "~" + i.pos2.x.ToString() + "~" + i.pos2.y.ToString() + "~" + i.pos2.z.ToString()
                            + "~" + i.type.ToString());

                
            }
            //count = 0;
            output.Write("s"+saveStreamSingle.Count.ToString());
            foreach (SaveSingle i in saveStreamSingle)
            {
                

                output.Write("|"+i.pos1.x.ToString() + "~" + i.pos1.y.ToString() + "~" + i.pos1.z.ToString() + "~"+i.type.ToString());
            }  
            //Debug.Log("String Char Count "+node.ToString().Length+" "+node.ToString());
            
            output.Close();
        }
		saving = false;
	}
}
