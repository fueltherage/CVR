using UnityEngine;
using System.Collections;

public class PanelSlider : MonoBehaviour {

	public Vector3 OnScreenPos;
	public Vector3 OffScreenPos;
	public float AnimationTime;
	public AnimationCurve speedOverTime;

	float ThreshHold = 2f;
	bool isAnimating = false;
	RectTransform trans;

	enum State { OffScreen, OnScreen}
	enum AniState { GoingOffScreen, GoingOnScreen }
	State current;
	AniState aniCurrent;
	float elapsedTime;
	Vector3 pos;
	public void Toggle()
	{
		elapsedTime=0;
		if(current == State.OffScreen)
		{
			current = State.OffScreen;
			GoOnScreen();
		}else
		{
			current = State.OnScreen;
			GoOffScreen();
		}
	}

	public void GoOffScreen()
	{
		isAnimating = true;
		aniCurrent = AniState.GoingOffScreen;

		pos.x = Mathf.Lerp(transform.position.x, OffScreenPos.x, speedOverTime.Evaluate(elapsedTime)/10.0f);
		pos.y = Mathf.Lerp(transform.position.y, OffScreenPos.y, speedOverTime.Evaluate(elapsedTime)/10.0f);
		pos.z = Mathf.Lerp(transform.position.z, OffScreenPos.z, speedOverTime.Evaluate(elapsedTime)/10.0f);
		trans.position = pos;
	}
	public void GoOnScreen()
	{
		isAnimating = true;
		aniCurrent = AniState.GoingOnScreen;

		pos.x = Mathf.Lerp(transform.position.x, OnScreenPos.x, speedOverTime.Evaluate(elapsedTime)/10.0f);
		pos.y = Mathf.Lerp(transform.position.y, OnScreenPos.y, speedOverTime.Evaluate(elapsedTime)/10.0f);
		pos.z = Mathf.Lerp(transform.position.z, OnScreenPos.z, speedOverTime.Evaluate(elapsedTime)/10.0f);
		trans.position = pos;

	}

	void Start () {
		current = State.OnScreen;
		trans = GetComponent<RectTransform>();
		pos= new Vector3();
		OnScreenPos = trans.position;
		OffScreenPos = trans.position + OffScreenPos;
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime+=Time.deltaTime;
		if(isAnimating)
		{
			float dif;
			switch(aniCurrent)
			{
				case AniState.GoingOffScreen	:
					GoOffScreen();
					dif = (transform.position - OffScreenPos).magnitude;
					if(dif <= ThreshHold)
					{
						transform.position = OffScreenPos;
						current = State.OffScreen;
						Debug.Log("OffScreenOver");
						isAnimating = false;
					}
					break;

				case AniState.GoingOnScreen:
					GoOnScreen();
					dif = (transform.position - OnScreenPos).magnitude;
					if(dif <= ThreshHold)
					{
						transform.position = OnScreenPos;
						current = State.OnScreen;
						Debug.Log("OnScreenOver");
						isAnimating = false;
					}
					break;
			}
		}	
	}
}
