using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class LoadNewScene : MonoBehaviour {

	private static bool created = false;

	Vector3 stage1StartingPosition;
	Vector3 stage2StartingPosition;
	Vector3 stage3StartingPosition;
	Vector3 trainingRoomStartingPosition;

	public EffectManager effectManager;
	public InputManager inputManager;
	public CameraShake cameraShake;
	public CameraFollow cameraFollow;

	public GameObject captainSummers;
	public GameObject theWonder;
	public GameObject x42;
	public GameObject ronin;

	public Collider exit1;
	public Collider exit2;

	public string currentCharacter = "Not Selected";
	int currentHP;
	Vector3 currentPosition;

	private GameObject player;
	public GameObject gameOver;

	public GameObject canvas;
	private PauseMenu pauseMenu;

	bool fading = false;

	// Use this for initialization
	void Awake () {
		if (!created) {
			DontDestroyOnLoad (this.gameObject);
			created = true;
		}
		stage1StartingPosition = new Vector3 (-3.83f, 0f, -1.34f);
		stage2StartingPosition = new Vector3 (-3.725f, 0f, -1.766f);
		stage3StartingPosition = new Vector3 (-3.942f, 0f, -1.54f);
		trainingRoomStartingPosition = new Vector3 (-2.5f, 0f, -1.85f);
		pauseMenu = GetComponent<PauseMenu> ();
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (SceneManager.GetActiveScene ().name == "mainMenu") {
			pauseMenu.enabled = false;
			gameOver.SetActive (false);
			currentCharacter = "Not Selected";
			canvas.SetActive (false);
			exit1.enabled = false;
			exit2.enabled = false;
		} else {
			onStage ();
			FindGameObjects ();
			inputManager.pauseMenu = this.gameObject.GetComponent<PauseMenu> ();
			if (SceneManager.GetActiveScene ().name == "stage1") {
				CharacterSelect (stage1StartingPosition, 100);
				exit1.enabled = true;
			}
			if (SceneManager.GetActiveScene ().name == "stage2") {
				CharacterSelect (stage2StartingPosition, 100);
			}
			if (SceneManager.GetActiveScene ().name == "stage3") {
				CharacterSelect (stage3StartingPosition, 100);
			}
			if (SceneManager.GetActiveScene ().name == "trainingRoom") {
				CharacterSelect (trainingRoomStartingPosition, 100);
			}
		}
	}

	public void LoadScene (string sceneName) {
		if (sceneName == "mainMenu") {
			Initiate.Fade ("mainMenu", Color.black, 2.0f);
		}
		if (SceneManager.GetActiveScene ().name == "mainMenu" && sceneName == "stage1") {
			Color color;
			if (currentCharacter == "Captain Summers") {
				color = Color.cyan;
			}
			else if (currentCharacter == "Ronin") {
				color = Color.red;
			}
			else if (currentCharacter == "X-42") {
				color = Color.magenta;
			}
			else if (currentCharacter == "The Wonder") {
				color = Color.yellow;
			} else {
				color = Color.black;
			}
			Initiate.Fade ("stage1", color, 2.0f);
		} else if (SceneManager.GetActiveScene ().name == "mainMenu" && sceneName == "trainingRoom") {
			Initiate.Fade ("trainingRoom", Color.black, 2.0f);
		}
		else if (SceneManager.GetActiveScene ().name == "stage1") {
			Initiate.Fade ("stage2", Color.black, 2.0f);
			exit1.enabled = false;
		}
		else if (SceneManager.GetActiveScene ().name == "stage2") {
			Initiate.Fade ("stage3", Color.black, 2.0f);
			exit2.enabled = false;
		}
	}

	void CharacterSelect(Vector3 spawnPoint, int currentHP) {
		if (currentCharacter == "Captain Summers") {
			player = Instantiate (captainSummers, spawnPoint, Quaternion.identity);
			cameraFollow.target = player.transform;
			SummersController Scontroller = player.GetComponent<SummersController> ();
			Scontroller.effectManager = this.effectManager;
			Scontroller.camera = cameraShake;
			inputManager.controller = Scontroller;
		}
		if (currentCharacter == "The Wonder") {
			player = Instantiate (theWonder, spawnPoint, Quaternion.identity);
			cameraFollow.target = player.transform;
			WondieController Wcontroller = player.GetComponent<WondieController> ();
			Wcontroller.effectManager = this.effectManager;
			Wcontroller.camera = cameraShake;
			inputManager.controller = Wcontroller;
		}
		if (currentCharacter == "Ronin") {
			player = Instantiate (ronin, spawnPoint, Quaternion.identity);
			cameraFollow.target = player.transform;
			RoninController Rcontroller = player.GetComponent<RoninController> ();
			Rcontroller.effectManager = this.effectManager;
			Rcontroller.camera = cameraShake;
			inputManager.controller = Rcontroller;
		}
		if (currentCharacter == "X-42") {
			player = Instantiate (x42, spawnPoint, Quaternion.identity);
			cameraFollow.target = player.transform;
			X42Controller Xcontroller = player.GetComponent<X42Controller> ();
			Xcontroller.effectManager = this.effectManager;
			Xcontroller.camera = cameraShake;
			inputManager.controller = Xcontroller;
		}
		HUDManager hudM = FindObjectOfType<HUDManager> ();
		hudM.ChangeHUD (currentCharacter);
	}

	public void CharacterChange (string newCharacterName, Vector3 spawnPoint) {
		currentCharacter = newCharacterName;
		CharacterSelect (spawnPoint, getPlayerHP());
		EnemyController[] enemies = FindObjectsOfType<EnemyController>();
		foreach (EnemyController enemy in enemies) {
			enemy.ChangeCharacter (player.transform);
		}
	}

	void FindGameObjects () {
		effectManager = (EffectManager)FindObjectOfType(typeof(EffectManager));
		inputManager = FindObjectOfType <InputManager> ();
		cameraShake = FindObjectOfType <CameraShake> ();
		cameraFollow = FindObjectOfType <CameraFollow> ();
	}

	public int getPlayerKinergy() {
		PlayerController controller = player.GetComponent<PlayerController> ();
		if (controller.currentKinergy > 100) {
			return 100;
		}
		else if (controller.currentKinergy < 0) {
			return 0;
		} else {
			return controller.currentKinergy;
		}
	}

	public int getPlayerHP() {
		PlayerController controller = player.GetComponent<PlayerController> ();
		if (controller.remainingHP > 100) {
			return 100;
		}
		else if (controller.remainingHP <= 0) {
			if (!gameOver.activeSelf && SceneManager.GetActiveScene().name != "mainMenu") {
				StartCoroutine (waitForDead ());
			}
			return 0;
		} else {
			return controller.remainingHP;
		}
	}

	public string getPlayerName () {
		PlayerController controller = player.GetComponent<PlayerController> ();
		return controller.characterName;
	}

	IEnumerator waitForDead () {
		fading = true;
		yield return new WaitForSeconds (1f);
		Time.timeScale = 1f;
		gameOver.GetComponent<Fade> ().FadeIn();
		gameOver.SetActive (true);
		yield return new WaitForSeconds (8f);
		LoadScene ("mainMenu");
		fading = false;
	}

	void onStage() {
		canvas.SetActive (true);
		pauseMenu.enabled = true;
	}
}
