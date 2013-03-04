using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	private UserInfo user;
	
	// Use this for initialization
	void Start () {
		user = gameObject.GetComponentInChildren<UserInfo>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		
//		if (GUI.Button (new Rect (10,10,150,100), "Press me, " + user.GetUsername())) {
//			GetComponent<SceneManager>().DisplayCurrentLevel();
	//	}
	}
	
	private bool GetMyUser()
	{
		//SceneManager parent = (SceneManager)transform.parent.gameObject;
		//user = parent.GetUser();
		user = transform.parent.gameObject.GetComponentInChildren<UserInfo>();
		
		if(null == user)
			return false;
		
		return true;
	}
}
