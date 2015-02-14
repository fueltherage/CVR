using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHider : MonoBehaviour {

	[SerializeField]
	private Button MyButton = null; // assign in the editor
	PanelSlider ps;
	void Start()
	{
		ps = GetComponent<PanelSlider>();
		MyButton.onClick.AddListener(() =>  ps.Toggle());  
	}
}
