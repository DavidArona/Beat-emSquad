using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WondieHM : HitboxManager {

	animationsWondie prevAnimation = animationsWondie.clear;

	public enum animationsWondie { 
		clear = -1,
		idle,
		light01,
		run,
		light02Balancing,
		light02OraOraLoop,
		heavy01ChargingUp,
		heavy01MaxChargedTransition,
		heavy01Shock,
		heavy01MaxChargedFinish,
		heavy02ChargeFingerShot,
		heavy02FingerShot,
		heavy02FSMiss,
		heavy03Start,
		heavy03Thinking,
		heavy03ASolution,
		heavy03ShockingEnough,
		heavy03ToFallDown,
		heavy01NotMaxChargedTransition,
		heavy01NotMaxChargedFinish,
		light02HokutoFinish,
		light02Recover,
		heavy02FSHit,
		heavy02HitTransition,
		heavy02HitFinish,
		prepToJump,
		jumping,
		midAir,
		falling,
		landing,
		jumpLight,
		heavy01MegaShock,
		dash,
		runStop,
		intro
	}

	public void setColliders (animationsWondie animation) {
		if (animation == animationsWondie.clear) {
			clearColliders ();
		} else { 
			checkHitCondition (animation);
			if (prevAnimation != animation) {
				frameCounter = 0;
				prevAnimation = animation;
			}
			if (frameCounter < colliders [(int)animation].Length) {
				boxes = colliders [(int)animation] [frameCounter];
			} else {
				Debug.Log ("mismatched hitbox: " + animation);
			}
			if (boxes != null) { enableColliders (boxes, true); }
			if (prevBoxes != null) { enableColliders (prevBoxes, false); }
			prevBoxes = boxes;
			frameCounter++;
		}
	}

	public void clearColliders() {
		if (prevBoxes != null) {
			enableColliders (prevBoxes, false);
		}
		frameCounter = 0;
		prevAnimation = animationsWondie.clear;
		boxes = null;
	}

	void checkHitCondition(animationsWondie animation) {
		if (animation == animationsWondie.heavy02ChargeFingerShot ||
		    animation == animationsWondie.heavy02FingerShot ||
		    animation == animationsWondie.heavy02FSHit ||
		    animation == animationsWondie.heavy02FSMiss ||
		    animation == animationsWondie.heavy02HitFinish ||
		    animation == animationsWondie.heavy02HitTransition) {
			timeMaintaining = 0f;
		} else {
			timeMaintaining = 0.1f;
		}
	}
}
