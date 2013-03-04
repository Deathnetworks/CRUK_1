using UnityEngine;
using System.Collections;

public class MirrorControl : MonoBehaviour {
	public bool movement;
	// Use this for initialization
	void Start () {
		movement = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		//TrailRenderer tr = GetComponent<TrailRenderer>();
		
		//tr.enabled = movement;
		Vector3 vec = GameObject.Find("Ship").transform.position;
		vec.x = -vec.x;
		vec.y = vec.y;
		Quaternion qt = GameObject.Find("Ship").transform.rotation;
		transform.position = vec;
		transform.rotation = Quaternion.Euler(qt.eulerAngles.x,qt.eulerAngles.y,-qt.eulerAngles.z);
	
	}
}
