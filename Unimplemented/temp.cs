using UnityEngine;
using System.Collections;

public class temp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.touchCount==2)
			Application.LoadLevel("scene1");
		if(Input.touchCount==3)
			Application.Quit();
	
	}
}
