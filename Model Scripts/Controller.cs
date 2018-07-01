using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//template for future controllers
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour {

	public abstract void ReadInput(InputData data);

	protected Rigidbody rb;
	protected Collider coll;
	protected bool newInput;
	protected AnimationManager am;

	void Awake(){
		rb = GetComponent<Rigidbody>();
		am = GetComponent<AnimationManager> ();
		coll = GetComponent<Collider> ();
		Physics.IgnoreLayerCollision (8, 8);
	}

	protected void PassActions(ActionData actionData) {
		am.ReadActions (actionData);
	}
}
