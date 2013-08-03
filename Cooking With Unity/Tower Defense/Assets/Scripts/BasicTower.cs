using UnityEngine;
using System.Collections;

public class BasicTower : MonoBehaviour {
	
	public GameObject bullet;
	public float bulletSpeed = 1.0f;
	public float fireRate = 1.0f;
	
	// Use this for initialization
	void Start() {
		InvokeRepeating("SpawnBullet", fireRate, fireRate);
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void SpawnBullet() {
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
		
		GameObject newBullet = Instantiate(bullet, transform.position, bullet.transform.rotation) as GameObject;
		newBullet.rigidbody.AddForce((targets[0].transform.position - transform.position).normalized * bulletSpeed, ForceMode.VelocityChange);
	}
}