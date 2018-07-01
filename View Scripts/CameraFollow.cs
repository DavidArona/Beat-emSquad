using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	Vector3 startingPosition;

	public float smoothSpeed = 0.75f;
	public float stageLimit = 50f;
	public Vector3 destinedPosition;

	public float adjustment = 0f;

	void Start() {
		startingPosition = transform.position;
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void LateUpdate() {
		destinedPosition = new Vector3 (target.position.x+adjustment, transform.position.y, transform.position.z);

		if (destinedPosition.x <= startingPosition.x) {
			destinedPosition = new Vector3 (startingPosition.x+adjustment, transform.position.y, transform.position.z);
		}
		if (destinedPosition.x >= stageLimit) {
			destinedPosition = new Vector3 (stageLimit+adjustment, transform.position.y, transform.position.z);
		}

		if (transform.position != destinedPosition) {
			transform.position = Vector3.Lerp (transform.position, destinedPosition, smoothSpeed);
		}
	}

	public void disableEncounterLimits() {
		BoxCollider[] encounterLimits = this.transform.GetComponentsInChildren<BoxCollider> ();
		foreach (BoxCollider encounterLimit in encounterLimits) {
			encounterLimit.enabled = false;
		}
	}

	public void enableEncounterLimits() {
		BoxCollider[] encounterLimits = this.transform.GetComponentsInChildren<BoxCollider> ();
		foreach (BoxCollider encounterLimit in encounterLimits) {
			encounterLimit.enabled = true;
		}
	}
}
