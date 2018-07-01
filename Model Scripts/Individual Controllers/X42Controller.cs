using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X42Controller : PlayerController {

	bool heavy02ComboReady = false;
	public GameObject projectileKinergyBullets;
	public GameObject projectileDualBullets;
	public GameObject nuke;
	public Animator animator;
	float verticalOffset = 0.008f;
	bool overcharged;
	SpriteRenderer sR;
	Color oldColor;
	Color newColor = Color.magenta;
	int bulletCost = 0;
	bool instaDeathActive = true;

	public override void LateUpdate ()
	{
		base.LateUpdate ();
		if (currentKinergy == 100) {
			if (!overcharged) {
				StartCoroutine (overchargedDamage ());
			}
		}
		if (overcharged)
			sR.color = Color.Lerp (oldColor, Color.magenta, Mathf.PingPong (Time.time, 0.5f));
	}

	void OnEnable() {
		StartCoroutine (regenerateKinergy (0.1f));
		sR = GetComponent<SpriteRenderer> ();
		oldColor = sR.color;
	}

	public override void ReadInput (InputData data)
	{
		prevWalkVelocity = walkVelocity;
		ResetMovementToZero ();

		//set vertical movement
		if (data.axes [0] != 0f && !stunned && !movementLock) {
			if (!changingCharacter) {
				walkVelocity += Vector3.forward * data.axes [0] * newWalkSpeed;
				if (!movementPushed) {
					movementPushed = true;
				}
			} else {
				if (data.axes [0] > 0f) {
					CharacterChange ("up");
				}
			}
		}

		//set horizontal movement
		if (data.axes [1] != 0f && !stunned && !movementLock) {
			if (!changingCharacter) {
				walkVelocity += Vector3.right * data.axes [1] * newWalkSpeed;
				if (!movementPushed) {
					movementPushed = true;
				}
			} else {
				if (data.axes [1] > 0f) {
					CharacterChange ("right");
				}
				if (data.axes [1] < 0f) {
					CharacterChange ("left");
				}
			}
		}

		//set vertical jump
		if (data.buttons [0] == true && attack == Attacks.None && !stunned) {
			if (jumpPressTime == 0f) {
				if (Grounded ()) {
					startJumping = true;
					if (walkVelocity != Vector3.zero) {
						Jump (jumpSpeed);
					}
				}
			}
			jumpPressTime += Time.deltaTime;
		} else {
			jumpPressTime = 0f;
		}

		//use attacks
		if (!isDown) {
			if (data.buttons [1] == true && !stunned) {
				if (Grounded () && attack == Attacks.None && !prevLButtonPush) {
					if (!light02Ready) {
						attack = Attacks.Light01;
						restartCombos ();
						ResetMovementToZero ();
					} else {
						attack = Attacks.Light02;
						restartCombos ();
						ResetMovementToZero ();
					}
				} else if (!Grounded () && attack == Attacks.None && !prevLButtonPush) {
					attack = Attacks.JumpLight;
				}
				if (isChargingL) {
					lightChargeTime += Time.deltaTime;
				} else { //si se acaba el ataque
					lightChargeTime = 0f;
				} //si sueltas el boton
				prevLButtonPush = true;
			} else { 
				isMovingAttackL = false;
				lightChargeTime = 0f;
				isChargingL = false;
				if (isBarragingL && prevLButtonPush) {
					barrageCounterL += 1;
				} else if (isBarragingL && !prevLButtonPush && !movingWhileAttacking) {
					isBarragingL = false;
				}
				prevLButtonPush = false;
			}

			if (data.buttons [2] == true && !stunned) {
				if (Grounded () && attack == Attacks.None && !prevHButtonPush) {
					if (!heavy03Ready) {
						if (!heavy02Ready) {
							if (heavy02ComboReady) {
								heavy02ComboReady = false;
								animator.SetTrigger ("useHeavy02Combo");
								LoseKinergy (50);
								restartCombos ();
								ResetMovementToZero ();
							}
							else {
								UseAttack (Attacks.Heavy01);
							}
						} else {
							heavy02ComboReady = true;
							UseAttack (Attacks.Heavy02);
						}
					} else {
						UseAttack (Attacks.Heavy03);
					}
				}
				if (isChargingH) {
					heavyChargeTime += Time.deltaTime;
				} else { //si se acaba el ataque
					heavyChargeTime = 0f;
				} //si sueltas el boton
				prevHButtonPush = true;
			} else { 
				heavyChargeTime = 0f; 
				isChargingH = false;
				if (isBarragingH && prevHButtonPush) {
					barrageCounterH += 1;
				} else if (isBarragingH && !prevHButtonPush && !movingWhileAttacking) {
					isBarragingH = false;
				}
				prevHButtonPush = false;
			}
		} else {
			if (data.buttons [1] == true || data.buttons [2] == true) {
				isDown = false;
				StartCoroutine (waitForRecover());
			}
		}

		if (data.buttons [4] == true && attack == Attacks.None && !stunned && currentKinergy >= 60 && !changingCharacter) {
			currentKinergy = 0;
			StartChangingCharacter ();
			Debug.Log ("hello");
		}
			
		//going diagonally you move faster (Pythagorean theorem), so we normalize the vector
		if (!isMovingAttackH && !isMovingAttackL) {
			walkVelocity = walkVelocity.normalized * newWalkSpeed;
		}

		newInput = true;
	}

	public override void comboReady (int comboNumber) {
		if (comboNumber == 2) {
			effectManager.SpawnEffect (EffectManager.Effects.stage2X42, this.transform);
		} else if (comboNumber == 3) {
			effectManager.SpawnEffect (EffectManager.Effects.stage3X42, this.transform);
		}
	}

	public void spawnProjectile(BulletTypes type) {
		BoxCollider box = this.GetComponent<BoxCollider> ();
		float facingFloat = (facing == FacingDirection.East) ? 1f : -1f;
		if (type == BulletTypes.KinergyBulletFront) {
			verticalOffset = 0.008f;
			spawnProjectileKinergyBullets (type, verticalOffset, facingFloat);
		} else if (type == BulletTypes.KinergyBulletBack) {
			verticalOffset = 0.008f;
			spawnProjectileKinergyBullets (type, verticalOffset, facingFloat);
		} else if (type == BulletTypes.KinergyBulletUp) {
			verticalOffset = 0.02f;
			spawnProjectileKinergyBullets (type, verticalOffset, facingFloat);
		} else if (type == BulletTypes.DualBullets1 || type == BulletTypes.DualBullets2 || type == BulletTypes.DualBullets3 ||
		           type == BulletTypes.DualBullets4 || type == BulletTypes.DualBullets5 || type == BulletTypes.DualBullets6) {
			spawnProjectileDualBullets (type, facingFloat, verticalOffset);
		} else if (type == BulletTypes.Nuke) {
			spawnProjectileNuke (type, facingFloat, verticalOffset);
		}
	}

	void spawnProjectileKinergyBullets(BulletTypes type, float verticalOffset, float facingFloat) {
		LoseKinergy (1);
		BoxCollider box = this.GetComponent<BoxCollider> ();
		Vector3 spawnPosition;
		if (facingFloat == 1f) {
			spawnPosition = new Vector3 (box.bounds.max.x + 0.3f, box.bounds.center.y+verticalOffset, box.bounds.max.z + 0.01f);
		} else {
			spawnPosition = new Vector3 (box.bounds.min.x - 0.3f, box.bounds.center.y+verticalOffset, box.bounds.max.z + 0.01f);
		}
		var bullet = (GameObject)Instantiate (projectileKinergyBullets, spawnPosition, Quaternion.identity, this.transform);
		SpriteRenderer sr = bullet.GetComponent<SpriteRenderer> ();
		ProjectileProperties projProp = bullet.GetComponent<ProjectileProperties> ();
		if (facingFloat != 1f) {
			sr.flipX = true;
		}
		projProp.Fire (facingFloat, 3, 1f, type);
	}

	void spawnProjectileDualBullets(BulletTypes type, float facingFloat, float verticalOffset) {
		bulletCost++;
		if (bulletCost >= 2) {
			bulletCost = 0;
			LoseKinergy (1);
		}
		BoxCollider box = this.GetComponent<BoxCollider> ();
		Vector3 spawnPosition;
		if (facingFloat == 1f) {
			spawnPosition = new Vector3 (box.bounds.max.x + 0.35f, box.bounds.center.y+verticalOffset, box.bounds.max.z + 0.01f);
		} else {
			spawnPosition = new Vector3 (box.bounds.min.x - 0.35f, box.bounds.center.y+verticalOffset, box.bounds.max.z + 0.01f);
		}
		var bullet = (GameObject)Instantiate (projectileDualBullets, spawnPosition, Quaternion.identity, this.transform);
		SpriteRenderer sr = bullet.GetComponent<SpriteRenderer> ();
		ProjectileProperties projProp = bullet.GetComponent<ProjectileProperties> ();
		if (facingFloat != 1f) {
			sr.flipX = true;
		}
		projProp.Fire (facingFloat, 3, 1f, type);
	}

	void spawnProjectileNuke(BulletTypes type, float facingFloat, float verticalOffset) {
		BoxCollider box = this.GetComponent<BoxCollider> ();
		Vector3 spawnPosition;
		if (facingFloat == 1f) {
			spawnPosition = new Vector3 (box.bounds.max.x + 0.75f, box.bounds.center.y+verticalOffset, box.bounds.max.z + 0.01f);
		} else {
			spawnPosition = new Vector3 (box.bounds.min.x - 0.75f, box.bounds.center.y+verticalOffset, box.bounds.max.z + 0.01f);
		}
		var bullet = (GameObject)Instantiate (nuke, spawnPosition, Quaternion.identity, this.transform);
		SpriteRenderer sr = bullet.GetComponent<SpriteRenderer> ();
		ProjectileProperties projProp = bullet.GetComponent<ProjectileProperties> ();
		if (facingFloat != 1f) {
			sr.flipX = true;
		}
		projProp.Fire (facingFloat, 5, 1f, type);
	}

	public void setRandomOffset (){
		verticalOffset = Random.Range (-0.1f, 0.1f);
	}

	public void resetRandomOffset (){
		verticalOffset = 0.008f;
	}

	public void InstantDeath (GameObject unfortunateEnemy) {
		EnemyController enemyController = unfortunateEnemy.GetComponent<EnemyController> ();
		if (enemyController != null && instaDeathActive) {
			instaDeathActive = false;
			string name = enemyController.characterName;
			if (name == "High Ro11er") {
				animator.SetBool ("hitHighRo11er", true);
			} else if (name == "Slu66er") {
				animator.SetBool ("hitSlu66er", true);
			}
			unfortunateEnemy.GetComponent<SpriteRenderer> ().enabled = false;
			StartCoroutine (waitForInstantDeath (enemyController));
		}
	}
		
	IEnumerator waitForInstantDeath(EnemyController enemyController) {
		yield return new WaitForSeconds (0.1f);
		enemyController.Die ();
		animator.SetBool ("hitSlu66er", false);
		animator.SetBool ("hitHighRo11er", false);
		yield return new WaitForSeconds (1f);
		instaDeathActive = true;
	}

	IEnumerator waitForHeavyCombo() {
		yield return new WaitForSeconds(1f);
		animator.ResetTrigger ("useHeavy02Combo");
		heavy02ComboReady = false;
	}

	IEnumerator overchargedDamage() {
		overcharged = true;
		remainingHP--;
		yield return new WaitForSeconds (0.1f);
		if (currentKinergy == 100)
			StartCoroutine (overchargedDamage ());
		else {
			overcharged = false;
			sR.color = oldColor;
		}
	}

	public override void CharacterChange (string direction)
	{
		isChanged = true;
		LoadNewScene sceneManager = FindObjectOfType<LoadNewScene> ();
		if (direction == "up") {
			sceneManager.CharacterChange ("Captain Summers", this.transform.position);
		}
		if (direction == "right" && facing == FacingDirection.East || direction == "left" && facing == FacingDirection.West) {
			sceneManager.CharacterChange ("Ronin", this.transform.position);
		}
		if (direction == "right" && facing == FacingDirection.West || direction == "left" && facing == FacingDirection.East) {
			sceneManager.CharacterChange ("The Wonder", this.transform.position);
		}
		Time.timeScale = 1f;
		StartCoroutine (waitForDestruction());
	}

	public enum BulletTypes {
		KinergyBulletUp,
		KinergyBulletFront,
		KinergyBulletBack,
		DualBullets1,
		DualBullets2,
		DualBullets3,
		DualBullets4,
		DualBullets5,
		DualBullets6,
		Nuke
	}
}
