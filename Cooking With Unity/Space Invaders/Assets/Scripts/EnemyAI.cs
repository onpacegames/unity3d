using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	
	public float rightEdgePadding;
	public float leftEdgePadding;
	public float dropDistance = 1.0f;
	
	// TODO should be an enum
	private int direction = 1;
	
	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		Vector3 newPosition = transform.position;
		newPosition.x += GameManager.enemySpeed * direction * Time.deltaTime;
		transform.position = newPosition;
		
		if (Camera.main.WorldToScreenPoint(transform.position).x > Screen.width - rightEdgePadding) {
			direction = -1;
			newPosition = transform.position;
			float  cameraObjectZDifference = transform.position.z - Camera.main.transform.position.z;
			newPosition.x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, cameraObjectZDifference)).x;
			newPosition.y -= dropDistance;
			transform.position = newPosition;
		} else if (Camera.main.WorldToScreenPoint(transform.position).x < 0 + leftEdgePadding) {
			direction = 1;
			newPosition = transform.position;
			float  cameraObjectZDifference = transform.position.z - Camera.main.transform.position.z;
			newPosition.x = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, cameraObjectZDifference)).x;
			newPosition.y -= dropDistance;
			transform.position = newPosition;
		}
	}
}