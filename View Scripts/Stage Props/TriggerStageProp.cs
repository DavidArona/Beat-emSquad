using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStageProp : MonoBehaviour {

	Animator animator;
	public bool active = false;

	// Use this for initialization
	void Start () {
		animator = GetComponentInParent<Animator> ();
	}
	
	void OnTriggerEnter (Collider other) {
		if (other.transform.tag == "Player" || other.transform.tag == "Enemy") {
			animator.SetTrigger ("active");
			active = true;
		}
	}
}
