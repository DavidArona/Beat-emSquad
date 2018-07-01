using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherits from DeviceTracker
public class KeyboardTracker : DeviceTracker {

	public AxisKeys[] axisKeys;
	public KeyCode[] buttonKeys;

	void Reset() { //set the length of the two arrays to match the InputManager
		im = GetComponent<InputManager>();
		axisKeys = new AxisKeys[im.axisCount];
		buttonKeys = new KeyCode[im.buttonCount];
	}

	public override void Refresh(){
		im = GetComponent<InputManager> ();

		//create 2 temp arrays for buttons and axes
		KeyCode[] newButtons = new KeyCode[im.buttonCount];
		AxisKeys[] newAxes = new AxisKeys[im.axisCount];

		if (buttonKeys != null) {
			for (int i = 0; i < Mathf.Min (newButtons.Length, buttonKeys.Length); i++) {
				newButtons [i] = buttonKeys [i];
			}
		}
		buttonKeys = newButtons;

		if (axisKeys != null) {
			for (int i = 0; i < Mathf.Min (newAxes.Length, axisKeys.Length); i++) {
				newAxes [i] = axisKeys [i];
			}
		}
		axisKeys = newAxes;
	}

	// Update is called once per frame
	void Update () {
		//check for inputs, if inputs detected, set newData to true
		//populate InputData to pass to the InputManager

		for (int i = 0; i < axisKeys.Length; i++) {
			float val = 0f;
			if (Input.GetKey (axisKeys [i].positive)) {
				val += 1f;
				newData = true;
			}
			if (Input.GetKey (axisKeys [i].negative)) {
				val -= 1f;
				newData = true;
			}
			if (Input.GetKeyUp (axisKeys [i].negative) && Input.GetKeyUp (axisKeys [i].positive)) {
				val = 0f;
				newData = true;
			}
			data.axes [i] = val;
		}

		for (int i = 0; i < buttonKeys.Length; i++) {
			if (Input.GetKey (buttonKeys [i])) {
				data.buttons [i] = true;
				newData = true;
			}
			if (Input.GetKeyUp (buttonKeys [i])) {
				data.buttons [i] = false;
				newData = true;
			}
		}

		if (newData) {
			im.PassInput (data);
			newData = false;
			data.Reset ();
		}
	}
}

[System.Serializable]
public struct AxisKeys { //to make an axis with two buttons, keyboard specific
	public KeyCode positive;
	public KeyCode negative;
}