using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {
	
	public float paddleSpeed = 10.0f;
	
	public GameObject ballPrefab;
	
	GameObject attachedBall = null;
	
	int lives = 4;
	GUIText guiLives;
	
	int score = 0;
	public GUISkin scoreboardSkin;

	// Use this for initialization
	void Start() {
		DontDestroyOnLoad(gameObject);
		
		guiLives = GameObject.Find("GUILives").GetComponent<GUIText>();
		guiLives.text = "Lives: " + lives;
		DontDestroyOnLoad(guiLives);
		
		SpawnBall();
	}
	
	public void OnLevelWasLoaded(int level) {
		SpawnBall();
	}
	
	public void LoseLife() {
		lives--;
		guiLives.text = "Lives: " + lives;
		
		if (lives > 0) {
			SpawnBall();
		} else {
			Destroy(gameObject);
			Application.LoadLevel("gameOver");
		}
	}
	
	public void SpawnBall() {
		// Spawn/Instantiate new ball
		if (ballPrefab == null) {
			Debug.Log ("Hey dummy, you forgot to link to the ball prefab in the inspector.");
			return;
		}
		
		attachedBall = (GameObject) Instantiate(ballPrefab, transform.position + new Vector3(0, 0.75f, 0), Quaternion.identity);
	}
	
	void OnGUI() {
		GUI.skin = scoreboardSkin;
		
		GUI.Label(new Rect(0, 10, 300, 100), "Score: " + score); 
	}
	
	// Update is called once per frame
	void Update() {
//		if (Input.GetAxis("Horizontal") < 0) {
//			Debug.Log("Left");
//			
//			transform.Translate(-10.0f * Time.deltaTime, 0, 0);
//		} else if (Input.GetAxis("Horizontal") > 0) {
//			Debug.Log("Right");
//			
//			transform.Translate(10.0f * Time.deltaTime, 0, 0);
//		}
		
		// Left-Right
		transform.Translate(paddleSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), 0, 0);
		
//		if (Input.GetButton("Jump")) {
//			Debug.Log("Jump");
//		}
		
		if (transform.position.x > 7.4f) {
			transform.position = new Vector3(7.4f, transform.position.y, transform.position.z);
		} else if (transform.position.x < -7.4f) {
			transform.position = new Vector3(-7.4f, transform.position.y, transform.position.z);
		}
		
		if (attachedBall != null) {
			Rigidbody ballRigidBody = attachedBall.rigidbody;
			
			ballRigidBody.position = transform.position + new Vector3(0, 0.75f, 0);
				
			if (Input.GetButtonDown("Jump")) {
				// Fire the ball
				// TODO .rigidbody is inefficient
				ballRigidBody.isKinematic = false;
				ballRigidBody.AddForce(300f * Input.GetAxis("Horizontal"), 300f, 0);
				attachedBall = null;
			}
		}
	}
	
	void FixedUpdate() {
		
	}
	
	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			if (contact.thisCollider == collider) {
				// This is the paddle's contact point
				float english = contact.point.x - transform.position.x;
				
				contact.otherCollider.rigidbody.AddForce(300f * english, 0, 0);
			}
		}
	}
	
	public void AddPoint(int v) {
		score += v;
	}
}