using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {
	
	public Texture2D[] levelPictures;
	private int currentLevel;
	public Texture2D backgroundTexture;
	
	// Use this for initialization
	void Start () {
		currentLevel = 0;
	}
	
	void OnGUI()
	{
		GUI.DrawTexture ( new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);
		
		if( GUI.Button (new Rect( 200, (Screen.height - 40), 50, 20), "<"))
		{
			if( currentLevel > 0 )
			{
				--currentLevel;
			}
		}
		
		if( GUI.Button( new Rect( Screen.width - 250, (Screen.height - 40), 50, 20), ">"))
		{
			if( currentLevel < levelPictures.Length - 1)
			{
				++currentLevel;
			}
		}
		
		GUI.Label ( new Rect(Screen.width/2, 10, 300, 50), "SELECT A LEVEL:");
		int picWidth = 300;
		int picHeight = 200;
		GUI.DrawTexture (new Rect((Screen.width/2)-(picWidth/2), (Screen.height/2)-(picHeight/2), picWidth, picHeight), levelPictures[currentLevel],ScaleMode.StretchToFill);
		
		if( GUI.Button ( new Rect( (Screen.width / 2) - 50, (Screen.height - 40), 100, 20 ), "START LEVEL"))
		{
			Debug.Log ( "Start Level Pressed");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
