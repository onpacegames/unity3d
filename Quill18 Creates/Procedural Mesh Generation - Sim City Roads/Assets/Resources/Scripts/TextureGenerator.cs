using UnityEngine;
using System.Collections;
using LibNoise.Unity;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

public class TextureGenerator : MonoBehaviour {
	
	public Texture2D generatedTexture;
	public ModuleBase noise;
	public int Octaves;
	public float Frequency;
	public float Scalarv;
	public float noisemod = 1;
	public RidgedMultifractal mountainTerrain = new RidgedMultifractal();
	public Billow baseFlatTerrain = new Billow();
	public Perlin terrainType = new Perlin();
	public Vector3 startpoint;
	public Vector3 endpoint;
	public int res;
	int xc = 1;
	int yc = 2;
	int zc = 3;
	
	Mesh qMesh;
	
	// Use this for initialization
	void Start () {
		
		qMesh = GetComponent<MeshFilter>().mesh;
		GenerateNoise();
		
		//generatedTexture = GenerateTexture(startpoint, endpoint, res, noise);
		generatedTexture = AltGenTex(qMesh.vertices, noise);
		
		renderer.material.mainTexture = generatedTexture;
		ApplyTerrain();
		
	}
	
	Texture2D AltGenTex(Vector3[] verts, ModuleBase module)
	{
		Texture2D tex = new Texture2D((int) Mathf.Sqrt(verts.Length), (int) Mathf.Sqrt(verts.Length));
		
		int reso = (int) Mathf.Sqrt(verts.Length);
		int pixelx = 0;
		int pixely = 0;
		
		for (int i = 0; i < verts.Length; i++) 
		{
			verts[i] = transform.TransformPoint(verts[i]);
			if (pixelx == reso)
				{
					pixelx = 0;
					pixely += 1;
				}
			float noiseval = (float) module.GetValue(verts[i]);
			noiseval = Mathf.Clamp(noiseval + 0.5f, 0f, 1f);
			Color pixelColor = new Color(noiseval, noiseval, noiseval, 0);
			tex.SetPixel(pixelx, pixely, pixelColor);
			tex.Apply();
			pixelx += 1;
		}
		
		tex.Apply();
		return tex;
	}
	
	Texture2D GenerateTexture(Vector3 start, Vector3 end, int resolution, ModuleBase noise)
	{
		Texture2D texture = new Texture2D(resolution, resolution); //since this is for square planes texture is always square
		texture.Apply();
		//Cases for planes on a cube
		Vector3 testpoint = start;
		float addamount;
		
		
		if (start.x == end.x) //Then get y and z
		{
			Debug.Log(xc);
			int pixelx = 0;
			int pixely = 0;
			addamount = Mathf.Abs(start.y - end.y)/ resolution; //Gets the amount to add to testpoint
			
			for (int i = 0; i < resolution*resolution; i++)
			{
				if (pixelx == resolution)
				{
					pixelx = 0;
					pixely += 1;
				}
				if (pixely == resolution)
				{
					//This shouldn't happen ever but if it does add code to end loop
				}	
				if (testpoint.y == end.y)
				{
					testpoint.y = start.y;
					testpoint.z += addamount;
				}
				if (testpoint.z == end.z)
				{
					//Shouldn't happen either, but in case add code to stop loop	
				}
				if (testpoint.y < end.y)
				{
					//Do the getting value and setting value on texture, then
					//addamount to tespoint.y and +=1 on pixelx
					float pixelval = (float) noise.GetValue(testpoint);
					pixelval = Mathf.Clamp(pixelval + 0.5f, 0f, 1f);
					Color pixelColor = new Color(pixelval, pixelval, pixelval, 0);
					texture.SetPixel(pixelx, pixely, pixelColor);
					texture.Apply();
					pixelx += 1;
					
					testpoint.y += addamount;	
				}
			}
		}
		
		if (start.y == end.y) //Then get x and z
		{
			Debug.Log(yc);
			int pixelx = 0;
			int pixely = 0;
			addamount = Mathf.Abs(start.x - end.x)/ resolution; //Gets the amount to add to testpoint
			
			for (int i = 0; i < resolution*resolution; i++)
			{
				if (pixelx == resolution)
				{
					pixelx = 0;
					pixely += 1;
				}
				if (pixely == resolution)
				{
					//This shouldn't happen ever but if it does add code to end loop
				}	
				if (testpoint.x == end.x)
				{
					testpoint.x = start.x;
					testpoint.z += addamount;
				}
				if (testpoint.z == end.z)
				{
					//Shouldn't happen either, but in case add code to stop loop	
				}
				if (testpoint.x < end.x)
				{
					//Do the getting value and setting value on texture, then
					//addamount to tespoint.y and +=1 on pixelx
					float pixelval = (float) noise.GetValue(testpoint);
					pixelval = Mathf.Clamp(pixelval + 0.5f, 0f, 1f);
					Color pixelColor = new Color(pixelval, pixelval, pixelval, 0);
					texture.SetPixel(pixelx, pixely, pixelColor);
					texture.Apply();
					pixelx += 1;
					
					testpoint.x += addamount;	
				}
			}
		}
		
		if (start.z == end.z) //Then get x and y
		{
			Debug.Log(zc);
			int pixelx = 0;
			int pixely = 0;
			addamount = Mathf.Abs(start.x - end.x)/ resolution; //Gets the amount to add to testpoint
			
			for (int i = 0; i < resolution*resolution; i++)
			{
				if (pixelx == resolution)
				{
					pixelx = 0;
					pixely += 1;
				}
				if (pixely == resolution)
				{
					//This shouldn't happen ever but if it does add code to end loop
				}	
				if (testpoint.x == end.x)
				{
					testpoint.x = start.x;
					testpoint.y += addamount;
				}
				if (testpoint.y == end.y)
				{
					//Shouldn't happen either, but in case add code to stop loop	
				}
				if (testpoint.x < end.x)
				{
					//Do the getting value and setting value on texture, then
					//addamount to tespoint.y and +=1 on pixelx
					float pixelval = (float) noise.GetValue(testpoint);
					pixelval = Mathf.Clamp(pixelval + 0.5f, 0f, 1f);
					Color pixelColor = new Color(pixelval, pixelval, pixelval, 0);
					texture.SetPixel(pixelx, pixely, pixelColor);
					texture.Apply();
					pixelx += 1;
					
					testpoint.x += addamount;	
				}
			}
		}
		
		
		return texture;
		
	}
	
	void GenerateNoise()
	{
		
		
		mountainTerrain.OctaveCount = Octaves;
		mountainTerrain.Frequency = 2f;
		baseFlatTerrain.OctaveCount = Octaves;
		terrainType.OctaveCount = Octaves;
		
		baseFlatTerrain.Frequency = 2.0f;
		ScaleBias flatTerrain = new ScaleBias(0.125, -0.75, baseFlatTerrain);
		terrainType.Frequency = 0.5f;
		terrainType.Persistence = 0.25;
		
		
		Select terrainSelector = new Select(flatTerrain, mountainTerrain, terrainType);
		terrainSelector.SetBounds(0.0, 1000.0);
		terrainSelector.FallOff = 0.125;
		Turbulence finalTerrain = new Turbulence(0.25, terrainSelector);
		finalTerrain.Frequency = 2.0f;
		
		
		noise = new Scale(Scalarv, Scalarv, Scalarv, finalTerrain);	
	}
	
	
	void ApplyTerrain()
	{
		Vector3[] vertices = qMesh.vertices;
		Vector3[] verticesN = qMesh.vertices;
		Vector3[] normals = qMesh.normals;
		Vector3[] truevpos = qMesh.vertices;
		
		
		
		for (int i = 0; i < vertices.Length; i++) 
		{
			
			truevpos[i] = (transform.TransformPoint(vertices[i]));
			truevpos[i].y = 0f;
			verticesN[i] = new Vector3(vertices[i].x, (float) noise.GetValue(truevpos[i]), vertices[i].z);
		}
		transform.rotation = Quaternion.Euler(0,0,0);
		qMesh.vertices = verticesN;
		qMesh.RecalculateNormals();
		qMesh.RecalculateBounds();
	}
		
	// Update is called once per frame
	void Update () {	

	}
}
