using UnityEngine;
using System.Collections;

public class GUI_VoxelSelection : MonoBehaviour {

	// Use this for initialization

    //public Color SelectedColor;
    public int boxWidth = 25;
    public int boxHeight = 25;
	public int numberOfBoxes = 6;
   

	int currentSelectedBox = 1;

    

	void OnGUI()
	{
        for (int i = 1; i <= numberOfBoxes; i++ )
        {
            if (currentSelectedBox == i)
            { GUI.color = Color.yellow; }
            else GUI.color = Color.black;

		    GUI.Box (new Rect (Screen.width*i/(numberOfBoxes+1), Screen.height - Screen.height/9, boxWidth, boxHeight), i.ToString());
        }
       
	}
	void Start () {
        //cc = CameraOrigin.GetComponent<CameraControls>();
        VoxelEvents.VoxelSwitched(currentSelectedBox);
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < numberOfBoxes; i++) {
			if(Input.GetKeyDown (KeyCode.Alpha1 + i)&&currentSelectedBox != i+1){currentSelectedBox =i+1; VoxelEvents.VoxelSwitched(currentSelectedBox);}
		}

        //if (Input.GetKeyDown(KeyCode.Space)) cc.CenterCamera();
	}
}
