using UnityEngine;
using System.Collections;

public class DestroyAfterPlay : MonoBehaviour {

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		if (!audio.isPlaying) {
			Destroy(gameObject);
		}
	}
}