using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public float startingEnemySpeed = 2.5f;
	public static float enemySpeed;
	
	public static int score;
	private static int highScore;
	
	public static bool gameOver = false;

	// Use this for initialization
	void Start() {
		enemySpeed = startingEnemySpeed;
	}
	
	// Update is called once per frame
	void Update() {
		if (gameOver) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				gameOver = false;
				
				if (score > highScore) {
					highScore = score;
				}
				score = 0;
				
				Application.LoadLevel("MainScene");
			}
		}
	}
	
	void OnGUI() {
		GUI.Label(new Rect(0, 0, 120, 20), "Score: " + score.ToString());
		
		GUI.Label(new Rect(0, 20, 120, 20), "High Score: " + highScore.ToString());
		
		if (gameOver) {
			GUI.Label(new Rect(Screen.width / 2.0f, Screen.height / 2.0f, 120, 20), "GAME OVER!");
		}
		
//		if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button")) {
//			print ("You clicked the button!");
//		}
	}
}