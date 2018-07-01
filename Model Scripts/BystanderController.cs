using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BystanderStates {
	Idle,
	Moving,
	Wandering,
	Intro
}

public class BystanderController : MonoBehaviour {

	public Vector3 target;

	protected NavMeshAgent nav;
	protected Rigidbody rb;
	protected Animator anim;
	protected EncounterManager eM;
	protected SpriteRenderer sR;
	protected WanderingAI wAI;

	//movement control
	public FacingDirection facing;
	Vector3 position;
	Vector3 prevPosition;

	// Use this for initialization
	void Start () {
		nav = GetComponent <NavMeshAgent> ();
		rb = GetComponent <Rigidbody> ();
		anim = GetComponent <Animator> ();
		sR = GetComponent <SpriteRenderer> ();
		wAI = GetComponent <WanderingAI> ();
		eM = GameObject.FindObjectOfType<EncounterManager> ();
		anim.SetBool ("IntroPlaying", true);
		Physics.IgnoreLayerCollision (11, 8);
	}
	
	// Update is called once per frame
	public virtual void Update () {
		CheckFacing (this.transform.position, prevPosition);
		if (this.transform.position == prevPosition) {
			anim.SetBool ("Moving", false);
		} else {
			anim.SetBool ("Moving", true);
		}
		prevPosition = this.transform.position;
	}

	public void StartWandering() {
		nav.enabled = true;
		wAI.enabled = true;
	}

	public void StopWandering() {
		wAI.enabled = false;
		nav.enabled = false;
	}

	public void StopMoving () {
		nav.enabled = false;
	}

	public void StartMoving() {
		nav.enabled = true;
		anim.SetBool ("Moving", true);
	}

	public virtual void FinishIntro() {
		anim.SetBool ("IntroPlaying", false);
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
		sR.flipX = !sR.flipX;
	}
}
