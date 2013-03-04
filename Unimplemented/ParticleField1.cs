using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleField1 : MonoBehaviour
{
	string dataUrl = "http://generun.cloudapp.net/api/level?LevelName=S3_BAF_Chrom8&Width=1000&Height=325";
	private WWW dataRequest;
	private List<string> requestVariables = new List<string> ();
	private List<GameObject> energyObject = new List<GameObject> ();
	private List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
	byte[] bits;
	
	public Color particleColor;
	public float particleSize;
	public int collectorLength = 8;
	public int imageWidth = 1000;
	public int imageHeight = 325;
	public int particleScaleX = 10;
	public int particleScaleY = 10;
	public int scorePerParticle = 1;
	
	public void Start ()
	{
		Debug.Log ("Start");
		ParticleSystem ps = gameObject.GetComponent(typeof(ParticleSystem)) as ParticleSystem;
		ps.Stop();
		ps.Clear();
		dataRequest = new WWW (dataUrl);

		StartCoroutine (WaitForData ());
	}

	public int collectParticles(float xWorld, float yWorld)
	{
		int score = 0;
		// translate world coordinates to bitmap coordinates
		int y = (int)(xWorld * particleScaleX + imageHeight);
		int x = (int)(yWorld * particleScaleY + imageWidth / 2);
		
		int y0 = Mathf.Max(y - collectorLength, 0);
		int y1 = Mathf.Min(y + collectorLength, imageHeight - 1);
		
		int byteIndex0 = (x + y0 * imageWidth) / 8;
		int bitIndex0 = (x + y0 * imageWidth) % 8;
		
		int byteIndex1 = (x + y1 * imageWidth) / 8;
		int bitIndex1 = (x + y1 * imageWidth) % 8;
		
		for (int i=byteIndex0*8+bitIndex0; i<=byteIndex1*8+bitIndex1; ++i)
		{
			int byteIndex = i / 8;
			int bitIndex = i % 8;
			bool isOn = (bits[byteIndex] & (1 << bitIndex)) > 0 ? true : false;
			if (isOn)
			{
				score += scorePerParticle;	
			}
		}
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
		return new Color(particleColor.r, particleColor.g, particleColor.b, y/imageHeight);
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
					p1.position = new Vector3((y-imageHeight) / particleScaleY, (x-imageWidth/2) / particleScaleX, 0);
					p1.angularVelocity = 0;
					p1.velocity = new Vector3(0,0,0);
					p1.lifetime = 99999;
					p1.size = particleSize;
					p1.color = getColor(y);
					particles.Add(p1);

					ParticleSystem.Particle p2 = new ParticleSystem.Particle();	
					p2.position = new Vector3((imageHeight-y) / particleScaleY, (x-imageWidth/2) / particleScaleX, 0);
					p2.angularVelocity = 0;
					p2.velocity = new Vector3(0,0,0);
					p2.lifetime = 99999;
					p2.size = particleSize;
					p2.color = getColor(y);
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
