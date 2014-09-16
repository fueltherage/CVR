using UnityEngine;
using System.Collections;

public class GUI_VoxelSelection : MonoBehaviour {

	// Use this for initialization
	GUIStyle guiText;
	int boxWidth = 25;
	int boxHeight = 25;
	int numberOfBoxes = 5;

	int currentSelectedBox=1;

	void OnGUI()
	{
		if(currentSelectedBox==1)GUI.color = Color.yellow;
		else GUI.color = Color.black;
		GUI.Box (new Rect (Screen.width*1/numberOfBoxes, Screen.height - Screen.height/10, boxWidth, boxHeight), "1");

		if(currentSelectedBox==2)GUI.color = Color.yellow;
		else GUI.color = Color.black;
		GUI.Box (new Rect (Screen.width*2/numberOfBoxes, Screen.height - Screen.height/10, boxWidth, boxHeight), "2");

		if(currentSelectedBox==3)GUI.color = Color.yellow;
		else GUI.color = Color.black;
		GUI.Box (new Rect (Screen.width*3/numberOfBoxes, Screen.height - Screen.height/10, boxWidth, boxHeight), "3");

		if(currentSelectedBox==4)GUI.color = Color.yellow;
		else GUI.color = Color.black;
		GUI.Box (new Rect (Screen.width*4/numberOfBoxes, Screen.height - Screen.height/10, boxWidth, boxHeight), "4");
	}
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Alpha1)&&currentSelectedBox != 1){currentSelectedBox =1; VoxelEvents.VoxelSwitched(currentSelectedBox);}
		if(Input.GetKeyDown (KeyCode.Alpha2)&&currentSelectedBox != 2){currentSelectedBox =2; VoxelEvents.VoxelSwitched(currentSelectedBox);}
		if(Input.GetKeyDown (KeyCode.Alpha3)&&currentSelectedBox != 3){currentSelectedBox =3; VoxelEvents.VoxelSwitched(currentSelectedBox);}
		if(Input.GetKeyDown (KeyCode.Alpha4)&&currentSelectedBox != 4){currentSelectedBox =4; VoxelEvents.VoxelSwitched(currentSelectedBox);}
	}
}
