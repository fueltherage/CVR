using UnityEngine;
using System.Collections;

public class PauseToggle : MonoBehaviour {

    public KeyCode PauseKey;
    public GameObject PauseMenu;
    public bool startPaused;

	// Use this for initialization
	void Start () {
        GameState.gamePaused = startPaused;
	}
	
	// Update is called once per frame
	void Update () {
       
        if(GameState.gamePaused)
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true); 
        }
        else
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }
        if(Input.GetKeyDown(PauseKey))
        {
            GameState.gamePaused = !GameState.gamePaused;            
        }	
	}
}
