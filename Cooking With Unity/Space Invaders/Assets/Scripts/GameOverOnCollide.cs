using UnityEngine;
using System.Collections;

public class GameOverOnCollide : MonoBehaviour {

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void OnTriggerEnter() {
		GameManager.gameOver = true;
	}
}