using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour {
    public float EnergyUpkeep = 1.0f;
    public KeyCode LightToggle = KeyCode.F;
    public float ThreshHold = 0.1f;
    public float flickerChange = 0.05f;
    public float maxLightIntensity = 1.5f;

    private IEnumerator coruFlicker;

    bool flickering = false;
    bool inUse = false;
    float[] transVals;
    int transValCount = 100;
    float lowPowerFade;
    EnergyManager e;
    Light light;
    int count = 0;

    private IEnumerator LightFlicker()
    {   
        while (true)
        {
            flickering = true;
            ++count;
            if (count >= transValCount) count = 0;
            //Debug.Log(transVals[count] * maxLightIntensity);
            light.intensity = transVals[count] * maxLightIntensity * (lowPowerFade / ThreshHold);
            yield return null; //new WaitForSeconds(flickerChange);
        }
    }


	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
        e = GameObject.FindGameObjectWithTag("Player").GetComponent<EnergyManager>();
        coruFlicker = LightFlicker();

        transVals = new float[transValCount];
        for (int i = 0; i < transValCount; i++)
        {
            transVals[i] = Mathf.PerlinNoise(i/2.0f, 0.0f);
        }          
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(LightToggle))
        {
            inUse = !inUse;
        }
        if (inUse)
        {
            light.enabled = true;
            lowPowerFade = e.CurrentEnergy / e.maxEnergy;
            if (!e.UseEnergy(EnergyUpkeep * Time.deltaTime))
            {
                inUse = false;
                return;
            }
            if (lowPowerFade < ThreshHold)
            {
                if (!flickering)
                    StartCoroutine(coruFlicker);
            }
            else
            {
                flickering = false;
                StopCoroutine(coruFlicker);
                light.intensity = maxLightIntensity;
            }
        }
        else
        {
            light.enabled = false;
            StopCoroutine(coruFlicker);
            flickering = false;
        }
	}
}
