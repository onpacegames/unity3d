using UnityEngine;
using System.Collections;

public class ChangeScoreOnCollide : MonoBehaviour {
	
	public int pointValue;
	
	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void OnTriggerEnter() {
		GameManager.score += pointValue;
	}
}