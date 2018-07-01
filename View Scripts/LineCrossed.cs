using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCrossed : MonoBehaviour {

	EncounterManager eM;
	public int stageNumber;
	public int encounterNumber;
	bool triggered = false;

	void Awake() {
		eM = GetComponentInParent<EncounterManager> ();
	}

	void OnTriggerEnter (Collider other) {
		if (!triggered) {
			if (other.transform.tag == "Player") {
				eM.encounterActive (stageNumber, encounterNumber);
				triggered = true;
			}
		}
	}
}
