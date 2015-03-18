using UnityEngine;
using System.Collections;

public class SphereRoom : MonoBehaviour {

    VoxelSystemGreedy vs;
    VoxSystemChunkManager vcm;
    bool init = false;
    Node node;
    public int radius;
    // Use this for initialization
    void Start()
    {
        node = GetComponent<Node>();
        vs = node.vs;
        vcm = node.vs.GetComponent<VoxSystemChunkManager>();
        node.s_updateCalls += DoRoom;
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
        vcm.AddVoxelAoE(this.transform.position, radius+(int)(2*vs.VoxelSpacing),4, true);
        vcm.RemoveVoxelAoE(this.transform.position, radius, true);
    }
}
