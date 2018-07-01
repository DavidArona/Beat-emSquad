using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour {

	public GameObject camera;
	CameraFollow cameraFollow;

	public GameObject slu66er;
	public GameObject highRo11er;

	public bool encounterOngoing = false;
	public int enemyCounter = 0;
	public Vector3[] spawnPoints;
	public GameObject[] spawnObjects;

	EffectManager effectManager;
	LoadNewScene sceneManager;

	void Start() {
		cameraFollow = camera.GetComponent<CameraFollow> ();
		effectManager = GameObject.FindObjectOfType<EffectManager> ();
		sceneManager = GameObject.FindObjectOfType<LoadNewScene> ();
	}

	public void encounterActive(int stageNumber, int encounterNumber) {
		cameraFollow.enabled = false;
		cameraFollow.enableEncounterLimits ();
		if (stageNumber == 1) {
			if (encounterNumber == 1) {
				if (!encounterOngoing) {
					enemyCounter = 4;
					encounterOngoing = true;
				}
				encounter1of1 ();
			}
			if (encounterNumber == 2) {
				if (!encounterOngoing) {
					enemyCounter = 7;
					encounterOngoing = true;
				}
				encounter1of2 ();
			}
		}
		if (stageNumber == 2) {
			if (encounterNumber == 1) {
				if (!encounterOngoing) {
					enemyCounter = 10;
					encounterOngoing = true;
				}
				encounter2of1 ();
			}
		}
		if (stageNumber == 3) {
			if (encounterNumber == 1) {
				if (!encounterOngoing) {
					enemyCounter = 7;
					encounterOngoing = true;
				}
				encounter3of1 ();
			}
			if (encounterNumber == 2) {
				if (!encounterOngoing) {
					enemyCounter = 7;
					encounterOngoing = true;
				}
				encounter3of2 ();
			}
		}
	}

	void encounter1of1 () {
		if (enemyCounter == 4) {
			StartCoroutine (waitForSpawn (0, 0f));
			StartCoroutine (waitForDeath (0, 1, 1));
		} else if (enemyCounter == 3) {
			StartCoroutine (waitForSpawn (1, 0.5f));
			StartCoroutine (waitForSpawn (2, 0f));
			StartCoroutine (waitForDeath (1, 1, 1));
			StartCoroutine (waitForDeath (2, 1, 1));
		} else if (enemyCounter == 2) {
			StartCoroutine (waitForSpawn (3, 0f));
			StartCoroutine (waitForDeath (3, 1, 1));
		} else if (enemyCounter == 0) {
			encounterOver ();
		}
	}

	void encounter1of2() {
		if (enemyCounter == 7) {
			StartCoroutine (waitForSpawn (4, 0f));
			StartCoroutine (waitForSpawn (5, 0.5f));
			StartCoroutine (waitForSpawn (6, 1f));
			StartCoroutine (waitForDeath (4, 1, 2));
			StartCoroutine (waitForDeath (5, 1, 2));
			StartCoroutine (waitForDeath (6, 1, 2));
		} else if (enemyCounter == 6) {
			StartCoroutine (waitForSpawn (7, 0.5f));
			StartCoroutine (waitForDeath (7, 1, 2));
		} else if (enemyCounter == 5) {
			StartCoroutine (waitForSpawn (8, 0.5f));
			StartCoroutine (waitForDeath (8, 1, 2));
		} else if (enemyCounter == 2) {
			StartCoroutine (waitForSpawn (9, 0.5f));
			StartCoroutine (waitForSpawn (10, 10f));
			StartCoroutine (waitForDeath (9, 1, 2));
			StartCoroutine (waitForDeath (10, 1, 2));
		} else if (enemyCounter == 0) {
			encounterOver ();
		}
	}

	void encounter2of1() {
		if (enemyCounter == 10) {
			StartCoroutine (waitForSpawn (0, 0f));
			StartCoroutine (waitForSpawn (1, 0f));
			StartCoroutine (waitForDeath (0, 2, 1));
			StartCoroutine (waitForDeath (1, 2, 1));
		} else if (enemyCounter == 8) {
			StartCoroutine (waitForSpawn (2, 0f));
			StartCoroutine (waitForSpawn (3, 5f));
			StartCoroutine (waitForSpawn (4, 10f));
			StartCoroutine (waitForSpawn (5, 15f));
			StartCoroutine (waitForDeath (2, 2, 1));
			StartCoroutine (waitForDeath (3, 2, 1));
			StartCoroutine (waitForDeath (4, 2, 1));
			StartCoroutine (waitForDeath (5, 2, 1));
		} else if (enemyCounter == 7) {
			StartCoroutine (waitForSpawn (6, 0f));
			StartCoroutine (waitForDeath (6, 2, 1));
		} else if (enemyCounter == 6) {
			StartCoroutine (waitForSpawn (7, 0f));
			StartCoroutine (waitForDeath (7, 2, 1));
		} else if (enemyCounter == 5) {
			StartCoroutine (waitForSpawn (8, 0f));
			StartCoroutine (waitForDeath (8, 2, 1));
		} else if (enemyCounter == 4) {
			StartCoroutine (waitForSpawn (9, 0f));
			StartCoroutine (waitForDeath (9, 2, 1));
		} else if (enemyCounter == 0) {
			sceneManager.exit2.enabled = true;
			encounterOngoing = false;
		}
	}

	void encounter3of1(){
		if (enemyCounter == 7) {
			StartCoroutine (waitForSpawn (0, 0f));
			StartCoroutine (waitForSpawn (1, 0.25f));
			StartCoroutine (waitForSpawn (2, 0.5f));
			StartCoroutine (waitForSpawn (3, 1f));
			StartCoroutine (waitForDeath (0, 3, 1));
			StartCoroutine (waitForDeath (1, 3, 1));
			StartCoroutine (waitForDeath (2, 3, 1));
			StartCoroutine (waitForDeath (3, 3, 1));
		}
		if (enemyCounter == 6) {
			StartCoroutine (waitForSpawn (4, 0f));
			StartCoroutine (waitForDeath (4, 3, 1));
		} if (enemyCounter == 5) {
			StartCoroutine (waitForSpawn (5, 0f));
			StartCoroutine (waitForDeath (5, 3, 1));
		} if (enemyCounter == 4) {
			StartCoroutine (waitForSpawn (6, 0f));
			StartCoroutine (waitForDeath (6, 3, 1));
		} else if (enemyCounter == 0) {
			encounterOver ();
		}
	}

	void encounter3of2(){
		if (enemyCounter == 7) {
			StartCoroutine (waitForSpawn (7, 0f));
			StartCoroutine (waitForSpawn (8, 0.5f));
			StartCoroutine (waitForSpawn (9, 1f));
			StartCoroutine (waitForSpawn (10, 1.5f));
			StartCoroutine (waitForSpawn (11, 2f));
			StartCoroutine (waitForSpawn (12, 2.5f));
			StartCoroutine (waitForSpawn (13, 3f));
			StartCoroutine (waitForDeath (7, 3, 2));
			StartCoroutine (waitForDeath (8, 3, 2));
			StartCoroutine (waitForDeath (9, 3, 2));
			StartCoroutine (waitForDeath (10, 3, 2));
			StartCoroutine (waitForDeath (11, 3, 2));
			StartCoroutine (waitForDeath (12, 3, 2));
			StartCoroutine (waitForDeath (13, 3, 2));
		} else if (enemyCounter == 0) {
			encounterOver ();
		}
	}

	public IEnumerator waitForSpawn (int spawnNumber, float time) {
		yield return new WaitForSeconds (time);
		effectManager.SpawnEffect (EffectManager.Effects.incoming, spawnPoints [spawnNumber]);
		yield return new WaitForSeconds (0.8f);
		spawnObjects[spawnNumber] = Instantiate (spawnObjects [spawnNumber], spawnPoints [spawnNumber], Quaternion.identity);
		spawnObjects [spawnNumber].GetComponent<EnemyController> ().camera = this.camera.GetComponent<CameraShake> ();
	}

	public IEnumerator waitForDeath (int spawNumber, int myStageNumber, int myEncounterNumber) {
		while (spawnObjects [spawNumber] != null) {
			yield return new WaitForSeconds (0.1f);
		}
		Time.timeScale = 1f;
		enemyCounter -= 1;
		encounterActive (myStageNumber, myEncounterNumber);
	}

	void encounterOver () {
		encounterOngoing = false;
		cameraFollow.enabled = true;
		cameraFollow.disableEncounterLimits ();
	}
}
