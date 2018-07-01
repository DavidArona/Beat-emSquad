using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(InputManager))]
[CanEditMultipleObjects]
public class InputManagerEditor : Editor {

	public override void OnInspectorGUI(){

		InputManager im = target as InputManager;

		EditorGUI.BeginChangeCheck ();

		base.OnInspectorGUI ();

		if (EditorGUI.EndChangeCheck ()) {
			im.RefreshTracker ();
		}
	}
}
