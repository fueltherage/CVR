using UnityEngine;
using System.Collections;

public class ShootBall : MonoBehaviour {
	public GameObject ball;
	public float force;
    public int LaunchRadius = 1;
    public float gap = 2.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame	
	void Update () {
		if(Input.GetKeyDown(KeyCode.B))
		{
			GameObject newBall = Instantiate(ball, this.transform.position, transform.rotation) as GameObject;
			newBall.GetComponent<Rigidbody>().velocity = this.transform.forward*force;
		}
        if(Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.LeftControl))
        {
            GameObject newBall;
            Vector3 value = new Vector3();
            for(int x = -LaunchRadius; x<= LaunchRadius; x++)
                for(int y = -LaunchRadius; y<= LaunchRadius; y++)
            {
                //A cool way of getting a grid facing the camera 
                value = transform.right * (x * gap);
                value += transform.up * (y * gap);
                newBall = Instantiate(ball, this.transform.position + value, transform.rotation) as GameObject;
                newBall.GetComponent<Rigidbody>().velocity = this.transform.forward*force;
            }
        }
	}
}
