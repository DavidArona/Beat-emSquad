using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingClouds : MonoBehaviour {

	public float smoothSpeed = 0.5f;
	public float stageLimit;

	private float startingPosition;

	void Start () {
		startingPosition = transform.position.x-transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 destinedPosition = new Vector3 (transform.position.x + 0.0001f, transform.position.y, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, destinedPosition, smoothSpeed);
		if (transform.position.x >= stageLimit) {
			transform.position = new Vector3 (startingPosition, transform.position.y, transform.position.z);
		}
	}
}
