using UnityEngine;
using System.Collections;

public class UserInfo : MonoBehaviour {
	
	int userID;
	string userName;
	int highScore;
	
	
	// Use this for initialization
	void Start () {
		highScore = 12332;
		userName = "User.";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public string GetUsername()
	{
		return userName;
	}
}
