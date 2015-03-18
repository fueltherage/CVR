using UnityEngine;
using System.Collections;

public class VoxelSystemUpdateManager : MonoBehaviour {

	//Notes 
	/*
	 * This class handles queueing chunks for updating
	 * Their priority level is based on how far they're from the player
	 */

	public GameObject PlayerPos;
	public float P1Distance = 10;
	public float P2Distance = 20;
	public float P3Distance = 30;
	Rigidbody rb;

	VoxelThreads vt;
	// Use this for initialization
	void Start () {
		vt = VoxelThreads.Current;
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void QueueChunkForUpdate(ref VoxelSystemChunkGreedy _chunk)
	{
		if(!_chunk.queuedForUpdate)
		{
			if(rb!= null)rb.WakeUp();

			Vector3 difference = PlayerPos.transform.position - _chunk.transform.position;

			if(difference.magnitude < P1Distance)
			{
                VoxelThreads.Current.QueueVoxMeshUpdate(ref _chunk,1);
			}
			else if(difference.magnitude < P2Distance)
			{
                VoxelThreads.Current.QueueVoxMeshUpdate(ref _chunk, 2);
			}
			else
			{
                VoxelThreads.Current.QueueVoxMeshUpdate(ref _chunk, 3);
			}	
		}
	}

	public void QueueChunkForUpdate(ref VoxelSystemChunkGreedy _chunk, System.Action up, System.Action gen)
	{
        if(!_chunk.queuedForUpdate)
        {
			if(rb!= null)rb.WakeUp();

    		Vector3 difference = PlayerPos.transform.position - _chunk.transform.position;

    		if(difference.magnitude < P1Distance)
    		{
    			vt.QueueVoxMeshUpdate(ref _chunk,up,gen,1);
    		}
    		else if(difference.magnitude < P2Distance)
    		{
    			vt.QueueVoxMeshUpdate(ref _chunk,up,gen, 2);
    		}
    		else
    		{
    			vt.QueueVoxMeshUpdate(ref _chunk,up,gen, 3);
    		}

        }
	}
}
