using UnityEngine;
using System.Collections;

public class PathThroughObjects : MonoBehaviour {
	
	public GameObject[] pathPoints;
	public float speed = 1.0f;
	public float goalSize = 0.1f;
	
	private int currentPathIndex = 0;
	private Vector3 movementDirection;
	
	// Use this for initialization
	void Start() {
		movementDirection = (pathPoints[currentPathIndex].transform.position - transform.position).normalized;
	}
	
	// Update is called once per frame
	void Update() {
		transform.position += movementDirection * speed * Time.deltaTime;
		
//		if (Vector3.Distance(transform.position, pathPoints[currentPathIndex].transform.position) < goalSize) {
//			currentPathIndex++;
//			movementDirection = (pathPoints[currentPathIndex].transform.position - transform.position).normalized;
//		}
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject == pathPoints[currentPathIndex]) {
			currentPathIndex++;
			movementDirection = (pathPoints[currentPathIndex].transform.position - transform.position).normalized;
		}
	}
}