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
    public CellStats playerStats;
    public int LaserDamage = 100;
    public bool worldSpace = true;
    Camera cam; 

    Vector3 laserOffset = new Vector3(0.0f, 1.0f, 0.0f);
	// Use this for initialization
	void Start () {
        en = GameObject.FindGameObjectWithTag("Player").GetComponent<EnergyManager>();
        if (GameState.ControllerEnabled)
        {
            cam = transform.parent.GetComponent<Camera>();
        }else cam = GetComponent<Camera>();
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameState.ControllerEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                shoot();
            }
        }
        else
        {
            float rtrigger = Input.GetAxis("RightTrigger");
            if (rtrigger > 0.5f)
            {
                shoot();
            }
        }
	}
    void shoot()
    {
        if (en.UseEnergy(cost))
        {
            Ray laserRay;
            if (!GameState.ControllerEnabled)
            {
                laserRay = cam.ScreenPointToRay(Input.mousePosition);
            }
            else
            {
                laserRay = cam.ScreenPointToRay(new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f));
            }
            Vector3 localPos = laserRay.GetPoint(Range) - this.transform.position;
            RaycastHit[] hit;
            hit = Physics.RaycastAll(laserRay, Range, mask);
            bool hitwall = false;
            for (int i = 0; i < hit.Length; i++)
            {

                RemoveVoxel(hit[i]);
                if (hit[i].transform.parent != null)
                {
                    Rigidbody rb = hit[i].transform.parent.GetComponent<Rigidbody>();
                    if (rb != null) rb.AddForceAtPosition(this.transform.forward * 1000, hit[i].point);
                }

                VoxelSystemChunkGreedy vsg = hit[i].transform.gameObject.GetComponent<VoxelSystemChunkGreedy>();
                if (vsg != null)
                    if (vsg.systemParent.tag == "Environment")
                    {
                        hitwall = true;
                        if (worldSpace)
                            effect.go(this.transform.position - ((-cam.transform.up + cam.transform.right) * 2), hit[i].point);
                        else effect.go(((-cam.transform.up + cam.transform.right) * 2), hit[i].point - this.transform.position);
                    }
            }
            if (!hitwall)
            {
                if (worldSpace)
                    effect.go(this.transform.position - ((-cam.transform.up + cam.transform.right) * 2), laserRay.GetPoint(Range));
                else effect.go(-cam.transform.up + cam.transform.right, localPos);
            }
        }
    }
    
    void RemoveVoxel(RaycastHit hitPos)
    {
        List<VoxSystemChunkManager> v = new List<VoxSystemChunkManager>();
        hitPos.transform.parent.gameObject.GetComponentsInChildren<VoxSystemChunkManager>(v);
        int temp = AOE;

        if (AOE > 0) foreach (VoxSystemChunkManager vs in v)
        {
            if (vs.transform.parent.parent != null)
            {
                CellStats cs = vs.transform.parent.parent.gameObject.GetComponent<CellStats>();
                if (cs != null)
                    if (!cs.Viral) playerStats.threat += 10;

                cs.health -= LaserDamage;
            }

            if (vs.vSystem.VoxelSpacing == 2) temp = AOE * (int)vs.vSystem.VoxelSpacing;
            vs.RemoveVoxelAoE(hitPos, temp, true);
        }
        else foreach (VoxSystemChunkManager vs in v) vs.RemoveVoxel(hitPos, true);
    }

}
