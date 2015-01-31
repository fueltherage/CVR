using UnityEngine;
using System.Collections;

public class MicReader : MonoBehaviour {

	// Use this for initialization
	float elapsedTime =0;
	LineRenderer line;
	float[] data;

	void Start () {
		line = GetComponent<LineRenderer>();

	}
	
	// Update is called once per frame
	void Update () {
		if(audio.clip!=null)
		{
			data = new float[audio.clip.samples * audio.clip.channels];
			audio.clip.GetData(data,1);
			for(int i=0; i<data.Length;i++)
			{
				line.SetPosition(i, new Vector3(i,data[i]*100));
			}
		}

	
	}
}
