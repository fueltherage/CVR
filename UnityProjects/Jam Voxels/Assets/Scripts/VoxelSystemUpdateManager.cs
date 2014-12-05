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
	// Use this for initialization
	void Start () {
		if(PlayerPos == null) PlayerPos = this.gameObject;	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void QueueChunkForUpdate(ref VoxelSystemChunkGreedy _chunk)
	{
		Vector3 difference = PlayerPos.transform.position - _chunk.transform.position;
		if(difference.magnitude < P1Distance)
		{
			VoxelThreads.QueueVoxMeshUpdate(ref _chunk,1);
		}
		else if(difference.magnitude < P2Distance)
		{
			VoxelThreads.QueueVoxMeshUpdate(ref _chunk, 2);
		}
		else
		{
			VoxelThreads.QueueVoxMeshUpdate(ref _chunk,3);
		}

	}
}
