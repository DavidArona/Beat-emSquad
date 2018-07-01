using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RonaldoHM : HitboxManager {

	animationsRonaldo prevAnimation = animationsRonaldo.clear;

	public enum animationsRonaldo {
		clear = -1,
		idle,
		run,
		dump,
		attack,
		empty
	}

	public void setColliders (animationsRonaldo animation) {
		if (animation == animationsRonaldo.clear) {
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
		prevAnimation = animationsRonaldo.clear;
		boxes = null;
	}
}
