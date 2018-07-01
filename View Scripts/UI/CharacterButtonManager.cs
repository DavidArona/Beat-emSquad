using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonManager : MonoBehaviour {

	[SerializeField]
	ButtonHighlighted[] buttonList;

	[SerializeField]
	bool roninHighlighted = false;
	public bool summersHighlighted = false;
	bool wondieHighlighted = false;
	bool x42Highlighted = false;

	public GameObject red;
	public GameObject pink;
	public GameObject yellow;
	public GameObject blue;

	public SpriteRenderer redBck;
	public SpriteRenderer pinkBck;
	public SpriteRenderer yellowBck;
	public SpriteRenderer blueBck;

	public Animator animator;

	// Use this for initialization
	void Start () {
		int children = transform.childCount;
		buttonList = new ButtonHighlighted[children];
		for (int i = 1; i < children; i++) {
			buttonList [i] = transform.GetChild (i).GetComponent<ButtonHighlighted> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		roninHighlighted = activeButton (buttonList [1].onButton, red, redBck);
		summersHighlighted = activeButton (buttonList [2].onButton, blue, blueBck);
		wondieHighlighted = activeButton (buttonList [3].onButton, yellow, yellowBck);
		x42Highlighted = activeButton (buttonList [4].onButton, pink, pinkBck);
		HandleAnimator ();
	}

	void HandleAnimator() {
		animator.SetBool ("roninHighlighted", roninHighlighted);
		animator.SetBool ("summersHighlighted", summersHighlighted);
		animator.SetBool ("wondieHighlighted", wondieHighlighted);
		animator.SetBool ("x42Highlighted", x42Highlighted);
	}

	bool activeButton (bool active, GameObject forefront, SpriteRenderer background) {
		forefront.SetActive (active);
		background.enabled = !active;
		return active;
	}
}
