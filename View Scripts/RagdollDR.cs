using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RagdollDR : DamageReceiver {

	bool stunned = false;
	public CameraShake camera;

	public Text dmgText;
	int dmgCounter = 0;

	public override void Start ()
	{
		em = GameObject.Find ("EffectManager").GetComponent<EffectManager>();
		if (!em) {
			Debug.Log ("There is no Effect Manager");
		}
	}

	public override void receiveDamage (int damage, float stunTime, string dmgType, string whatever, bool lolz, Transform helloThereImJustAThrowawayVariableNothingMore) {
		if (receivedHit(damage, stunTime, true)) {
			if (dmgType == "physical") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.strikeWeak, transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.strikeStrong, transform.root);
				}
			} else if (dmgType == "slashing") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.slashWeak, transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.slashStrong, transform.root);
				}
			} else if (dmgType == "electric") {
				if (damage < 5) {
					em.SpawnEffect (EffectManager.Effects.electricWeak, transform.root);
				} else {
					em.SpawnEffect (EffectManager.Effects.electricStrong, transform.root);
				}
				em.SpawnEffect (EffectManager.Effects.electrified, transform.root);
			}
		}
	}

	public bool receivedHit (int damageReceived, float stunTime, bool front) {
		if (!stunned) {
			stunned = true;
			dmgCounter += damageReceived;
			StartCoroutine (inmuneTimer (stunTime));
			StartCoroutine (waitForNoDmg (dmgCounter));
			camera.decreaseFactor = 2f;
			camera.shakeDuration = damageReceived / 30f;
			camera.enabled = true;
			Time.timeScale = 0.20f / damageReceived;
			return true;
		} else {
			return false;
		}
	}

	//counts the time until you can damage the character again
	IEnumerator inmuneTimer(float stunTime){
		yield return new WaitForSeconds(0.005f);
		Time.timeScale = 1f;
		yield return new WaitForSeconds(stunTime);
		stunned = false;
	}

	IEnumerator waitForNoDmg(int oldDmgCounter){
		yield return new WaitForSeconds (1f);
		if (oldDmgCounter == dmgCounter) {
			dmgCounter = 0;
		}
	}

	void Update() {
		dmgText.text = dmgCounter.ToString ();
	}
}
