using UnityEngine;
using System.Collections;

public class PauseDeltaTime : MonoBehaviour {

	public float PauseTime = 10.0f;
	public bool paused=false;
	float pauseStartTime=0;
	float pauseEnd;
	// Use this for initialization

	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P))
		{
			Pause();
		}
	}
	public void Pause()
	{
		if(!paused)
			StartCoroutine(PauseForTime());
		else pauseEnd += PauseTime;

	}
	IEnumerator PauseForTime()
	{
		pauseStartTime = Time.realtimeSinceStartup;
		pauseEnd = pauseStartTime + PauseTime;

		paused = true;
		new WaitForSeconds(PauseTime);
		Time.timeScale = 1;
		paused = false;

		yield return null;
	}
}
