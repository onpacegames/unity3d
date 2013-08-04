using UnityEngine;
using System;
using System.Collections;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;
using System.Linq;
using System.Collections.Generic;
using System.IO;


public class QuadPlane : MonoBehaviour {

	// Use this for initialization
	bool beginTesting = false;
	
	public float second;
	public GameObject m_sun;
	public GameObject planeObject;
	
	public float rad = 5f;
	public float scaleFactor = 1f;
	Mesh qMesh;
	
	GameObject q1;
	GameObject q2;
	GameObject q3;
	GameObject q4;
	
	QuadPlane q1s;
	QuadPlane q2s;
	QuadPlane q3s;
	QuadPlane q4s;
	
	public Vector3 q1pos;
	public Vector3 q2pos;
	public Vector3 q3pos;
	public Vector3 q4pos;
	
	public bool keepx = false;
	public bool keepy = false;
	public bool keepz = false;
	public float mod;
	public float maxdist;
	public Quaternion rotation;
	
	public bool z1;
	public bool z2;
	public bool x1;
	public bool x2;
	public bool y1;
	public bool y2;
	
	public ModuleBase noise;
	
	public bool hasSplit = false;
	public bool cannotRevert = false;
	
	public GameObject parent;
	public float noisemod;

	public Vector3 planetPos;
	public Vector3 relativePos;
	
	public bool hasOcean = false;
	public Color lowhold;
	
	GameObject cameras;
	
	GameObject sq1;
	GameObject sq2;
	GameObject sq3;
	
	public bool render = true;

	
	public double left;
	public double right;
	public double bottom;
	public double top;
	
	public bool generateMap = false;
	
	public Material groundfromspace;
	
	Vector3 oldCamPos;
	
	public int splitCount;
	public int timesSplit;
	
	public bool initSplit = false;
	public bool splitAgain = false;
	
	public GameObject planetMaker;
	
	Plane[] planes;
	
	public float camTimeWithin = 0;
	
	public bool isLoaded = false;
	
	public float latitude = 0;
	public float longitude = 0;
	
	public Material groundFromGround;
	
	public bool fromground;
	public bool testmat;
	
	
	
	public InitializePlanet planetInit;
	
	
	
	
	
	//MeshCollider meshCollider;
	

	
	void Awake()
	{
		//meshCollider = GetComponent<MeshCollider>();
		cameras = GameObject.FindGameObjectWithTag("MainCamera");
		rotation = transform.rotation;
		
		
		
		
		
		
		
	}
	
	
	
	public void CalculatePositions()
	{
		
		relativePos = gameObject.transform.position - planetPos;
		maxdist = (6f*mod);
		
		//with a radius of 5, top would have y = 5, 2.5, 2.5 and negatives of x and z
		
		if (keepx == true) //Quad is on the x plane
			{
				
				q1pos = new Vector3(transform.position.x, transform.position.y + mod, transform.position.z + mod);
				q2pos = new Vector3(transform.position.x, transform.position.y - mod, transform.position.z + mod);
				q3pos = new Vector3(transform.position.x, transform.position.y - mod, transform.position.z - mod);
				q4pos = new Vector3(transform.position.x, transform.position.y + mod, transform.position.z - mod);
				
			}
		
		if (keepy == true) //Quad is on the x plane
			{
				
				q1pos = new Vector3(transform.position.x + mod, transform.position.y, transform.position.z + mod);
				q2pos = new Vector3(transform.position.x - mod, transform.position.y, transform.position.z + mod);
				q3pos = new Vector3(transform.position.x - mod, transform.position.y, transform.position.z - mod);
				q4pos = new Vector3(transform.position.x + mod, transform.position.y, transform.position.z - mod);
				
			}
		
		if (keepz == true) //Quad is on the x plane
			{
				
				q1pos = new Vector3(transform.position.x + mod, transform.position.y + mod, transform.position.z);
				q2pos = new Vector3(transform.position.x - mod, transform.position.y + mod, transform.position.z);
				q3pos = new Vector3(transform.position.x - mod, transform.position.y - mod, transform.position.z);
				q4pos = new Vector3(transform.position.x + mod, transform.position.y - mod, transform.position.z);
				
			}
		
		
		if (keepx == false && keepy == false && keepz == false) //script hasn't inherited these values and is the first quad
		{
			if (relativePos.x != 0) //Quad is on the x plane
			{
				keepx = true;
				q1pos = new Vector3(transform.position.x, transform.position.y + mod, transform.position.z + mod);
				q2pos = new Vector3(transform.position.x, transform.position.y - mod, transform.position.z + mod);
				q3pos = new Vector3(transform.position.x, transform.position.y - mod, transform.position.z - mod);
				q4pos = new Vector3(transform.position.x, transform.position.y + mod, transform.position.z - mod);
				
				
				
			}
			
			if (relativePos.y != 0) //Quad is on the x plane
			{
				keepy = true;
				q1pos = new Vector3(transform.position.x + mod, transform.position.y, transform.position.z + mod);
				q2pos = new Vector3(transform.position.x - mod, transform.position.y, transform.position.z + mod);
				q3pos = new Vector3(transform.position.x - mod, transform.position.y, transform.position.z - mod);
				q4pos = new Vector3(transform.position.x + mod, transform.position.y, transform.position.z - mod);
				
			}
			
			if (relativePos.z != 0) //Quad is on the x plane
			{
				keepz = true;
				q1pos = new Vector3(transform.position.x + mod, transform.position.y + mod, transform.position.z);
				q2pos = new Vector3(transform.position.x - mod, transform.position.y + mod, transform.position.z);
				q3pos = new Vector3(transform.position.x - mod, transform.position.y - mod, transform.position.z);
				q4pos = new Vector3(transform.position.x + mod, transform.position.y - mod, transform.position.z);
				
			}
		}
		if (relativePos.z == rad)
		{
			rotation = Quaternion.Euler(90,0,0);
		}
		if (relativePos.z == -rad)
		{
			rotation = Quaternion.Euler(270,0,0);
		}
		if (relativePos.y == rad)
		{
			rotation = Quaternion.Euler(0,0,0);
		}
		if (relativePos.y == -rad)
		{
			rotation = Quaternion.Euler(0,0,180);
		}
		if (relativePos.x == rad)
		{
			rotation = Quaternion.Euler(0,0,270);
		}
		if (relativePos.x == -rad)
		{
			rotation = Quaternion.Euler(0,0,90);
		}
		
		
		
		
			
		
	
				
			
	}

	
	void Scale(float scalefactor)
	{
		qMesh = GetComponent<MeshFilter>().mesh;
		//qMesh.RecalculateBounds();
		
		
		
		
		
		Vector3[] vertices = qMesh.vertices;
		Vector3[] verticesN = qMesh.vertices;
		Vector3[] normals = qMesh.normals;
		Vector3[] truevpos = qMesh.vertices;
		
		for (int i = 0; i < vertices.Length; i++) 
		{
			truevpos[i] = transform.TransformPoint(vertices[i]);
			verticesN[i] = vertices[i]/scalefactor;
		}
		qMesh.vertices = verticesN;
		qMesh.RecalculateNormals();
		qMesh.RecalculateBounds();
	}
	
	void Spherify(float radius, ModuleBase noi)
	{
		Vector3[] vertices = qMesh.vertices;
		Vector3[] verticesN = qMesh.vertices;
		Vector3[] normals = qMesh.normals;
		Vector3[] truevpos = qMesh.vertices;
		
		for (int i = 0; i < vertices.Length; i++) 
		{
			truevpos[i] = (transform.TransformPoint(vertices[i])) - planetPos;
			
			verticesN[i] = (((truevpos[i].normalized) * (radius + (((float) noi.GetValue((truevpos[i].normalized * radius) + planetPos)) * noisemod)))) - (relativePos);
			//Debug.Log(planetMaker.name + (truevpos[i].normalized * radius));
			
		}
		transform.rotation = Quaternion.Euler(0,0,0);
		qMesh.vertices = verticesN;
		qMesh.RecalculateNormals();
		qMesh.RecalculateBounds();
	}
	
	Texture2D AltGenTex(Vector3[] verts, ModuleBase module)
	{
		Texture2D tex = new Texture2D((int) Mathf.Sqrt(verts.Length), (int) Mathf.Sqrt(verts.Length));
		Vector3[] interpolatedPoints = new Vector3[verts.Length];
		
		int reso = (int) Mathf.Sqrt(verts.Length);
		int pixelx = 0;
		int pixely = 0;
		
		
		for (int i = 0; i < verts.Length; i++) 
		{
			if (i < verts.Length - 1)
			{
			interpolatedPoints[i] = ((verts[i] + verts[i+1])/2);
			}
			
			verts[i] = transform.TransformPoint(verts[i]) - planetPos;
			if (pixelx == reso)
				{
					pixelx = 0;
					pixely += 1;
				}
			float noiseval = (float) module.GetValue((verts[i].normalized * rad) + planetPos);
			noiseval = Mathf.Clamp((noiseval + 0.5f)/2f, 0f, 1f);
			Color pixelColor = new Color(noiseval, noiseval, noiseval, 0);
			tex.SetPixel(pixelx, pixely, pixelColor);
			tex.Apply();
			pixelx += 1;
		}
		
		tex.wrapMode = TextureWrapMode.Clamp;
		tex.Apply();
		return tex;
	}
	
	void SplitQuad()
	{
		
		if (hasSplit == false)
		{
		q1 = (GameObject) Instantiate(planeObject);
		//q1 = (GameObject) Instantiate(gameObject);
		q1.transform.position = q1pos;
		q1.transform.rotation = rotation;
		q2 = (GameObject) Instantiate(planeObject);
		//q2 = (GameObject) Instantiate(gameObject);
		q2.transform.position = q2pos;
		q2.transform.rotation = rotation;
		q3 = (GameObject) Instantiate(planeObject);
		//q3 = (GameObject) Instantiate(gameObject);
		q3.transform.position = q3pos;
		q3.transform.rotation = rotation;
		q4 = (GameObject) Instantiate(planeObject);
		//q4 = (GameObject) Instantiate(gameObject);
		q4.transform.position = q4pos;
		q4.transform.rotation = rotation;
		
		q1s = (QuadPlane) q1.GetComponent(typeof(QuadPlane));
		q2s = (QuadPlane) q2.GetComponent(typeof(QuadPlane));
		q3s = (QuadPlane) q3.GetComponent(typeof(QuadPlane));
		q4s = (QuadPlane) q4.GetComponent(typeof(QuadPlane));
		

		q1s.rad = rad;
		q1s.scaleFactor = scaleFactor*2;
		q1s.keepx = keepx;
		q1s.keepy = keepy;
		q1s.keepz = keepz;
		q1s.mod = (mod/2);
		q1s.noise = noise;
		//q1.renderer.material = renderer.material;
		q1s.parent = gameObject;
		q1s.cannotRevert = false;
		q1s.noisemod = noisemod;
		q1s.planetPos = planetPos;
		q1s.lowhold = lowhold;
		
		
		
		q2s.rad = rad;
		q2s.scaleFactor = scaleFactor*2;
		q2s.keepx = keepx;
		q2s.keepy = keepy;
		q2s.keepz = keepz;
		q2s.mod = (mod/2);

		q2s.noise = noise;
		//q2.renderer.material = renderer.material;
		q2s.parent = gameObject;
		q2s.cannotRevert = false;
		q2s.noisemod = noisemod;
		q2s.planetPos = planetPos;
		q2s.lowhold = lowhold;
		
			
		
		q3s.rad = rad;
		q3s.scaleFactor = scaleFactor*2;
		q3s.keepx = keepx;
		q3s.keepy = keepy;
		q3s.keepz = keepz;
		q3s.mod = (mod/2);

		q3s.noise = noise;
		//q3.renderer.material = renderer.material;
		q3s.parent = gameObject;
		q3s.cannotRevert = false;
		q3s.noisemod = noisemod;
		q3s.planetPos = planetPos;
		q3s.lowhold = lowhold;
		
		
		q4s.rad = rad;
		q4s.scaleFactor = scaleFactor*2;
		q4s.keepx = keepx;
		q4s.keepy = keepy;
		q4s.keepz = keepz;
		q4s.mod = (mod/2);
		q4s.noise = noise;
		//q4.renderer.material = renderer.material;
		q4s.parent = gameObject;
		q4s.cannotRevert = false;
		q4s.noisemod = noisemod;
		q4s.planetPos = planetPos;
		q4s.lowhold = lowhold;
		
			
		q1s.sq1 = q2;
		q1s.sq2 = q3;
		q1s.sq3 = q4;
			
		q2s.sq1 = q1;
		q2s.sq2 = q3;
		q2s.sq3 = q4;
			
		q3s.sq1 = q1;
		q3s.sq2 = q2;
		q3s.sq3 = q4;
			
		q4s.sq1 = q1;
		q4s.sq2 = q2;
		q4s.sq3 = q3;
			
		q1s.m_sun = m_sun;
	 	q2s.m_sun = m_sun;
		q3s.m_sun = m_sun;
		q4s.m_sun = m_sun;
			
		q1s.groundfromspace = groundfromspace;
		
			
		q2s.groundfromspace = groundfromspace;
		
			
		q3s.groundfromspace = groundfromspace;
		
			
		q4s.groundfromspace = groundfromspace;
			
		q1s.splitCount = splitCount;
		q1s.timesSplit = timesSplit + 1;
		
		q2s.splitCount = splitCount;
		q2s.timesSplit = timesSplit + 1;
	
		q3s.splitCount = splitCount;
		q3s.timesSplit = timesSplit + 1;
		
		q4s.splitCount = splitCount;
		q4s.timesSplit = timesSplit + 1;
			
		q1s.planetMaker = planetMaker;
		q2s.planetMaker = planetMaker;
		q3s.planetMaker = planetMaker;
		q4s.planetMaker = planetMaker;
	
		q1.transform.parent = planetMaker.transform;
		q2.transform.parent = planetMaker.transform;
		q3.transform.parent = planetMaker.transform;
		q4.transform.parent = planetMaker.transform;
			
		q1s.planeObject = planeObject;
		q2s.planeObject = planeObject;
		q3s.planeObject = planeObject;
		q4s.planeObject = planeObject;
			
		q1s.groundFromGround = groundFromGround;
		q2s.groundFromGround = groundFromGround;
		q3s.groundFromGround = groundFromGround;
		q4s.groundFromGround = groundFromGround;
			
		q1s.planetInit = planetInit;
		q2s.planetInit = planetInit;
		q3s.planetInit = planetInit;
		q4s.planetInit = planetInit;
			
			
		}	
		else
		{ 
			q1.SetActive(true);
			q2.SetActive(true);
			q3.SetActive(true);
			q4.SetActive(true);
		}
		hasSplit = true;
		
		
		beginTesting = true;
		gameObject.SetActive(false);
		
		
			
	}
	
	public void InitialSplit()
	{
		renderer.enabled = false;
		
		q1 = (GameObject) Instantiate(planeObject);
		q1.transform.position = q1pos;
		q1.transform.rotation = rotation;
		q2 = (GameObject) Instantiate(planeObject);
		q2.transform.position = q2pos;
		q2.transform.rotation = rotation;
		q3 = (GameObject) Instantiate(planeObject);
		q3.transform.position = q3pos;
		q3.transform.rotation = rotation;
		q4 = (GameObject) Instantiate(planeObject);
		q4.transform.position = q4pos;
		q4.transform.rotation = rotation;
		
		q1s = (QuadPlane) q1.GetComponent(typeof(QuadPlane));
		q2s = (QuadPlane) q2.GetComponent(typeof(QuadPlane));
		q3s = (QuadPlane) q3.GetComponent(typeof(QuadPlane));
		q4s = (QuadPlane) q4.GetComponent(typeof(QuadPlane));
		
		//q1.SetActive = false;
		//q2.SetActive = false;
		//q3.SetActive = false;
		//q4.SetActive = false;
		
		q1s.rad = rad;
		q1s.scaleFactor = scaleFactor*2;
		q1s.keepx = keepx;
		q1s.keepy = keepy;
		q1s.keepz = keepz;
		q1s.mod = (mod/2);
		q1s.noise = noise;
		//q1.renderer.material = renderer.material;
		q1s.parent = gameObject;
		q1s.cannotRevert = true;
		q1s.noisemod = noisemod;
		q1s.planetPos = planetPos;
		q1s.lowhold = lowhold;
		q1s.hasOcean = hasOcean;
		//q1s.Spherify(rad, noise);
		
		
		q2s.rad = rad;
		q2s.scaleFactor = scaleFactor*2;
		q2s.keepx = keepx;
		q2s.keepy = keepy;
		q2s.keepz = keepz;
		q2s.mod = (mod/2);
		q2s.noise = noise;
		//q2.renderer.material = renderer.material;
		q2s.parent = gameObject;
		q2s.cannotRevert = true;
		q2s.noisemod = noisemod;
		q2s.planetPos = planetPos;
		q2s.lowhold = lowhold;
		q2s.hasOcean = hasOcean;
			
		
		q3s.rad = rad;
		q3s.scaleFactor = scaleFactor*2;
		q3s.keepx = keepx;
		q3s.keepy = keepy;
		q3s.keepz = keepz;
		q3s.mod = (mod/2);
		q3s.noise = noise;
		//q3.renderer.material = renderer.material;
		q3s.parent = gameObject;
		q3s.cannotRevert = true;
		q3s.noisemod = noisemod;
		q3s.planetPos = planetPos;
		q3s.lowhold = lowhold;
		q3s.hasOcean = hasOcean;
		
		q4s.rad = rad;
		q4s.scaleFactor = scaleFactor*2;
		q4s.keepx = keepx;
		q4s.keepy = keepy;
		q4s.keepz = keepz;
		q4s.mod = (mod/2);
		q4s.noise = noise;
		//q4.renderer.material = renderer.material;
		q4s.parent = gameObject;
		q4s.cannotRevert = true;
		q4s.noisemod = noisemod;
		q4s.planetPos = planetPos;
		q4s.lowhold = lowhold;
		q4s.hasOcean = hasOcean;
		
		q1s.m_sun = m_sun;
		q2s.m_sun = m_sun;
		q3s.m_sun = m_sun;
		q4s.m_sun = m_sun;
		
		q1s.groundfromspace = groundfromspace;
		
		
		q2s.groundfromspace = groundfromspace;
		
		
		q3s.groundfromspace = groundfromspace;

		
		q4s.groundfromspace = groundfromspace;
		
		q1s.splitCount = splitCount;
		q1s.timesSplit = timesSplit + 1;
		
		q2s.splitCount = splitCount;
		q2s.timesSplit = timesSplit + 1;
	
		q3s.splitCount = splitCount;
		q3s.timesSplit = timesSplit + 1;
		
		q4s.splitCount = splitCount;
		q4s.timesSplit = timesSplit + 1;
		
		q1s.planetMaker = planetMaker;
		q2s.planetMaker = planetMaker;
		q3s.planetMaker = planetMaker;
		q4s.planetMaker = planetMaker;
	
		q1.transform.parent = planetMaker.transform;
		q2.transform.parent = planetMaker.transform;
		q3.transform.parent = planetMaker.transform;
		q4.transform.parent = planetMaker.transform;
		
		q1s.planeObject = planeObject;
		q2s.planeObject = planeObject;
		q3s.planeObject = planeObject;
		q4s.planeObject = planeObject;
		
		q1s.groundFromGround = groundFromGround;
		q2s.groundFromGround = groundFromGround;
		q3s.groundFromGround = groundFromGround;
		q4s.groundFromGround = groundFromGround;
		
		q1s.planetInit = planetInit;
		q2s.planetInit = planetInit;
		q3s.planetInit = planetInit;
		q4s.planetInit = planetInit;
	
		
		
		
		beginTesting = true;
		gameObject.SetActive(false);
	}
	
	
	void CacheChildren()
	{
		//Put stuff here so on chunk load children values are set and then children are set inactive	
	}
	

	
	
	
	void Start () {
		
		
		
		
		if (((cameras.transform.position - planetPos).magnitude < 1.025f*rad))
		{
			fromground = true;
			renderer.material = groundFromGround;
			renderer.material.SetTexture("_DetailTex", ((Texture2D) Resources.Load("Models/Bump")));
			renderer.material.SetTexture("_DetailTex2", ((Texture2D) Resources.Load("Models/Gbumb")));
			renderer.material.SetFloat("_MultTest", (4096)/(Mathf.Pow(2, timesSplit)));
		}
		if (((cameras.transform.position - planetPos).magnitude > 1.025f*rad))
		{
			fromground = false;
			renderer.material = groundfromspace;
			renderer.material.SetTexture("_DetailTex", ((Texture2D) Resources.Load("Models/Bump")));
			renderer.material.SetTexture("_DetailTex2", ((Texture2D) Resources.Load("Models/Gbumb")));
			renderer.material.SetFloat("_MultTest", (4096)/(Mathf.Pow(2, timesSplit)));
		}
		testmat = true;
		
		//beginTesting = true;
		m_sun = GameObject.Find ("Sunlight");
		//renderer.material = groundfromspace;
		CalculatePositions();
		Scale(scaleFactor);
		renderer.material.SetFloat("_MultTest", (4096)/(Mathf.Pow(2, timesSplit)));
		
		
		
		
		//TextureScale.Bilinear (genTex, 10, 10);
		//Texture2D genTex = AltGenTex(qMesh.vertices, noise);
		renderer.material.SetTexture("_DetailTex", ((Texture2D) Resources.Load("Models/Bump")));
		renderer.material.SetTexture("_DetailTex2", ((Texture2D) Resources.Load("Models/Gbumb")));
		Spherify(rad, noise);
		
		Vector3 grelativePos = transform.TransformPoint(qMesh.vertices[60]) - transform.parent.position;
		latitude = (Mathf.Asin(grelativePos.y/rad)*180)/Mathf.PI;
		float LAT = latitude * Mathf.PI/180;
		
		
		float longitude1 = (180 * (Mathf.Asin((grelativePos.z)/(rad*Mathf.Cos(LAT)))))/Mathf.PI;	//There are two possible solutions for longitude, compare these by re-entering them to the XYZ formula
		float longitude2 = (180 * (Mathf.Acos((grelativePos.x)/(-rad*Mathf.Cos(LAT)))))/Mathf.PI;
		
		float LON1 = longitude1 * Mathf.PI/180;
		float LON2 = longitude2 * Mathf.PI/180;
		
		Vector3 testVector1 = new Vector3();
		testVector1.x = -rad * Mathf.Cos(LAT) * Mathf.Cos(LON1);
    	testVector1.y =  rad * Mathf.Sin(LAT);
    	testVector1.z =  rad * Mathf.Cos(LAT) * Mathf.Sin(LON1);
		testVector1 = testVector1 + planetPos;
		
		Vector3 testVector2 = new Vector3();
		testVector2.x = -rad * Mathf.Cos(LAT) * Mathf.Cos(LON2);
    	testVector2.y =  rad * Mathf.Sin(LAT);
    	testVector2.z =  rad * Mathf.Cos(LAT) * Mathf.Sin(LON2);
		testVector2 = testVector2 + planetPos;
		
		testVector1.x = Mathf.Round(testVector1.x);
		testVector1.y = Mathf.Round(testVector1.y);
		testVector1.z = Mathf.Round(testVector1.z);
		
		testVector2.x = Mathf.Round(testVector2.x);
		testVector2.y = Mathf.Round(testVector2.y);
		testVector2.z = Mathf.Round(testVector2.z);
		
		Vector3 testpos = new Vector3();
		testpos.x = Mathf.Round(transform.TransformPoint(qMesh.vertices[60]).x);
		testpos.y = Mathf.Round(transform.TransformPoint(qMesh.vertices[60]).y);
		testpos.z = Mathf.Round(transform.TransformPoint(qMesh.vertices[60]).z);
		
		Debug.Log (testVector1);
		Debug.Log(testVector2);	
		if (testpos.x > testVector1.x - 10f && testpos.x < testVector1.x + 10f)
		{
			if (testpos.y > testVector1.y - 10f && testpos.y < testVector1.y + 10f)
			{
				if (testpos.z > testVector1.z - 10f && testpos.z < testVector1.z + 10f)
				{
					longitude = longitude1;
				}
			}
		}
		else
		{
			longitude = longitude2;
		}
		
		qMesh.RecalculateBounds();
		//if (scaleFactor < ((5/rad)*(128)))
		//{
		//	beginTesting = true;
		//	SplitQuad();
		//}
		//else
		//{
			beginTesting = true;
		
		//}
		//meshCollider.sharedMesh = qMesh;
		//meshCollider.enabled = false;
		//renderer.material.mainTexture = genTex;
		//byte[] bytes = genTex.EncodeToPNG();
		if (planeObject == null)
		{
			Debug.Log("Quad at" + transform.position.ToString() + "null planeObject");
		}
		
		if (timesSplit < splitCount)
		{	
			timesSplit +=1;
			InitialSplit();
			//timesSplit 
		}
		isLoaded = true;
		gameObject.name = ("Quad " + Mathf.Round(latitude).ToString() + " " + Mathf.Round(longitude).ToString());
		
			
		
		
		
		
		//File.WriteAllBytes(Application.dataPath + "/../QuadTextures/Quad-" + mod.ToString() + ".png", bytes);
		
		//if (splitCount < timesSplit)
		//{
		//	InitialSplit();
		//	timesSplit += 1;
		//}
		
		
		

	}
	
	// Update is called once per frame
	void Update () {
		
		
	//second = (float) (Math.Round((Math.Round ((double) Time.time, 2)) * 100, 0)) ;
	if(Input.GetKeyDown(KeyCode.T))
		{
			SplitQuad();
		}
	 	
		renderer.material.SetVector("v3LightPos", m_sun.transform.forward*-1.0f);
		
		planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		if(GeometryUtility.TestPlanesAABB(planes, qMesh.bounds))
		{
			//Debug.Log("plane at" + transform.position.ToString() + "within bounds");
		}
		
	
	if((transform.TransformPoint(qMesh.vertices[60]) - cameras.transform.position).magnitude < maxdist)
		{
			camTimeWithin += Time.deltaTime;
			
			if (camTimeWithin > 1f)
			{
			if (beginTesting == true)
			{
			  if(GeometryUtility.TestPlanesAABB(planes, renderer.bounds))
			  {
				if (scaleFactor < ((5/rad)*(512)))
				{
				SplitQuad();
				}
				}
			}
			}
			
			
			
		}
	if((transform.TransformPoint(qMesh.vertices[60]) - cameras.transform.position).magnitude > maxdist)
		{
			camTimeWithin = 0f;
		}
	if ((transform.TransformPoint(qMesh.vertices[60]) - cameras.transform.position).magnitude > 4f*maxdist)
		{
			
			if (beginTesting == true)
			{
			//lodHandler.deletedPlanes.Last().SetActive(true);
			//lodHandler.DoLod();
			if (cannotRevert == false)
			{
			sq1.SetActive(false);	//deactivates sibling quads to remove "scanning" effect
			sq2.SetActive(false);
			sq3.SetActive(false);
					
			if (((cameras.transform.position - planetPos).magnitude > 1.025f*rad))
					{
						parent.renderer.material = groundfromspace;
						parent.renderer.material.SetTexture("_DetailTex", ((Texture2D) Resources.Load("Models/Bump")));
						parent.renderer.material.SetTexture("_DetailTex2", ((Texture2D) Resources.Load("Models/Gbumb")));
						parent.renderer.material.SetFloat("_MultTest", (4096)/(Mathf.Pow(2, timesSplit-1)));
					}
					else
					{
						parent.renderer.material = groundFromGround;
						parent.renderer.material.SetTexture("_DetailTex", ((Texture2D) Resources.Load("Models/Bump")));
						parent.renderer.material.SetTexture("_DetailTex2", ((Texture2D) Resources.Load("Models/Gbumb")));
						parent.renderer.material.SetFloat("_MultTest", (4096)/(Mathf.Pow(2, timesSplit-1)));
					}
				
			parent.SetActive(true);
			gameObject.SetActive(false);
			
			
			}
			}
			
		}
		
		
	
		
		if (testmat == true)
		{
	if (((cameras.transform.position - planetPos).magnitude < 1.025f*rad) && fromground == false)
		{
			fromground = true;
			renderer.material = groundFromGround;
			renderer.material.SetTexture("_DetailTex", ((Texture2D) Resources.Load("Models/Bump")));
			renderer.material.SetTexture("_DetailTex2", ((Texture2D) Resources.Load("Models/Gbumb")));
			renderer.material.SetFloat("_MultTest", (4096)/(Mathf.Pow(2, timesSplit)));
		}
	if (((cameras.transform.position - planetPos).magnitude > 1.025f*rad) && fromground == true)
		{
			fromground = false;
			renderer.material = groundfromspace;
			renderer.material.SetTexture("_DetailTex", ((Texture2D) Resources.Load("Models/Bump")));
			renderer.material.SetTexture("_DetailTex2", ((Texture2D) Resources.Load("Models/Gbumb")));
			renderer.material.SetFloat("_MultTest", (4096)/(Mathf.Pow(2, timesSplit)));
		}
		}
	
		
		
		
		
		
		
		
		oldCamPos = cameras.transform.position;
		
	
	}
}
