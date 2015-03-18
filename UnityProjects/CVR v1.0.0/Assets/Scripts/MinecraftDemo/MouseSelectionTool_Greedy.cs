using UnityEngine;
using System.Collections;

public class MouseSelectionTool_Greedy : MonoBehaviour {
	public float SelectRange = 100.0f;
    public bool DebugMode = false;
    public int radius=0;

    int currentSelectVoxel;

	// Use this for initialization
    Color rayColor;
	RaycastHit hit;
	Ray mouseRay;
	string[] layerName = {"Rays","RaysConvex"};
	LayerMask mask;
	void Start () {
		mask =  LayerMask.GetMask(layerName);
        VoxelEvents.onVoxelSwitch += SelectedVoxel;
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
                        if (Input.GetKey(KeyCode.LeftShift))
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
    						if (Input.GetKey(KeyCode.LeftShift))
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
                        if (Input.GetKey(KeyCode.LeftShift))
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
        if(radius > 0) hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxelAoE(hitPos, radius, true, currentSelectVoxel);
        else           hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().QuickAdd(hitPos, currentSelectVoxel, true);        
	}
	void RemoveVoxel(RaycastHit hitPos)
	{
        if(radius > 0) hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(hitPos, radius, true);
        else           hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().QuickRemove(hitPos, true);
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
