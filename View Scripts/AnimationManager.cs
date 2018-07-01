using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
	
	protected Animator animator;
	protected SpriteRenderer spriteRenderer;
	protected HitboxManager hitboxManager;

	public bool isSamurai = false;

	// Use this for initialization
	protected void Start () {
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		hitboxManager = GetComponent<HitboxManager> ();
	}

	public void ReadActions (ActionData actions) {

		//reset all triggers
		animator.ResetTrigger("useLight01");
		animator.ResetTrigger("useHeavy01");
		animator.ResetTrigger("useLight02");
		animator.ResetTrigger("useHeavy02");
		animator.ResetTrigger("useHeavy03");
		animator.ResetTrigger("useJumpLight");
		animator.ResetTrigger("startJumping");
		animator.ResetTrigger ("isDashing");
		animator.ResetTrigger ("isDead");
		animator.ResetTrigger ("hitSomething");

		if (actions.isChanged) {
			animator.SetTrigger ("isChanged");
		}

		//jumping and falling
		animator.SetBool ("isJumping", !actions.isGrounded);
		if (actions.startJumping) {
			animator.SetTrigger("startJumping");
		}
		if (!actions.isGrounded && actions.isFalling) {
			animator.SetBool ("isFalling", true);
		} else {
			animator.SetBool ("isFalling", false);
		}

		//to walk
		if (actions.isMoving == true) {
			animator.SetBool ("isWalking", true);
		} else {
			animator.SetBool ("isWalking", false);
		}

		if (actions.isDashing) {
			animator.SetTrigger ("isDashing");
		}

		//changing directions
		ChangeDirections (actions.isFacing, isSamurai);

		//to attack
		if (actions.attack == Attacks.Light01) {
			animator.SetTrigger ("useLight01");
		}

		if (actions.attack == Attacks.Light02) {
			animator.SetTrigger ("useLight02");
		}

		if (actions.attack == Attacks.Heavy01) {
			animator.SetTrigger ("useHeavy01");
		}

		if (actions.attack == Attacks.Heavy02) {
			animator.SetTrigger ("useHeavy02");
		}

		if (actions.attack == Attacks.Heavy03) {
			animator.SetTrigger ("useHeavy03");
		}

		if (actions.attack == Attacks.JumpLight) {
			if (!actions.isGrounded) {
				animator.SetTrigger ("useJumpLight");
			}
		}

		//charging
		animator.SetBool ("isChargingH", actions.isChargingH);
		animator.SetBool ("isChargingL", actions.isChargingL);
		animator.SetFloat ("ChargeTime", actions.heavyChargeTime);

		//barraging
		if (actions.isBarragingL) {
			animator.SetBool ("isBarragingL", true);
			animator.SetFloat ("BarrageSpeed", animator.GetFloat ("BarrageSpeed") + 0.02f);
		} else {
			animator.SetBool ("isBarragingL", false);
			if (!actions.isBarragingH) {
				animator.SetFloat ("BarrageSpeed", 1f);
			}
		}

		//barraging
		if (actions.isBarragingH) {
			animator.SetBool ("isBarragingH", true);
			animator.SetFloat ("BarrageSpeed", animator.GetFloat ("BarrageSpeed") + 0.02f);
		} else {
			animator.SetBool ("isBarragingH", false);
			if (!actions.isBarragingL) {
				animator.SetFloat ("BarrageSpeed", 1f);
			}
		}

		//dying
		animator.SetBool ("isDown", actions.isDown);

		animator.SetBool ("characterChange", actions.characterChanging);
	}

	public void ChangeCharacter (bool changeCharacter) {
		animator.SetBool ("characterChange", changeCharacter);
	}

	//different levels of pain
	public void PlayerHit (int dmgLevel) {
		if (dmgLevel == 1) {
			animator.SetTrigger ("dmgLow");
		}
		if (dmgLevel == 2) {
			animator.SetTrigger ("dmgMedium");
		}
		if (dmgLevel == 3) {
			animator.SetTrigger ("dmgHigh");
		}
		if (dmgLevel == 0) {
			animator.SetTrigger ("isDead");
		}
	}

	//if you hit something, something happens
	public void HitSomething (int hitSomething){
		animator.SetTrigger ("hitSomething");
	}

	public void IsChanged () {
		animator.SetTrigger ("isChanged");
	}

	//changing facing, extra fancy for the samurai dude
	void ChangeDirections (bool lookingRight, bool isSamurai) {
		if (!isSamurai) {
			if (lookingRight) {
				if (spriteRenderer.flipX == true) {	
					if (hitboxManager != null) {
						hitboxManager.reverseColliders ();
					}
				}
				spriteRenderer.flipX = false;
			} else {
				if (spriteRenderer.flipX == false) { 
					if (hitboxManager != null) {
						hitboxManager.reverseColliders ();
					}
				}
				spriteRenderer.flipX = true;
			}
		} else {
			if (lookingRight) {
				animator.SetBool ("lookingRight", true);
			} else {
				animator.SetBool ("lookingRight", false);
			}
		}
	}
}
