using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummersController : PlayerController {

	Animator animator;
	public bool isBlocking = false;

	void OnEnable() {
		StartCoroutine (regenerateKinergy (1));
	}

	public override void comboReady (int comboNumber) {
		if (comboNumber == 2) {
			effectManager.SpawnEffect (EffectManager.Effects.stage2Summers, this.transform);
		} else if (comboNumber == 3) {
			effectManager.SpawnEffect (EffectManager.Effects.stage3Summers, this.transform);
		}
	}

	public void BlockStart () {
		isBlocking = true;
	}

	public void BlockFinish () {
		Animator animator = GetComponent <Animator> ();
		animator.ResetTrigger ("attackBlocked");
		isBlocking = false;
	}

	public override bool receivedHit (int damageReceived, float stunTime, bool front, Transform transform) {
		if (!stunned) {
			if (!isBlocking) {
				remainingHP -= damageReceived;
				if (remainingHP <= 0) {
					DestroyPlayer (front);
					return true;
				}
				stunned = true;
				StartCoroutine (inmuneTimer (stunTime));
				camera.shakeDuration = damageReceived / 30f;
				camera.enabled = true;
				if (damageReceived > 10) {
					if (facing == FacingDirection.East && transform.position.x < this.transform.position.x) {
						reverseFacing ();
					} else if (facing == FacingDirection.West && transform.position.x > this.transform.position.x) {
						reverseFacing ();
					}
					am.PlayerHit (3);
					isDown = true;
				} else if (damageReceived <= 10 && damageReceived > 5) {
					am.PlayerHit (2);
				} else {
					am.PlayerHit (1);
				}
				Time.timeScale = 0.20f / damageReceived;
				GainKinergy (5);
				Debug.Log ("Remaining Health: " + remainingHP);
			} else {
				GainKinergy (1);
				if (facing == FacingDirection.East) {
					effectManager.SpawnEffect (EffectManager.Effects.block, this.transform);
				} else {
					effectManager.SpawnEffect (EffectManager.Effects.blockLeft, this.transform);
				}
				Animator animator = GetComponent <Animator> ();
				animator.SetTrigger ("attackBlocked");
			}
			return true;
		} else {
			return false;
		}
	}

	public void spawnShockwave() {
		if (facing == FacingDirection.East) {
			effectManager.SpawnEffect (EffectManager.Effects.summersHeavy03ShockwaveR, this.transform);
		} else if (facing == FacingDirection.West) {
			effectManager.SpawnEffect (EffectManager.Effects.summersHeavy03ShockwaveL, this.transform);
		}
	}

	public override void CharacterChange (string direction)
	{
		isChanged = true;
		LoadNewScene sceneManager = FindObjectOfType<LoadNewScene> ();
		if (direction == "up") {
			sceneManager.CharacterChange ("X-42", this.transform.position);
		}
		if (direction == "right" && facing == FacingDirection.East || direction == "left" && facing == FacingDirection.West) {
			sceneManager.CharacterChange ("The Wonder", this.transform.position);
		}
		if (direction == "right" && facing == FacingDirection.West || direction == "left" && facing == FacingDirection.East) {
			sceneManager.CharacterChange ("Ronin", this.transform.position);
		}
		Time.timeScale = 1f;
		StartCoroutine (waitForDestruction());
	}
}
