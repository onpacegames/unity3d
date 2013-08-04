using UnityEngine;
using System.Collections;

public class LockToCursor : MonoBehaviour {
	
	// TODO make configurable to set which plane to move the object on
	// TODO make configurable for a fixed dimension value
	// TODO make camera configurable
	
	public bool alwaysLockedToCursorOverride = false;
	
	private bool alwaysLockedToCursor = false;
	
	private bool isLockedToCursor = false;
	private GameObject lockTarget = null;
	
	// Use this for initialization
	void Start() {
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
			alwaysLockedToCursor = false;
		} else {
			alwaysLockedToCursor = alwaysLockedToCursorOverride;
			
			if (alwaysLockedToCursor) {
				// TODO repeated code
				lockTarget = new GameObject("Lock Target");
				lockTarget.transform.position = transform.position;
				transform.parent = lockTarget.transform;
				
				Screen.showCursor = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (!alwaysLockedToCursor) {
			if (Input.GetMouseButtonDown(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				if (collider.Raycast(ray, out hitInfo, Mathf.Infinity)) {
					isLockedToCursor = true;
					
					// TODO repeated code
					Vector3 cursorScreenPosition = Input.mousePosition;
					cursorScreenPosition.z = -Camera.main.transform.position.z;
					
					Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
					
					// TODO repeated code
					lockTarget = new GameObject("Lock Target");
					lockTarget.transform.position = cursorWorldPosition;
					transform.parent = lockTarget.transform;
				}
			}
			
			if (Input.GetMouseButtonUp(0)) {
				isLockedToCursor = false;
				transform.parent = null;
				Destroy(lockTarget);
				lockTarget = null;
			}
		}
		
		if (isLockedToCursor || alwaysLockedToCursor) {
			// TODO repeated code
			Vector3 cursorScreenPosition = Input.mousePosition;
			cursorScreenPosition.z = -Camera.main.transform.position.z;
				
			Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
			
			lockTarget.transform.position = cursorWorldPosition;
		}
	}
}