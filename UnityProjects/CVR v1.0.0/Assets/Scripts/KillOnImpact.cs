using UnityEngine;
using System.Collections;

public class KillOnImpact : MonoBehaviour {
    public GameObject DeathAnimation;
    public LayerMask mask;
    public int RadiusExplosion = 2;
	public float RayDistance = 5;
	// Use this for initialization
    GameObject deffect;
	ParticleSystem particle_deffect;
	Rigidbody this_rb; 

	RaycastHit rayhit;
	void Start () {
        deffect =  Instantiate(DeathAnimation, transform.position, Quaternion.identity) as GameObject;
		particle_deffect = deffect.GetComponent<ParticleSystem>();
		deffect.transform.parent = transform;
        deffect.transform.localPosition = Vector3.zero;
		particle_deffect.Pause();
		this_rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		
		if(Physics.Raycast(transform.position, this_rb.velocity, out rayhit, RayDistance, mask))
		{
			Rigidbody rb = rayhit.transform.parent.GetComponent<Rigidbody>();
			deffect.transform.parent = null;
			particle_deffect.Play();
			Destroy(deffect, particle_deffect.duration);
			if(rb!=null)
			{
				rb.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(rayhit, RadiusExplosion, false);
				rb.AddForceAtPosition(this_rb.velocity * this_rb.mass, rayhit.point);

			}
			else rayhit.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(rayhit, RadiusExplosion, false);
			
			
			
			Destroy(gameObject);
		}//else  Debug.DrawRay(transform.position, rigidbody.velocity, Color.green);
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
            
           

            
        }
    }
}
