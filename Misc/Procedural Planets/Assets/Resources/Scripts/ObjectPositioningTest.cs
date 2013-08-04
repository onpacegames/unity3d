using UnityEngine;
using System.Collections;

public class ObjectPositioningTest : MonoBehaviour {

	// Use this for initialization
	public float latitude = 0;
	public float longitude = 0;
	public float offset = 0;
	public GameObject planet;
	public InitializePlanet planetInit;
	void Start () {
		planetInit = (InitializePlanet) planet.GetComponent(typeof(InitializePlanet));
		//transform.position = planetInit.SurfacePoint(latitude, longitude, offset, planetInit.noise);
		//transform.rotation = planetInit.SurfaceRotation(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = planetInit.SurfacePoint(latitude, longitude, offset, planetInit.noise, planet);
		//transform.rotation = planetInit.SurfaceRotation(transform.position);
		Vector3 v3 = transform.position - planet.transform.position;
    	transform.rotation = Quaternion.FromToRotation(transform.up, v3) * transform.rotation;
		
		
		
		
		
	}
}
