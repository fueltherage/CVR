using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxelChunkEmpty: VoxelChunk {
	//Notes
	/* 
	 * 
	*/
	public VoxelShell emptyShell;
	// Use this for initialization
	void Start () {

	}
	public override void Init (){
		//Shift the game oject over so the center is always the origin.
		//thisChunk = gameObject.GetComponent<VoxelChunkEmpty>();
		//this.transform.Translate (-VoxelSpacing / 2.0f, -VoxelSpacing / 2.0f, -VoxelSpacing / 2.0f);

		emptyShell = new VoxelShell();
		emptyShell.filled = false;
		emptyShell.locked = true;

		//Initializing Voxel/VoxelShell array
        //blocks = new VoxelShell[XSize,YSize,ZSize];
        //for (int x =0; x < XSize; x++){
        //    for (int y =0; y < YSize; y++){
        //        for (int z =0; z < ZSize; z++){
        //            if(x == 0 || x == XSize -1 ||
        //               y == 0 || y == YSize -1 ||
        //               z == 0 || z == ZSize -1)
        //            {
        //                blocks[x,y,z] = emptyShell;
        //            }
        //        }
        //    }
        //}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public override void UpdateMesh()
	{

	}
}
