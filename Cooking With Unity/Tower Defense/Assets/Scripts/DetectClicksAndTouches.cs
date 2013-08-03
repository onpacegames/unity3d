//Copyright 2012 Maximilian Wolfgang Maroe, feel free to use or modify this script at your leisure
//This script sets it up so that when you click or touch, it tries to find an object in the scene
//that is in the direction of where you clicked, and sends a "Clicked()" message to the object.
//Supports touch (And multitouch!)

using UnityEngine;
using System.Collections;

public class DetectClicksAndTouches : MonoBehaviour
{	
	//This variable is optional; if not set it will default to the main camera
	//This is so that you can detect clicks/touches on a separate UI Camera
	//This variable does NOT update in real time
	public Camera detectionCamera;
	
	//This variable adds a Debug.Log call to show what was touched
	public bool debug = false;
	
	//This is the actual camera we reference in the update loop, set in Start()
	private Camera _camera;
	
	void Start()
	{
		if(detectionCamera != null)
		{
			_camera = detectionCamera;
		}
		else
		{
			_camera = Camera.main;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		Ray ray;
		RaycastHit hit;
	
		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			foreach(Touch touch in Input.touches)
			{
				if(touch.phase == TouchPhase.Began)
				{
					ray = _camera.ScreenPointToRay(touch.position);
					
					if(Physics.Raycast(ray, out hit, Mathf.Infinity))
					{
						if(debug)
						{
							Debug.Log("You touched " + hit.collider.gameObject.name,hit.collider.gameObject);
						}
						hit.transform.gameObject.SendMessage ("Clicked", hit.point, SendMessageOptions.DontRequireReceiver);
					}
				}		
			}
		}
		else //We are on a computer (more than likely)
		{
			if(Input.GetMouseButtonDown(0))  //Check to see if we've clicked
			{
				//Set up our ray from screen to scene
				ray = _camera.ScreenPointToRay(Input.mousePosition); 
				
				//If we hit...
				if(Physics.Raycast (ray, out hit, Mathf.Infinity))
				{
					//Tell the system what we clicked something if in debug
					if(debug)
					{
						Debug.Log("You clicked " + hit.collider.gameObject.name,hit.collider.gameObject);
					}
					
					//Run the Clicked() function on the clicked object
					hit.transform.gameObject.SendMessage("Clicked", hit.point, SendMessageOptions.DontRequireReceiver);
				}			
			}
		}
	}
}
