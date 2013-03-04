using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	private UserInfo user;
	public Texture2D bgImg;
	public Texture2D playBtn;
	public Texture2D fbBtn;
	public GUIStyle myStyle;
	float fbOffset;
	public float fbHeight;
	public float fbWidth;
	float playOffset;
	
	SceneManager manager;
	// Use this for initialization
	void Start () {
		manager = transform.parent.gameObject.GetComponent<SceneManager>();
		
		fbOffset = 90f;
		
		playOffset = 171f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnGUI() {
        
        //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bgImg, ScaleMode.StretchToFill, true, 0.5F);
		
		float playTop, fbTop, playLeft, fbLeft;
		
		playTop = Screen.height * 0.75f - playOffset;
		fbTop = Screen.height * 0.75f - fbOffset;
		playLeft = Screen.width / 2f - 128f;
		fbLeft = Screen.width / 2f - 128f;
		
		if(GUI.Button(new Rect(playLeft, playTop, 256f, 32f), playBtn,myStyle)){
			manager.GetComponent<tutorialScript1>().enabled = true;
			enabled = false;
			
			
		}
		
		if(GUI.Button(new Rect(fbLeft, fbTop, 256f, 32f), fbBtn,myStyle)){
			Debug.Log("Facebook Yo!");
		}
    }
}

