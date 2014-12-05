using UnityEngine;
using System.Collections;

public class ClearAlphaSystem : MonoBehaviour {

	public Texture2D texture;
	public Vector2 sheetLayout;
	public float alphaCuttoff;
	public int frame =1;
	VoxelSystem vs;
	bool init = false;

	// Use this for initialization
	void Start () {
		vs = GetComponent<VoxelSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!init)
		{
			if(vs.Initialized)
			{
				for (int x = 0; x < texture.width/sheetLayout.x; x++) {
					for (int z = 0; z < texture.height/sheetLayout.y; z++) {
						Color pixColor = texture.GetPixel(x,z);

						for (int y = 0; y < vs.ChunkSizeY; y++)
						{
							if(pixColor.a <= alphaCuttoff)
							{
								vs.RemoveVoxel(new VoxelPos(x,y,z), false);
							}else vs.AddVoxel(new VoxelPos(x,y,z), false);
						}														
						
					}
				}
				vs.UpdateMeshes();
				init = true;
			}

		}
	}
}
