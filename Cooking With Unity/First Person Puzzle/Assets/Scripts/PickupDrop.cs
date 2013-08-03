using UnityEngine;
using System.Collections;

public class PickupDrop : MonoBehaviour {
	
	public float pickupDistance = 2.0f;
	public float holdDistance = 2.0f;
	
	private bool isHoldingSomething = false;
	private GameObject heldObject;
	
	public LayerMask pickupMask;

	// Use this for initialization
	void Start() {
	
	}
	
	// Update is called once per frame
	void Update() {
		if (!isHoldingSomething) {
			// We are not holding an object:
			
			RaycastHit hitInfo;
			// TODO necessary to say "out" here?
			if (Physics.Raycast(transform.position, transform.forward, out hitInfo, pickupDistance, pickupMask)) {
//				Debug.Log("Looking at " + hitInfo.collider);
				// TODO hardcoded mouse buttons
				if (Input.GetMouseButtonDown(0)) {
					hitInfo.collider.transform.parent = transform.parent;
					hitInfo.collider.rigidbody.isKinematic = true;
					
//						Vector3 newPosition = transform.position;
//						newPosition += transform.forward * holdDistance;
//						hitInfo.collider.transform.position = newPosition;
					
					heldObject = hitInfo.collider.gameObject;
					
					isHoldingSomething = true;
				}
			}
		} else {
			// We are holding an object:
			
			Vector3 newPosition = transform.position;
			newPosition += transform.forward * holdDistance;
			heldObject.transform.position = newPosition;
			
			if (Input.GetMouseButtonDown(0)) {
				heldObject.rigidbody.isKinematic = false;
				heldObject.transform.parent = null;
				heldObject = null;
				
				isHoldingSomething = false;
			}
		}
	}
}