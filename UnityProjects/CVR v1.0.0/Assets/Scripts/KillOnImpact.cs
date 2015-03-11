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
        deffect.GetComponent<ParticleSystem>().Pause();

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
            if(Physics.Raycast(transform.position, GetComponent<Rigidbody>().velocity.normalized, out rayhit, 5, mask))
            {
                
                deffect.transform.parent = null;
                deffect.GetComponent<ParticleSystem>().Play();
                Destroy(deffect, deffect.GetComponent<ParticleSystem>().duration);
                if(rayhit.rigidbody!=null)
                {
                    rayhit.transform.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(rayhit, RadiusExplosion);
                    if(hit.transform.parent.GetComponent<Rigidbody>() != null)
					{
						Vector3 dif = transform.position - hit.transform.position;
                    	hit.transform.parent.GetComponent<Rigidbody>().AddForceAtPosition(GetComponent<Rigidbody>().velocity * GetComponent<Rigidbody>().mass, transform.position + new Vector3(0.0f,Random.Range(-20,20)/10.0f,0.0f));
					}
                }
                else rayhit.transform.parent.gameObject.GetComponent<VoxSystemChunkManager>().RemoveVoxelAoE(rayhit, RadiusExplosion);


                
                Destroy(gameObject);
            }//else  Debug.DrawRay(transform.position, rigidbody.velocity, Color.green);
            
        }
    }
}
