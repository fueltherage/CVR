using UnityEngine;
using System.Collections;

public class CameraControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void CenterCamera()
    {
        if(transform.parent != null)
        if (transform.parent.gameObject.tag == "VoxelChunk")
        {
            VoxelChunk temp = transform.parent.gameObject.GetComponent<VoxelChunk>();
            Vector3 t = new Vector3((-((temp.XSize-1) % 2) * temp.VoxelSpacing) / 2.0f,
	                                (-((temp.YSize-1) % 2) * temp.VoxelSpacing) / 2.0f,
	                                (-((temp.ZSize-1) % 2) * temp.VoxelSpacing) / 2.0f);

            transform.localPosition = t;
        }
        else if (transform.parent.gameObject.tag == "VoxelSystem")
        {
			VoxelSystemGreedy temp = transform.parent.gameObject.GetComponent<VoxelSystemGreedy>();
			transform.localPosition = new Vector3((-((temp.XSize-1) % 2) * temp.VoxelSpacing) / 2.0f,
			                                      (-((temp.YSize-1) % 2) * temp.VoxelSpacing) / 2.0f,
			                                      (-((temp.ZSize-1) % 2) * temp.VoxelSpacing) / 2.0f);
        }
    }

}
