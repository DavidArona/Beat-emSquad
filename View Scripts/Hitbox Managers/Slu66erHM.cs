using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slu66erHM : HitboxManager {

	animationsSlu66er prevAnimation = animationsSlu66er.clear;

	public enum animationsSlu66er {
		clear = -1,
		idle,
		run,
		weakAttack,
		strongAttack,
		strongAttackHomeRun,
		strongAttackFinish,
		malfunctioning,
		deathPhysicalExplosion,
		intro
	}

	public void setColliders (animationsSlu66er animation) {
		if (animation == animationsSlu66er.clear) {
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
				//Debug.Log (prevAnimation + " " + animation + " " + frameCounter + "boxes " + boxes.Length + "prev " + prevBoxes.Length);
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
		prevAnimation = animationsSlu66er.clear;
		boxes = null;
	}
}
