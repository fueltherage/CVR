using UnityEngine;
using System.Collections;

public class SphereRoom : MonoBehaviour {

    VoxelSystemGreedy vs;
    VoxSystemChunkManager vcm;
    public int type = 4;
    bool init = false;
    Node node;
    public int radius;
    // Use this for initialization
    void Start()
    {
        node = GetComponent<Node>();
        vs = node.vs;
        vcm = node.vs.GetComponent<VoxSystemChunkManager>();
        node.second_updateCalls += DoRoom;
    }

    // Update is called once per frame
    void Update()
    {
        //		if(!init)
        //		{
        //			if(vs.Initialized)
        //			{
        //				RemoveArea();
        //				init=true;
        //			}
        //		}
    }
    void DoRoom()
    {
        vcm.QuickAddVoxelAoE(this.transform.position, radius+(int)(2*vs.VoxelSpacing), type, true);
        //vcm.AddVoxelAoE(this.transform.position, radius + (int)(2 * vs.VoxelSpacing), type, true);
        //vcm.RemoveVoxelAoE(this.transform.position, radius, true);
        vcm.QuickRemoveVoxelAoE(this.transform.position, radius, true);
    }
}
