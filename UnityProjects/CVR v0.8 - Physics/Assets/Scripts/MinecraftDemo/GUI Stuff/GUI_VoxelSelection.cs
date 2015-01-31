using UnityEngine;
using System.Collections;

public class GUI_VoxelSelection : MonoBehaviour {

	// Use this for initialization

    //public Color SelectedColor;
    public int boxWidth = 25;
    public int boxHeight = 25;
	int numberOfBoxes = 5;
   

	int currentSelectedBox=1;

    //CameraControls cc;

//	void OnGUI()
//	{
//        for (int i = 1; i < numberOfBoxes; i++ )
//        {
//            if (currentSelectedBox == i)
//            { GUI.color = Color.yellow; }
//            else GUI.color = Color.black;
//
//		    GUI.Box (new Rect (Screen.width*i/numberOfBoxes, Screen.height - Screen.height/10, boxWidth, boxHeight), i.ToString());
//        }
//       
//	}
	void Start () {
        //cc = CameraOrigin.GetComponent<CameraControls>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Alpha1)&&currentSelectedBox != 1){currentSelectedBox =1; VoxelEvents.VoxelSwitched(currentSelectedBox);}
		if(Input.GetKeyDown (KeyCode.Alpha2)&&currentSelectedBox != 2){currentSelectedBox =2; VoxelEvents.VoxelSwitched(currentSelectedBox);}
		if(Input.GetKeyDown (KeyCode.Alpha3)&&currentSelectedBox != 3){currentSelectedBox =3; VoxelEvents.VoxelSwitched(currentSelectedBox);}
		if(Input.GetKeyDown (KeyCode.Alpha4)&&currentSelectedBox != 4){currentSelectedBox =4; VoxelEvents.VoxelSwitched(currentSelectedBox);}
        //if (Input.GetKeyDown(KeyCode.Space)) cc.CenterCamera();
	}
}
