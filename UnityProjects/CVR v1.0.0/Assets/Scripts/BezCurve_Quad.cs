using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class BezCurve_Quad : MonoBehaviour {

    public Transform P0;
    public Transform P1;
    public Transform P2;
    public int numPoints = 3;
    LineRenderer line;
    public float step;
    
    
    public Vector3[] verts;
    bool init = false;
    public Vector3 At(float t)
    {
        Vector3 P01 = P0.position + t * (P1.position - P0.position);
        Vector3 P12 = P1.position + t * (P2.position - P1.position);
        return P01 + t * (P12 - P01);
        //return (1.0f - t*t)*P01 + t * P12;
    }
    void Start()
    {        
        line = GetComponent<LineRenderer>();
    }
    public void UpdateLine()
    {
        P1v = P1.position;
        P2v = P2.position;
        P0v = P0.position;
        verts = new Vector3[numPoints];
        step = 1.0f / (numPoints + 1);
        line.SetVertexCount(numPoints + 2);
        line.SetPosition(0, P0.position);
        line.SetPosition(numPoints + 1, P2.position);
        for (int i = 1; i <= numPoints; i++)
        {
            verts[i - 1] = At(step * i);
            line.SetPosition(i, verts[i - 1]);
        }
    }
    Vector3 P0v, P1v, P2v;

    void Update()
    {

        if (!init && Application.isPlaying)
        {
            if (P1v != P1.position || P2v != P2.position || P0v != P0.position)
            UpdateLine();
            init = true;
            Destroy(this);
        }
        if (!Application.isPlaying)
            UpdateLine();
    }
}
