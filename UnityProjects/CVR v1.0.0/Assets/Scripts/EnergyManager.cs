using UnityEngine;
using System.Collections;

public class EnergyManager : MonoBehaviour {

    public float maxEnergy = 100f;
    public float energyRegen = 1.0f;
    private float currentEnergy;

    public float energy;

    public float CurrentEnergy
    {
        get { return currentEnergy; }        
    }  

	// Use this for initialization
	void Start () {
        currentEnergy = maxEnergy;
	}
	
	// Update is called once per frame
	void Update () {
        float regen =  energyRegen * Time.deltaTime;
        if (currentEnergy + regen < maxEnergy) currentEnergy += energyRegen * Time.deltaTime;
        else if (currentEnergy <= maxEnergy) currentEnergy = maxEnergy;
        energy = currentEnergy;
	}
    public bool UseEnergy(float _energy)
    {
        if (_energy <= currentEnergy)
        {
            currentEnergy -= _energy;
            return true;
        }
        else return false;
    }
    public void GainEnergy(float _energy)
    {
        currentEnergy = Mathf.Max(maxEnergy, currentEnergy + _energy);
    }
}
