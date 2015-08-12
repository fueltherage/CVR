using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class InertiaCalculator : MonoBehaviour {

	public Vector3 Inertia = new Vector3(1,1,1);
	public Vector3 CenterOfMass = new Vector3(0.0f,0.0f,0.0f);
	VoxelSystemGreedy vs;
	Rigidbody rb;

	// Use this for initialization

	void Awake () {
        
		rb = GetComponent<Rigidbody>();
		//rb.AddTorque(new Vector3(0,1,0));
        rb.centerOfMass = CenterOfMass;
        rb.inertiaTensor = Inertia;
		vs = GetComponent<VoxelSystemGreedy>();
	}
	
	// Update is called once per frame
    public void CalcIntertia()
    {
        if(rb != null)
        {
           
                float SystemMass = 0;

                int x, y, z;
                Inertia = Vector3.zero;
                CenterOfMass = Vector3.zero;
                for (x = 0; x < vs.XSize; ++x)
                    for (y = 0; y < vs.YSize; ++y)
                        for (z = 0; z < vs.ZSize; ++z)
                        {
                            SystemMass += vs.chunks_vcs[x, y, z].chunkMass;
                        }
                if (SystemMass != 0)
                    for (x = 0; x < vs.XSize; ++x)
                        for (y = 0; y < vs.YSize; ++y)
                            for (z = 0; z < vs.ZSize; ++z)
                            {
                                CenterOfMass += (vs.chunks_vcs[x, y, z].centerOfMass + vs.chunks_vcs[x, y, z].transform.localPosition) * vs.chunks_vcs[x, y, z].chunkMass / SystemMass;
                            }
                else CenterOfMass = Vector3.zero;
                if (vs.XSize > 1 && vs.YSize > 1 && vs.ZSize > 1)
                {
                    for (x = 0; x < vs.XSize; ++x)
                        for (y = 0; y < vs.YSize; ++y)
                            for (z = 0; z < vs.ZSize; ++z)
                            {
                                Vector3 diff = (vs.chunks_vcs[x, y, z].centerOfMass + vs.chunks_vcs[x, y, z].transform.localPosition) - CenterOfMass;

                                Inertia.x += diff.x * diff.x * vs.chunks_vcs[x, y, z].chunkMass;
                                Inertia.y += diff.y * diff.y * vs.chunks_vcs[x, y, z].chunkMass;
                                Inertia.z += diff.z * diff.z * vs.chunks_vcs[x, y, z].chunkMass;
                            }
                } 
				else 
				{ 
				  Inertia = new Vector3(1, 1, 1);
				  CenterOfMass = Vector2.zero;
				}
                //transform.localPosition = new Vector3(transform.localPosition.x, , transform.localPosition.z);


                CenterOfMass = Vector2.zero;
                rb.centerOfMass = CenterOfMass;
               
                rb.inertiaTensor = Inertia;
                rb.mass = SystemMass;
            
        }
    }
	void Update () {


	}
}
