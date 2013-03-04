using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour {
	
	public int levelQueueSize;
	public Queue<DataLevel> loadedLevels;
	public DataLevel currentLevel {get; set;}
	public Queue<string> masterLevelList {get; set;}
	public bool isInitalized {get; set;}
	LevelLoader loader;
	// ResultReporter reporter;
	
	
	
	// Use this for initialization
	void Start () {
		levelQueueSize = 4;
		loadedLevels = new Queue<DataLevel>();
		masterLevelList = new Queue<string>();
		loader = GetComponent<LevelLoader>();
		loader.SetWidth(1000);
		loader.SetHeight(325);
		currentLevel = null;
		isInitalized = false;
		StartCoroutine(Initialize());
	}
	
	// Update is called once per frame
	void Update () {
		if(isInitalized){
			
			if(currentLevel == null){
				
				if(loadedLevels.Count > 0){
					SetNextLevel();
				}
			}
		}
		
	
	}
	
	IEnumerator Initialize(){
		Debug.Log("Entering Initialize()");
		// get id list
		yield return StartCoroutine(loader.GetLevelList());
		
		// get levels
		for(int index = 0; index < levelQueueSize; ++index){
			
			yield return StartCoroutine(loader.GetLevel(masterLevelList.Dequeue()));
		}
		
		foreach(DataLevel level in loadedLevels){
			Debug.Log("Loaded Level: " + level.name);
		}
		Debug.Log("Exiting Initialize()");
	}
	
	public UserInfo GetUser() 
	{
		return GetComponentInChildren<UserInfo>();
	}
	
	private void SetNextLevel(){
	
		if(loadedLevels.Count < 1){
			//Game Over	
		}
		// pop next up off the List
		currentLevel = loadedLevels.Dequeue();
		Debug.Log("CurrentLevel name: " + currentLevel.name);
		
		// ask loader for another
		// loader.getLevel(1);
	}
	
	public void FinishedLevel(int[] line, int score){
		// fill in DataLevel
		currentLevel.theLine = line;
		currentLevel.score = score;
		// push to Reporter
		// null currentLevel
		 try {
			Debug.Log(currentLevel.name);
       WWWForm form = new WWWForm();
       form.AddField("userName","517149358");
       form.AddField("Access_Token", "AAADLwDo6ftEBAJiBsZBnruP74Ot3QtD2XDTjLd2044bnnxFdQ1iy3hefpnypjTOOo4h4lEyNRP2Jbm7cbMvcjEUj3jZBzqAJqTNU5ErQZDZD");
       form.AddField("score", score);
       form.AddField("level", "S3_BAF_Chrom8");

       for (int i=0; i<line.Length; ++i)
               form.AddField("classificationResult[]", line[i]);
       WWW www = new WWW("http://generun.cloudapp.net/api/results",form);
			Debug.Log("http");
		}
		
catch 
{
				
}
		currentLevel = null;
		SetNextLevel();		
	}
	
	public byte[] GetNextLevel(){
		
		return currentLevel.data;
	}
	
	public void DisplayCurrentLevel(){
		if(currentLevel == null){
			SetNextLevel();
		}
		Debug.Log("Current Level Name: " + currentLevel.name);
	}
}
