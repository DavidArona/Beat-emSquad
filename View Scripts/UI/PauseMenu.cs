using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public static bool isPaused;
	bool buttonPushed;

	public GameObject pauseMenuUI;

	float prevTimeScale;

	public void ReadInput (InputData data) {
		if (data.buttons [3] == true) {
			if (isPaused && buttonPushed == false) {
				Resume ();
			} else if (!isPaused && buttonPushed == false) {
				Pause ();
			}
			buttonPushed = true;
		} else {
			buttonPushed = false;
		}
	}

	public void Resume() {
		pauseMenuUI.SetActive (false);
		Time.timeScale = prevTimeScale;
		isPaused = false;
	}

	void Pause() {
		pauseMenuUI.SetActive (true);
		prevTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		isPaused = true;
	}

	public void Quit() {
		LoadNewScene sceneManager = this.GetComponent<LoadNewScene> ();
		Resume ();
		sceneManager.LoadScene ("mainMenu");
	}
}
