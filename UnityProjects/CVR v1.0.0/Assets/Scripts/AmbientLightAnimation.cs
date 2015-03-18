using UnityEngine;
using System.Collections;

public class AmbientLightAnimation : MonoBehaviour {
	public float Amplitude = 0.5f;
	public float animationTime = 2.0f;
	public float MinIntensity = 1.0f;
	public float value;
	public Color FogColorMax;
	public Color FogColorMin;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		value = Amplitude *  Mathf.Sin(Time.timeSinceLevelLoad / animationTime) + MinIntensity;
		Color color = new Color();
		color.r = Mathf.Lerp (FogColorMin.r, FogColorMax.r, value);
		color.b = Mathf.Lerp (FogColorMin.b, FogColorMax.b, value);
		color.g = Mathf.Lerp (FogColorMin.g, FogColorMax.g, value);
		//color.a = Mathf.Lerp (FogColorMax.r, FogColorMin.r, value);
		RenderSettings.fogColor = color;
		CameraState.CurrentCamera.backgroundColor = color;
		RenderSettings.ambientIntensity = value;
	}
}
