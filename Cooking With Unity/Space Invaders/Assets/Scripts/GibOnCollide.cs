using UnityEngine;
using System.Collections;

public class GibOnCollide : MonoBehaviour {
	
	public GameObject[] physicsGibs;
	public GameObject[] staticGibs;
	public float explosionForce;
	public float spawnRadius = 1.0f;

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void OnTriggerEnter() {
		foreach (GameObject gib in physicsGibs) {
			GameObject gibInstance = Instantiate(gib, transform.position + Random.insideUnitSphere * spawnRadius, transform.rotation) as GameObject;
			gibInstance.rigidbody.AddExplosionForce(explosionForce, transform.position, spawnRadius);
		}
		
		foreach (GameObject gib in staticGibs) {
			Instantiate(gib, transform.position, transform.rotation);
		}
		
		Destroy(gameObject);
	}
}