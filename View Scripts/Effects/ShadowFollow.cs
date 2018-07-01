using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollow : MonoBehaviour {

	PlayerController wc;
	float actualDistance;
	float prevDistance = 0;
	float distanceDifference;
	Vector3 newVector;
	Vector3 ogScale;
	Color ogColor;
	Color newColor;
	SpriteRenderer sr;
	Quaternion ogRotation;

	// Use this for initialization
	void Start () {
		ogScale = this.transform.localScale;
		wc = GetComponentInParent<PlayerController> ();
		sr = GetComponent<SpriteRenderer> ();
		ogColor = sr.color;
		ogRotation = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.root.tag == "Player") {
			transform.position = new Vector3 (this.transform.position.x, 0.001f, this.transform.position.z);
			if (!wc.Grounded ()) {
				RaycastHit hit;
				Ray upRay = new Ray (transform.position, Vector3.up);
				if (Physics.Raycast (upRay, out hit)) {
					actualDistance = hit.distance;
				}
				distanceDifference = actualDistance - prevDistance; 
				prevDistance = actualDistance;
				if (distanceDifference != 0) {
					newColor = new Color (ogColor.r, ogColor.g, ogColor.b, ogColor.a - distanceDifference * 10);
					newVector = new Vector3 (ogScale.x - distanceDifference * 10, ogScale.y - distanceDifference * 10, ogScale.z);
					transform.localScale = Vector3.Lerp (transform.localScale, newVector, Time.deltaTime);
					sr.color = Color.Lerp (sr.color, newColor, Time.deltaTime);
				}
			} else {
				transform.localScale = ogScale;
				sr.color = ogColor;
			}
		} else {
			transform.position = new Vector3 (this.transform.parent.position.x, 0.001f, this.transform.parent.position.z);
			transform.rotation = ogRotation;
		}
	}
}
