using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	
	enum ScreenSide {
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right = 3
	};
	
	public float spawnTime = 1.0f;
	public GameObject enemyPrefab;

	// Use this for initialization
	void Start() {
		InvokeRepeating("SpawnEnemy", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void SpawnEnemy() {
		Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
		Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z));
		
		// Choose a random side of the screen to begin on:
		ScreenSide side = (ScreenSide)Random.Range(0, System.Enum.GetNames(typeof(ScreenSide)).Length);
		
		float size = Random.Range(Constants.ENEMY_SIZE_MIN, Constants.ENEMY_SIZE_MAX);
		
		float halfSizeX = size / 2.0f;
		float halfSizeY = size / 2.0f;
		
		// TODO a lot of repeated code here that could be made better
		Vector3 startPosition = Vector3.zero;
		Vector3 endPosition = Vector3.zero;
		switch (side) {
		case ScreenSide.Top:
			startPosition.x = Random.Range(bottomLeft.x + halfSizeX, topRight.x - halfSizeX);
			startPosition.y = topRight.y + halfSizeY;
			
			endPosition.x = Random.Range(bottomLeft.x + halfSizeX, topRight.x - halfSizeX);
			endPosition.y = bottomLeft.y - halfSizeY;
			break;
			
		case ScreenSide.Bottom:
			startPosition.x = Random.Range(bottomLeft.x + halfSizeX, topRight.x - halfSizeX);
			startPosition.y = bottomLeft.y - halfSizeY;
			
			endPosition.x = Random.Range(bottomLeft.x + halfSizeX, topRight.x - halfSizeX);
			endPosition.y = topRight.y + halfSizeY;
			break;
			
		case ScreenSide.Left:
			startPosition.x = bottomLeft.x - halfSizeX;
			startPosition.y = Random.Range(bottomLeft.y + halfSizeY, topRight.y - halfSizeY);
			
			endPosition.x = topRight.x + halfSizeX;
			endPosition.y = Random.Range(bottomLeft.y + halfSizeY, topRight.y - halfSizeY);
			break;
			
		case ScreenSide.Right:
			startPosition.x = topRight.x + halfSizeX;
			startPosition.y = Random.Range(bottomLeft.y + halfSizeY, topRight.y - halfSizeY);
			
			endPosition.x = bottomLeft.x - halfSizeX;
			endPosition.y = Random.Range(bottomLeft.y + halfSizeY, topRight.y - halfSizeY);
			break;
		}
		
		GameObject enemy = (GameObject) Instantiate(enemyPrefab, startPosition, Quaternion.identity);
		enemy.transform.localScale *= size;
		
		Vector3 velocity = (endPosition - startPosition).normalized * Random.Range(Constants.ENEMY_SPEED_MIN, Constants.ENEMY_SPEED_MAX);
		enemy.rigidbody.velocity = velocity;
	}
}