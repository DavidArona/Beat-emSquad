using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetection : MonoBehaviour {
	
	EnemyController ec;

	void Awake() {
		ec = GetComponentInParent<EnemyController> ();
	}

	public void OnTriggerEnter (Collider other) {
		Debug.Log ("mec");
		if (other.gameObject.tag == "Player") {
			ec.inRange = true;
		}
	}

	public void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "Player") {
			ec.inRange = false;
		}
	}
}
