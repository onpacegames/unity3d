using UnityEngine;
using System.Collections;

public class ActivateOnTrigger : MonoBehaviour {
	
	public GameObject objectToActivate;
	
	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.renderer.material.color == renderer.material.color) {
			objectToActivate.SendMessage("ObjectActivate");
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.renderer.material.color == renderer.material.color) {
			objectToActivate.SendMessage("ObjectDeactivate");
		}
	}
}