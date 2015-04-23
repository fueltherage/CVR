using UnityEngine;
using System.Collections;

public class CellStats : MonoBehaviour {

    public float health = 100.0f;
    public float infectionPercent = 0.0f;
    public float atp_DropedOnDeath = 10.0f;
    public float threat = 0;
    public bool Viral = false;
    float elapsedTime = 0;
    float infectionDeathTime = 20.0f;
    public GameObject VirusOnDeath;
    public Transform VirusLoc;
	public bool passive = false;
    // Use this for initialization

	void Start () {
        if (Viral) threat = 100;
        infectionDeathTime += Random.Range(10.0f, 20.0f);
        fw = GetComponent<FollowWaypoints>();
        tag = this.gameObject.tag;
	
	}
    GameObject g;
    FollowWaypoints temp;
    FollowWaypoints fw;
    string tag;
	// Update is called once per frame
	void Update () {

        Profiler.BeginSample("CellStats Update");
        if (tag == "Infectable")
        {
            if (Viral)
            {
                Profiler.BeginSample("Time Update");
                elapsedTime += Time.deltaTime;
                infectionPercent = 100 * (elapsedTime/infectionDeathTime);
                Profiler.EndSample();
                if (infectionPercent >= 100)
                {
                    Profiler.BeginSample("Temp VarCreation");
                    
                    

                    Profiler.EndSample();
                    Profiler.BeginSample("Virus Creation");
                    for (int i = 0; i < 5; i++)
                    {
                        g = Instantiate(VirusOnDeath, this.transform.position, Quaternion.AngleAxis(Random.Range(0.0f, 360.0f),new Vector3(Random.Range(-1.0f, 1.0f),Random.Range(-1.0f, 1.0f),Random.Range(-1.0f, 1.0f))))  as GameObject;
                        g.transform.parent = VirusLoc;
                        temp = g.GetComponent<FollowWaypoints>();
                        temp.StartingWaypoint = fw.currentTarget;
                        
                    }
                    gameObject.SetActive(false);
					Destroy(gameObject,10.0f);
                    Profiler.EndSample();
                }
            }
        }

		//Return to pool
        if (health <= 0) 
		{
			if(Viral) GameState.VirusCount--;
			if(gameObject.tag == "Player")
			{
				GameOverFadeIn.FadeIn();
			}else 
			{
				this.gameObject.SetActive(false);
				Destroy(gameObject,10.0f);
			}
		}
        Profiler.EndSample();

		if(!Viral) threat -= Mathf.Min(0, threat - Time.deltaTime);
	
	}
    public void DealDamage(float _damage)
    {
        health -= _damage;
        if (health < 0) health = 0;
    }
}
