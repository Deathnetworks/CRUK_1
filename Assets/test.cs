using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		collider.transform.localScale = new Vector3(1,1,5);
		
		//particleEmitter.
	
	
	}
	void OnCollisionEnter(Collision other)
	{
		Debug.Log("test");
	}
	// Updaste is called once per frame
	void Update () {
		Vector3 vec = transform.position;
		vec.z -= ShipControl.shipSpeed*Time.deltaTime*50;
		//gameObject.transform.position = vec;
		rigidbody.AddForce(new Vector3(0,0,-Time.deltaTime*50));
		
	
	}
}
