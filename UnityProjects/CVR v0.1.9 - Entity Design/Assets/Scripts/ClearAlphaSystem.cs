using UnityEngine;
using System.Collections;

public class ClearAlphaSystem : MonoBehaviour {

	//Uses a texture as a cookie cutter on a voxel system
	//Areas that are alpha are removed

	public Texture2D Top;
	public Texture2D Side;
	public Texture2D Front;
	public Vector2 sheetLayout;
	public float alphaCuttoff;
	public int frame =1;
	VoxelSystemGreedy vs;
	bool init = false;

	// Use this for initialization
	void Start () {
		vs = GetComponent<VoxelSystemGreedy>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!init)
		{
			if(vs.Initialized)
			{
				for (int x = 0; x < Top.width/sheetLayout.x; x++) {
					for (int z = 0; z < Top.height/sheetLayout.y; z++) {
						Color pixColor = Top.GetPixel(x,z);


						for (int y = 0; y < vs.ChunkSizeY * vs.YSize; y++)
						{
							if(pixColor.a <= alphaCuttoff)
							{
								vs.RemoveVoxel(new VoxelPos(x,y,z), false);
							}else vs.AddVoxel(new VoxelPos(x,y,z), false);
						}														
						
					}
				}
				for (int y = 0; y < Side.width/sheetLayout.x; y++) {
					for (int z = 0; z < Side.height/sheetLayout.y; z++) {
						Color pixColor = Side.GetPixel(y,z);
						
						for (int x = 0; x < vs.ChunkSizeX * vs.XSize; x++)
						{
							if(pixColor.a <= alphaCuttoff)
							{
								vs.RemoveVoxel(new VoxelPos(x,y,z), false);
							}//else vs.AddVoxel(new VoxelPos(x,y,z), false);
						}														
						
					}
				}
				for (int x = 0; x < Front.width/sheetLayout.x; x++) {
					for (int y = 0; y < Front.height/sheetLayout.y; y++) {
						Color pixColor = Front.GetPixel(x,y);

						for (int z = 0; z < vs.ChunkSizeZ * vs.ZSize; z++)
						{
							if(pixColor.a <= alphaCuttoff)
							{
								vs.RemoveVoxel(new VoxelPos(x,y,z), false);
							}//else vs.AddVoxel(new VoxelPos(x,y,z), false);
						}														
						
					}
				}
				vs.UpdateMeshes();
				init = true;
			}

		}
	}
}
