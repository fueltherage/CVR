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
		vs = GetComponent<VoxelSystemGreedy>();
	}
	
	// Update is called once per frame
    public void CalcIntertia()
    {
        if(rb != null)
        {
			float SystemMass=0;

			int x,y,z;
			Inertia = Vector3.zero;
			CenterOfMass=Vector3.zero;
			for(x = 0; x < vs.XSize; ++x)			
				for(y = 0; y < vs.YSize; ++y)				
					for(z = 0; z < vs.ZSize; ++z)
				{
					SystemMass += vs.chunks_vcs[x,y,z].chunkMass;
				}
			if(SystemMass!=0)
				for(x = 0; x < vs.XSize; ++x)			
					for(y = 0; y < vs.YSize; ++y)				
						for(z = 0; z < vs.ZSize; ++z)
					{
						CenterOfMass += (vs.chunks_vcs[x,y,z].centerOfMass + vs.chunks_vcs[x,y,z].transform.localPosition ) * vs.chunks_vcs[x,y,z].chunkMass / SystemMass;
					}
			else CenterOfMass = Vector3.zero;

			for(x = 0; x < vs.XSize; ++x)			
				for(y = 0; y < vs.YSize; ++y)				
					for(z = 0; z < vs.ZSize; ++z)
				{
					Vector3 diff = (vs.chunks_vcs[x,y,z].centerOfMass + vs.chunks_vcs[x,y,z].transform.localPosition ) - CenterOfMass;

					Inertia.x += diff.x * diff.x * vs.chunks_vcs[x,y,z].chunkMass;
					Inertia.y += diff.y * diff.y * vs.chunks_vcs[x,y,z].chunkMass;
					Inertia.z += diff.z * diff.z * vs.chunks_vcs[x,y,z].chunkMass;
				}

			//transform.localPosition = new Vector3(transform.localPosition.x, , transform.localPosition.z);

			rb.centerOfMass = CenterOfMass;
			if(Inertia.x<0.01||Inertia.y<0.01||Inertia.z<0.01){Inertia = new Vector3(1,1,1);}
			rb.inertiaTensor = Inertia;
			rb.mass = SystemMass;
        }
    }
	void Update () {


	}
}
