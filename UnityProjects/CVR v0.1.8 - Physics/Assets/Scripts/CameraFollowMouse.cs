using UnityEngine;
using System.Collections;

public class CameraFollowMouse : MonoBehaviour {

	// Use this for initialization
	public bool ControllerInput = true;
	public GameObject Player;
	public float MaxCameraWanterDistance = 15f;
    public float YDisanceFromPlayer = 25f;
	Vector3 currentMousePosOnScreen;
	Vector3 currentMousePosWorld;
	Vector3 centerScreen;
	Vector3 CameraWorldPos;
	public float DeadZone = 10f;
	void Awake()
	{
		Application.targetFrameRate = 60;
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(ControllerInput)
		{
			currentMousePosOnScreen = new Vector2(Screen.width/2+Input.GetAxis("RightStickX")*Screen.width/2, Screen.height/2+Input.GetAxis("RightStickY")*Screen.width/2);
			Debug.Log(currentMousePosOnScreen);
		}			
		else currentMousePosOnScreen = Input.mousePosition;	
			
		//Debug.Log (currentMousePosOnScreen);
		centerScreen.x = Screen.width/2.0f;
		centerScreen.y = Screen.height/2.0f;
		centerScreen.z = 0;			
		currentMousePosWorld = currentMousePosOnScreen - centerScreen; 
		
		currentMousePosWorld = currentMousePosWorld/60f;
		if(currentMousePosWorld.magnitude<DeadZone)
		{
			currentMousePosWorld = Vector3.zero;
		}
		else{
			currentMousePosWorld = currentMousePosWorld.normalized * (currentMousePosWorld.magnitude - DeadZone);
		}

        if(currentMousePosWorld.magnitude>MaxCameraWanterDistance)
		{
			currentMousePosWorld = currentMousePosWorld.normalized*MaxCameraWanterDistance;
		}
		//CameraWorldPos = currentMousePosWorld + Player.transform.position;
		/*if((this.transform.position-(Player.transform.position+currentMousePosWorld)).magnitude > 0.01f)
		{
		this.transform.Translate(Mathf.Lerp(this.transform.position.x, Player.transform.position.x + currentMousePosWorld.x ,0.1f),
		                         Mathf.Lerp(this.transform.position.y, Player.transform.position.y+10f,0.1f),
		                         Mathf.Lerp(this.transform.position.z, Player.transform.position.z + currentMousePosWorld.y, 0.1f),Space.World);
		}*/


		this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x,(Player.transform.position.x+ currentMousePosWorld.x),0.1f),			
                                              (Player.transform.position.y+YDisanceFromPlayer),
											   Mathf.Lerp(this.transform.position.z,(Player.transform.position.z + currentMousePosWorld.y),0.1f));
		
	}
}

