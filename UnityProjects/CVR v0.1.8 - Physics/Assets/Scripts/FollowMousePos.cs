using UnityEngine;
using System.Collections;

public class FollowMousePos : MonoBehaviour {

	// Use this for initialization


	public bool ControllerInput = true;
	void Start () {
	
	}

	Vector3 currentMousePosOnScreen;
	Vector3 currentMousePosWorld;
	Vector3 centerScreen;
	float difference;
	Vector3 objectCurrentDirection;
	float rotationSpeed = 200f;
	float mouseAngleInRadians;	
	float thisAngle;
	float differenceAngle;
			
	// Update is called once per frame
	float UnitToRadian(Vector3 unit)
	{
		return Mathf.Atan2(unit.y,unit.x);
	}

	void Update () {

		if(ControllerInput)
			currentMousePosOnScreen = new Vector2(Screen.width/2+Input.GetAxis("RightStickX")*Screen.width/2, Screen.height/2+Input.GetAxis("RightStickY")*Screen.width/2);			                                  
		else 
		currentMousePosOnScreen = Input.mousePosition;	


		centerScreen.x = Screen.width/2.0f;
		centerScreen.y = Screen.height/2.0f;
		centerScreen.z = 0;

		currentMousePosWorld = currentMousePosOnScreen - centerScreen; 
	
		mouseAngleInRadians = UnitToRadian(currentMousePosWorld);
		mouseAngleInRadians = (mouseAngleInRadians < 0) ? mouseAngleInRadians + 2 * Mathf.PI : mouseAngleInRadians;

		thisAngle = UnitToRadian(new Vector2(this.transform.forward.x,this.transform.forward.z));
		thisAngle = (thisAngle < 0) ? thisAngle + 2 * Mathf.PI : thisAngle;

		difference = mouseAngleInRadians - thisAngle;

		//Debug.Log(UnitToRadian(currentMousePosWorld));

		if(difference > Mathf.PI || difference < -Mathf.PI)
		{
			difference = -difference;
		}
		if(Mathf.Abs(difference)>rotationSpeed/(180/Mathf.PI)*Time.deltaTime)
		{
			if(difference > 0)
			{

				this.transform.Rotate(0,-rotationSpeed*Time.deltaTime,0);
			}else 
			{
				this.transform.Rotate(0,rotationSpeed*Time.deltaTime,0);
			}
		}else
		{
			//Debug.Log("difference"+difference/(Mathf.PI/180));
			this.transform.Rotate(new Vector3(0,1,0),-difference/(Mathf.PI/180));
		}



	}
}
