﻿using UnityEngine;
using System.Collections;

public class MouseSelectionTool_MultiChunk : MonoBehaviour {
	public float SelectRange = 100.0f;
    public bool DebugMode = false;
	// Use this for initialization
    Color rayColor;
	RaycastHit hit;
	Ray mouseRay;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (DebugMode)
        {
            if (Input.GetMouseButtonDown(0))
            {                
				if (Physics.Raycast(mouseRay, out hit, SelectRange))
                {
                    rayColor = Color.green;

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        RemoveVoxel(hit);
                    }
                    else AddVoxel(hit);
                }else rayColor = Color.red;
            }
            if(!Input.GetMouseButton(0))
            {
				Debug.DrawLine(mouseRay.origin, mouseRay.direction * 100, Color.blue);
				if(Input.GetKey (KeyCode.LeftControl))
				{                
					if (Physics.Raycast(mouseRay, out hit, SelectRange))
					{
						rayColor = Color.green;
						
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
				if (Physics.Raycast(mouseRay, out hit, SelectRange))
                {
                   

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        RemoveVoxel(hit);
                    }
                    else AddVoxel(hit);
                }
            }
        }	
	}
	void AddVoxel (RaycastHit hitPos)
	{
        if (hitPos.transform.gameObject.tag == "VoxelChunk")
            hitPos.transform.gameObject.GetComponent<VoxChunkManager>().AddVoxel(hitPos,true);
        else if (hitPos.transform.gameObject.tag == "VoxelSystemChunk")
			hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxel(hitPos, true);
	}
	void RemoveVoxel(RaycastHit hitPos)
	{
        if (hitPos.transform.gameObject.tag == "VoxelChunk")
            hitPos.transform.gameObject.GetComponent<VoxChunkManager>().RemoveVoxel(hitPos,true);
        else if (hitPos.transform.gameObject.tag == "VoxelSystemChunk")
            hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxel(hitPos, true);		
	}


}
