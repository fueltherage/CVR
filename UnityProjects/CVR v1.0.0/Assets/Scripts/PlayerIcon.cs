using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerIcon : MonoBehaviour {
    Transform player;
    public Vector2 offset;
    public float multiplier =1;
    Image thisimage;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisimage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.localPosition = new Vector3((player.position.x+offset.x) * multiplier,
											  (player.position.z+offset.y) * multiplier, 0);
	
	}
}
