using UnityEngine;
using System.Collections;

public class GUI_EditorInfo : MonoBehaviour {

	// Use this for initialization
	bool shiftHeld;
    public Color Remove = Color.red;
    public Color Add = Color.green;
	void Start () {
       

	}

	void OnGUI()
	{
        

		if (shiftHeld) {
			GUI.color = Remove;
			GUI.Box (new Rect (10, 10, 100, 25), "Remove");
		}
		else {
			GUI.color = Add;
			GUI.Box (new Rect (10, 10, 100, 25), "Add");
		}
					

	}
	
	// Update is called once per frame
	void Update () {
		shiftHeld = Input.GetKey(KeyCode.LeftShift);
		
	}
}
