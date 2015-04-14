using UnityEngine;
using System.Collections;

public class MovingTextures : MonoBehaviour {
	public Vector2 scrollSpeed = Vector2.zero;
	Vector2 offSet;
    Renderer rend;
    
    bool lineMode = false;
    LineRenderer lineRend;
   
	// Use this for initialization
	void Start () {                  
        lineRend = GetComponent<LineRenderer>();
        lineMode = true;       

	}
    float elapsedTime=0;

	// Update is called once per frame
	void Update () {
        
        	
	
	}
}
