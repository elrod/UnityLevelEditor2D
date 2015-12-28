using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public float minX = -10f;
	public float maxX = 10f;

	public float speed = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 targetPos = transform.position;
		float screenAspect = (float) Screen.width / (float) Screen.height;
		float camHalfHeight = Camera.main.orthographicSize;
		float camHalfWidth = screenAspect * camHalfHeight;

		targetPos = targetPos + (Vector3.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime);

		if(targetPos.x - camHalfWidth <= minX){
			targetPos.x = minX + camHalfWidth;
		}
		if(targetPos.x + camHalfWidth >= maxX){
			targetPos.x = maxX - camHalfWidth;
		}

		transform.position = targetPos;

	}
}
