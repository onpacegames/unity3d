using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetAxis("Horizontal") < 0) {
			Debug.Log("Left");
			
			transform.Translate(-10.0f * Time.deltaTime, 0, 0);
		} else if (Input.GetAxis("Horizontal") > 0) {
			Debug.Log("Right");
			
			transform.Translate(10.0f * Time.deltaTime, 0, 0);
		}
		
//		if (Input.GetButton("Jump")) {
//			Debug.Log("Jump");
//		}
	}
}