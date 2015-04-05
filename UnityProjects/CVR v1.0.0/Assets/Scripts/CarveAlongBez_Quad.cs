using UnityEngine;
using System.Collections;

public class CarveAlongBez_Quad : MonoBehaviour {

    VoxelSystemGreedy vs;
	// Use this for initialization
    public int radius = 4;
    public bool pureVoxel = true;
    BezCurve_Quad line;
    VoxSystemChunkManager vscm;
    Node node;

	void Start () {
        line = GetComponent<BezCurve_Quad>();       
        node = GetComponent<Node>();
        vs = node.vs;
        vscm = node.vs.GetComponent<VoxSystemChunkManager>();
        node.second_updateCalls += Carve;
        
	}
    public void Carve()
    {
        for (int i = 0; i < line.verts.Length; i++)
        {
            vscm.QuickRemoveVoxelAoE(line.verts[i], radius, pureVoxel);
            //vscm.RemoveVoxelAoE(line.verts[i], radius, true);
        }
    }

    byte b;
    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.C)) Carve();
	}
}
