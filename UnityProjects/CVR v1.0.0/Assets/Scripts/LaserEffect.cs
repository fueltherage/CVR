using UnityEngine;
using System.Collections;

public class LaserEffect : MonoBehaviour {

    LineRenderer line;
    public float lineLife = 2.0f;
	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public IEnumerator lineTimer()
    {
        yield return new WaitForSeconds(lineLife);
        line.enabled = false;
    }   
    public void go(Vector3 Start, Vector3 End)
    {
        line.enabled = true;
        line.SetPosition(0, Start);
        line.SetPosition(1, End);
        StartCoroutine(lineTimer());
    }
}
