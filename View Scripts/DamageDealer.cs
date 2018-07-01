using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {

	public int damage = 3;
	public float stunTime = 0.2f;
	AnimationManager am;
	public bool hitCondition = false;
	public bool finisher = false;
	public bool general = false;
	public bool continuous = false;
	public string dmgType = "physical";

	private Collider collider;

	void Start(){
		am = GetComponentInParent<AnimationManager> ();
		collider = GetComponent<Collider> ();
		if (!collider.isTrigger) {
			collider.isTrigger = true;
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Hurtbox") {
			if (true) {
				if (!general)
					other.gameObject.GetComponent<DamageReceiver> ().receiveDamage (damage, stunTime, dmgType, this.transform.root.tag, finisher, this.transform);
				else
					other.gameObject.GetComponent<DamageReceiver> ().receiveDamage (damage, stunTime, dmgType, "General", finisher, this.transform);
				if (this.transform.root.gameObject.tag == "Player") {
					if (this.transform.root.gameObject.GetComponent<PlayerController> ().characterName == "Ronin") {
						RoninController rC = this.transform.root.gameObject.GetComponent<RoninController> ();
						rC.successfulHit = true;
					}
				}
			}
			if (hitCondition && this.gameObject.tag != "Projectile") {
				if (am != null) {
					if (this.transform.root.GetComponent<PlayerController> ().characterName == "X-42") {
						this.transform.root.GetComponent<X42Controller> ().InstantDeath (other.transform.root.gameObject);
					}
					am.HitSomething (1);
				}
				else {
					this.transform.root.GetComponent<Animator> ().SetTrigger ("hitCondition");
				}
			}
			if (this.gameObject.tag == "Projectile") {
				if (other.transform.root.gameObject.tag == "Enemy") {
					ProjectileProperties pp = this.GetComponent<ProjectileProperties> ();
					pp.Explode (hitCondition);
				}
			}
			if (!continuous) StartCoroutine (maintainColliders ());
		}
	}

	IEnumerator maintainColliders() {
		yield return new WaitForSeconds (0.05f);
		GetComponent<Collider> ().enabled = false;
	}
}
