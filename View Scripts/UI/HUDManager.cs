using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	[SerializeField]
	private float HPFillAmount = 1;
	[SerializeField]
	private float KinergyFillAmount = 1;
	[SerializeField]
	private Image HPContent;
	[SerializeField]
	private Image KinergyContent;

	public Image X42Kinergy;
	public Image RoninKinergy;
	public Image SummersKinergy;
	public Image WondieKinergy;

	private LoadNewScene sceneManager;

	// Use this for initialization
	void Start () {
		sceneManager = this.transform.root.GetComponent<LoadNewScene> ();
		string name = sceneManager.getPlayerName ();
		if (name == "Captain Summers") {
			KinergyContent = SummersKinergy;
			SummersKinergy.enabled = true;
		}
		if (name == "The Wonder") {
			KinergyContent = WondieKinergy;
			WondieKinergy.enabled = true;
		}
		if (name == "X-42") {
			KinergyContent = X42Kinergy;
			X42Kinergy.enabled = true;
		}
		if (name == "Ronin") {
			KinergyContent = RoninKinergy;
			RoninKinergy.enabled = true;
		}
		KinergyContent.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		HandleHP ();
		HandleKinergy ();
		HPContent.fillAmount = HPFillAmount;
		KinergyContent.fillAmount = Mathf.Lerp(KinergyContent.fillAmount, KinergyFillAmount, 0.1f);

	}

	void HandleHP (){
		HPFillAmount = sceneManager.getPlayerHP ();
		HPFillAmount = HPFillAmount / 100;
	}

	void HandleKinergy() {
		KinergyFillAmount = sceneManager.getPlayerKinergy ();
		KinergyFillAmount = KinergyFillAmount / 100;
	}

	public void ChangeHUD(string name) {
		WondieKinergy.enabled = false;
		X42Kinergy.enabled = false;
		RoninKinergy.enabled = false;
		SummersKinergy.enabled = false;
		if (name == "Captain Summers") {
			KinergyContent = SummersKinergy;
			SummersKinergy.enabled = true;
		}
		if (name == "The Wonder") {
			KinergyContent = WondieKinergy;
			WondieKinergy.enabled = true;
		}
		if (name == "X-42") {
			KinergyContent = X42Kinergy;
			X42Kinergy.enabled = true;
		}
		if (name == "Ronin") {
			KinergyContent = RoninKinergy;
			RoninKinergy.enabled = true;
		}
	}
}
