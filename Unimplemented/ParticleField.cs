using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleField : MonoBehaviour
{
	string dataUrl = "http://generun.cloudapp.net/api/level?LevelName=S3_BAF_Chrom8&Width=1000&Height=325";
	private WWW dataRequest;
	private List<string> requestVariables = new List<string> ();
	private List<GameObject> energyObject = new List<GameObject> ();
	private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
	public Dictionary<int,int> samples = new Dictionary<int, int>();
	//public KeyValuePair<int,int> samples = new KeyValuePair<int, int>();
	
	byte[] bits;
	
	public Color particleColor;
	public float particleSize;
	public float zOffset;
	
	public int collectorLength = 2;
	public int imageWidth = 1000;
	
	public int imageHeight = 325;
	public int particleScaleX = 10;
	public int particleScaleY = 10;
	public int scorePerParticle = 1;
	public SceneManager manager;
	public bool isInit;
	public void Start ()
	{
		isInit = false;
		Debug.Log ("Start");
		ParticleSystem ps = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		ps.Stop();
		ps.Clear();
		dataRequest = new WWW (dataUrl); //GetNextLeve
		manager = transform.parent.gameObject.GetComponent<SceneManager>();
		StartCoroutine (WaitForData ());
		//GetData(manager.GetNextLevel());
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+zOffset);
		isInit = true;
	}
	public void Init()
	{
		
			Debug.Log ("Start");
			ParticleSystem ps = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
			ps.Stop();
			ps.Clear();
			dataRequest = new WWW (dataUrl); //GetNextLeve
			manager = transform.parent.gameObject.GetComponent<SceneManager>();
			//StartCoroutine (WaitForData ());
			GetData(manager.GetNextLevel());
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z+zOffset);
		
		
	}
	public int x4 = 0;
	public Vector3 vector;
	public int collectParticles(float xWorld, float yWorld)
	{
		
		if(x4%100==0)
		{
			Vector3 xas= new Vector3(xWorld, 1, yWorld);
			vector = xas;
			
		}
		int score = 0;
		// translate world coordinates to bitmap coordinates
		int y = imageHeight - (int)(xWorld * particleScaleY);
		int x = (int)(yWorld * particleScaleX);
		if(x<0||x>=imageWidth||samples.ContainsKey(x))
			return 0;
		samples[x] = y;
	
		//Debug.Log(y);
		int y0 = Mathf.Max(y - collectorLength, 0);
		int y1 = Mathf.Min(y + collectorLength, imageHeight - 1);
		for (int i=y0; i<=y1; ++i)
		{
			int byteIndex = (i * imageWidth + x)/8;
			int bitIndex = (i * imageWidth + x)%8;
			if ((bits[byteIndex]&(1<<bitIndex)) > 0)
				score += scorePerParticle;
			
		}
/*		Debug.DrawLine (new Vector3((y0-imageHeight) / particleScaleY, 0, ((x-imageWidth/2) / particleScaleX)/2),
			new Vector3((y1-imageHeight) / particleScaleY, 0, ((x-imageWidth/2) / particleScaleX)/2 ), Color.red,10.0f);
		*/
		//Debug.Log("x: "+x.ToString()+" y: "+y.ToString() + " score " + score.ToString());
		return score;
	}

	public void setVariables (List<string> vars)
	{
		requestVariables = vars;
	}

	public List<string> getVariables ()
	{
		return requestVariables;
	}

	public void AddVariable (string varName, System.Object varValue)
	{

		requestVariables.Add (varName + "=" + varValue.ToString ());
	}

	public string getVarString ()
	{

		string fullString = "";
		for (int index=0; index < requestVariables.Count; ++index) {
			if (index == 0)
				fullString += "?";
			else
				fullString += "&";
   
			fullString += requestVariables [index];
   
		}

		return fullString;
	}
	
	public Color getColor(float y)
	{
		return new Color(particleColor.r * y/325, particleColor.g, particleColor.b, y/325);
	}
	public Color getColor(float x, float y)
       {
               float normalizedY = y / imageHeight;
               float nomralizedX = x / imageWidth;
               
               float fourValue = 4 * (nomralizedX);
               float red   = Mathf.Min(fourValue - 1.5f, -fourValue + 4.5f);
               float green = Mathf.Min(fourValue - 0.5f, -fourValue + 3.5f);
               float blue  = Mathf.Min(fourValue + 0.5f, -fourValue + 2.5f);
               float alpha = normalizedY;
               
               //Debug.Log("r: " + red.ToString() + " g: " + green.ToString() + " b: " + blue.ToString() + " a: " + alpha.ToString());
               return new Color(red, green, blue, alpha);
       }
	void Update()
	{
		//if(isInit);
		//Init();
	}
	public IEnumerator WaitForData ()
	{

		yield return dataRequest;
		bits = convertResponse (dataRequest.text);
		Debug.Log ("bits length:" + bits.Length.ToString ());
		int x = 0, y = 0;
		for (int index=0; index<bits.Length; ++index) {
			for (int i=0; i<8; ++i) {
				bool isOn = (bits [index] & (1 << i)) > 0 ? true : false;
				if (isOn) {
					ParticleSystem.Particle p1 = new ParticleSystem.Particle();	
					p1.position = new Vector3((y-imageHeight) / particleScaleY, 0, (x) / particleScaleX);
					p1.angularVelocity = 0;
					p1.velocity = new Vector3(0,0,0);
					p1.lifetime = 99999;
					p1.size = particleSize;
					p1.color = getColor(x,y);
					particles.Add(p1);

					ParticleSystem.Particle p2 = new ParticleSystem.Particle();	
					p2.position = new Vector3((imageHeight-y) / particleScaleY, 0, (x) / particleScaleX);
					p2.angularVelocity = 0;
					p2.velocity = new Vector3(0,0,0);
					p2.lifetime = 99999;
					p2.size = particleSize;
					p2.color = getColor(x,y);
					particles.Add(p2);
				}
				x++;
				if (x >= 1000) {
					x = 0;
					y++;
				}
			}
		}
		
		Debug.Log("Particles: " + particles.Count.ToString());
		ParticleSystem ps = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		ps.Clear();
		ps.Emit(particles.Count);
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[ps.particleCount];
		ps.GetParticles(array);
		for (int i=0; i<array.Length; ++i)
		{
			array[i].position = particles[i].position;
			array[i].size = particleSize;
			array[i].color = particles[i].color;
			array[i].velocity = new Vector3(0,0,0);
			array[i].angularVelocity = 0f;
		}
		
		ps.SetParticles(array, array.Length);
		
		Debug.Log(ps.particleCount);
	}
	
	public void GetData (byte[] bitInput)
	{		
		bits = bitInput;
		Debug.Log ("bits length:" + bits.Length.ToString ());
		int x = 0, y = 0;
		for (int index=0; index<bits.Length; ++index) {
			for (int i=0; i<8; ++i) {
				bool isOn = (bits [index] & (1 << i)) > 0 ? true : false;
				if (isOn) {
					ParticleSystem.Particle p1 = new ParticleSystem.Particle();	
					p1.position = new Vector3((y-imageHeight) / particleScaleY, 0, (x) / particleScaleX);
					p1.angularVelocity = 0;
					p1.velocity = new Vector3(0,0,0);
					p1.lifetime = 99999;
					p1.size = particleSize;
					p1.color = getColor(x,y);
					particles.Add(p1);

					ParticleSystem.Particle p2 = new ParticleSystem.Particle();	
					p2.position = new Vector3((imageHeight-y) / particleScaleY, 0, (x) / particleScaleX);
					p2.angularVelocity = 0;
					p2.velocity = new Vector3(0,0,0);
					p2.lifetime = 99999;
					p2.size = particleSize;
					p2.color = getColor(x,y);
					particles.Add(p2);
				}
				x++;
				if (x >= 1000) {
					x = 0;
					y++;
				}
			}
		}
		
		Debug.Log("Particles: " + particles.Count.ToString());
		ParticleSystem ps = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		ps.Clear();
		ps.Emit(particles.Count);
		ParticleSystem.Particle[] array = new ParticleSystem.Particle[ps.particleCount];
		ps.GetParticles(array);
		for (int i=0; i<array.Length; ++i)
		{
			array[i].position = particles[i].position;
			array[i].size = particleSize;
			array[i].color = particles[i].color;
			array[i].velocity = new Vector3(0,0,0);
			array[i].angularVelocity = 0f;
		}
		
		ps.SetParticles(array, array.Length);
		
		Debug.Log(ps.particleCount);
	}
	
	public byte[] convertResponse (string data)
	{
		Debug.Log (data.Replace ('"', ' ').Trim ().Length);
		return System.Convert.FromBase64String (data.Replace ('"', ' ').Trim ());        

	}	
}
