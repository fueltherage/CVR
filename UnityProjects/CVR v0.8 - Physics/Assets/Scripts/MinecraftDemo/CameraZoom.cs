using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {

    public float ZoomSensitivity = 2.0f;
    public float StartingPos = 0;
	// Use this for initialization
	void Start () {
        transform.Translate(new Vector3(0.0f, 0.0f, StartingPos));
	}
	
	// Update is called once per frame
	void Update () {

        if(!GameState.gamePaused)
		transform.Translate(new Vector3(0,0,ZoomSensitivity*Input.GetAxis ("Mouse ScrollWheel")),Space.Self	);
	
	}
}
