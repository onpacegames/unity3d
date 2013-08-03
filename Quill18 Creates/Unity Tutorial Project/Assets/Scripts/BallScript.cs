using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	public void Die() {
		Destroy(gameObject);
		
		GameObject paddle = GameObject.Find("Paddle");
		PaddleScript paddleScript = paddle.GetComponent<PaddleScript>();
		paddleScript.LoseLife();
	}
}