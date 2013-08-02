using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	// Use this for initialization
	void Start() {
		rigidbody.AddForce(100f, 300f, 0);
	}
	
	// Update is called once per frame
	void Update() {
	
	}
}