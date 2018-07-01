using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BystanderDR : DamageReceiver {

	public override void receiveDamage (int damage, float stunTime, string dmgType, string attacker, bool finisher, Transform transform) {
		Debug.Log ("damage has been received by child");
	}
}
