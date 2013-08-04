using UnityEngine;
using System.Collections;

public class Forestiy : MonoBehaviour {

	// Use this for initialization
	
	public GameObject planet;
	InitializePlanet planetInit;
	public GameObject treeObject;
	public int treeCount;
	public float treescale = 1;
	void Start () {
	planetInit = (InitializePlanet) planet.GetComponent(typeof(InitializePlanet));
		
		for(int i = 0; i < treeCount; i++)
		{
			GameObject temptree = (GameObject) Instantiate(treeObject);
			temptree.transform.position = planetInit.SurfacePoint(Random.Range(-90, 90), Random.Range(-180, 180), 0, planetInit.noise, planet);
			Vector3 v3 = temptree.transform.position - planet.transform.position;
    		temptree.transform.rotation = Quaternion.FromToRotation(transform.up, v3) * transform.rotation;
			temptree.transform.localScale = new Vector3(treescale, treescale, treescale);
			temptree.transform.parent = gameObject.transform;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
