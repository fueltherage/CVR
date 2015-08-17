using UnityEngine;
using System.Collections;

public class ClearAlphaSystem : MonoBehaviour {

	//Uses a texture as a cookie cutter on a voxel system
	//Areas that are alpha are removed

	public Texture2D Top;
	public Texture2D Side;
	public Texture2D Front;
	public Vector2 sheetLayout = new Vector2(1,1);
	public float alphaCuttoff;
	public int frame =1;
	VoxelSystemGreedy vs;
	bool init = false;
	VoxelPos spacing;

	// Use this for initialization
	void Start () {
		vs = GetComponent<VoxelSystemGreedy>();

		if(sheetLayout.magnitude!=1)
		{			
			spacing = new VoxelPos(Top.width / (vs.XSize * vs.ChunkSizeX),
			                       Side.height / (vs.YSize * vs.ChunkSizeY),
			                       Top.height / (vs.ZSize * vs.ChunkSizeZ ));
		}else
		{
			spacing = new VoxelPos(1,1,1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!init)
		{
			if(vs.Initialized)
			{
				Clear();
			}
		}
		if(Input.GetKeyDown (KeyCode.C))
		{
			Clear ();
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			alphaCuttoff = Mathf.Clamp(alphaCuttoff+0.1f, 0,1); 
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			alphaCuttoff = Mathf.Clamp(alphaCuttoff-0.1f, 0,1); 
		}

	}
	void Clear()
	{
		if(Top!=null)
		for (int x = 0; x < Top.width/sheetLayout.x/spacing.x; x++) {
			for (int z = 0; z < Top.height/sheetLayout.y/spacing.y; z++) {
				Color pixColor = Top.GetPixel(x * spacing.x,z* spacing.z);
				
				
				for (int y = 0; y < vs.ChunkSizeY * vs.YSize; y++)
				{
					if(pixColor.a <= alphaCuttoff)
					{
						vs.RemoveVoxel(new VoxelPos(x,y,z), false);
					}//else vs.AddVoxel(new VoxelPos(x,y,z), false);
				}														
				
			}
		}
        if(Side!=null)
		for (int z = 0; z < Side.width/sheetLayout.x/spacing.z; z++) {
			for (int y = 0; y < Side.height/sheetLayout.y/spacing.y; y++) {
				Color pixColor = Side.GetPixel(z* spacing.z,y* spacing.y);
				
				for (int x = 0; x < vs.ChunkSizeX * vs.XSize; x++)
				{
					if(pixColor.a <= alphaCuttoff)
					{
						vs.RemoveVoxel(new VoxelPos(x,y,z), false);
					}//else vs.AddVoxel(new VoxelPos(x,y,z), false);
				}														
				
			}
		}
        if(Front!=null)
        for (int x = 0; x < Front.width / sheetLayout.x / spacing.x; x++)
        {
            for (int y = 0; y < Front.height / sheetLayout.y / spacing.y; y++)
            {
                Color pixColor = Front.GetPixel(x * spacing.x, y * spacing.y);

                for (int z = 0; z < vs.ChunkSizeZ * vs.ZSize; z++)
                {
                    if (pixColor.a <= alphaCuttoff)
                    {
                        vs.RemoveVoxel(new VoxelPos(x, y, z), false);
                    }//else vs.AddVoxel(new VoxelPos(x,y,z), false);
                }

            }
        }
		vs.UpdateMeshes();
		init = true;
	}
}
