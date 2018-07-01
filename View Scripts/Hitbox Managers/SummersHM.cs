using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummersHM : HitboxManager {

	animationsSummers prevAnimation = animationsSummers.clear;

	public enum animationsSummers {
		clear = -1,
		idle,
		run,
		heavy01Guard,
		heavy01GuardFinish,
		heavy01GuardHit,
		heavy01GuardSpark,
		heavy01GuardStart,
		heavy02ArmorSmash,
		heavy02ChargeSmash2nd,
		heavy02ChargeStart,
		heavy02Recover,
		heavy02WeakSmash,
		light01FlailFinish,
		light01FlailingLoop,
		light01PrepToFlail,
		light02GroundKissing,
		light02OnTheEdge,
		light02WeakPunch,
		light02WPMiss,
		heavy03JusticePunchStart,
		heavy03JusticePunch,
		heavy03JP1stCharge,
		light02WPHit,
		heavy03LawfulPunch,
		heavy03JPunchFinish,
		prepToJump,
		jumping,
		midAir,
		falling,
		landing,
		jumpLight,
		light01JusticeShove,
		dash,
		runStop,
		heavy03SamaritanPunch,
		heavy02CrystallizedSmash,
		heavy01GuardRebound,
		intro
	}

	public void setColliders (animationsSummers animation) {
		if (animation == animationsSummers.clear) {
			clearColliders ();
		} else {
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
		prevAnimation = animationsSummers.clear;
		boxes = null;
	}
}
