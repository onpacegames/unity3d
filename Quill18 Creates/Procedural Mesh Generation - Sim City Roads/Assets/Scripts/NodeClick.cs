using UnityEngine;
using System.Collections;

public class NodeClick : MonoBehaviour {
	
	public GroundClick ground;
	
	void OnMouseDown() {
		ground.SetNodeStart(gameObject);
	}
}