using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection {
	East,
	West
}

public enum Attacks {
	None,
	Light01,
	Light02,
	Heavy01,
	Heavy02,
	Heavy03,
	JumpLight
}

//manages basic movement and actions started there
//MOVEMENT: Horizontal, Forward/Back
//ACTIONS: Attack
public class PlayerController : Controller {

	public string characterName;
	public EffectManager effectManager;

	//movement information
	protected Vector3 walkVelocity;
	protected Vector3 prevWalkVelocity;
	public FacingDirection facing;
	protected Attacks attack;
	protected float adjVertVelocity;
	protected float jumpPressTime;

	//settings
	public float ogWalkSpeed = 3f;
	public float jumpSpeed = 6f;
	public float fallMultiplier = 1.25f;
	public float lowJumpMultiplier = 2.5f;
	protected float newWalkSpeed = 3f;

	//actionData manager
	protected ActionData actions;
	protected ActionData prevActions;

	//combo actions
	[SerializeField]
	protected bool light02Ready = false;
	[SerializeField]
	protected bool heavy02Ready = false;
	protected bool heavy03Ready = false;

	//charge and barrage attacks
	protected bool isChargingH = false;
	protected bool isChargingL = false;
	protected float heavyChargeTime;
	protected float lightChargeTime;
	protected bool isBarragingH = false;
	protected bool isBarragingL = false;
	protected bool prevLButtonPush = false;
	protected bool prevHButtonPush = false;
	protected int barrageCounterL = 0;
	protected int barrageCounterH = 0;

	protected bool isMovingAttackH = false;
	protected bool isMovingAttackL = false;
	protected float movingAttackSpeed;
	protected float movingAttackDirection;

	protected bool startJumping = false;

	//damage control
	protected int maxHP = 100;
	public int remainingHP = 100;
	protected bool stunned = false;
	protected bool isDown = false;
	protected bool dead = false;

	//dash control
	protected bool movementPushed = false;
	protected int dashCounter = 0;

	protected bool movementLock = false;
	protected bool movingWhileAttacking = false;

	public CameraShake camera;

	public bool changingCharacter;
	protected bool isChanged = false;

	//attack costs
	public int currentKinergy = 0;
	public int[] attackCosts;

	//reads buttons and acts accordingly
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
							UseAttack (Attacks.Heavy01);
						} else {
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
		}

		//going diagonally you move faster (Pythagorean theorem), so we normalize the vector
		if (!isMovingAttackH && !isMovingAttackL) {
			walkVelocity = walkVelocity.normalized * newWalkSpeed;
		}

		newInput = true;
	}

	//main loop
	public virtual void LateUpdate(){
		if (!newInput) {
			prevWalkVelocity = walkVelocity;
			ResetMovementToZero ();
			jumpPressTime = 0f;
			if (movementPushed) {
				movementPushed = false;
				dashCounter += 1;
				StartCoroutine (waitForDash());
			}
		}
		if (prevWalkVelocity != walkVelocity) {
			//check if there is a face change
			CheckForFacingChange();
		}
		rb.velocity = new Vector3 (walkVelocity.x, rb.velocity.y + adjVertVelocity, walkVelocity.z);
		newInput = false;
		actions = new ActionData (walkVelocity != Vector3.zero, Grounded (), facing == FacingDirection.East, attack, isChargingL, isChargingH, heavyChargeTime, lightChargeTime, isBarragingH, 
			isBarragingL, barrageCounterL, barrageCounterH, isDown, rb.velocity.y<0, startJumping, dashCounter>=1 && movementPushed, changingCharacter, isChanged);
		if (dashCounter >= 1 && movementPushed) {
			newWalkSpeed = ogWalkSpeed+1f;
		} 
		if (!actions.sameAs (prevActions)) {
			PassActions (actions);
		}
		prevActions = actions;
	}

	//method that will look below the character and see if there is a collider
	public virtual bool Grounded(){
		return Physics.Raycast (transform.position, Vector3.down, coll.bounds.extents.y + 0.001f, 1 << LayerMask.NameToLayer("Ground"));
	}

	//checks if facing has to change based on the last movements
	void CheckForFacingChange(){
		//if you stop moving
		if (walkVelocity == Vector3.zero) {
			return;
		}
		if (walkVelocity.z == 0) {
			//change our facing based on walkVelocity
			ChangeFacing(walkVelocity);
		} else {
			if (prevWalkVelocity.x == 0) {
				ChangeFacing (new Vector3 (walkVelocity.x, 0, 0));
			} else {
				ChangeFacing (walkVelocity);
			}
		}
	}
		
	//changes facing in the x axis
	void ChangeFacing(Vector3 dir){
		if (dir.x != 0 && !movingWhileAttacking) {
			facing = (dir.x > 0) ? FacingDirection.East : FacingDirection.West;
		}
	}

	//resets movement if is not moving by outside forces
	protected void ResetMovementToZero(){
		if (!isMovingAttackL && !isMovingAttackH) {
			walkVelocity = Vector3.zero;
			adjVertVelocity = 0f;
		} if (attack != Attacks.None && !movingWhileAttacking) {
			movementLock = true;
		}
	}

	//update to make movement flow better
	void Update(){
		if (rb.velocity.y < 0) {
			rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		if (rb.velocity.y > 0 && !Grounded () && jumpPressTime == 0f) {
			rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}
	}

	public void Jump (float jumpAmount) {
		startJumping = false;
		effectManager.SpawnEffect (EffectManager.Effects.jump, this.transform);
		if (Grounded ()) {
			if (jumpPressTime != 0f) {
				adjVertVelocity = jumpSpeed;
			} else {
				rb.velocity = new Vector3 (walkVelocity.x, rb.velocity.y + jumpAmount, walkVelocity.z);
			}
		}
	}

	//if the character receives damage, its health diminishes and the proper animation plays
	public virtual bool receivedHit (int damageReceived, float stunTime, bool front, Transform transform) {
		if (!stunned) {
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
			} else if (damageReceived <= 5 && damageReceived > 0) {
				am.PlayerHit (1);
			}
			Time.timeScale = 0.20f / damageReceived;
			Debug.Log ("Remaining Health: " + remainingHP);
			return true;
		} else {
			return false;
		}
	}

	protected void DestroyPlayer(bool front) {
		Time.timeScale = 0.2f;
		if (!front) {
			if (facing == FacingDirection.East) {
				facing = FacingDirection.West;
			} else {
				facing = FacingDirection.East;
			}
		}
		am.PlayerHit (0);
		dead = true;
		stunned = true;
	}

	protected IEnumerator regenerateKinergy(float amount) {
		yield return new WaitForSeconds (amount);
		if (!dead) {
			if (currentKinergy < 100) {
				currentKinergy++;
			}
		}
		StartCoroutine (regenerateKinergy (amount));
	}

	public virtual void CharacterChange (string direction){
		isChanged = true;
		StartCoroutine (waitForDestruction());
	}

	public void Destroy () {
		Destroy (this.gameObject);
	}

	/*--- WAIT EVENTS ---*/
	#region
	//when certain time passes after idle, combos stages restart
	public IEnumerator waitForInput(){
		yield return new WaitForSeconds(1f);
		restartCombos ();
	}

	protected IEnumerator waitForRecover(){
		yield return new WaitForSeconds(0.3f);
		if (!dead) {
			stunned = false;
		}
	}

	//waits until the animation for prepToJump is finished
	protected virtual IEnumerator waitForJump(){
		startJumping = true;
		yield return new WaitForSeconds (0.1f);
		if (Grounded () && jumpPressTime > 0.1f) {
			Jump (jumpSpeed);
		}
		startJumping = false;
	}

	//counts the time until you can damage the character again
	protected IEnumerator inmuneTimer(float stunTime){
		yield return new WaitForSeconds(0.005f);
		Time.timeScale = 1f;
		yield return new WaitForSeconds(stunTime);
		if (!isDown && !dead) {
			stunned = false;
		}
	}

	IEnumerator waitForDash () {
		yield return new WaitForSeconds (0.1f);
		dashCounter = 0;
	}

	public IEnumerator waitForDestruction() {
		yield return new WaitForSeconds (0.5f);
		Destroy ();
	}
	#endregion
	/*--- WAIT EVENTS ---*/
		
	/*--- ANIMATION EVENTS ---*/
	#region

	//when an attack finishes, tells the others to stay ready for input
	public virtual void attackOngoing (Attacks ongoingAttack) {
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
	}

	public void StartChargeAttack (Attacks attackType) {
		if (attackType == Attacks.Light01 || attackType == Attacks.Light02) {
			isChargingL = true;
		} else if (attackType == Attacks.Heavy01 || attackType == Attacks.Heavy02 || attackType == Attacks.Heavy03) {
			isChargingH = true;
		}
		effectManager.SpawnEffect (EffectManager.Effects.holdIt, this.transform);
	}

	public void EndChargeAttack (Attacks attackType) {
		isChargingL = false;
		isChargingH = false;
	}

	public void StartBarrageAttack (Attacks attackType) {
		if (attackType == Attacks.Light01 || attackType == Attacks.Light02) {
			isBarragingL = true;
		} else if (attackType == Attacks.Heavy01 || attackType == Attacks.Heavy02 || attackType == Attacks.Heavy03) {
			isBarragingH = true;
		}
		effectManager.SpawnEffect (EffectManager.Effects.mashIt, this.transform);
	}

	public void EndBarrageAttack (Attacks attackType) {
		if (barrageCounterL < 1) {
			isBarragingL = false;
		} 
		barrageCounterL = 0;
		if (barrageCounterH < 1) {
			isBarragingH = false;
		}
		barrageCounterH = 0;
		attack = Attacks.None;
	}

	public void StartMovingAttack (Attacks attackType) {
		movingAttackDirection = (facing == FacingDirection.East) ? 1f : -1f;
		walkVelocity = Vector3.right * movingAttackDirection * movingAttackSpeed;
		if (attackType == Attacks.Light01 || attackType == Attacks.Light02) {
			isMovingAttackL = true;
		} else if (attackType == Attacks.Heavy01 || attackType == Attacks.Heavy02 || attackType == Attacks.Heavy03) {
			isMovingAttackH = true;
		}
	}

	public void EndMovingAttack (Attacks attackType) {
		movingAttackDirection = 0f;
		walkVelocity = Vector3.zero;
		movingAttackSpeed = 0f;
		isMovingAttackH = false;
		isMovingAttackL = false;
	}

	//changes the speed of the moving attack
	public void SetMovingAttackSpeed (int movingAttackSpeed) {
		this.movingAttackSpeed = (float)movingAttackSpeed;
		walkVelocity = Vector3.right * movingAttackDirection * movingAttackSpeed;
	}

	//spawns combo ready effect depending on character and combo stage number
	public virtual void comboReady (int comboNumber) {
		EffectManager em = GameObject.Find ("EffectManager").GetComponent<EffectManager>();
		if (!em) {
			Debug.Log ("There is no Effect Manager");
		}
	}
		
	//reverses facing 'artificially'
	public void reverseFacing () {
		facing = (facing == FacingDirection.West) ? FacingDirection.East : FacingDirection.West;
	}

	//shakes the camera, use a string parted by commas to indicate duration and intensity -> "0.1,3"
	public void ShakeItUp(string info) {
		camera.decreaseFactor = 1.0f;
		string[] infoArray = info.Split (',');
		float duration = float.Parse (infoArray [0]);
		float intensity = float.Parse (infoArray [1]);
		if (infoArray.Length > 2) {
			float dF = float.Parse (infoArray [2]);
			camera.decreaseFactor = dF;
		}
		camera.shakeDuration = duration;
		camera.shakeAmount = intensity;
		camera.enabled = true;
	}

	public void callLandingEffect () {
		effectManager.SpawnEffect (EffectManager.Effects.landLeft, this.transform);
	}

	public void callAccelerationEffect () {
		if (facing == FacingDirection.East) {
			effectManager.SpawnEffect (EffectManager.Effects.accelLeft, this.transform);
		} else {
			effectManager.SpawnEffect (EffectManager.Effects.accelRight, this.transform);
		}
	}

	public void callStopingEffect () {
		if (facing == FacingDirection.East) {
			effectManager.SpawnEffect (EffectManager.Effects.brakeLeft, this.transform);
		} else {
			effectManager.SpawnEffect (EffectManager.Effects.brakeRight, this.transform);
		}
	}

	public void callJumpingEffect () {
		effectManager.SpawnEffect (EffectManager.Effects.jump, this.transform);
	}

	public void unlockMovement(float speed) {
		if (speed == 0) {
			newWalkSpeed = ogWalkSpeed;
			movingWhileAttacking = false;
		} else {
			newWalkSpeed = speed;
			movingWhileAttacking = true;
		}
		movementLock = false;
	}

	public void lockMovement () {
		movementLock = true;
	}

	public void restartCombos() {
		light02Ready = false;
		heavy02Ready = false;
		heavy03Ready = false;
	}

	public void clear() {
		attack = Attacks.None;
		movementLock = false;
		restartCombos ();
		if (!dead) {
			stunned = false;
		}
		EndBarrageAttack (Attacks.None);
		EndChargeAttack(Attacks.None);
		EndMovingAttack (Attacks.None);
		newWalkSpeed = ogWalkSpeed;
	}

	public void StartChangingCharacter(){
		changingCharacter = true;
		Debug.Log ("hello");
		Time.timeScale = 0.5f;
	}

	public void StopChangingCharacter(){
		changingCharacter = false;
		Time.timeScale = 1f;
	}

	#endregion
	/*--- ANIMATION EVENTS ---*/

	/*--- ATTACK COSTS ---*/
	#region
	public virtual void UseAttack (Attacks ongoingAttack) {
		attack = ongoingAttack;
		if (attack != Attacks.JumpLight) {
			restartCombos ();
			ResetMovementToZero ();
		}
		if (attack == Attacks.Heavy01) {
			LoseKinergy (attackCosts [0]);
		} else if (attack == Attacks.Heavy02) {
			LoseKinergy (attackCosts [1]);
		} else if (attack == Attacks.Heavy03) {
			LoseKinergy (attackCosts [2]);
		} 
	}

	public void LoseKinergy (int amount) {
		if (currentKinergy - amount < 0)
			currentKinergy = 0;
		else
			currentKinergy -= amount;
	}

	public void GainKinergy (int amount) {
		if (currentKinergy + amount > 100)
			currentKinergy = 100;
		else
			currentKinergy += amount;
	}
	#endregion
}

public struct ActionData {

	public bool isMoving;
	public bool isGrounded;
	public bool isFacing;
	public bool isChargingL;
	public bool isChargingH;
	public Attacks attack;
	public float heavyChargeTime;
	public float lightChargeTime;
	public bool isBarragingH;
	public bool isBarragingL;
	public int barrageCounterL;
	public int barrageCounterH;
	public bool isDown;
	public bool isFalling;
	public bool startJumping;
	public bool isDashing;
	public bool characterChanging;
	public bool isChanged;

	public ActionData(bool isMoving, bool isGrounded, bool isFacing, Attacks attack, bool isChargingL, bool isChargingH, float heavyChargeTime, float lightChargeTime, bool isBarragingH, bool isBarragingL, 
		int barrageCounterL, int barrageCounterH, bool isDown, bool isFalling, bool startJumping, bool isDashing, bool characterChanging, bool isChanged)
	{
		this.isMoving = isMoving;
		this.isGrounded = isGrounded;
		this.isFacing = isFacing;
		this.attack = attack;
		this.isChargingL = isChargingL;
		this.isChargingH = isChargingH;
		this.heavyChargeTime = heavyChargeTime;
		this.lightChargeTime = lightChargeTime;
		this.isBarragingH = isBarragingH;
		this.isBarragingL = isBarragingL;
		this.barrageCounterL = barrageCounterL;
		this.barrageCounterH = barrageCounterH;
		this.isDown = isDown;
		this.isFalling = isFalling;
		this.startJumping = startJumping;
		this.isDashing = isDashing;
		this.characterChanging = characterChanging;
		this.isChanged = isChanged;
	}

	public bool sameAs (ActionData prevActions) {
		if (isMoving == prevActions.isMoving &&
			isFacing == prevActions.isFacing &&
			isGrounded == prevActions.isGrounded &&
			isChargingL == prevActions.isChargingL &&
			isChargingH == prevActions.isChargingH &&
			heavyChargeTime == prevActions.heavyChargeTime &&
			lightChargeTime == prevActions.lightChargeTime &&
			isBarragingL == prevActions.isBarragingL &&
			isBarragingH == prevActions.isBarragingH &&
			barrageCounterL == prevActions.barrageCounterL &&
			barrageCounterH == prevActions.barrageCounterH &&
			attack == prevActions.attack &&
			isDown == prevActions.isDown &&
			isFalling == prevActions.isFalling &&
			startJumping == prevActions.startJumping &&
			isDashing == prevActions.isDashing &&
			characterChanging == prevActions.characterChanging &&
			isChanged == prevActions.isChanged) {
			return true;
		} else { return false; }
	}
}