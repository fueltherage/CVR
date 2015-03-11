using UnityEngine;
using System.Collections;

public class MouseSelectionTool_Greedy : MonoBehaviour {
	public float SelectRange = 100.0f;
    public bool DebugMode = false;
    public int radius=0;
	// Use this for initialization
    Color rayColor;
	RaycastHit hit;
	Ray mouseRay;
	string[] layerName = {"Rays","RaysConvex"};
	LayerMask mask;
	void Start () {
		mask =  LayerMask.GetMask(layerName);

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
        if(radius > 0){
			if(hitPos.transform.gameObject.GetComponent<Rigidbody>())
			{
	    		if (hitPos.transform.gameObject.tag == "Chunk")
	                hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxelAoE(hitPos, radius);
				else if (hitPos.transform.gameObject.tag == "System")
	                hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxelAoE(hitPos, radius);
	
			}else
			{
				if (hitPos.transform.gameObject.tag == "Chunk")
					hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxelAoE(hitPos, radius);
				else if (hitPos.transform.gameObject.tag == "System")
					hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxelAoE(hitPos, radius);
			}
        }else 
        {
			if(hitPos.transform.gameObject.GetComponent<Rigidbody>())
			{
	            if (hitPos.transform.gameObject.tag == "Chunk")
	                hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxel(hitPos, true);
	            else if (hitPos.transform.gameObject.tag == "System")
	                hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxel(hitPos, true);
			}else
			{
				if (hitPos.transform.gameObject.tag == "Chunk")
					hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxel(hitPos, true);
				else if (hitPos.transform.gameObject.tag == "System")
					hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxel(hitPos, true);
			}
        }
	}
	void RemoveVoxel(RaycastHit hitPos)
	{
		//This looks redundant but that's because UNITY WONT LET ME RAYCAST TO A NON CONCAVE MESH COLLIDER THAT'S CHILDED TO A RIDGIDBODY .. ERR
        if(radius > 0)
        {
			if(hitPos.transform.gameObject.GetComponent<Rigidbody>())
			{
	    		if (hitPos.transform.gameObject.tag == "Chunk")
					hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(hitPos,radius);
	    		else if (hitPos.transform.gameObject.tag == "System")
	                hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(hitPos,radius);	
			}else{
				if (hitPos.transform.gameObject.tag == "Chunk")
					hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(hitPos,radius);
				else if (hitPos.transform.gameObject.tag == "System")
					hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(hitPos,radius);	
			}
        }
        else 
        {
			if(hitPos.transform.gameObject.GetComponent<Rigidbody>())
			{
	            if (hitPos.transform.gameObject.tag == "Chunk")
					hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxel(hitPos,true);
	            else if (hitPos.transform.gameObject.tag == "System")
	                hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxel(hitPos,true);  
			}else{
				if (hitPos.transform.gameObject.tag == "Chunk")
					hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxel(hitPos,true);
				else if (hitPos.transform.gameObject.tag == "System")
					hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxel(hitPos,true);
			}
        }
	}


}
