using UnityEngine;
using System.Collections;
/// <summary>
/// Oculus Toggle 
/// Switches the Active state of the first child
/// </summary>
public class OculusToggle : MonoBehaviour {

	public bool EnableWhenOculusIsOn = false;
	public int numChildren =0;
	Transform[] child;

	// Use this for initialization
	void Start () {
		child = new Transform[numChildren];
		for (int i = 0; i < numChildren; i++) {
			child[i] = transform.GetChild(i);			
		}
		SetChildren( EnableWhenOculusIsOn == GameState.OculusEnabled);
		GameState.OculusModeUpdater += OculusStateSwitched;
	}
	void OculusStateSwitched(bool enabled)
	{
		if(EnableWhenOculusIsOn)
		{
			if(enabled)
			{

				SetChildren(true);
			}else
			{
				SetChildren(false);
			}
		}
		else
		{
			if(enabled)
			{
				SetChildren(false);
			}else
			{
				SetChildren(true);
			}

		}
	}
	void SetChildren(bool active)
	{
		for (int i = 0; i < numChildren; i++) {
			child[i].gameObject.SetActive(active);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
