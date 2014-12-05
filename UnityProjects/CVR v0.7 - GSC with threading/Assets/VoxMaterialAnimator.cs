using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoxMaterialAnimator : MonoBehaviour {

	public AnimationCurve ColorChangeOverTime;
	VoxMaterialFactory factory;
	public float animationLoopTime = 1.0f; 
	public float delayTime = 0.0f;
	int numberOfMats;
	float elapsedTime;
	List<Color> colors = new List<Color>();

	// Use this for initialization
	void Start () {
		factory = GetComponent<VoxelSystemGreedy>().factory;
		numberOfMats = factory.VoxelMats.Count-1;
		for(int i = 1; i<=numberOfMats; i++)
			colors.Add(factory.VoxelMats[i].color);
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 1; i <= numberOfMats; i++) {
			Color color;
			color.a = colors[i-1].a * (Mathf.Sin( 4*i * animationLoopTime / numberOfMats + Time.time )+1)*0.5f;
			color.r = colors[i-1].r * (Mathf.Sin( 4*i * animationLoopTime / numberOfMats + Time.time )+1)*0.5f;
			color.g = colors[i-1].g * (Mathf.Sin( 4*i * animationLoopTime / numberOfMats + Time.time )+1)*0.5f;
			color.b = colors[i-1].b * (Mathf.Sin( 4*i * animationLoopTime / numberOfMats + Time.time )+1)*0.5f;
			factory.VoxelMats[i].color = color;
		}		
	}
	void OnDestroy()
	{
		for (int i = 1; i <= numberOfMats; i++) {		
			factory.VoxelMats[i].color = colors[i-1];
		}
	}
}
