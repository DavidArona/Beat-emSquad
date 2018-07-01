using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ButtonHighlighted : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public bool onButton;

	// When highlighted with mouse.
	public void OnPointerEnter(PointerEventData eventData)
	{
		onButton = true;
		// Do something.
		Debug.Log("<color=red>Event:</color> Completed mouse highlight.");
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		onButton = false;
		Debug.Log("The cursor exited the selectable UI element.");
	}
}
