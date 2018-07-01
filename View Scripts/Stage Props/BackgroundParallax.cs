using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour {

	public Transform cameraTransform;
	public float parallaxSpeed;
	public float smoothSpeed = 0.5f;

	private float lastCameraX;

	// Use this for initialization
	void Start () {
		lastCameraX = cameraTransform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		float deltaX = cameraTransform.position.x - lastCameraX;
		Vector3 destinedPosition = transform.position + Vector3.right * (deltaX * parallaxSpeed);
		transform.position = Vector3.Lerp (transform.position, destinedPosition, smoothSpeed);
		lastCameraX = cameraTransform.position.x;
	}
}
