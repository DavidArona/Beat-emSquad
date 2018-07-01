using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitStage : MonoBehaviour {

	LoadNewScene sceneManager;

	void Awake(){
		sceneManager = GetComponentInParent<LoadNewScene> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.transform.tag == "Player") {
			sceneManager.LoadScene ("continuingScene");
		}
	}
}
