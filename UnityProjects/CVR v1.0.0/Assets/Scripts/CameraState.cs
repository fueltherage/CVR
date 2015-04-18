using UnityEngine;
using System.Collections;


public class CameraState : MonoBehaviour {

	public enum CameraView { EditorView, GamePlayView};

	public Transform OculusCenter;
	public Camera OculusLeftEye;
	public Camera OculusRightEye;
	public Camera NormalCamera;
	public static Camera CurrentCamera;
	Camera OcRE;
	Camera Norm;

	int viewSwapCount=0;
	CameraView current;
	CameraView past;
 
	// Use this for initialization
	void Start () {
		GameState.OculusModeUpdater += OculusStateSwap;
		CurrentCamera = NormalCamera;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(GameState.OculusEnabled)
		OculusRightEye.backgroundColor = OculusLeftEye.backgroundColor;
	}
	void OculusStateSwap(bool enabled)
	{
		if(enabled)
		{
			CurrentCamera = OculusLeftEye;
		}
		else 
		{
			CurrentCamera = NormalCamera;
		}	
	}


}
