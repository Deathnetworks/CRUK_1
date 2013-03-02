using UnityEngine;
using System.Collections;

public class control : MonoBehaviour {

	public float speed = 5.0f;
	public static float totalDistance;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		totalDistance += (speed * Time.deltaTime);
		Debug.Log(totalDistance);
	
	}
}
