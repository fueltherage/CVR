using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootLaser : MonoBehaviour {
    public LaserEffect effect;
    EnergyManager en;
    public float cost = 25f;
    public float Range = 100f;
    public int AOE = 3;
    public LayerMask mask;

    Camera cam; 

    Vector3 laserOffset = new Vector3(0.0f, 1.0f, 0.0f);
	// Use this for initialization
	void Start () {
        en = GameObject.FindGameObjectWithTag("Player").GetComponent<EnergyManager>();
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (en.UseEnergy(cost))
            {
                Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(mouseRay, out hit, Range, mask))
                {
                    effect.go(this.transform.position - laserOffset + (cam.transform.right * 2), hit.point);
                    RemoveVoxel(hit);                   
                }               
            }
        }	
	}
    
    void RemoveVoxel(RaycastHit hitPos)
    {
        List<VoxSystemChunkManager> v = new List<VoxSystemChunkManager>();
        hitPos.transform.parent.gameObject.GetComponentsInChildren<VoxSystemChunkManager>(v);
        if (AOE > 0) foreach (VoxSystemChunkManager vs in v) vs.RemoveVoxelAoE(hitPos, AOE, true);
        else foreach (VoxSystemChunkManager vs in v) vs.RemoveVoxel(hitPos, true);
    }

}
