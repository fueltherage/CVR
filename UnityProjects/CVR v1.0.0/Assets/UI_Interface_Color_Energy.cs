using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Interface_Color_Energy : MonoBehaviour {

    public Color FullEnergy;
    public Color NoEnergy;
	// Use this for initialization
    Image thisImage;
    EnergyManager eng;
	void Start () {
        eng = GameObject.FindGameObjectWithTag("Player").GetComponent<EnergyManager>();
        thisImage = GetComponent<Image>();
        thisImage.color = FullEnergy;
        current = eng.CurrentEnergy / eng.maxEnergy;
	}
    float current;
	// Update is called once per frame
	void Update () {
        float percent = eng.CurrentEnergy / eng.maxEnergy;
        current = Mathf.Lerp(current, percent, 0.2f);
        Color col = new Color();
        col.r = Mathf.Lerp(NoEnergy.r, FullEnergy.r, current);
        col.b = Mathf.Lerp(NoEnergy.b, FullEnergy.b, current);
        col.g = Mathf.Lerp(NoEnergy.g, FullEnergy.g, current);
        col.a = Mathf.Lerp(NoEnergy.a, FullEnergy.a, current);
        thisImage.color = col;
	}
}
