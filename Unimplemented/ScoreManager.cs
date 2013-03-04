using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	
	public int totalScore;
	public int score;
	// Use this for initialization
	void Start () {
	
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void addScore()
	{
		//score+=Time.deltaTime;
	}
	public int reportScore()
	{
		totalScore+=score;
		int tmp = score;
		//report
		resetScore();
		return tmp;
	}
	public void resetScore()
	{
		score = 0;
	}
	public void addScore(int score)
	{
		this.score+=score;
	}
	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width - Screen.width/3.0f, 0.0f, Screen.width/3.0f, Screen.height/7.0f),"Score: "+score.ToString());
			
	}
}
