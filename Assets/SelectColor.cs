using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onSelectColor(BaseEventData data) {
		string name = data.selectedObject.name;
		gameObject.transform.parent.gameObject.GetComponent<ColorPickerController> ().close ();

		switch (name) {
		case "Blue": 
			Cursor.currentColor = Color.blue;
			break;
		case "White":
			Cursor.currentColor = Color.white;
			break;
		case "Red":
			Cursor.currentColor = Color.red;
			break;
		case "Yellow":
			Cursor.currentColor = Color.yellow;
			break;
		default:
			print("No color \"" + name + "\" found in ColorPicker");
			break;
		}
	}
}
