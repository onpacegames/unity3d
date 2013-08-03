using UnityEngine;
using System.Collections;

public class DeathFieldScript : MonoBehaviour {

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void OnTriggerEnter(Collider other) {
		BallScript ballScript = other.GetComponent<BallScript>();
		
		if (ballScript != null) {
			ballScript.Die();
		}
	}
}