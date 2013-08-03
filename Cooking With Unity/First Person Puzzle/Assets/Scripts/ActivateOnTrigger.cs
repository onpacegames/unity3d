using UnityEngine;
using System.Collections;

public class ActivateOnTrigger : MonoBehaviour {
	
	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == tag) {
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag)) {
				obj.SendMessage("ObjectActivate", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	
	void OnTriggerExit(Collider other) {
		if (other.tag == tag) {
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag)) {
				obj.SendMessage("ObjectDeactivate", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}