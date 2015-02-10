using UnityEngine;
using System.Collections;

public class KillOnImpact : MonoBehaviour {
    public GameObject DeathAnimation;
    public LayerMask mask;
    public int RadiusExplosion = 2;
	// Use this for initialization
    GameObject deffect;
	void Start () {
        deffect =  Instantiate(DeathAnimation, transform.position, Quaternion.identity) as GameObject;
        deffect.transform.parent = transform;
        deffect.transform.localPosition = Vector3.zero;
        deffect.particleSystem.Pause();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider hit){
        CollideWithSomething(hit);
    }
    void OnTriggerStay(Collider hit)
    {   
        CollideWithSomething(hit);
    }
    void CollideWithSomething(Collider hit)
    {
        
        if(hit.gameObject.tag == "Chunk")
        {
            
            RaycastHit rayhit;
            if(Physics.Raycast(transform.position, rigidbody.velocity.normalized, out rayhit, 5, mask))
            {
                
                deffect.transform.parent = null;
                deffect.particleSystem.Play();
                Destroy(deffect, deffect.particleSystem.duration);
                if(rayhit.rigidbody!=null)
                {
                    rayhit.transform.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(rayhit, RadiusExplosion);
                    hit.transform.parent.rigidbody.AddForceAtPosition(rigidbody.velocity * rigidbody.mass, transform.position);
                }
                else rayhit.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(rayhit, RadiusExplosion);


                
                Destroy(gameObject);
            }//else  Debug.DrawRay(transform.position, rigidbody.velocity, Color.green);
            
        }
    }
}
