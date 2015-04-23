using UnityEngine;
using System.Collections;

public class WhiteBloodCellBehaviour : MonoBehaviour {

   
    public enum WhiteBloodState { FollowWaypoints, Wandering, Seeking, Consuming }
    public WhiteBloodState StartingState;

    public LayerMask sphereCastMask;
    public WhiteBloodState current;

    static int _OverLapSphereRadius = 50;
    static float _ThinkRate = 3.0f;
    static float _consumeRadius = 10.0f;
    static float _Damage = 50.0f;


    GameObject Target;
    CellStats TargetStats;
    MoveToTarget mtt;
    float pastTime;
    FollowWaypoints FW; 

	void Start () {
        mtt = GetComponent<MoveToTarget>();
        pastTime = Time.timeSinceLevelLoad;
        FW = GetComponent<FollowWaypoints>();
        
	}
	
	// Update is called once per frame
	void Update () {

        //Movement Behaviour
        switch (current)
        {
            case WhiteBloodState.FollowWaypoints:
                {
                    if (FindTarget()) current = WhiteBloodState.Seeking;
                    else FollowWaypoint();
                    break;
                }
            case WhiteBloodState.Wandering:
                {
                    if (FindTarget()) current = WhiteBloodState.Seeking;
                    else Wander();
                    break;
                }
            case WhiteBloodState.Seeking:
                {
                    Seek();               
                    break;
                }
            case WhiteBloodState.Consuming:
                {
                    Consuming();
                    break;
                }
        }

	
	}
    bool FindTarget()
    {
        //If(CurrentTargetNotValid)
        if (pastTime + _ThinkRate < Time.timeSinceLevelLoad)
        {
            pastTime = Time.timeSinceLevelLoad;
            Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, _OverLapSphereRadius,sphereCastMask);
            float _threat = 0;
            int _tar = 0;
			bool playerTarget = false;
            if (cols.Length > 0)
            {
                
                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i].tag == "CellPrimCollider")
                    {
                        CellStats _stat = cols[i].transform.parent.parent.gameObject.GetComponent<CellStats>();
                        if (_stat != null)
                            if (_stat.threat > _threat && _stat.Viral)
                            {

								playerTarget = false;
                                _tar = i;
                                _threat = _stat.threat;
                            }
                    }
					if(cols[i].tag == "Player")
					{
						CellStats _stat = cols[i].transform.gameObject.GetComponent<CellStats>();
						if (_stat != null)
							if (_stat.threat > _threat && _stat.threat > 100.0f)
						{
							playerTarget = true;
							_tar = i;
							_threat = _stat.threat;
						}
					}
                }
                if (_threat > 0)
                {
					if(playerTarget)
					{
						Target = cols[_tar].transform.gameObject;
						return true;
					}
					else{
						Target = cols[_tar].transform.parent.parent.gameObject;
						return true;
					}
                    
                }
            }
        }
        return false;
    }
    void FollowWaypoint()
    {
        mtt.moving = true;
        mtt.moveType = MoveToTarget.MovementType.WorldPos;
        mtt.vecTarget = FW.WpTarget_v3;
    }
    void Wander()
    {
       
        //Wander around looking for targets.
    }
    void Seek()
    {
        if(Target==null)
		{
			current = WhiteBloodState.FollowWaypoints;
			return;
		}
		else if((Target.transform.position - this.transform.position).magnitude > 60) 
		{
			current = WhiteBloodState.FollowWaypoints;
			return;
		}
        if (TargetStats == null) TargetStats = Target.GetComponent<CellStats>();
        else if (Target.gameObject != TargetStats.gameObject) TargetStats = Target.GetComponentInChildren<CellStats>();

        if (TargetStats.health > 0)
        {
            mtt.moveType = MoveToTarget.MovementType.Transform;
            mtt.transTarget = Target.transform;
            Vector3 difference = Target.transform.position - transform.position;
            if (difference.magnitude < _consumeRadius)
            {
                targetTrans = Target.transform;
                targetRB = Target.GetComponent<Rigidbody>();
                current = WhiteBloodState.Consuming;
            }
        }
        else current = WhiteBloodState.FollowWaypoints;//Choose a new movement method
    }
    Rigidbody targetRB;
    Transform targetTrans;
    void Consuming()
    {
		if(Target == null)
		{
			current = WhiteBloodState.FollowWaypoints;
			return;
		}
        if (TargetStats.Viral && TargetStats.health >= 0)
        {

            mtt.moving = false;
            targetRB.Sleep();
            targetTrans.parent = this.transform;
            targetTrans.FindChild("Colliders").gameObject.SetActive(false);
            if((targetRB.position - this.transform.position).magnitude > 0.1f)
            targetRB.transform.position = new Vector3(Mathf.Lerp(targetTrans.position.x, this.transform.position.x, 0.1f),
                                                      Mathf.Lerp(targetTrans.position.y, this.transform.position.y, 0.1f),
                                                      Mathf.Lerp(targetTrans.position.z, this.transform.position.z, 0.1f));
            if (pastTime + _ThinkRate < Time.timeSinceLevelLoad)
            {
                pastTime = Time.timeSinceLevelLoad;
                TargetStats.DealDamage(_Damage);
            }
            if (TargetStats.health <= 0)
            {
                current = WhiteBloodState.FollowWaypoints;
                Target.SetActive(false);
				Destroy(Target, 2.0f);
            }
            //Child the virus to the white blood cell
            //turn off the virus' movement
            //deal damage per think rate until virus is dead
            //white blood cell is stationary during consumsion

        }
        else if (Target.gameObject.tag == "Player")
        {
			if (pastTime + _ThinkRate < Time.timeSinceLevelLoad)
			{
				pastTime = Time.timeSinceLevelLoad;
				TargetStats.DealDamage(_Damage);
			}
            //Deal damage to player per think rate
            //If player's threat goes below another then switch to seek
        }
    }
    
}
