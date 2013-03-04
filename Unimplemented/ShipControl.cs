using UnityEngine;
using System.Collections;

public enum GameState{
	StartAnim,
	ChoosePath,
	Play,
	EndAnim,
	End,
};
	
public class ShipControl : MonoBehaviour {
	public Vector3 cameraInitialLocation;
	public Transform cameraTransform;
	public float speed = 5.0f;
	public float xPosition = 0.0f;
	public static float totalDistance = 0;
	public static float shipSpeed = 0.10f;
	public float swipeSpeed = 5.0f;
	//public static float shipSpeed = 0.00f;
	public float lerpSpeed = 0.2f;
	public float bounds =10.0f;
	public float score = 0.0f;
	private bool isTouched;
	private Vector2 touchPos, touchLast, touchMoved;
	private float touchMag = 0.0f;
	private float maxRot = Mathf.PI/2.0f;
	private bool newMode;
	private bool isStart;
	private Vector3 cameraInitialPos;
	private bool isCamMove;
	public GameState gameState;
	private SceneManager manager;
	private int rotDir;
	// Use this for initialization
	void Start () {
		gameState = GameState.StartAnim;
		cameraTransform.camera.fieldOfView = 90;
		cameraInitialPos = cameraTransform.position;
		manager = transform.parent.gameObject.GetComponent<SceneManager>();
		Debug.Log("start");
	}

	public int x = 0;
	void FixedUpdate()
	{
		if(x%10==0)
		{

		}
		x++;
	}
	
	void processMovement(float speed)
	{
		//Debug.Log(speed);
	}
	void rotZ()
	{
		//Vector3 rotV = ;
		
		float rotV = 0;
		Vector3 zAxis = new Vector3(0.0f, 0.0f, 1.0f);
		
		if(touchMag>0)
		{
			rotV = Mathf.Lerp(transform.rotation.z,maxRot,0.1f);
		}
		else if (touchMag==0)
		{
			rotV = Mathf.Lerp(transform.rotation.z,0,0.2f);
		}
		else
		{
			rotV = Mathf.Lerp(transform.rotation.z,-maxRot,0.1f);
		}
		transform.rotation = Quaternion.identity;
		transform.RotateAround(Vector3.up,Mathf.PI);
		transform.RotateAround(Vector3.forward,-rotV);
	}
	// Update is called once per frame
	void Init()
	{
		transform.position = new Vector3(0.0f,0.0f,-0.0f);
		cameraTransform.position = cameraInitialPos;
		gameState = GameState.StartAnim;
		cameraTransform.camera.fieldOfView = 90;
		cameraTransform.position = cameraInitialPos;
		manager = transform.parent.gameObject.GetComponent<SceneManager>();
		Debug.Log("start");
	}
	void ChoosePath()
	{
		if(Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);
				float posx = Mathf.Abs(touch.position.x/Screen.width - 0.5f);
				if(touch.phase == TouchPhase.Began)
					
				transform.position = new Vector3(32.5f*posx, transform.position.y,0 );
				gameState = GameState.Play;
			}
	}
	int count = 0;
	void StartAnim()
	{
		if(count>=4)
		gameState = GameState.ChoosePath;
		count++;
	}
	void PlayFoo()
	{
		elapsedTime = 0;
			float z0 = Mathf.Lerp(cameraTransform.position.z,transform.position.z -20.0f, 0.1f);
			cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, z0);
			cameraTransform.camera.fieldOfView = Mathf.Lerp(cameraTransform.camera.fieldOfView, 60.0f, 0.1f);
			var sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
			var o = GameObject.Find("Energy 1").GetComponent<ParticleField>() as ParticleField;
			sm.addScore(o.collectParticles(transform.position.x,transform.position.z));
			totalDistance += (speed * Time.deltaTime);
			touchMag = 0;			
			
			if(Input.touchCount == 1)
			{
				Touch touch = Input.GetTouch(0);
				float posz = Time.deltaTime * speed + transform.position.z;
				float posx = Mathf.Abs(touch.position.x/Screen.width - 0.5f);
				if(touch.phase == TouchPhase.Began)	{				
				transform.position = new Vector3(32.5f*posx, transform.position.y,posz );
				}	
				
				particleSystem.Play();
				GameObject.Find("ShipMirror").particleSystem.Play();
			}
				
			float xpos = transform.position.x;
			Vector3 posVec = transform.position;
			touchMag += Input.GetAxis("Horizontal")*Time.deltaTime;
			xpos += touchMag*swipeSpeed;
			xpos=Mathf.Clamp(xpos,0,bounds);
			posVec.x = xpos;
			posVec.z += Time.deltaTime * speed;
			transform.position = posVec;
			//rotZ();
		if(transform.position.z>=200.0f)
		{
			gameState = GameState.EndAnim;
		}
		}
	float elapsedTime;
	void EndAnim()
	{
		elapsedTime+=Time.deltaTime;
		Debug.Log("Endanim");
		transform.position = new Vector3(transform.position.x,transform.position.y,2.0f+transform.position.z) ;
		cameraTransform.position = new Vector3(cameraTransform.position.x,cameraTransform.position.y-1.0f,cameraTransform.position.z) ;
		if(elapsedTime>3.0f)
			gameState = GameState.End;
	}
	void End()
	{
		Debug.Log("End");
		var o = GameObject.Find("Energy 1").GetComponent<ParticleField>() as ParticleField;
		var sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>() as ScoreManager;
		Application.LoadLevel("scene1");
		int[] tx = new int[1000];
		for(int x = 0;x<1000;x++)
		{
			tx[x] = o.samples.ContainsKey(x)?o.samples[x] : 0;
		}
		
		
		manager.FinishedLevel(tx,sm.reportScore());
		o.Start();
		Init();
	}
	void Update () {
		
		switch(gameState)
		{
		case GameState.StartAnim:
			StartAnim();
			break;
		case GameState.ChoosePath:
			ChoosePath();
			break;
		case GameState.Play:
			PlayFoo();
			break;
		case GameState.EndAnim:
			EndAnim();
			break;
		case GameState.End:
			End ();
			break;
		}
		if(Input.GetKeyDown(KeyCode.A))
			gameState = GameState.End;
	}
	

}


				
				//Touch touch = Input.GetTouch(0);
	//			if(touch.phase == TouchPhase.Began)
	//			{
	//				touchMoved = Vector2.zero;
	//				touchLast = Vector2.zero;
	//				touchPos = touch.position;	
	//				touchMag = 0.0f;
	//			}
	//			else if(touch.phase == TouchPhase.Moved)
	//			{
	//				touchMoved = touch.position - touchPos;
	//				touchLast = touchPos;
	//				touchPos = touch.position;
	//				touchMag = touchMoved.x / Screen.width;
	//			}
	//			else if(touch.phase == TouchPhase.Stationary)
	//			{
	//				touchLast = touchPos;
	//				touchPos = touch.position;
	//				touchMag = 0.0f;
	//			}
	//			else if(touch.phase == TouchPhase.Ended||touch.phase == TouchPhase.Canceled)
	//			{
	//				touchMag = 0.0f;
	//			}
