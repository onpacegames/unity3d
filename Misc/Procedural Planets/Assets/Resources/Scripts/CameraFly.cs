using UnityEngine;
using System.Collections;

public class CameraFly : MonoBehaviour {

	// Use this for initialization
	Transform camTransform;
	public float speed = 10f;
	public float boostspeed = 100f;
	float rotateSpeed = 60f;
	float totalRun = 0.0f;
	float maxShift;
	void Awake()
	{
		camTransform = gameObject.transform;
		maxShift = boostspeed * 10f;
		
		
	}
	
	void Start () {
	
	}
	
	private Vector3 GetBaseInput()
	{ //returns the basic values, if it's 0 than it's not active.

   	 Vector3 p_Velocity = new Vector3();

   	 if (Input.GetKey (KeyCode.W)){

    	    p_Velocity += new Vector3(0, 0 , 1);

   	 }

   	 if (Input.GetKey (KeyCode.S)){

    	    p_Velocity += new Vector3(0, 0 , -1);

   	 }

   	 if (Input.GetKey (KeyCode.A)){
	
    	    p_Velocity += new Vector3(-1, 0 , 0);

   	 }

  	  if (Input.GetKey (KeyCode.D)){

    	    p_Velocity += new Vector3(1, 0 , 0);

   	 }

    	return p_Velocity;

	}
	
	void DoRotation()
	{
		if (Input.GetKey(KeyCode.Space) == false)
		{
		if (Input.GetKey(KeyCode.Keypad8))
		{
			transform.Rotate((new Vector3(-rotateSpeed, 0, 0)) * Time.deltaTime);
		}
		if (Input.GetKeyUp(KeyCode.Keypad8))
		{
			Quaternion oldRot = transform.rotation;
			transform.rotation = oldRot;
		}
		if (Input.GetKey(KeyCode.Keypad2))
		{
			transform.Rotate((new Vector3(rotateSpeed, 0, 0)) * Time.deltaTime);
		}
		if (Input.GetKeyUp(KeyCode.Keypad2))
		{
			Quaternion oldRot = transform.rotation;
			transform.rotation = oldRot;
		}
		if (Input.GetKey(KeyCode.Keypad3))
		{
			transform.Rotate((new Vector3(0, rotateSpeed, 0)) * Time.deltaTime);
		}
		if (Input.GetKeyUp(KeyCode.Keypad3))
		{
			Quaternion oldRot = transform.rotation;
			transform.rotation = oldRot;
		}
		if (Input.GetKey(KeyCode.Keypad1))
		{
			transform.Rotate((new Vector3(0, -rotateSpeed, 0)) * Time.deltaTime);
		}
		if (Input.GetKeyUp(KeyCode.Keypad1))
		{
			Quaternion oldRot = transform.rotation;
			transform.rotation = oldRot;
		}
		if (Input.GetKey(KeyCode.Keypad6))
		{
			transform.Rotate((new Vector3(0, 0, -rotateSpeed)) * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.Keypad4))
		{
			transform.Rotate((new Vector3(0, 0, rotateSpeed)) * Time.deltaTime);
		}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 p = GetBaseInput(); 
		
		
		if (Input.GetKey (KeyCode.LeftShift))
		{ 
			DoRotation();
        	totalRun += Time.deltaTime; 

        	p  = p * totalRun * boostspeed; 

       		p.x = Mathf.Clamp(p.x, -maxShift, maxShift); 
	
        	p.y = Mathf.Clamp(p.y, -maxShift, maxShift);

        	p.z = Mathf.Clamp(p.z, -maxShift, maxShift);

    		}
		else
		{
			DoRotation();
        	totalRun = Mathf.Clamp(totalRun * 0.5f, 1, 1000); 

        	p = p * speed;
		}

    	
		p = p * Time.deltaTime;
		transform.Translate( p);
		
		
		
		
		
		
		/*
		if (Input.GetKey(KeyCode.W))
		{
			camTransform.position += (camTransform.forward)*(speed)*(Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D))
		{
			
		}
		*/
	}
}
