using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

	public enum Effects {
		strikeWeak,
		strikeStrong,
		slashWeak,
		slashStrong,
		stage2Wondie,
		stage3Wondie,
		stage2Summers,
		stage3Summers,
		stage2X42,
		stage3X42,
		stage2RoninLeft,
		stage3RoninLeft,
		stage2RoninRight,
		stage3RoninRight,
		accelLeft,
		accelRight,
		block,
		brakeLeft,
		brakeRight,
		electrified,
		jump,
		landLeft,
		landRight,
		blockLeft,
		electricWeak,
		electricStrong,
		nukeExploding,
		roninHeavy02,
		vanish,
		explosion,
		summersHeavy03ShockwaveR,
		summersHeavy03ShockwaveL,
		malfunctioning,
		lostScrew,
		incoming,
		slu66erHead,
		highRo11erHead,
		weakened,
		zapIt,
		destroyIt,
		smashIt,
		slashIt,
		mashIt,
		holdIt
	}

	public GameObject strikeWeak;
	public GameObject strikeStrong;
	public GameObject slashWeak;
	public GameObject slashStrong;
	public GameObject stage2Wondie;
	public GameObject stage3Wondie;
	public GameObject stage2Summers;
	public GameObject stage3Summers;
	public GameObject stage2X42;
	public GameObject stage3X42;
	public GameObject stage2RoninLeft;
	public GameObject stage3RoninLeft;
	public GameObject stage2RoninRight;
	public GameObject stage3RoninRight;
	public GameObject accelLeft;
	public GameObject accelRight;
	public GameObject block;
	public GameObject brakeLeft;
	public GameObject brakeRight;
	public GameObject electrified;
	public GameObject jump;
	public GameObject landLeft;
	public GameObject landRight;
	public GameObject blockLeft;
	public GameObject electricWeak;
	public GameObject electricStrong;
	public GameObject nukeExploding;
	public GameObject roninHeavy02a;
	public GameObject roninHeavy02b;
	public GameObject roninHeavy02c;
	public GameObject vanish;
	public GameObject explosion;
	public GameObject summersHeavy03ShockwaveR;
	public GameObject summersHeavy03ShockwaveL;
	public GameObject malfunctioning;
	public GameObject lostScrew;
	public GameObject incoming;
	public GameObject slu66erHead;
	public GameObject highRo11erHead;
	public GameObject weakened;
	public GameObject zapIt;
	public GameObject destroyIt;
	public GameObject smashIt;
	public GameObject slashIt;
	public GameObject mashIt;
	public GameObject holdIt;

	public void SpawnEffect (Effects effectName, Transform parent) {
		BoxCollider box = parent.gameObject.GetComponent<BoxCollider> ();
		if (effectName == Effects.strikeWeak) {
			Instantiate (strikeWeak, RandomSpawn(box), Quaternion.identity, this.transform);
		}
		if (effectName == Effects.strikeStrong) {
			Instantiate (strikeStrong, RandomSpawn(box), Quaternion.identity, this.transform);
		}
		if (effectName == Effects.slashWeak) {
			Instantiate (slashWeak, RandomSpawn(box), Quaternion.identity, this.transform);
		}
		if (effectName == Effects.slashStrong) {
			Instantiate (slashStrong, RandomSpawn(box), Quaternion.identity, this.transform);
		}
		if (effectName == Effects.electricWeak) {
			Instantiate (electricWeak, RandomSpawn(box), Quaternion.identity, this.transform);
		}
		if (effectName == Effects.electricStrong) {
			Instantiate (electricStrong, RandomSpawn(box), Quaternion.identity, this.transform);
		}
		if (effectName == Effects.stage2RoninLeft) {Instantiate (stage2RoninLeft, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage2RoninRight) {Instantiate (stage2RoninRight, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage2Summers) {Instantiate (stage2Summers, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage2Wondie) {Instantiate (stage2Wondie, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage2X42) {Instantiate (stage2X42, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage3RoninLeft) {Instantiate (stage3RoninLeft, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage3RoninRight) {Instantiate (stage3RoninRight, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage3Summers) {Instantiate (stage3Summers, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage3Wondie) {Instantiate (stage3Wondie, CenterSpawn(box), Quaternion.identity, this.transform);}
		if (effectName == Effects.stage3X42) {Instantiate (stage3X42, CenterSpawn(box), Quaternion.identity, this.transform);}

		if (effectName == Effects.jump) {
			Instantiate (jump, new Vector3 (box.bounds.center.x, box.bounds.min.y, box.bounds.center.z), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.landLeft) {
			Instantiate (landRight, new Vector3 (box.bounds.max.x, box.bounds.min.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
			Instantiate (landLeft, new Vector3 (box.bounds.min.x, box.bounds.min.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.accelLeft) {
			Instantiate (accelLeft, new Vector3 (box.bounds.min.x, box.bounds.min.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.accelRight) {
			Instantiate (accelRight, new Vector3 (box.bounds.min.x, box.bounds.min.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.brakeLeft) {
			Instantiate (brakeLeft, new Vector3 (box.bounds.max.x, box.bounds.min.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.brakeRight) {
			Instantiate (brakeRight, new Vector3 (box.bounds.min.x, box.bounds.min.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.block) {
			Instantiate (block, new Vector3 (box.bounds.max.x - 0.1f, box.bounds.center.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.blockLeft) {
			Instantiate (blockLeft, new Vector3 (box.bounds.min.x + 0.1f, box.bounds.center.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.electrified) {
			Instantiate (electrified, new Vector3 (box.bounds.center.x, box.bounds.center.y, box.bounds.center.z - 0.01f), Quaternion.identity, box.transform);
		}

		if (effectName == Effects.nukeExploding) {
			Instantiate (nukeExploding, CenterSpawn (box), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.roninHeavy02) {
			int rN = Random.Range (1, 4);
			if (rN == 1) {
				Instantiate (roninHeavy02a, CenterSpawn (box), Quaternion.identity, this.transform);
			}
			if (rN == 2) {
				Instantiate (roninHeavy02b, CenterSpawn (box), Quaternion.identity, this.transform);
			}
			if (rN == 3) {
				Instantiate (roninHeavy02c, CenterSpawn (box), Quaternion.identity, this.transform);
			}
		}

		if (effectName == Effects.vanish) {
			Instantiate (vanish, new Vector3 (box.bounds.center.x, box.bounds.min.y, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.explosion) {
			Instantiate (explosion, RandomSpawn (box), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.summersHeavy03ShockwaveR) {
			Instantiate (summersHeavy03ShockwaveR, new Vector3 (box.bounds.max.x, box.bounds.center.y+0.2f, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.summersHeavy03ShockwaveL) {
			Instantiate (summersHeavy03ShockwaveL, new Vector3 (box.bounds.min.x, box.bounds.center.y+0.2f, box.bounds.center.z - 0.01f), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.malfunctioning) {
			Instantiate (malfunctioning, new Vector3 (box.bounds.center.x, box.bounds.center.y + 0.2f, box.bounds.center.z), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.lostScrew) {
			int rN = Random.Range (0, 2);
			SpriteRenderer sr = lostScrew.GetComponent<SpriteRenderer> ();
			if (rN >= 1)
				sr.flipX = true;
			else
				sr.flipX = false;
			Instantiate (lostScrew, RandomSpawn (box), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.slu66erHead) {
			Instantiate (slu66erHead, new Vector3 (box.bounds.center.x, 4f, box.bounds.center.z), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.highRo11erHead) {
			Instantiate (highRo11erHead, new Vector3 (box.bounds.center.x, 4f, box.bounds.center.z), Quaternion.identity, this.transform);
		}

		if (effectName == Effects.weakened) {
			Instantiate (weakened, CenterSpawn (box), Quaternion.identity, this.transform);
		}
			
		if (effectName == Effects.zapIt) {
			Instantiate (zapIt, CenterSpawn (box), Quaternion.identity, parent);
		}
		if (effectName == Effects.destroyIt) {
			Instantiate (destroyIt, CenterSpawn (box), Quaternion.identity, parent);
		}
		if (effectName == Effects.smashIt) {
			Instantiate (smashIt, CenterSpawn (box), Quaternion.identity, parent);
		}
		if (effectName == Effects.slashIt) {
			Instantiate (slashIt, CenterSpawn (box), Quaternion.identity, parent);
		}

		if (effectName == Effects.mashIt) {
			Instantiate (mashIt, new Vector3 (box.bounds.min.x, box.bounds.max.y, box.bounds.center.z - 0.01f), Quaternion.identity, parent);
		}
		if (effectName == Effects.holdIt) {
			Instantiate (holdIt, new Vector3 (box.bounds.min.x, box.bounds.max.y, box.bounds.center.z - 0.01f), Quaternion.identity, parent);
		}
	}

	public EffectProperties SpawnEffect (Effects effectName, Transform parent, bool confirmation) {
		BoxCollider box = parent.gameObject.GetComponent<BoxCollider> ();
		if (effectName == Effects.zapIt) {
			return Instantiate (zapIt, CenterSpawn (box), Quaternion.identity, parent).GetComponent<EffectProperties>();
		}
		if (effectName == Effects.destroyIt) {
			return Instantiate (destroyIt, CenterSpawn (box), Quaternion.identity, parent).GetComponent<EffectProperties>();
		}
		if (effectName == Effects.smashIt) {
			return Instantiate (smashIt, CenterSpawn (box), Quaternion.identity, parent).GetComponent<EffectProperties>();
		}
		if (effectName == Effects.slashIt) {
			return Instantiate (slashIt, CenterSpawn (box), Quaternion.identity, parent).GetComponent<EffectProperties> ();
		} else {
			return null;
		}
	}

	public void SpawnEffect (Effects effectName, Vector3 spawnPoint) {
		if (effectName == Effects.incoming) {
			Instantiate (incoming, new Vector3 (spawnPoint.x-0.05f, spawnPoint.y, spawnPoint.z), Quaternion.identity, this.transform);
		}
	}

	Vector3 RandomSpawn (BoxCollider box) {
		return new Vector3 (Random.Range (box.bounds.max.x, box.bounds.min.x), Random.Range (box.bounds.max.y, box.bounds.min.y+0.2f), box.bounds.center.z - 0.1f);
	}

	Vector3 CenterSpawn (BoxCollider box) {
		return new Vector3 (box.bounds.center.x, box.bounds.center.y, box.bounds.center.z);
	}
}
