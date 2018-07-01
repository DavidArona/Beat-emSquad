using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates {
	Intro,
	WaitForAttack,
	Attacking,
	Moving,
	Stunned,
	Malfunctioning,
	Dead,
	Downed,
	MovingAttack
}

public class EnemyController : MonoBehaviour {

	//initialized later
	Transform player;
	NavMeshAgent nav;
	Rigidbody rb;
	Collider coll;
	Animator animator;
	SpriteRenderer sr;
	HitboxManager hitboxManager;
	BoxCollider collider;
	EffectManager effectManager;

	//movement control
	public FacingDirection facing;
	Vector3 position;
	Vector3 prevPosition;

	//attack cycle control
	public bool inRange;
	private int probNumber;
	public int attackChance = 0;
	int firstAttackChance;

	//moving attacks
	protected Vector3 walkVelocity;
	protected float movingAttackSpeed;
	protected float movingAttackDirection;
	protected bool isMovingAttack;

	//damage control
	private int remainingHP = 50;
	private bool inmune = false;
	protected bool isDown = false;

	public CameraShake camera;
	private int rN;

	Vector3 target;
	public EnemyStates currentState = EnemyStates.Intro;

	public string characterName;

	AttackData [] attacks;
	public string[] attackNames;
	public int[] attackPriorities;

	[SerializeField]
	string[] ballot;
	[SerializeField]
	EffectProperties finishItEffect;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		nav = GetComponent <NavMeshAgent> ();
		rb = GetComponent <Rigidbody> ();
		coll = GetComponent <Collider> ();
		sr = GetComponent <SpriteRenderer> ();
		animator = GetComponent <Animator> ();
		hitboxManager = GetComponent <HitboxManager> ();
		collider = GetComponent <BoxCollider> ();
		effectManager = GameObject.FindObjectOfType<EffectManager> ();
	}

	void Start() {
		firstAttackChance = attackChance;
		prevPosition = this.transform.position;
		target = player.position;
		attacks = new AttackData[attackNames.Length];
		for (int i = 0; i < attacks.Length; i++) {
			attacks [i] = new AttackData (attackNames [i], attackPriorities [i]);
		}
		calculateAttackChances ();
	}
	
	// Update is called once per frame
	void Update() {
		target = player.position;
		if (currentState == EnemyStates.Moving) {
			nav.SetDestination (player.position);
			CheckFacing (this.transform.position, prevPosition);
			prevPosition = this.transform.position;
			CheckRange ();
		}
		if (currentState == EnemyStates.WaitForAttack) {
			CheckRange ();
		}
	}

	void FixedUpdate() {
		rb.velocity = new Vector3 (walkVelocity.x, rb.velocity.y, rb.velocity.z);
	}

	public void CheckRange () {
		if (inRange) {
			if (currentState != EnemyStates.WaitForAttack) {
				enableWaitForAttack (true);
			}
		} else {
			if (currentState != EnemyStates.Moving) {
				enableMoving (true);
			}
		}
	}

	void enableMoving (bool enabled) {
		currentState = EnemyStates.Moving;
		firstAttackChance = attackChance;
		StartMoving ();
	}

	void enableWaitForAttack (bool enabled) {
		currentState = EnemyStates.WaitForAttack;
		StopMoving ();
		if ((target.x > transform.position.x && facing == FacingDirection.West) ||
			(target.x < transform.position.x && facing == FacingDirection.East))
			ChangeFacing ();
		StartCoroutine (attackCycle (firstAttackChance));
	}

	void enableAttacking (bool enabled) {
		currentState = EnemyStates.Attacking;
		firstAttackChance = 0;
		useHighestPriorityAttack ();
	}

	void enableStunned() {
		currentState = EnemyStates.Stunned;
		StopMoving ();
	}

	void enableDowned() {
		currentState = EnemyStates.Downed;
		StopMoving ();
	}

	void enableMalfunctioning() {
		currentState = EnemyStates.Malfunctioning;
		spawnWeakened ();
		spawnFinishIt ();
		animator.SetTrigger ("isMalfunctioning");
		StopMoving ();
	}

	void enableDead(string dmgType) {
		currentState = EnemyStates.Dead;
		stopFinishIt ();
		animator.SetTrigger ("isDead");
		deathType (dmgType);
		StopMoving ();
	}

	void StopMoving () {
		nav.enabled = false;
		animator.SetBool ("isMoving", false);
		EndMovingAttack ();
	}

	void StartMoving() {
		nav.enabled = true;
		animator.SetBool ("isMoving", true);
	}

	void CheckFacing(Vector3 position, Vector3 prevPosition){
		if (position.x > prevPosition.x && facing == FacingDirection.West) {
			ChangeFacing ();
		}
		if (position.x < prevPosition.x && facing == FacingDirection.East) {
			ChangeFacing ();
		}
	}

	public void ChangeFacing () {
		if (facing == FacingDirection.West)
			facing = FacingDirection.East;
		else
			facing = FacingDirection.West;
		sr.flipX = !sr.flipX;
		hitboxManager.reverseColliders ();
	}

	IEnumerator attackCycle(int randomNormalizer) {
		int upperLimit = 3;
		probNumber = Random.Range (randomNormalizer, upperLimit);
		if (probNumber >= upperLimit-1) {
			enableAttacking (true);
		} else {
			yield return new WaitForSeconds (2f);
			if (currentState == EnemyStates.WaitForAttack) {
				StartCoroutine (attackCycle (randomNormalizer+1));
			}
		}
	}

	public bool receivedHit (int damageReceived, float stunTime, bool front, bool finisher, string dmgType) {
		if (!inmune) {
			if (currentState != EnemyStates.Malfunctioning) {
				rN = Random.Range (0, 3);
				remainingHP -= damageReceived;
				if (remainingHP <= 0) {
					enableMalfunctioning ();
					return true;
				}
				enableStunned ();
				StartCoroutine (inmuneTimer (stunTime));
				camera.decreaseFactor = 2f;
				camera.shakeDuration = damageReceived / 30f;
				camera.enabled = true;
				if (damageReceived > 10) {
					if ((target.x > transform.position.x && facing == FacingDirection.West) ||
					    (target.x < transform.position.x && facing == FacingDirection.East))
						ChangeFacing ();
					SetMovingAttackSpeed (-damageReceived / 10f);
					animator.SetTrigger ("hurtHigh");
					enableDowned ();
				} else if (damageReceived <= 10 && damageReceived > 5) {
					if (rN == 1) {
						animator.SetTrigger ("hurtMediumA");
					} else {
						animator.SetTrigger ("hurtMediumB");
					}
				} else if (damageReceived <= 5 && damageReceived > 0) {
					if (rN == 1) {
						animator.SetTrigger ("hurtLowA");
					} else {
						animator.SetTrigger ("hurtLowB");
					}
				} else {
					return true;
				}
				Time.timeScale = 0.20f / damageReceived;
				Debug.Log ("Remaining Health: " + remainingHP);
			} else {
				StartCoroutine (inmuneTimer (stunTime));
				camera.decreaseFactor = 2f;
				camera.shakeDuration = damageReceived / 30f;
				camera.enabled = true;
				if (Time.timeScale == 1f) {
					Time.timeScale = 0.20f / damageReceived;
				}
				if (finisher) {
					if ((target.x > transform.position.x && facing == FacingDirection.West) ||
					    (target.x < transform.position.x && facing == FacingDirection.East)) {
						ChangeFacing ();
					}
					enableDead (dmgType);
				}
			}
			return true;
		} else {
			return false;
		}
	}

	//counts the time until you can damage the character again
	IEnumerator inmuneTimer(float stunTime){
		inmune = true;
		yield return new WaitForSeconds(0.003f);
		Time.timeScale = 1f;
		yield return new WaitForSeconds(stunTime);
		inmune = false;
	}

	void calculateAttackChances() {
		int ballotLength = 0;
		int j = 0;
		int currentAttackChances;
		foreach (AttackData attack in attacks) {
			ballotLength += attack.priority;
		}
		ballot = new string[ballotLength];
		for (int i = 0; i < attacks.Length; i++) {
			currentAttackChances = attacks[i].priority;
			while (currentAttackChances > 0) {
				ballot [j] = attacks [i].name;
				currentAttackChances--;
				j++;
			}
		}
	}

	void useHighestPriorityAttack () {
		rN = Random.Range (0, ballot.Length);
		animator.SetTrigger (ballot [rN]);
	}

	public void Die() {
		Time.timeScale = 1f;
		Destroy (this.gameObject, 0f);
	}

	public void StartMovingAttack () {
		movingAttackDirection = (facing == FacingDirection.East) ? 1f : -1f;
		walkVelocity = Vector3.right * movingAttackDirection * movingAttackSpeed;
		isMovingAttack = true;
	}

	public void EndMovingAttack () {
		movingAttackDirection = 0f;
		walkVelocity = Vector3.zero;
		movingAttackSpeed = 0f;
		isMovingAttack = false;
	}

	public void SetMovingAttackSpeed (float movingAttackSpeed) {
		this.movingAttackSpeed = movingAttackSpeed;
		walkVelocity = Vector3.right * movingAttackDirection * movingAttackSpeed;
	}

	void deathType (string type) {
		if (type == "physical") {
			animator.SetTrigger ("isPhysical");
		} else if (type == "slashing") {
			animator.SetTrigger ("isSlashing");
		} else if (type == "electric") {
			animator.SetTrigger ("isElectric");
		} else if (type == "explosive") {
			animator.SetTrigger ("isExplosive");
		} else if (type == "nuke") {
			animator.SetTrigger ("isNuked");
		}
	}

	public void spawnMalfunctioning () {
		effectManager.SpawnEffect (EffectManager.Effects.malfunctioning, this.transform);
	}

	public void spawnLostScrew () {
		effectManager.SpawnEffect (EffectManager.Effects.lostScrew, this.transform);
	}

	public void spawnSlu66erHead() {
		effectManager.SpawnEffect (EffectManager.Effects.slu66erHead, this.transform);
	}

	public void spawnHighRo11erHead() {
		effectManager.SpawnEffect (EffectManager.Effects.highRo11erHead, this.transform);
	}

	public void spawnWeakened() {
		effectManager.SpawnEffect (EffectManager.Effects.weakened, this.transform);
	}

	public void spawnFinishIt() {
		string name = player.GetComponent<PlayerController> ().characterName;
		if (name == "Captain Summers")
			finishItEffect = effectManager.SpawnEffect (EffectManager.Effects.smashIt, this.transform, true);
		if (name == "X-42")
			finishItEffect = effectManager.SpawnEffect (EffectManager.Effects.destroyIt, this.transform, true);
		if (name == "The Wonder")
			finishItEffect = effectManager.SpawnEffect (EffectManager.Effects.zapIt, this.transform, true);
		if (name == "Ronin")
			finishItEffect = effectManager.SpawnEffect (EffectManager.Effects.slashIt, this.transform, true);
	}

	public void stopFinishIt() {
		if (finishItEffect)
			finishItEffect.EndMyLifePlease ();
	}

	public void ChangeCharacter (Transform newChar) {
		player = newChar;
	}
}

public struct AttackData {
	public string name;
	public int priority;

	public AttackData (string name, int priority) {
		this.name = name;
		this.priority = priority;
	}

	public void ChangePriority (int newPriority) {
		priority = newPriority;
	}
}
