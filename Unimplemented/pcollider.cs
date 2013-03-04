using UnityEngine;
using System.Collections;

public class pcollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void OnParticleCollision(GameObject other)
	{
		Debug.Log("particle");
	}
	// Update is called once per frame
	void Update () {
	
	}
}
