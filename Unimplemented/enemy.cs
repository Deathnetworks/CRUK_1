using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {
	
	public float modRate;
	public float speed;
	public float xOffset;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		Debug.Log (transform.position.ToString ());
		Vector3 pos = transform.position;
		float sinx = (float)Time.realtimeSinceStartup;
		Debug.Log (Time.frameCount.ToString ());
		pos.x = Mathf.Sin (sinx) * modRate;
		
		pos.x += xOffset;
		
		pos.z -= speed * Time.deltaTime;
		
		transform.position = pos;
	}
}
