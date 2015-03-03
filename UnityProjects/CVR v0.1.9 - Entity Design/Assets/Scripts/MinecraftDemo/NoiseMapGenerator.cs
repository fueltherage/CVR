using UnityEngine;
using System.Collections;

public class NoiseMapGenerator : MonoBehaviour {

	public Texture2D noiseMap;
	public int StartX=0;
	public int StartY=0;
	public bool FillImage = false;
	public float Cuttoff = 0.1f;
	public int numberOfLayers = 2;
    public bool SinWave = false;
    public float w = 0.1f;
	public int maxHeight = 16;
	VoxSystemChunkManager vcm;
	VoxelSystemGreedy vs;
	int layerStep;
    Color[] textAlpha;
	public int type;
	// Use this for initialization
	void Start () {
        vcm = gameObject.GetComponent<VoxSystemChunkManager>();
        vs = gameObject.GetComponent<VoxelSystemGreedy>();
	}

	void LoadImageData()
	{
		
        int width = vs.XSize* vs.ChunkSizeX;
        int height = vs.ZSize * vs.ChunkSizeZ;
        textAlpha = noiseMap.GetPixels(StartX, StartY, width, height);

		//int CellWidth = noiseMap.width / vs.ChunkSizeX / vs.XSize;
		//int CellHeight = noiseMap.height / vs.ChunkSizeZ / vs.ZSize;
		//layerStep = maxHeight/numberOfLayers;
		layerStep=1;
		VoxelPos vp = new VoxelPos();
		float alphaValue;

		for (int z = 0; z < vs.ZSize; z++) 
		{
			for(int x = 0; x < vs.ZSize; x++)
			{
				for(int zc = 0; zc < vs.ChunkSizeZ; zc++)
				{
					for (int xc = 0; xc < vs.ChunkSizeX; xc++)
					{
						vp.x = x * vs.ChunkSizeX + xc;
						vp.y = 0;
						vp.z = z * vs.ChunkSizeZ + zc;
						alphaValue = textAlpha[xc + zc * width + x * vs.ChunkSizeX + z * vs.ChunkSizeZ * width].a;

					    if(alphaValue > Cuttoff)
						{
							vp.y = Mathf.RoundToInt(maxHeight * alphaValue - (Cuttoff * maxHeight));


										
							//Debug.Log(vp.ToString());
							for (int h = vp.y; h >= 0; h--) {
                                if(SinWave)
                                {
                                    type = Mathf.RoundToInt((Mathf.Sin((w/2.0f*Mathf.PI)*(x*vs.ChunkSizeX+xc))+w)+
										   (Mathf.Sin((w/2.0f*Mathf.PI)*(h))+w)+
									       (Mathf.Sin((w/2.0f*Mathf.PI)*(z*vs.ChunkSizeZ+zc))+w));
									if(type<=0)vcm.RemoveVoxel(new VoxelPos(vp.x,h,vp.z),false);

                                }else
								{
									vcm.AddVoxel(new VoxelPos(vp.x,h,vp.z),false,type);
								}
							}							
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
            if(vs.Initialized)
            {               
    			Debug.Log("loading image");
    			LoadImageData();
            }else Debug.Log("Can not load noise map, System not initialized");
		}
	}
}
