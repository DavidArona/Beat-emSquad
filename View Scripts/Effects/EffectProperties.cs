using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectProperties : MonoBehaviour {

	int energyRaised= 0;
	WondieController pC;

	public void EndMyLifePlease(){
		Debug.Log ("Duty bounds meeself");
		DestroyObject (this.gameObject);
	}

	public void RaiseEnergy(){
		pC = GameObject.FindObjectOfType<WondieController> ();
		if (pC!=null) {
			energyRaised++;
			pC.GainKinergy (1);
			if (energyRaised >= 5) {
				EndMyLifePlease ();
			}
		}
	}
}
