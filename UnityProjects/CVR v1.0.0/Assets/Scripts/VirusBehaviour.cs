using UnityEngine;
using System.Collections;

public class VirusBehaviour : MonoBehaviour {

    Transform Colliders;
    int InfectionChance = 25;
	// Use this for initialization
    MoveToTarget mtt;
    FollowWaypoints FW;
	void Start () {
        Colliders = transform.FindChild("Colliders");
        mtt = GetComponent<MoveToTarget>();
        FW = GetComponent<FollowWaypoints>();
        mtt.moving = true;
	}
	
	// Update is called once per frame
	void Update () {
        mtt.vecTarget = FW.WpTarget_v3;
	}
    void OnTriggerEnter(Collider col)
    {
        GameObject go = col.transform.parent.parent.gameObject;
        if (go.tag == "Infectable")
        {
            if (Random.Range(1, 100) < InfectionChance)
            {
                this.transform.parent = col.transform;
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
                Colliders.gameObject.SetActive(false);
                this.transform.LookAt(col.transform);
                CellStats stats = go.GetComponent<CellStats>();
                stats.threat += 100;
                stats.Viral = true;
                mtt.enabled = false;                
                FW.enabled = false;
                DisableCollision();
            }
        }
    }
    void DisableCollision()
    {
        GetComponent<SphereCollider>().enabled = false;
        transform.FindChild("Colliders").gameObject.SetActive(false);
    }
}
