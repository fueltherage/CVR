using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseSelectionTool_Greedy : MonoBehaviour {
	public float SelectRange = 100.0f;
    public bool DebugMode = false;
    public int radius=0;

    int currentSelectVoxel;
    public static Vector3 MouseClickNormal = Vector3.zero;
    public static Vector3 MouseClickWorldPos = Vector3.zero;
	// Use this for initialization
    Color rayColor;
	RaycastHit hit;
	Ray mouseRay;
	string[] layerName = {"Rays","RaysConvex"};
	LayerMask mask;
	void Start () {
		mask =  LayerMask.GetMask(layerName);
        VoxelEvents.onVoxelSwitch += SelectedVoxel;
        VoxelEvents.VoxelSwitched(1);
	}

    void SelectedVoxel(int _current)
    {
        currentSelectVoxel = _current;
    }
	
	// Update is called once per frame
	void Update () {

        if(!GameState.gamePaused)
        {

    		mouseRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (DebugMode)
            {
                if (Input.GetMouseButtonDown(0))
                {                

					if (Physics.Raycast(mouseRay, out hit, SelectRange,mask))
                    {
                        rayColor = Color.green;
						//Debug.Log(hit.collider.gameObject.transform.parent.name);
                        if (Input.GetKey(KeyCode.LeftAlt))
                        {
                            RemoveVoxel(hit);
                        }
                        else AddVoxel(hit);
                    }else rayColor = Color.red;
                }
                if(Input.GetMouseButton(0))
                {
    				Debug.DrawLine(mouseRay.origin, mouseRay.direction * 100, Color.blue);
    				if(Input.GetKey (KeyCode.LeftControl))
    				{                
						if (Physics.Raycast(mouseRay, out hit, SelectRange, mask ))
    					{
    						rayColor = Color.green;
    						//Debug.Log(hit.collider.gameObject.transform.parent.name);
    						if (Input.GetKey(KeyCode.LeftAlt))
    						{
    							RemoveVoxel(hit);
    						}
    						else AddVoxel(hit);
    					}else rayColor = Color.red;
    				}

                }
                else Debug.DrawLine(mouseRay.origin, mouseRay.direction * 100, rayColor);
            }
		
            else
            {
                if (Input.GetMouseButtonDown(0))
                {             
					if (Physics.Raycast(mouseRay, out hit, SelectRange, mask))
                    {                       
						//Debug.Log(hit.collider.transform.parent.gameObject.name);
                        if (Input.GetKey(KeyCode.LeftAlt))
                        {
                            RemoveVoxel(hit);
                        }
                        else AddVoxel(hit);
                    }
                }
            }	
        }
	}
    
	void AddVoxel (RaycastHit hitPos)
	{
        //MouseClickNormal = hit.normal;
        //MouseClickWorldPos = hit.point;
        List<VoxSystemChunkManager> v = new List<VoxSystemChunkManager>();
        hitPos.transform.parent.gameObject.GetComponentsInChildren<VoxSystemChunkManager>(v); 
        if (radius > 0) foreach(VoxSystemChunkManager vs in v) vs.AddVoxelAoE(hitPos.point, radius, currentSelectVoxel, true);
        else foreach(VoxSystemChunkManager vs in v) vs.AddVoxel(hitPos, true, currentSelectVoxel);        
	}    
	void RemoveVoxel(RaycastHit hitPos)
	{
        MouseClickNormal = hit.normal;
        MouseClickWorldPos = hit.point;
        List<VoxSystemChunkManager> v = new List<VoxSystemChunkManager>();
        hitPos.transform.parent.gameObject.GetComponentsInChildren<VoxSystemChunkManager>(v);

        if (radius > 0) foreach (VoxSystemChunkManager vs in v) vs.RemoveVoxelAoE(hitPos.point, radius, false);
        else foreach (VoxSystemChunkManager vs in v) vs.RemoveVoxel(hitPos, true);     
	}
	public void ReduceRadius()
	{
		radius--;
		if(radius < 0 ) radius = 0;
	}
	public void IncreaseRadius()
	{
		radius++;
	}


}
