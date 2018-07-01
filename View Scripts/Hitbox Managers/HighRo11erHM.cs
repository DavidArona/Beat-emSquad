using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighRo11erHM : HitboxManager {

	animationsHighRo11er prevAnimation = animationsHighRo11er.clear;

	public enum animationsHighRo11er {
		clear = -1,
		idle,
		run,
		strongAttack,
		weakAttackSlide,
		weakAttackKick,
		malfunctioning,
		deathPhysicalExplosion,
		intro
	}

	public void setColliders (animationsHighRo11er animation) {
		if (animation == animationsHighRo11er.clear) {
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
		prevAnimation = animationsHighRo11er.clear;
		boxes = null;
	}
}
