using UnityEngine;
using System.Collections;

public class CarveAlongBez_Quad : MonoBehaviour {

    public VoxelSystemGreedy Environment;
	// Use this for initialization
    public int radius = 4;
    BezCurve_Quad line;
    VoxSystemChunkManager vscm;
	void Start () {
        line = GetComponent<BezCurve_Quad>();
        vscm = Environment.GetComponent<VoxSystemChunkManager>();
        
	}
    public void Carve()
    {
        for (int i = 0; i < line.verts.Length; i++)
        {
            vscm.RemoveVoxelAoE(line.verts[i], radius, false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) Carve();
	}
}
