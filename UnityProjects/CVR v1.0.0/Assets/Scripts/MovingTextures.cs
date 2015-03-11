using UnityEngine;
using System.Collections;

public class MovingTextures : MonoBehaviour {
	public Vector2 scrollSpeed = Vector2.zero;
	Vector2 offSet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		offSet = new Vector2 (Time.time * scrollSpeed.x, Time.time * scrollSpeed.y);
		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offSet);
		GetComponent<Renderer>().material.SetTextureOffset ("_BumpMap", offSet);
		GetComponent<Renderer>().material.SetTextureOffset ("_Detail", offSet);
		GetComponent<Renderer>().material.SetTextureOffset ("_Illum", offSet);
	
	}
}
