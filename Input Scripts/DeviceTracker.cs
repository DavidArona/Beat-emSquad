using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the DeviceTracker passes the information to the InputManager
//abstract class to customize different devices
[RequireComponent(typeof(InputManager))]
public abstract class DeviceTracker : MonoBehaviour {

	protected InputManager im;
	protected InputData data;
	protected bool newData;

	void Awake(){
		im = GetComponent<InputManager> ();
		data = new InputData (im.axisCount, im.buttonCount);
	}

	public abstract void Refresh ();
}
