using UnityEngine;
using System.Collections;

public class LoadLevelOnTrigger : MonoBehaviour {
	
	public string levelName;
	
	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			Application.LoadLevel(levelName);
		}
	}
}