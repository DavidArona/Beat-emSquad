using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeThingy : MonoBehaviour {

	public void TokiwoTomare(float timeScale) {
		if (Time.timeScale == 1.0f) {
			Time.timeScale = 0.1f;
			StartCoroutine (waitForTimeNormalization ());
		}
	}

	IEnumerator waitForTimeNormalization () {
		yield return new WaitForSeconds (0.05f);
		Time.timeScale = 1f;
	}
}
