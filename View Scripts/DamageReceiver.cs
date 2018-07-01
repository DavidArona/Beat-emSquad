using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour {

	public bool front = true;
	public int multiplier = 1;
	protected EffectManager em;
	public bool isEnemy;

	private Collider collider;

	// Use this for initialization
	public virtual void Start () {
		em = GameObject.Find ("EffectManager").GetComponent<EffectManager>();
		if (!em) {
			Debug.Log ("There is no Effect Manager");
		}
		collider = GetComponent<Collider> ();
		if (!collider.isTrigger) {
			collider.isTrigger = true;
		}
	}
	
	// Update is called once per frame
	public virtual void receiveDamage (int damage, float stunTime, string dmgType, string attacker, bool finisher, Transform transform) {
		if (this.transform.root.tag == "Player" && (attacker == "Enemy")) {
			playerReceiveDamage (damage, stunTime, dmgType, transform);
		}
		if (this.transform.root.tag == "Enemy" && (attacker == "Player" || attacker == "General")) {
			enemyReceiveDamage (damage, stunTime, dmgType, finisher);
		}
	}

	public void enemyReceiveDamage (int damage, float stunTime, string dmgType, bool finisher) {
		if (gameObject.GetComponentInParent<EnemyController> ().receivedHit (damage, stunTime, front, finisher, dmgType)) {
			if (dmgType == "physical") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.strikeWeak, this.transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.strikeStrong, this.transform.root);
				}
			} else if (dmgType == "slashing") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.slashWeak, this.transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.slashStrong, this.transform.root);
				}
			} else if (dmgType == "electric") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.electricWeak, this.transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.electricStrong, this.transform.root);
				}
				em.SpawnEffect (EffectManager.Effects.electrified, this.transform.root);
			} else if (dmgType == "phantomSlash") {
				em.SpawnEffect (EffectManager.Effects.roninHeavy02, this.transform.root);
			} else if (dmgType == "explosive") {
				em.SpawnEffect (EffectManager.Effects.explosion, this.transform.root);
			} else {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.strikeWeak, this.transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.strikeStrong, this.transform.root);
				}
			}
		}
	}

	public void playerReceiveDamage (int damage, float stunTime, string dmgType, Transform transform) {
		if (gameObject.GetComponentInParent<PlayerController> ().receivedHit (damage, stunTime, front, transform)) {
			if (dmgType == "physical") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.strikeWeak, this.transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.strikeStrong, this.transform.root);
				}
			} else if (dmgType == "slashing") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.slashWeak, transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.slashStrong, this.transform.root);
				}
			} else if (dmgType == "electric") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.electricWeak, this.transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.electricStrong, this.transform.root);
				}
				em.SpawnEffect (EffectManager.Effects.electrified, this.transform.root);
			} else if (dmgType == "phantomSlash") {
				em.SpawnEffect (EffectManager.Effects.roninHeavy02, this.transform.root);
			} else if (dmgType == "explosive") {
				em.SpawnEffect (EffectManager.Effects.explosion, this.transform.root);
			} else {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.strikeWeak, this.transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.strikeStrong, this.transform.root);
				}
			}
		}
	}
}
