using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour {

	float direction;
	int speed;
	float lifetime;
	EffectManager effectManager;

	void Awake() {
		effectManager = FindObjectOfType<EffectManager> ();
	}

	public void Fire (float direction, int speed, float lifeTime, X42Controller.BulletTypes type) {
		Animator am = this.GetComponent<Animator> ();
		Rigidbody rb = this.GetComponent<Rigidbody> ();
		if (type == X42Controller.BulletTypes.KinergyBulletFront) {
			am.Play ("projectileKinergyBullet");
		} else if (type == X42Controller.BulletTypes.KinergyBulletBack) {
			am.Play ("projectileKinergyBulletMid");
		} else if (type == X42Controller.BulletTypes.KinergyBulletUp) {
			am.Play ("projectileKinergyBulletUp");
		} else if (type == X42Controller.BulletTypes.DualBullets1) {
			am.Play ("projectileDualBullets1");
		} else if (type == X42Controller.BulletTypes.DualBullets2) {
			am.Play ("projectileDualBullets2");
		} else if (type == X42Controller.BulletTypes.DualBullets3) {
			am.Play ("projectileDualBullets3");
		} else if (type == X42Controller.BulletTypes.DualBullets4) {
			am.Play ("projectileDualBullets4");
		} else if (type == X42Controller.BulletTypes.DualBullets5) {
			am.Play ("projectileDualBullets5");
		} else if (type == X42Controller.BulletTypes.DualBullets6) {
			am.Play ("projectileDualBullets6");
		} else if (type == X42Controller.BulletTypes.Nuke) {
			am.Play ("projectileNukeTravelling");
		}
		rb.velocity = new Vector3 (rb.velocity.x + (direction * speed), rb.velocity.y, rb.velocity.z);
		Destroy (gameObject, 1f);
	}

	public void Explode (bool hitCondition) {
		if (hitCondition) {
			effectManager.SpawnEffect (EffectManager.Effects.nukeExploding, this.transform);
		}
		Destroy (gameObject);
	}

	public void Pierce () {
		SpriteRenderer sr = this.GetComponent<SpriteRenderer> ();
		sr.sortingLayerName = "Floor";
		sr.sortingLayerID = 1;
	}
}
