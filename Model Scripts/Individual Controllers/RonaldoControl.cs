using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonaldoControl : BystanderController {

	public TriggerStageProp trashTrigger;
	public GameObject shadow;

	// Update is called once per frame
	public override void Update () {
		base.Update ();
		if (trashTrigger.active) {
			FinishIntro ();
		}
	}

	public override void FinishIntro ()
	{
		base.FinishIntro ();
		shadow.SetActive (true);
	}
}
