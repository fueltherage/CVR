using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class SaveLoadVoxels : MonoBehaviour {
	public string Name = "";

	VoxelSystemGreedy vs; 

	// Use this for initialization
	void Start () {
		vs = GetComponent<VoxelSystemGreedy>();
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Y))
		{
			SaveSystem();
		}
	
	}
	public void SaveSystem()//(bool SaveWorldSpace)
	{	
		JSONNode node = new JSONClass();
		//JSONArray chunks = new JSONArray();

		if(Name == "") Name = vs.name;
		
		node[Name]["info"]["x"].AsInt = vs.XSize;
		node[Name]["info"]["y"].AsInt = vs.YSize;
		node[Name]["info"]["z"].AsInt = vs.ZSize;
		node[Name]["info"]["xc"].AsInt = vs.ChunkSizeX;
		node[Name]["info"]["yc"].AsInt = vs.ChunkSizeY;
		node[Name]["info"]["zc"].AsInt = vs.ChunkSizeZ;
		node[Name]["info"]["vs"].AsFloat = vs.VoxelSpacing;

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
		int count = 0;
		for (int x = 0; x < vs.XSize; x++)
			for (int y = 0; y < vs.YSize; y++)
				for (int z = 0; z < vs.ZSize; z++)
					for (int xc = 0; xc < vs.ChunkSizeX; xc++)
						for (int yc = 0; yc < vs.ChunkSizeY; yc++)
							for (int zc = 0; zc < vs.ChunkSizeZ; zc++){	
								if(vs.chunks_vcs[x,y,z].blocks[xc,yc,zc].voxel.VoxelType > 0)
								{
								//Save Their voxel position and value
								node["chunks"][count]["type"].AsInt = vs.chunks_vcs[x,y,z].blocks[xc,yc,zc].voxel.VoxelType;
								node["chunks"][count]["pos"][0].AsInt = x * vs.ChunkSizeX + xc;
								node["chunks"][count]["pos"][1].AsInt = y * vs.ChunkSizeY + yc;
								node["chunks"][count]["pos"][2].AsInt = z * vs.ChunkSizeZ + zc;
								count++;
								}
							}	

		
		System.IO.File.WriteAllText("./Assets/SavedVoxels/"+Name+".txt",node.ToString());
		Debug.Log(node.ToString());
	}
}
