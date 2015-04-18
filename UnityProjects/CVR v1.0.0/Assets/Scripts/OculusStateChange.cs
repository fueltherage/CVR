using UnityEngine;
using System.Collections;

public class OculusStateChange : MonoBehaviour {

	public bool StartWithOculusOn = false;
	public KeyCode OculusStateSwapButton; 
	// Use this for initialization
	void Start () {
		GameState.OculusEnabled = StartWithOculusOn;
		GameState.SwitchOculusMode(StartWithOculusOn);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(OculusStateSwapButton))
		{
			Debug.Log("Oculus Swap");
			GameState.OculusEnabled = !GameState.OculusEnabled;
			GameState.SwitchOculusMode(GameState.OculusEnabled);
		}	
	}
}
