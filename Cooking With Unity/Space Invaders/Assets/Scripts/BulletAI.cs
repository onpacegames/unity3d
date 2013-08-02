using UnityEngine;
using System.Collections;

public class BulletAI : MonoBehaviour {
	
	public float speed = 1.0f;
	
	// Use this for initialization
	void Start() {
		Vector3 newVelocity = Vector3.zero;
		newVelocity.y = speed;
		rigidbody.velocity = newVelocity;
	}
	
	// Update is called once per frame
	void Update() {
		// Vector3 newPosition = transform.position;
		// newPosition.y += speed * Time.deltaTime;
		// transform.position = newPosition;
	}
}