using UnityEngine;
using System.Collections;

public class ShootBallForward : MonoBehaviour {
    public GameObject ball;
    public float force;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(GameState.currentState == GameState.GState.Active)
        if(Input.GetMouseButtonDown(0))
        {
            GameObject newBall = Instantiate(ball, this.transform.position + this.transform.forward * 2.0f, transform.rotation) as GameObject;
            newBall.rigidbody.velocity = this.transform.forward*force;
        }
	}
}
