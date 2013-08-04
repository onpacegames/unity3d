using UnityEngine;
using System.Collections;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

public class InitializePlanet : MonoBehaviour {
	
	
	//Planetary Control variables
	public Color mainColor;
	public float radius = 10;
	public int splitCount = 1;
	public bool hasOcean;
	public bool hasAtmo;
	
	Transform atmoFromGroundTransform;
	Transform atmoFromSpaceTransform;
	
	//Camera Stuff
	public Transform camTransform;
	float cameraDistance;
	
	
	//Quad properties and values
	GameObject q1;
	GameObject q2;
	GameObject q3;
	GameObject q4;
	GameObject q5;
	GameObject q6;
	float scalar;
	QuadPlane q1s;
	QuadPlane q2s;
	QuadPlane q3s;
	QuadPlane q4s;
	QuadPlane q5s;
	QuadPlane q6s;
	
	GameObject planeObject;
	
	//Noise properties
	public ModuleBase noise;
	public int Octaves;
	public float Frequency;
	public float Scalarv;
	public float noisemod = 1;
	public RidgedMultifractal mountainTerrain = new RidgedMultifractal();
	public Billow baseFlatTerrain = new Billow();
	public Perlin terrainType = new Perlin();
	
	//Atmospheric Scattering Variables
	public GameObject m_sun;
	public Material m_groundMaterial;
	public Material m_skyGroundMaterial;
	public Material m_skySpaceMaterial;
	public Vector3 planetPosition;
	public Material m_groundfromgroundMaterial;
	
	public float m_hdrExposure = 0.8f;
	public Vector3 m_waveLength = new Vector3(0.65f,0.57f,0.475f); // Wave length of sun light
	public float m_ESun = 20.0f; 			// Sun brightness constant
	public float m_kr = 0.0025f; 			// Rayleigh scattering constant
	public float m_km = 0.0010f; 			// Mie scattering constant
	public float m_g = -0.990f;				// The Mie phase asymmetry factor, must be between 0.999 to -0.999
	
	//Dont change these
	private const float m_outerScaleFactor = 1.025f; // Difference between inner and ounter radius. Must be 2.5%
	private float m_innerRadius;		 	// Radius of the ground sphere
	private float m_outerRadius;		 	// Radius of the sky sphere
	private float m_scaleDepth = 0.25f; 	// The scale depth (i.e. the altitude at which the atmosphere's average density is found)
	
	InitializePlanet planetInit;

	
	void Awake () {
		planetPosition = gameObject.transform.position;
		camTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
		GenerateNoise();
		planetInit = (InitializePlanet) gameObject.GetComponent(typeof(InitializePlanet));
	}
	
	public Vector3 SurfacePoint(float latitude, float longitude, float raddif, ModuleBase noi, GameObject planetObject)	//Allows objects to be placed with ease by useing lat, long(-180, 180)
	{
		
		 
		 GenerateNoise();
		 Mathf.Clamp(latitude, -90, 90);
		 Mathf.Clamp(longitude, -180, 180);
		 Vector3 spoint;
		 float lat = latitude * Mathf.PI/180;
    	 float lon = longitude * Mathf.PI/180;
		 float rad = radius;
    	 spoint.x = (-rad * Mathf.Cos(lat) * Mathf.Cos(lon));
    	 spoint.y =  (rad * Mathf.Sin(lat));
   		 spoint.z =  (rad * Mathf.Cos(lat) * Mathf.Sin(lon));
		 spoint = spoint + planetObject.transform.position;
		//cubedpoint = (cubizePoint2(spoint.normalized) * radius) + planetObject.transform.position;
		//((float) noi.GetValue((truevpos[i].normalized * radius) + planetPos)) * noisemod)
		//truevpos = position - planetpos
		//((truevpos[i].normalized * radius) + planetPos)
		//(float) noi.GetValue((truevpos[i].normalized * radius) + planetPos)) * noisemod)
		 Vector3 trueplanetPos = spoint - planetPosition;
		 
		 raddif = (float) noi.GetValue(spoint);
		 //Debug.Log(raddif);
		 rad = radius + (raddif * noisemod);
		 spoint.x = (-rad * Mathf.Cos(lat) * Mathf.Cos(lon));
    	 spoint.y =  (rad * Mathf.Sin(lat));
   		 spoint.z =  (rad * Mathf.Cos(lat) * Mathf.Sin(lon));
		 
		 //noi.GetValue(truevpos[i])) * noisemod)
		 //rad = (float) noise.GetValue((cubizePoint2(spoint.normalized) * radius) - transform.position);
		 //Debug.Log(rad);
	      
		
		return (spoint + planetObject.transform.position);
	}
	
	
	
		
	
	
	
	
	
	public Quaternion SurfaceRotation(Vector3 position)
	{
		//position = position;
		//return Quaternion.Euler(position - transform.position);
		// y - 270 x - 270
		
		return Quaternion.LookRotation(position - transform.position);
		
		
	}
	
	
	
	
	void Start () {
		m_sun = GameObject.Find("Sunlight");
		m_innerRadius = radius;
		m_outerRadius = m_innerRadius * m_outerScaleFactor;
		GenerateNoise();
		
		
		if (hasAtmo == true)
		{
			CreateAtmosphere();
		}
		else
		{
			m_groundfromgroundMaterial = new Material(Shader.Find("DiffusePlanet"));
			m_groundMaterial = new Material(Shader.Find("DiffusePlanet"));
		}
		CreateQuads();
		
	
	}
	
	void CreateQuads()
	{
		planeObject = (GameObject) Resources.Load("Prefabs/QuadPlane");
		
		m_sun = GameObject.Find ("Sunlight");
		float x = transform.position.x;
		float y = transform.position.y;
		float z = transform.position.z;
		scalar = 5/radius;
		
		q1 = (GameObject) Instantiate(planeObject);
		q1.transform.position = new Vector3(x+radius,y,z);
		q1.transform.rotation = Quaternion.Euler(0,0,270);
		q1s = (QuadPlane) q1.GetComponent(typeof(QuadPlane));
		q1s.scaleFactor = scalar;
		q1s.rad = radius;
		q1s.mod = (radius/2);
		q1s.noise = noise;
		q1s.cannotRevert = true;
		q1s.noisemod = noisemod;
		q1s.planetPos = transform.position;
		q1s.hasOcean = hasOcean;
		q1.name = "q1";
		q1s.groundfromspace = m_groundMaterial;
		q1s.m_sun = m_sun;
		
		 
		
		q2 = (GameObject) Instantiate(planeObject);
		q2.transform.position = new Vector3(x,y,z+radius);
		q2.transform.rotation = Quaternion.Euler(90,0,0);
		q2s = (QuadPlane) q2.GetComponent(typeof(QuadPlane));
		q2s.scaleFactor = scalar;
		q2s.rad = radius;
		q2s.mod = (radius/2);
		q2s.noise = noise;
		q2s.cannotRevert = true;
		q2s.noisemod = noisemod;
		q2s.planetPos = transform.position;
		q2s.hasOcean = hasOcean;
		q2.name = "q2";
		q2s.groundfromspace = m_groundMaterial;
		q2s.m_sun = m_sun;
		
		
		
		q3 = (GameObject) Instantiate(planeObject);
		q3.transform.position = new Vector3(x,y,z-radius);
		q3.transform.rotation = Quaternion.Euler(270,0,0);
		q3s = (QuadPlane) q3.GetComponent(typeof(QuadPlane));
		q3s.scaleFactor = scalar;
		q3s.rad = radius;
		q3s.mod = (radius/2);
		q3s.noise = noise;
		q3s.cannotRevert = true;
		q3s.noisemod = noisemod;
		q3s.planetPos = transform.position;
		q3s.hasOcean = hasOcean;
		q3.name = "q3";
		q3s.groundfromspace = m_groundMaterial;
		q3s.m_sun = m_sun;
		
		
		
		q4 = (GameObject) Instantiate(planeObject);
		q4.transform.position = new Vector3(x,y+radius,z);
		q4.transform.rotation = Quaternion.Euler(0,0,0);
		q4s = (QuadPlane) q4.GetComponent(typeof(QuadPlane));
		q4s.scaleFactor = scalar;
		q4s.rad = radius;
		q4s.mod = (radius/2);
		q4s.noise = noise;
		q4s.cannotRevert = true;
		q4s.noisemod = noisemod;
		q4s.planetPos = transform.position;
		q4s.hasOcean = hasOcean;
		q4.name = "q4";
		q4s.groundfromspace = m_groundMaterial;
		q4s.m_sun = m_sun;
		
		
		
		q5 = (GameObject) Instantiate(planeObject);
		q5.transform.position = new Vector3(x,y-radius,z);
		q5.transform.rotation = Quaternion.Euler(0,0,180);
		q5s = (QuadPlane) q5.GetComponent(typeof(QuadPlane));
		q5s.scaleFactor = scalar;
		q5s.rad = radius;
		q5s.mod = (radius/2);
		q5s.noise = noise;
		q5s.cannotRevert = true;
		q5s.noisemod = noisemod;
		q5s.planetPos = transform.position;
		q5s.hasOcean = hasOcean;
		q5.name = "q5";
		q5s.groundfromspace = m_groundMaterial;
		q5s.m_sun = m_sun;
		
		
		q6 = (GameObject) Instantiate(planeObject);
		q6.transform.position = new Vector3(x-radius,y,z);
		q6.transform.rotation = Quaternion.Euler(0,0,90);
		q6s = (QuadPlane) q6.GetComponent(typeof(QuadPlane));
		q6s.scaleFactor = scalar;
		q6s.rad = radius;
		q6s.mod = (radius/2);
		q6s.noise = noise;
		q6s.cannotRevert = true;
		q6s.noisemod = noisemod;
		q6s.planetPos = transform.position;
		q6s.hasOcean = hasOcean;
		q6.name = "q6";
		q6s.groundfromspace = m_groundMaterial;
		q6s.m_sun = m_sun;
		
		q1s.CalculatePositions();
		q2s.CalculatePositions();
		q3s.CalculatePositions();
		q4s.CalculatePositions();
		q5s.CalculatePositions();
		q6s.CalculatePositions();
		
		q1s.initSplit = true;
		q2s.initSplit = true;
		q3s.initSplit = true;
		q4s.initSplit = true;
		q5s.initSplit = true;
		q6s.initSplit = true;
		
		q1s.splitAgain = true;
		q2s.splitAgain = true;
		q3s.splitAgain = true;
		q4s.splitAgain = true;
		q5s.splitAgain = true;
		q6s.splitAgain = true;
		
		
		
		
		q1s.splitCount = splitCount;
		q1s.timesSplit = 0;
		
		q2s.splitCount = splitCount;
		q2s.timesSplit = 0;
	
		q3s.splitCount = splitCount;
		q3s.timesSplit = 0;
		
		q4s.splitCount = splitCount;
		q4s.timesSplit = 0;
		
		q5s.splitCount = splitCount;
		q5s.timesSplit = 0;
		
		q6s.splitCount = splitCount;
		q6s.timesSplit = 0;
		
		q1.transform.parent = gameObject.transform;
		q2.transform.parent = gameObject.transform;
		q3.transform.parent = gameObject.transform;
		q4.transform.parent = gameObject.transform;
		q5.transform.parent = gameObject.transform;
		q6.transform.parent = gameObject.transform;
		
		q1s.planetMaker = gameObject;
		q2s.planetMaker = gameObject;
		q3s.planetMaker = gameObject;
		q4s.planetMaker = gameObject;
		q5s.planetMaker = gameObject;
		q6s.planetMaker = gameObject;
		
		q1s.planeObject = planeObject;
		q2s.planeObject = planeObject;
		q3s.planeObject = planeObject;
		q4s.planeObject = planeObject;
		q5s.planeObject = planeObject;
		q6s.planeObject = planeObject;
		
		q1s.groundFromGround = m_groundfromgroundMaterial;
		q2s.groundFromGround = m_groundfromgroundMaterial;
		q3s.groundFromGround = m_groundfromgroundMaterial;
		q4s.groundFromGround = m_groundfromgroundMaterial;
		q5s.groundFromGround = m_groundfromgroundMaterial;
		q6s.groundFromGround = m_groundfromgroundMaterial;
		
		q1s.planetInit = planetInit;
		q2s.planetInit = planetInit;
		q3s.planetInit = planetInit;
		q4s.planetInit = planetInit;
		q5s.planetInit = planetInit;
		q6s.planetInit = planetInit;
		
		
		
		
		
		
		
	}
	
	void GenerateNoise()
	{
		mountainTerrain.OctaveCount = Octaves;
		mountainTerrain.Frequency = 2f;
		baseFlatTerrain.OctaveCount = Octaves;
		terrainType.OctaveCount = Octaves;
		Voronoi vnoise = new Voronoi();
		vnoise.Frequency = 5f;
		Perlin pnoise = new Perlin();
		pnoise.Frequency = 2f;
		
		Scale scaledvnoise = new Scale(Scalarv, Scalarv, Scalarv, vnoise);
		Scale scaledpnoise = new Scale(Scalarv, Scalarv, Scalarv, pnoise);
		
		
		baseFlatTerrain.Frequency = 2.0f;
		ScaleBias flatTerrain = new ScaleBias(0.125, -0.75, baseFlatTerrain);
		terrainType.Frequency = 0.5f;
		terrainType.Persistence = 0.25;
		Select noiseSelect = new Select(scaledpnoise, scaledvnoise, terrainType);
		
		
		Select terrainSelector = new Select(flatTerrain, mountainTerrain, terrainType);
		terrainSelector.SetBounds(0.0, 1000.0);
		terrainSelector.FallOff = 0.125;
		Turbulence finalTerrain = new Turbulence(0.25, terrainSelector);
		finalTerrain.Frequency = 4.0f;
		
		
		noise = new Scale(Scalarv, Scalarv, Scalarv, finalTerrain);
		//noise = new Add(new Scale(Scalarv, Scalarv, Scalarv, finalTerrain), noiseSelect);
		//noise = new Add(noise, scaledvnoise);
	}
	
	public float[] XYZ2LATON(Vector3 XYZ)
	{
		Vector3 grelativePos = XYZ - planetPosition;
		float latitude = (Mathf.Asin(planetPosition.y/radius)*180)/Mathf.PI;
		float LAT = latitude * Mathf.PI/180;
		float longitude = 0;
		
		float longitude1 = (180 * (Mathf.Asin((grelativePos.z)/(radius*Mathf.Cos(LAT)))))/Mathf.PI;	//There are two possible solutions for longitude, compare these by re-entering them to the XYZ formula
		float longitude2 = (180 * (Mathf.Acos((grelativePos.x)/(-radius*Mathf.Cos(LAT)))))/Mathf.PI;
		
		float LON1 = longitude1 * Mathf.PI/180;
		float LON2 = longitude2 * Mathf.PI/180;
		
		Vector3 testVector1 = new Vector3();
		testVector1.x = -radius * Mathf.Cos(LAT) * Mathf.Cos(LON1);
    	testVector1.y =  radius * Mathf.Sin(LAT);
    	testVector1.z =  radius * Mathf.Cos(LAT) * Mathf.Sin(LON1);
		testVector1 = testVector1 + planetPosition;
		
		Vector3 testVector2 = new Vector3();
		testVector2.x = -radius * Mathf.Cos(LAT) * Mathf.Cos(LON2);
    	testVector2.y =  radius * Mathf.Sin(LAT);
    	testVector2.z =  radius * Mathf.Cos(LAT) * Mathf.Sin(LON2);
		testVector2 = testVector2 + planetPosition;
		
		testVector1.x = Mathf.Round(testVector1.x);
		testVector1.y = Mathf.Round(testVector1.y);
		testVector1.z = Mathf.Round(testVector1.z);
		
		testVector2.x = Mathf.Round(testVector2.x);
		testVector2.y = Mathf.Round(testVector2.y);
		testVector2.z = Mathf.Round(testVector2.z);
		
		Vector3 testpos = new Vector3();
		testpos.x = Mathf.Round(XYZ.x);
		testpos.y = Mathf.Round(XYZ.y);
		testpos.z = Mathf.Round(XYZ.z);
		
		
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
		float[] latlon = new float[2];
		latlon[0] = latitude;
		latlon[1] = longitude;
		
		return latlon;
	}
	
	void CreateAtmosphere()
	{
		m_sun = GameObject.Find("Sunlight");
		Shader groundfromSpace = Shader.Find("Atmosphere/GroundFromSpace");
		Shader skyFromSpace = Shader.Find("Atmosphere/SkyFromSpace");
		Shader skyFromGround = Shader.Find("Atmosphere/SkyFromAtmosphere");
		
		m_skyGroundMaterial = new Material(Shader.Find("Atmosphere/SkyFromAtmosphere"));
		m_skySpaceMaterial = new Material(Shader.Find("Atmosphere/SkyFromSpace"));
		m_groundMaterial = new Material(Shader.Find("Atmosphere/GroundFromSpace"));
		m_groundfromgroundMaterial = new Material(Shader.Find("DiffusePlanet"));
		
		
		
		InitializeMaterial(m_groundMaterial, Vector3.one);
		InitializeMaterial(m_skyGroundMaterial, new Vector3(3f, 3f, 3f));
		InitializeMaterial(m_skySpaceMaterial, new Vector3(1.8f, 1.8f, 1.8f));
		InitializeMaterial(m_groundfromgroundMaterial, Vector3.one);
		
		GameObject atmoFromGround = (GameObject) Instantiate(Resources.Load("Prefabs/Atmosphere"));
		atmoFromGround.transform.position = planetPosition;
		atmoFromGround.transform.localScale = new Vector3(m_outerRadius, m_outerRadius, m_outerRadius);
		atmoFromGroundTransform = atmoFromGround.transform.Find("SphereObject");
		GameObject atmoFromSpace = (GameObject) Instantiate(Resources.Load("Prefabs/Atmosphere"));
		atmoFromSpace.transform.position = planetPosition;
		atmoFromSpace.transform.localScale = new Vector3(m_outerRadius, m_outerRadius, m_outerRadius);
		atmoFromSpaceTransform = atmoFromSpace.transform.Find("SphereObject");
		
		atmoFromGroundTransform.renderer.material = m_skyGroundMaterial;
		atmoFromSpaceTransform.renderer.material = m_skySpaceMaterial;	
		atmoFromGround.transform.parent = gameObject.transform;
		atmoFromSpace.transform.parent = gameObject.transform;
		
	}
		
	
	void InitializeMaterial(Material mat, Vector3 cshift)
	{

		Vector3 invWaveLength4 = new Vector3(1.0f / Mathf.Pow(m_waveLength.x, 4.0f), 1.0f / Mathf.Pow(m_waveLength.y, 4.0f), 1.0f / Mathf.Pow(m_waveLength.z, 4.0f));
		float scale = 1.0f / (m_outerRadius - m_innerRadius);
		
		mat.SetVector("_ColorShift", cshift);
		mat.SetVector("_PlanetPos", planetPosition);
		mat.SetVector("v3LightPos", m_sun.transform.forward*-1.0f);
		mat.SetVector("v3InvWavelength", invWaveLength4);
		mat.SetFloat("fOuterRadius", m_outerRadius);
		mat.SetFloat("fOuterRadius2", m_outerRadius*m_outerRadius);
		mat.SetFloat("fInnerRadius", m_innerRadius);
		mat.SetFloat("fInnerRadius2", m_innerRadius*m_innerRadius);
		mat.SetFloat("fKrESun", m_kr*m_ESun);
		mat.SetFloat("fKmESun", m_km*m_ESun);
		mat.SetFloat("fKr4PI", m_kr*4.0f*Mathf.PI);
		mat.SetFloat("fKm4PI", m_km*4.0f*Mathf.PI);
		mat.SetFloat("fScale", scale);
		mat.SetFloat("fScaleDepth", m_scaleDepth);
		mat.SetFloat("fScaleOverScaleDepth", scale/m_scaleDepth);
		mat.SetFloat("fHdrExposure", m_hdrExposure);
		mat.SetFloat("g", m_g);
		mat.SetFloat("g2", m_g*m_g);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (hasAtmo == true)
		{
			cameraDistance = ((camTransform.position) - planetPosition).magnitude;
			
			if ((cameraDistance > radius * 1.5f) && atmoFromGroundTransform.renderer.enabled == true)
			{
					atmoFromGroundTransform.renderer.enabled = false;
					atmoFromSpaceTransform.renderer.enabled = true;
			}
			if ((cameraDistance < radius * 1.5f) && atmoFromSpaceTransform.renderer.enabled == true)
			{
					atmoFromGroundTransform.renderer.enabled = true;
					atmoFromSpaceTransform.renderer.enabled = false;	
			}
		}
	
	}
}
