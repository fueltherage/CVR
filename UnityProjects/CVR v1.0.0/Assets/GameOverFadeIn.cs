using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameOverFadeIn : MonoBehaviour {

	public Color FadeInColor;
	public float fadeTime = 0.1f;
	Image background;
	static bool fadingIn = false;
	static bool fadingOut = false;
	Text txt;
	Color col;
	bool txtMode = false;
	// Use this for initialization
	void Start () {
		background = GetComponent<Image>();
		if(background == null)
		{
			txt = GetComponent<Text>();
			txtMode = true;
		}
		col = FadeInColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(fadingIn)
		{
			col.a = Mathf.Lerp(col.a, 1.0f, fadeTime);
			if(!txtMode)
			background.color = col;
			else txt.color = col;
			if(col.a == 1.0f) fadingOut =false;
		}
		else if(fadingOut)
		{
			col.a = Mathf.Lerp(col.a, 0.0f, fadeTime);
			if(!txtMode)
			background.color = col;
			else txt.color = col;
			if(col.a == 0.0f) fadingOut =false;
		}
	}
	public static void FadeIn()
	{
		fadingIn = true;
		fadingOut = false;
	}
	
	public static void FadeOut()
	{
		fadingIn = false;
		fadingOut = true;
	}
}
