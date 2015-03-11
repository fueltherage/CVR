using UnityEngine;
using System.Collections;

public class TextureOffsetZoom : MonoBehaviour {
	public float AnimationTime = 5.0f;
	public float ZoomLevel = 1.0f;
	public float minZoom = 1.0f;
	public Vector2 offset;
	Vector2 originalOffset;
	// Use this for initialization
	void Start () {
		originalOffset = GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		offset = new Vector2 (originalOffset.x + ZoomLevel * Mathf.Sin(Time.time/ AnimationTime) + minZoom, originalOffset.y + ZoomLevel * Mathf.Sin(Time.time/ AnimationTime) + minZoom);

		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", -offset/2.0f);
		GetComponent<Renderer>().material.SetTextureOffset ("_BumpMap", -offset/2.0f);
		GetComponent<Renderer>().material.SetTextureOffset ("_Illum", -offset/2.0f);
		GetComponent<Renderer>().material.SetTextureScale ("_MainTex", offset);
		GetComponent<Renderer>().material.SetTextureScale ("_BumpMap", offset);
		GetComponent<Renderer>().material.SetTextureScale ("_Illum", offset);
	}
}
