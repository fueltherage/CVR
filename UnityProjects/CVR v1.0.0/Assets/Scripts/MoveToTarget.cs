using UnityEngine;
using System.Collections;

public class MoveToTarget : MonoBehaviour {

    public enum MovementType { Transform, WorldPos };
    
    public MovementType moveType = MovementType.Transform;
    public Transform transTarget;
    public Vector3 vecTarget;
    public float force =0;
	public static float forceMultiplier = 1.0f;
    public bool moving= false;

    SaveLoadVoxels slv;
    Rigidbody rb;
	// Use this for initialization
	void Start () {
        slv = GetComponent<SaveLoadVoxels>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
       if(moving)
       {

            if (moveType == MovementType.Transform)
            {
				if(transTarget != null)
				{
	                Vector3 direction = transTarget.position - this.transform.position;
					rb.AddForce(direction.normalized * force * forceMultiplier);
				}
            }
            else
            {
                Vector3 direction = vecTarget - this.transform.position;
				rb.AddForce(direction.normalized * force * forceMultiplier);
            }
            //rb.AddTorque(RotationAxis * rotationForce);
            
        }  
	
	}
}
