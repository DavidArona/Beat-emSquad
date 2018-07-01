using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class X42HM : HitboxManager {

	animationsX42 prevAnimation = animationsX42.clear;

	public enum animationsX42 {
		clear = -1,
		idle,
		run,
		light01,
		light02Start,
		light02DrillBreakerLoop,
		light02Recover,
		heavy01Start,
		heavy01PhaseOne,
		heavy01PhaseTwo,
		heavy01ToPhaseThree,
		heavy01PhaseThree,
		heavy01ToOverheat,
		heavy01OverheatLoop,
		heavy01OverheatFinish,
		heavy01StandardFinish,
		heavy02SmashingPumpkins,
		heavy02RollingSlow,
		heavy02RollingMedium,
		heavy02RollingFast,
		heavy02RockAndRolling,
		heavy02FinishHim,
		heavy02StylishRecover,
		prepToJump,
		jumping,
		midAir,
		falling,
		landing,
		jumpLight,
		heavy02MeleeCombo,
		heavy02Explodo,
		dash,
		runStop,
		heavy03Charging,
		heavy03Connect,
		heavy03NiceReceive,
		heavy03Smash,
		heavy03WarDeclaration,
		heavy02SmashHit66,
		heavy02RollingSlow66,
		heavy02RollingMedium66,
		heavy02RollingFast66,
		heavy02RockAndRolling66,
		heavy02FinishHim66,
		heavy02SmashHit11,
		heavy02RollingSlow11,
		heavy02RollingMedium11,
		heavy02RollingFast11,
		heavy02RockAndRolling11,
		heavy02FinishHim11,
		intro
	}

	public void setColliders (animationsX42 animation) {
		if (animation == animationsX42.clear) {
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
			if ((int)animation > 1) {
				Debug.Log (prevAnimation + " " + animation + " " + frameCounter + "boxes " + boxes.Length + "prev " + prevBoxes.Length);
			}
			prevBoxes = boxes;
			frameCounter++;
		}
	}

	public void clearColliders() {
		if (prevBoxes != null) {
			enableColliders (prevBoxes, false);
		}
		frameCounter = 0;
		prevAnimation = animationsX42.clear;
		boxes = null;
	}
}
