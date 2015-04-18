using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UI_SpeedText : MonoBehaviour {

    public RigidBodyController rbc;
    Text text;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        text.text = rbc.Impulse.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        text.text = rbc.Impulse.ToString();

    }
}
