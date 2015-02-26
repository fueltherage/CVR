using UnityEngine;
using System.Collections;

public class GUI_EditorInfo : MonoBehaviour {

	// Use this for initialization
	bool shiftHeld;
    public Color BaseText = new Color(0.0f, 0.5f, 0.0f);
    public Color Remove = Color.red;
    public Color Add = Color.green;
	void Start () {
       

	}

	void OnGUI()
	{
        GUI.color = BaseText;
		GUI.Box (new Rect (10, 10, 230, 25), "Use mouse to control camera and aim");
		
		GUI.Box (new Rect (10, 70, 150, 25), "Left Shift Add/Remove");

		if (shiftHeld) {
			GUI.color = Remove;
			GUI.Box (new Rect (10, 100, 100, 25), "Remove");
		}
		else {
			GUI.color = Add;
			GUI.Box (new Rect (10, 100, 100, 25), "Add");
		}
					

	}
	
	// Update is called once per frame
	void Update () {
		shiftHeld = Input.GetKey(KeyCode.LeftShift);
		
	}
}
