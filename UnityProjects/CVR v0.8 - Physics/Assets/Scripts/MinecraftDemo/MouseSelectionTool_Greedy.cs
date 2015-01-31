using UnityEngine;
using System.Collections;

public class MouseSelectionTool_Greedy : MonoBehaviour {
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
					//Debug.Log(hit.collider.gameObject.name);
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
					if (Physics.Raycast(mouseRay, out hit, SelectRange))
					{
						rayColor = Color.green;
						//Debug.Log(hit.collider.gameObject.name);
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
                   
					Debug.Log(hit.collider.gameObject.name);
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

		if (hitPos.transform.gameObject.tag == "Chunk")
			hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxel(hitPos, true);
		if (hitPos.transform.gameObject.tag == "System")
			hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().AddVoxel(hitPos, true);
		if (hitPos.transform.gameObject.tag == "Entity")
			hitPos.transform.FindChild("VoxelSystemGreedy").gameObject.GetComponent<VoxSystemChunkManager>().AddVoxel(hitPos, true);
	}
	void RemoveVoxel(RaycastHit hitPos)
	{
		if (hitPos.transform.gameObject.tag == "Chunk")
			hitPos.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(hitPos,5);
		else if (hitPos.transform.gameObject.tag == "System")
			hitPos.transform.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(hitPos,5);	
		else if (hitPos.transform.gameObject.tag == "Entity")
			hitPos.transform.FindChild("VoxelSystemGreedy").gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(hitPos,5);	
	}


}
