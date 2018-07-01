using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnAndOff : MonoBehaviour {
	
	public GameObject buttonOn;
	public GameObject buttonOff;

	public void On() {
		buttonOn.SetActive(true);
		buttonOff.SetActive (false);
	}

	public void Off() {
		buttonOn.SetActive(false);
		buttonOff.SetActive (true);
	}
}
