using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UI_ATPEnergy : MonoBehaviour {

    public EnergyManager en;
    Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = ((int)en.energy).ToString() + "%";
	    
	}
}
