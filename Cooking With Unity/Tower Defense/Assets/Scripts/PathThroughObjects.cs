using UnityEngine;
using System.Collections;

public class PathThroughObjects : MonoBehaviour {
	
	public GameObject[] pathPoints;
	public float speed = 1.0f;
	public float goalSize = 0.1f;
	
	private int currentPathIndex = 0;
	private Vector3 movementDirection;
	
	public GameObject graphicalPathObject;
	
	// Use this for initialization
	void Start() {
		movementDirection = (pathPoints[currentPathIndex].transform.position - transform.position).normalized;
		
		CreateGraphicalPathObjects();
	}
	
	// TODO this is messy - clean up
	void CreateGraphicalPathObjects() {
		// Create object between transform.position and first waypoint
		Vector3 pathObjectPosition = transform.position + ((pathPoints[0].transform.position - transform.position) * 0.5f);
		
		Quaternion pathObjectOrientation = Quaternion.LookRotation(pathPoints[0].transform.position - transform.position);
		
		GameObject pathObject = Instantiate(graphicalPathObject, pathObjectPosition, pathObjectOrientation) as GameObject;
		
		Vector3 newScale = Vector3.one;
		newScale.z = (pathPoints[0].transform.position - transform.position).magnitude;
		pathObject.transform.localScale = newScale;
		
		Vector2 newTextureScale = Vector2.one;
		newTextureScale.y = (pathPoints[0].transform.position - transform.position).magnitude;
		pathObject.renderer.material.mainTextureScale = newTextureScale;
		
		for (int i = 1; i < pathPoints.Length; i++) {
			pathObjectPosition = pathPoints[i - 1].transform.position + ((pathPoints[i].transform.position - pathPoints[i - 1].transform.position) * 0.5f);
		
			pathObjectOrientation = Quaternion.LookRotation(pathPoints[i].transform.position - pathPoints[i - 1].transform.position);
			
			pathObject = Instantiate(graphicalPathObject, pathObjectPosition, pathObjectOrientation) as GameObject;
			
			newScale = Vector3.one;
			newScale.z = (pathPoints[i].transform.position - pathPoints[i - 1].transform.position).magnitude;
			pathObject.transform.localScale = newScale;
			
			newTextureScale = Vector2.one;
			newTextureScale.y = (pathPoints[i].transform.position - pathPoints[i - 1].transform.position).magnitude;
			pathObject.renderer.material.mainTextureScale = newTextureScale;
		}
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
			transform.position = pathPoints[currentPathIndex].transform.position;
			currentPathIndex++;
			
			if (currentPathIndex >= pathPoints.Length) {
				// TODO ADD LOGIC HERE TO DEDUCT HEALTH FROM PLAYER/BASE
				Destroy(gameObject);
			} else {
				movementDirection = (pathPoints[currentPathIndex].transform.position - transform.position).normalized;
			}
		}
	}
}