using UnityEngine;
using System.Collections;

public class StayOnScreen : MonoBehaviour {
	
	// TODO possibly cache the constraints? Listen for a screen size change event in case they do change?
	// TODO make play space plane and fixed location configurable
	
	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		
	}
	
	void LateUpdate() {
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));
		
		Vector3 position = transform.position;
		float halfSizeX = transform.localScale.x / 2.0f;
		float halfSizeY = transform.localScale.y / 2.0f;
		if (position.x < bottomLeft.x + halfSizeX) {
			position.x = bottomLeft.x + halfSizeX;
		} else if (position.x > topRight.x - halfSizeX) {
			position.x = topRight.x - halfSizeX;
		}
		if (position.y < bottomLeft.y + halfSizeY) {
			position.y = bottomLeft.y + halfSizeY;
		} else if (position.y > topRight.y - halfSizeY) {
			position.y = topRight.y - halfSizeY;
		}
		transform.position = position;
	}
}