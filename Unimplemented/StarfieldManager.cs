using UnityEngine;
using System.Collections.Generic;

public class StarfieldManager : MonoBehaviour {

	public Transform prefab;
	public int numberOfObjects;
	public float recycleOffset;
	public float speedMultiplier = 1;
	public Vector3 minSize, maxSize;

	private Vector3 nextPosition;
	private Queue<Transform> objectQueue;
	

	void Start () {
		//Debug.Log("Start init");
		objectQueue = new Queue<Transform>(numberOfObjects);
		
		for(int i = 0; i < numberOfObjects; i++){
			objectQueue.Enqueue((Transform)Instantiate(prefab));
			//Debug.Log( objectQueue.Peek());
		}
		
		nextPosition = transform.localPosition;
		for(int i = 0; i < numberOfObjects; i++){
			Recycle();
		}
	}
	void Update () {
		foreach ( Transform t in objectQueue )
		{
			Vector3 t1 = t.position;
			t1.z -= ShipControl.shipSpeed*speedMultiplier;
			t.position = t1;
		}

		if(objectQueue.Peek().localPosition.z + recycleOffset < 0.0f){
			Recycle();
		}
	}

	private void Recycle () {
		Vector3 scale = Vector3.one;

		Vector3 position = nextPosition;
		position.x += scale.x * 0.5f;
		position.y += scale.y * 0.5f;

		Transform o = objectQueue.Dequeue();
		
		o.localScale = scale;
		o.localPosition = position;
		nextPosition.x += scale.x;
		objectQueue.Enqueue(o);
	}
}