using UnityEngine;
using System.Collections;

public class SinWaveGen : MonoBehaviour {
	VoxelSystemGreedy vc;
	int numberOfFilledVoxels;
	bool init = false;
	public bool Multicolor = false;
	public float w=0.1f;
	public float shift=0.0f;
	// Use this for initialization
	void Start () {
		vc = this.gameObject.GetComponent<VoxelSystemGreedy>();	


	}
	
	// Update is called once per frame
	void Update () {
		if(vc.Initialized && !init)
		{
			init = true;
			Noise();
		}
//		if (Input.GetKeyDown(KeyCode.RightArrow))
//		{
//			w += 0.1f;
//			Noise();
//		}		
//		if (Input.GetKeyDown(KeyCode.LeftArrow))
//		{
//			w -= 0.1f;
//			Noise();
//		}
//		if (Input.GetKey(KeyCode.UpArrow))
//		{
//
//		}
//		if (Input.GetKey(KeyCode.DownArrow))
//		{
//
//		}
//		if (Input.GetKeyDown(KeyCode.Space))
//		{
//
//		}

	
	}
//	void OnGUI()
//	{		
//		int BoxWidth = 155 +numberOfFilledVoxels.ToString().Length;
//		//GUI.Box (new Rect (Screen.width/2 - BoxWidth/2, 10, BoxWidth, 25), "Number Of Voxels "+numberOfFilledVoxels);
//	}
	void Noise()
	{

		numberOfFilledVoxels=0;
		for (int x = 0; x < vc.XSize; x++){
			for (int y = 0; y < vc.YSize; y++){
				for (int z = 0; z < vc.ZSize; z++){
					for (int xc = 0; xc < vc.ChunkSizeX; xc++){
						for (int yc = 0; yc < vc.ChunkSizeY; yc++){
							for (int zc = 0; zc < vc.ChunkSizeZ; zc++){
								float value = Mathf.Sin((w/2.0f*Mathf.PI)*(x*vc.ChunkSizeX+xc))+
											  Mathf.Sin((w/2.0f*Mathf.PI)*(y*vc.ChunkSizeY+yc))+
											  Mathf.Sin((w/2.0f*Mathf.PI)*(z*vc.ChunkSizeZ+zc));
								//Debug.Log(value);
								int VoxType;
								if(value<0)VoxType=0;
								else if(!Multicolor)
								{
									VoxType =1;
									numberOfFilledVoxels++;
								}
								else 
								{
									VoxType = Mathf.RoundToInt(value/3.0f*(vc.factory.VoxelMats.Count-1));
									numberOfFilledVoxels++;
								}
								VoxelFactory.GenerateVoxel(VoxType, ref vc.chunks_vcs[x,y,z].blocks[xc, yc, zc]);

							}
						}
					}
				}
			}
		}
        vc.UpdateMeshes();
	}
}
