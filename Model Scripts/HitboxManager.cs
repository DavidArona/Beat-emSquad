using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitboxManager : MonoBehaviour {

	protected int i, j, k = 0;
	protected int frameCounter = 0;
	protected float timeMaintaining = 0f;

	protected BoxCollider[][][] colliders;
	protected BoxCollider[] boxes;
	protected BoxCollider[] prevBoxes;

	// Use this for initialization
	void Start () {

		colliders = new BoxCollider[gameObject.transform.childCount][][];

		//breadth-first search
		foreach (Transform child in transform) {
			colliders [i] = new BoxCollider[child.childCount][];
			foreach (Transform grandchild in child) {
				colliders [i][j] = new BoxCollider[grandchild.childCount];
				foreach (Transform colliderType in grandchild) {
					colliders [i] [j] [k] = colliderType.GetComponent<BoxCollider> ();
					k++;
				}
				j++;
				k= 0;
			}
			i++;
			j = 0;
		}
		i = 0;
	}

	public void reverseColliders () {
		foreach (BoxCollider[][] colliderSet in colliders) {
			foreach (BoxCollider[] colliderSubSet in colliderSet) {
				foreach (BoxCollider collider in colliderSubSet) {
					if (collider != null) {
						collider.center = new Vector3 (collider.center.x * -1, collider.center.y, collider.center.z);
					}
				}
			}
		}
	}

	protected void enableColliders (BoxCollider[] colliderArray, bool enable) {
		if (enable) {
			foreach (BoxCollider collider in colliderArray) {
				collider.enabled = enable;
			}
		} else {
			StartCoroutine (maintainColliders (colliderArray));
		}
	}

	IEnumerator maintainColliders(BoxCollider[] colliderArray) {
		yield return new WaitForSeconds (timeMaintaining);
		foreach (BoxCollider collider in colliderArray) {
			collider.enabled = false;
		}
	}
}
