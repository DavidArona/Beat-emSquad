using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateStageProp : MonoBehaviour {

	Animator anim;
	public int randomFactor = 0;
	int randomNormalizer = 0;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
	}
	
	public void ActivateAnimation(){
		if (Random.Range (randomNormalizer, randomFactor+1) == randomFactor) {
			anim.SetTrigger ("active");
			randomNormalizer = 0;
		} else {
			randomNormalizer += 1;
		}
	}
}
