using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour {

	string dataUrl = "http://generun.cloudapp.net/api/level?";
	//string dataUrl = "http://generun.cloudapp.net/api/level?LevelName=S3_BAF_Chrom11&Width=1000&Height=325";
	string listUrl = "http://generun.cloudapp.net/api/level";
	private WWW dataRequest,levelRequest;
	private DataLevel tempLevel;
	private List<string> requestVariables = new List<string>();
	int width, height;
	SceneManager manager;
	
	public void Start()
	{
		manager = GetComponent<SceneManager>();
		//GetLevelList();
		//dataRequest = new WWW(dataUrl);
		
		//StartCoroutine(WaitForData());
	}
	
	public IEnumerator GetLevel(string levelName){
		Debug.Log("Entering GetLevel()");
		tempLevel = new DataLevel();
		tempLevel.name = levelName;
		string myRequestString = dataUrl + "LevelName=" + levelName +"&Width=" + width + "&Height=" + height;
		Debug.Log(myRequestString);
		levelRequest = new WWW(myRequestString);
		//levelRequest = new WWW(dataUrl);
		yield return StartCoroutine(WaitForData());
		
		manager.loadedLevels.Enqueue(tempLevel);
		
		Debug.Log("levels loaded: " + manager.loadedLevels.Count.ToString());
	}
	
	public IEnumerator GetLevelList(){
		Debug.Log("Entering GetLevelList()");
		dataRequest = new WWW(listUrl);
		yield return StartCoroutine(GetListFromServer());
	}
	
	public IEnumerator GetListFromServer(){
		Debug.Log("Entering GetListFromServer()");
		yield return dataRequest;
		if(dataRequest.error != null)
			print ("Error handling request: " + dataRequest.error);
		
		string[] letsTry = dataRequest.text.Split(',');
		
		foreach(string ohai in letsTry)
		{
			string tempString = ohai;
			tempString = tempString.Replace("\"","");
			tempString = tempString.Replace("[","");
			tempString = tempString.Replace("]","");
			
			manager.masterLevelList.Enqueue(tempString);
		}
		
		Debug.Log ("Length of levelList: " + manager.masterLevelList.Count.ToString());
		// parse the XML
		
	}

	
	public void AddVariable(string varName, System.Object varValue){
	
		requestVariables.Add(varName + "=" + varValue.ToString());
	}
	
	public string getVarString(){
		
		string fullString = "fo";
		for(int index=0; index < requestVariables.Count; ++index){
			if(index == 0)
				fullString += "?";
			else
				fullString += "&";
			
			fullString += requestVariables[index];	
		}
		
		return fullString;
	}
		
	
	public IEnumerator WaitForData(){
		Debug.Log("Entering WaitForData()");
		yield return levelRequest;
		byte[] bits = convertResponse(levelRequest.text);
		
		tempLevel.data = bits;
		manager.isInitalized = true;
	}
	
	public void SetWidth(int w){
		width = w;
	}
	
	public void SetHeight(int h){
		height = h;
	}
	
	public int GetWidth(){
		return width;
	}
	
	public int GetHeight(){
		return height;
	}
	
	public byte[] convertResponse(string data) {
		Debug.Log(data.Replace('"',' ').Trim().Length);
		return System.Convert.FromBase64String(data.Replace('"',' ').Trim());	
		
	}
	
}
