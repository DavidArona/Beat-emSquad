using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoninController : PlayerController {

	public int consecutiveHits = 0;
	public bool successfulHit = false;

	public override void comboReady (int comboNumber) {
		if (comboNumber == 2) {
			if (characterName == "Ronin") {
				if (facing == FacingDirection.East) {
					effectManager.SpawnEffect (EffectManager.Effects.stage2RoninRight, this.transform);
				} else {
					effectManager.SpawnEffect (EffectManager.Effects.stage2RoninLeft, this.transform);
				}
			}
		} else if (comboNumber == 3) {
			if (characterName == "Ronin") {
				if (facing == FacingDirection.East) {
					effectManager.SpawnEffect (EffectManager.Effects.stage3RoninRight, this.transform);
				} else {
					effectManager.SpawnEffect (EffectManager.Effects.stage3RoninLeft, this.transform);
				}
			}
		}
	}

	public void spawnVanish() {
		effectManager.SpawnEffect (EffectManager.Effects.vanish, this.transform);
	}

	public override void CharacterChange (string direction)
	{
		isChanged = true;
		LoadNewScene sceneManager = FindObjectOfType<LoadNewScene> ();
		if (direction == "up") {
			sceneManager.CharacterChange ("The Wonder", this.transform.position);
		}
		if (direction == "right" && facing == FacingDirection.West || direction == "left" && facing == FacingDirection.East) {
			sceneManager.CharacterChange ("Captain Summers", this.transform.position);
		}
		if (direction == "right" && facing == FacingDirection.East || direction == "left" && facing == FacingDirection.West) {
			sceneManager.CharacterChange ("X-42", this.transform.position);
		}
		StartCoroutine (waitForDestruction());
	}

	public override void attackOngoing (Attacks ongoingAttack) {
		attack = Attacks.None;
		if (ongoingAttack != Attacks.Light02) {
			if (ongoingAttack == Attacks.Light01) {
				comboReady (2);
				light02Ready = true;
				heavy02Ready = true;
			}
		} else if (ongoingAttack == Attacks.Light02) {
			comboReady (3);
			heavy03Ready = true;
		}
		if (successfulHit) {
			successfulHit = false;
			consecutiveHits++;
			GainKinergy (consecutiveHits*2);
		} else {
			consecutiveHits = 0;
		}
	}
}
