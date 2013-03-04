using UnityEngine;
using System.Collections;

public class tutorialScript1 : MonoBehaviour {
	
	public Texture2D[] tutorialTextures;
	private int currentPage;
	
	// Use this for initialization
	void Start () {
		currentPage = 0;
	}
	
	void OnGUI()
	{
		GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), tutorialTextures[currentPage]);
		
	}
	
	// Update is called once per frame
	void Update () {		
		//if( ( Input.touches.Length > 0 ) && ( Input.GetTouch(0).phase ==TouchPhase.Ended))//tapCount == 0 ))
		if(Input.GetMouseButtonUp (0))
		{
			currentPage++;
			if( currentPage >= 3 )
			{
				Debug.Log ( "LOAD GAME");
				currentPage = 0;
				this.enabled = false;
			}
		}
		
	}
}
