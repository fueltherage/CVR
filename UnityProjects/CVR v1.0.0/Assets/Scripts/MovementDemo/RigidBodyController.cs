using UnityEngine;
using System.Collections;


public class RigidBodyController : MonoBehaviour {

	public float Force = 10;
	public float Impulse = 10;
	public float maxSpeed = 10;
	public Vector3 velocity;
	public float ForceDampen = 2;
	Rigidbody rigidbody;
	float currentForce;
	bool gravity = false;
    public bool ControllerMode = false;
    void Awake()
    {
        GameState.ControllerEnabled = ControllerMode;
    }

    Transform cameraTransform;
	// Use this for initialization
	void Start () {
        
		rigidbody = this.GetComponent<Rigidbody>();
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		velocity = GetComponent<Rigidbody>().velocity;
        Vector2 LeftStick = new Vector2(Input.GetAxis("HorizontalLeftStick"), Input.GetAxis("VerticalLeftStick"));
        Vector2 RightStick = new Vector2(Input.GetAxis("HorizontalRightStick"), Input.GetAxis("VerticalRightStick"));

        cameraTransform = CameraState.CurrentCamera.transform;
        if (!GameState.ControllerEnabled)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                rigidbody.AddForce(new Vector3(0, 1, 0) * Impulse, ForceMode.Impulse);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                rigidbody.AddForce(-new Vector3(0, 1, 0) * Impulse, ForceMode.Impulse);
            }


            if (Input.GetKey(KeyCode.A))
            {
                rigidbody.AddForce(-cameraTransform.right * Impulse, ForceMode.Impulse);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rigidbody.AddForce(cameraTransform.right * Impulse, ForceMode.Impulse);
            }

            if (Input.GetKey(KeyCode.W))
            {
                rigidbody.AddForce(cameraTransform.forward * Impulse, ForceMode.Impulse);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rigidbody.AddForce(-cameraTransform.forward * Impulse, ForceMode.Impulse);
            }
        }
        else
        {
            if (LeftStick.magnitude > 0.1f)
            {
                rigidbody.AddForce((new Vector3(cameraTransform.right.x * LeftStick.x, 0, cameraTransform.right.z * LeftStick.x)
                                 + new Vector3(cameraTransform.forward.x * LeftStick.y,cameraTransform.forward.y * LeftStick.y, cameraTransform.forward.z * LeftStick.y))
                                 * Impulse, ForceMode.Impulse);

            }

            float aB = Input.GetAxis("A_Button");
            if (aB >= 0.9f)
            {
                rigidbody.AddForce(cameraTransform.up * Impulse, ForceMode.Impulse);
            }
            else
            {
                float bB = Input.GetAxis("B_Button");
                if (bB >= 0.9f)
                {
                    rigidbody.AddForce(-cameraTransform.up * Impulse, ForceMode.Impulse);
                }
            }
        }
        if (rigidbody.velocity.magnitude >= maxSpeed) rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;

	}
    public void UpSpeed()
    {
        Impulse += 1;
    }
    public void DownSpeed()
    {
        Impulse -= 1;
    }
}
