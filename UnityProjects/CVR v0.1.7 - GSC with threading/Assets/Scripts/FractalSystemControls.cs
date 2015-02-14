
using UnityEngine;
using System.Collections;

public class FractalSystemControls : MonoBehaviour {

	// Use this for initialization
	VoxelSystemGreedy vc;
	public float step = 0.2f;
	public float n = 2f;
	public float shiftStep = 0.5f;
	public float shift = 1.0f;
	public float scale = 0.45f;
	public float colorManip = 1.75f;
	public int layerCount =0;
	Vector3 zoomLocation;
	Ray mouseRay;
	RaycastHit hit;
	bool init = false;
	bool inverted = true;
	int numberOfFilledVoxels=0;

	// Use this for initialization
	void OnGUI()
	{
		int BoxWidth = 155 +numberOfFilledVoxels.ToString().Length;
		GUI.Box (new Rect (Screen.width/2 - BoxWidth/2, 10, BoxWidth, 25), "Number Of Voxels "+numberOfFilledVoxels);
	}
	void Start()
	{
		vc = gameObject.GetComponent<VoxelSystemGreedy>();
	}
	
	// Update is called once per frame
	void Update()
	{

		if (vc.Initialized && !init)
		{
			Fractalize();
			init = true;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			n += step;
			zoomLocation = Vector3.zero;
			shift = 0;
			//scale = 1;
			Debug.Log("n = " + n);
			Fractalize();
		}
		
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			n -= step;
			zoomLocation = Vector3.zero;
			shift = 0;
			//scale = 1;
			Debug.Log("n = " + n);
			if (n < step) n = step;
			Fractalize();
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			shift += shiftStep;
			Fractalize();
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			shift -= shiftStep;
			Fractalize();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log ("inverted:"+inverted);
			inverted = !inverted;
			Fractalize();
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			scale /= 1.2f;
			Fractalize();
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			scale *= 1.2f;
			Fractalize();
		}
		if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.Z))
		{
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(mouseRay, out hit, 200))
			{
				VoxelPos pos = hit.transform.gameObject.GetComponent<VoxChunkManager>().RayCastHitToVoxelSpace(hit);
				zoomLocation.x = (pos.x + vc.offset.x / 2.0f);
				zoomLocation.y = (pos.y + vc.offset.y / 2.0f);
				zoomLocation.z = (pos.z + vc.offset.z / 2.0f);
				Fractalize();
			}
		}
		
	}
	
	void Fractalize()
	{
		numberOfFilledVoxels=0;
		for (int x = 0; x < vc.XSize; x++){
			for (int y = 0; y < vc.YSize; y++){
				for (int z = 0; z < vc.ZSize; z++){
					for (int xc = 0; xc < vc.ChunkSizeX; xc++){
						for (int yc = 0; yc < vc.ChunkSizeY; yc++){
							for (int zc = 0; zc < vc.ChunkSizeZ; zc++){
								int VoxType = FractalBULLSHIT(x*vc.ChunkSizeX + xc, y*vc.ChunkSizeY + yc, z*vc.ChunkSizeZ + zc);
								if(VoxType!=0)numberOfFilledVoxels++;
								VoxelFactory.GenerateVoxel(VoxType, ref vc.chunks_vcs[x,y,z].blocks[xc, yc, zc], vc.offset, vc.VoxelSpacing);
							}
						}
					}
				}
			}
		}
		vc.UpdateMeshes();
	}
	
	int FractalBULLSHIT(int _x, int _y, int _z)
	{
//		float x = (_x + zoomLocation.x) * scale;
//		float y = (_y + zoomLocation.y) * scale;
//		float z = (_z + zoomLocation.z) * scale;

		float x = _x ;
		float y = _y ;
		float z = _z ;
		
		float r;
		float theta;
		
		float phi;
		Vector3 sum;
		Vector3 acc = new Vector3();
		Vector3 lastSum;
		sum = new Vector3( (x + vc.offset.x) / (vc.XSize * vc.ChunkSizeX /2.0f * scale) 
		                  ,(y + vc.offset.y) / (vc.YSize * vc.ChunkSizeY /2.0f * scale)
		                  ,(z + vc.offset.z) / (vc.ZSize * vc.ChunkSizeZ /2.0f * scale));
		
		for (float i = 0; i < n * step; i += step)
		{
			r = sum.magnitude;
			theta = Mathf.Atan2(Mathf.Sqrt(sum.x * sum.x + sum.y * sum.y), sum.z);
			phi = Mathf.Atan2(sum.y, sum.x);
			lastSum = sum;
			sum += new Vector3(r * r * Mathf.Sin(theta * n + shift ) * Mathf.Cos(phi * n)
			                   , r * r * Mathf.Sin(theta * n  * shift) * Mathf.Sin(phi * n )
			                   , r * r * Mathf.Cos(theta * n  + shift));
			acc += sum - lastSum;
		}
		//print(acc.ToString());
		if (!inverted)
		if (sum.magnitude < 2)
		{
			int count = 0;
			int loopCuttoff = vc.factory.VoxelMats.Count;
			int loopcount = 1;
			do
			{
				if (acc.magnitude >= float.MaxValue)
				{
					count = vc.factory.VoxelMats.Count - 1;
					count =0;
					break;
				}
				else
				{
					if (acc.magnitude < Mathf.Pow(Mathf.Sqrt(n*count), count / n / colorManip))
						break;
					else
					{
						count++;
						if (count >= layerCount) count = 0;
					}
					
					if (loopcount >= loopCuttoff)
					{
						loopcount = 1;
						count=0;
						break;
					}
					loopcount++;
				}
			} while (true);
			return count;
		}
		else
		{
			return 0;
		}

		if (inverted)
		if (sum.magnitude > 2)
		{
			int count = 1;
			int loopCuttoff = vc.factory.VoxelMats.Count;
			int loopcount = 1;
			do
			{
				if (acc.magnitude >= float.MaxValue)
				{
					count = vc.factory.VoxelMats.Count - 1;
					count = 0;
					break;
				}
				else
				{
					if (acc.magnitude < Mathf.Pow(Mathf.Sqrt(n * count), count / n * 0.5f))
						break;
					else
					{
						count++;
						if (count >= layerCount) count = 0;
					}
					
					if (loopcount >= loopCuttoff)
					{
						loopcount = 0;
						count=0;
						break;
						
					}
					loopcount++;
				}
			} while (true);
			return count;
		}
		else
		{
			return 0;
		}

		return 0;
		
	}
}
