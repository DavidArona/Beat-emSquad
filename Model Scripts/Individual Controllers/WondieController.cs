using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WondieController : PlayerController {

	void OnEnable() {
		StartCoroutine (regenerateKinergy (1));
	}

	public override void comboReady (int comboNumber) {
		if (comboNumber == 2) {
			effectManager.SpawnEffect (EffectManager.Effects.stage2Wondie, this.transform);
		} else if (comboNumber == 3) {
			effectManager.SpawnEffect (EffectManager.Effects.stage3Wondie, this.transform);
		}
	}

	public override void CharacterChange (string direction)
	{
		isChanged = true;
		LoadNewScene sceneManager = FindObjectOfType<LoadNewScene> ();
		if (direction == "up") {
			sceneManager.CharacterChange ("Ronin", this.transform.position);
		}
		if (direction == "right" && facing == FacingDirection.East || direction == "left" && facing == FacingDirection.West) {
			sceneManager.CharacterChange ("X-42", this.transform.position);
		}
		if (direction == "right" && facing == FacingDirection.West || direction == "left" && facing == FacingDirection.East) {
			sceneManager.CharacterChange ("Captain Summers", this.transform.position);
		}
		StartCoroutine (waitForDestruction());
	}
}
