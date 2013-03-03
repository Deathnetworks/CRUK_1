using UnityEngine;
using System.Collections;

public class scoremanager : MonoBehaviour {
	
	public float score = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void applyScore()
	{
		score+=Time.deltaTime;
	}
}
