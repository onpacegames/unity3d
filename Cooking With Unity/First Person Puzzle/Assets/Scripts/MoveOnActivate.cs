using UnityEngine;
using System.Collections;

public class MoveOnActivate : MonoBehaviour {
	
	public Vector3 movementVector;
	public float movementTime = 2.0f;
	
	private Vector3 startPosition;
	private bool moveEnabled = false;
	private bool reverse = false;
	
	// Use this for initialization
	void Start() {
		startPosition = transform.position;
		
		StartCoroutine(MoveOverTime());
	}
	
	void ObjectActivate() {
		moveEnabled = true;
	}
	
	void ObjectDeactivate() {
		moveEnabled = false;
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	// Coroutine practice
	IEnumerator MoveOverTime() {
		float timer = 0;
		while (true) {
			if (moveEnabled) {
				if (!reverse) {
					timer += Time.deltaTime;
				} else {
					timer -= Time.deltaTime;
				}
				
				if (timer > movementTime) {
					reverse = true;
				} else if (timer < 0) {
					reverse = false;
				}
				
				transform.position = Vector3.Lerp(startPosition, startPosition + movementVector, timer / movementTime);
			}
			yield return null;
		}
	}
}