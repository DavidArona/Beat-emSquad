using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeppersControl : BystanderController {
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		if (eM.encounterOngoing) {
			FinishIntro ();
			target = GameObject.FindGameObjectWithTag ("Player").transform.position;
			nav.SetDestination (target);
		}
	}
}
