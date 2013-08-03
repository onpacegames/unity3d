using UnityEngine;
using System.Collections;

public class CreateTowerOnClick : MonoBehaviour {
	
	public GameObject tower;
	
	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void Clicked(Vector3 position) {
		Instantiate(tower, position + Vector3.up * 0.5f, tower.transform.rotation);
	}
}