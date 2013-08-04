using UnityEngine;
using System.Collections;

public class ReconcilerTester : MonoBehaviour {

	// Use this for initialization
	Mesh qMesh;
	public int identifier;
	public float height;
	void Start () {
	
		qMesh = GetComponent<MeshFilter>().mesh;
	}
	
	void Applyheight()
	{
		Vector3[] vertices = qMesh.vertices;
		Vector3[] verticesN = qMesh.vertices;
		Vector3[] normals = qMesh.normals;
		Vector3[] truevpos = qMesh.vertices;
		
		
		
		
			
			truevpos[identifier] = (transform.TransformPoint(vertices[identifier]));
			truevpos[identifier].y = height;
			verticesN[identifier] = new Vector3(vertices[identifier].x, truevpos[identifier].y, vertices[identifier].z);
		
		//transform.rotation = Quaternion.Euler(0,0,0);
		qMesh.vertices = verticesN;
		qMesh.RecalculateNormals();
		qMesh.RecalculateBounds();
	}
	
	// Update is called once per frame
	void Update () {
		
		//qMesh.vertices[identifier].y = height;
		Applyheight();
	
	}
}
