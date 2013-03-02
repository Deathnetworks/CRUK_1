using UnityEngine;
using System.Collections;

public class ShipControl : MonoBehaviour {

	public float speed = 5.0f;
	public float xPosition = 0.0f;
	public static float totalDistance = 0;
	public static float shipSpeed = 0.10f;
	public float lerpSpeed = 0.2f;
	public float bounds =10.0f;
	private bool rightB;
	private bool leftB;
	// Use this for initialization
	void Start () {
		rightB = false;
		leftB = false;
		Debug.Log("start");
	}
	void OnCollisionEnter(Collision other)
	{
		Debug.Log("test");
		
	}
	void OnCollisionStay(Collision other)
	{
		Debug.Log("test");
	}
	void OnCollisionExit(Collision other)
	{
		Debug.Log("exit");
	}
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("enter");
	}
	void OnTriggerStay(Collider other)
	{
		Debug.Log("stay");
	}
	void OnTriggerExit( Collider other)
	{
		Debug.Log ("leave");
	}
	// Update is called once per frame
	void Update () {
		
		totalDistance += (speed * Time.deltaTime);
		 if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			float touchPos = Input.GetTouch(0).position.x/Screen.width - 0.5f;
            Vector2 touchDeltaPosition = Input.GetTouch(0).position;
			//Debug.Log(touchPos);
			Vector3 xVec = transform.position;
			xVec.x = Mathf.Lerp(transform.position.x, bounds* touchPos,lerpSpeed);
            transform.position = xVec;
			
		}
//		if(Input.GetAxis("Horizontal")!=0)
//		{
//			float touchPos = Input.GetTouch(0).position.x/Screen.width - 0.5f;
//            float touchDeltaPosition = Input.GetAxis("Horizontal");
//			Debug.Log(touchPos);
//			Vector3 xVec = transform.position;
//			xVec.x += touchDeltaPosition*Time.deltaTime;//Mathf.Lerp(transform.position.x, 10* touchPos,0.2f);
//            transform.position = xVec;
//		}

		//Debug.Log(xPosition);
	
	}

}
