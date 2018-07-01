using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoninHM : HitboxManager {

	animationsRonin prevAnimation = animationsRonin.clear;

	public enum animationsRonin { 
		clear = -1,
		idleLeft,
		idleRight,
		runLeft,
		runRight,
		light01Left,
		light01Right,
		light02LeftStart,
		light02LeftHit,
		light02LeftMiss,
		light02RightStart,
		light02RightHit,
		light02RightMiss,
		heavy01LeftStart,
		heavy01LeftKanjiStrike,
		heavy01LeftFinish,
		heavy01RightStart,
		heavy01RightEnergySlash,
		heavy01RightFinish,
		heavy02LeftGustFromTheWest,
		heavy02LeftDashing,
		heavy02LeftGustFromTheEast,
		heavy02RightDualWielding,
		heavy02RightSakuraTornado,
		heavy02RightFinish,
		prepToJumpLeft,
		jumpingLeft,
		midAirLeft,
		fallingLeft,
		landingLeft,
		jumpLightLeft,
		prepToJumpRight,
		jumpingRight,
		midAirRight,
		fallingRight,
		landingRight,
		jumpLightRight,
		dashLeft,
		dashRight,
		runStopLeft,
		runStopRight,
		heavy03RightDDFinish,
		heavy03RightDemonDance,
		heavy03RightStart,
		heavy02RightJudgement,
		heavy03LeftNudeDeclaration,
		heavy03LeftBushidoPride,
		heavy03LeftRoar,
		heavy03LeftJusticeServed,
		heavy03LeftFinish,
		intro
	}

	public void setColliders (animationsRonin animation) {
		if (animation == animationsRonin.clear) {
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
		prevAnimation = animationsRonin.clear;
		boxes = null;
	}

	void checkHitCondition(animationsRonin animation) {
		if (animation == animationsRonin.light02LeftHit ||
			animation == animationsRonin.light02LeftMiss ||
			animation == animationsRonin.light02LeftStart ||
			animation == animationsRonin.light02RightHit ||
			animation == animationsRonin.light02RightMiss ||
			animation == animationsRonin.light02RightStart) {
			timeMaintaining = 0f;
		} else {
			timeMaintaining = 0.1f;
		}
	}
}
