using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
	public GameObject camera;
	public Transform cameraTransform;
	private CameraShake cameraShake;
	private float ogAmount;
	private float ogDecrease;
	public float smoothSpeed = 0.5f;
	private LoadNewScene sceneManager;
	public GameObject sceneManagerObject;
	[SerializeField]
	private bool isDeploying = false;
	[SerializeField]
	private bool isTraining = false;

	public GameObject characterSelection;

	// Use this for initialization
	void Start () {
		if (GameObject.FindGameObjectWithTag ("SceneManager") == null) {
			sceneManager = Instantiate (sceneManagerObject).GetComponent<LoadNewScene> ();
		} else {
			sceneManager = GameObject.FindGameObjectWithTag ("SceneManager").GetComponent<LoadNewScene> ();
		}
		cameraShake = camera.GetComponent<CameraShake> ();
		ogAmount = cameraShake.shakeAmount;
		ogDecrease = cameraShake.decreaseFactor;
		StartCoroutine (trainRattle ());
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 destinedPosition = new Vector3 (cameraTransform.position.x + 0.1f, cameraTransform.position.y, cameraTransform.position.z);
		cameraTransform.position = Vector3.Lerp(cameraTransform.position, destinedPosition, smoothSpeed);
	}

	IEnumerator trainRattle() {
		yield return new WaitForSeconds (5f);
		cameraShake.shakeAmount = 0.1f;
		cameraShake.decreaseFactor = 1f;
		cameraShake.shakeDuration = 0.4f;
		yield return new WaitForSeconds (0.3f);
		cameraShake.shakeAmount = ogAmount;
		cameraShake.decreaseFactor = ogDecrease;
		StartCoroutine (trainRattle ());
	}

	public void Deploy() {
		isDeploying = true;
		isTraining = false;
		characterSelection.SetActive (true);
	}

	public void Train() {
		isDeploying = false;
		isTraining = true;
		characterSelection.SetActive (true);
	}

	public void Quit() {
		Application.Quit ();
	}

	public void SelectSummers() {
		sceneManager.currentCharacter = "Captain Summers";
		LoadNext ();
	}

	public void SelectWondie() {
		sceneManager.currentCharacter = "The Wonder";
		LoadNext ();
	}

	public void SelectX42() {
		sceneManager.currentCharacter = "X-42";
		LoadNext ();
	}

	public void SelectRonin() {
		sceneManager.currentCharacter = "Ronin";
		LoadNext ();
	}

	void LoadNext() {
		if (isDeploying) {
			sceneManager.LoadScene ("stage1");
		}
		if (isTraining) {
			sceneManager.LoadScene ("trainingRoom");
		}
	}
}
