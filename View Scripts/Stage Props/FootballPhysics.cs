using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballPhysics : MonoBehaviour {

	float thrust;
	Rigidbody rb;
	public GameObject hitbox;

	public float prueba = 5f;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void Update() {
		if (rb.velocity.x > prueba || rb.velocity.y > prueba || rb.velocity.z > prueba) {
			Debug.Log ("X: " + rb.velocity.x + " Y: " + rb.velocity.y + " Z: " + rb.velocity.z);
			hitbox.SetActive (true);
		} else {
			hitbox.SetActive (false);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.transform.tag == "Hitbox") {
			float direction;
			if (other.transform.position.x < this.transform.position.x) {
				direction = 1f;
			} else {
				direction = -1f;
			}
			rb.AddForce (3f *direction, 3f, 0f, ForceMode.Impulse);
		}
	}
}
