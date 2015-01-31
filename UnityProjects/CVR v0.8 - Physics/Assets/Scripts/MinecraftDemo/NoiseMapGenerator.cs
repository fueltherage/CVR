using UnityEngine;
using System.Collections;

public class NoiseMapGenerator : MonoBehaviour {

	public Texture2D noiseMap;
	public int StartX=0;
	public int StartY=0;
	public bool FillImage = false;
	public float Cuttoff = 0.1f;
	public int numberOfLayers = 2;
	VoxSystemChunkManager vcm;
	VoxelSystemGreedy vs;
	int layerStep;
	// Use this for initialization
	void Start () {
	}

	void LoadImageData()
	{
		vcm = gameObject.GetComponent<VoxSystemChunkManager>();
		vs = gameObject.GetComponent<VoxelSystemGreedy>();
		int maxHeight = vs.ChunkSizeY * vs.YSize;
		//int CellWidth = noiseMap.width / vs.ChunkSizeX / vs.XSize;
		//int CellHeight = noiseMap.height / vs.ChunkSizeZ / vs.ZSize;
		layerStep = maxHeight/numberOfLayers;
		VoxelPos vp = new VoxelPos();
		float alphaValue;
		int type ;
		for (int x = 0; x < vs.XSize; x++) 
		{
			for(int y = 0; y < vs.ZSize; y++)
			{
				for(int xi = 0; xi < vs.ChunkSizeX; xi++)
				{
					for (int yi = 0; yi < vs.ChunkSizeZ; yi++)
					{
						vp.x = x * vs.ChunkSizeX + xi;
						vp.y = 0;
						vp.z = y * vs.ChunkSizeX + yi;
						alphaValue = noiseMap.GetPixel(StartX + vp.x ,StartY + vp.z).a;
						type =0; 
						if(alphaValue > Cuttoff)
						{
							vp.y = Mathf.RoundToInt(maxHeight * alphaValue);
							type = (vp.y / layerStep);				
							//Debug.Log(vp.ToString());
							for (int h = vp.y; h >= 0; h--) {
								vcm.AddVoxel(new VoxelPos(vp.x,h,vp.z),false,type);
							}
							vcm.AddVoxel(vp,false,type);
						}

					}
				}
			}
		}
		vs.UpdateMeshes();
	}

	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.G))
		{
			Debug.Log("loading image");
			LoadImageData();
		}
	}
}
