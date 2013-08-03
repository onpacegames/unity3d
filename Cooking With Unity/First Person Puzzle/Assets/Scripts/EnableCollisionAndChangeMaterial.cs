using UnityEngine;
using System.Collections;

public class EnableCollisionAndChangeMaterial : MonoBehaviour {
	
	public Material activatedMaterial;
	
	private Material deactivatedMaterial;
	
	// Use this for initialization
	void Start() {
		deactivatedMaterial = renderer.material;
	}
	
	// Update is called once per frame
	void Update() {
	
	}
	
	void ObjectActivate() {
		collider.enabled = true;
		renderer.material = activatedMaterial;
	}
	
	void ObjectDeactivate() {
		collider.enabled = false;
		renderer.material = deactivatedMaterial;
	}
}