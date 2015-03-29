using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_BrushRadiusText : MonoBehaviour {
	public MouseSelectionTool_Greedy BrushTool;
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		text.text = BrushTool.radius.ToString();
		
	}
	
	// Update is called once per frame
	void Update () {
		text.text = BrushTool.radius.ToString();
	
	}
}
