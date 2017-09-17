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
		gameObject.transform.parent.gameObject.GetComponent<HUDController> ().close ();

		switch (name) {
		case "Blue": 
			Cursor.currentColor = Color.blue;
			break;
		case "White":
			Cursor.currentColor = Color.white;
			break;
		case "Yellow":
			Cursor.currentColor = Color.yellow;
			break;
		case "Green":
			Cursor.currentColor = Color.green;
			break;
		case "Purple":
			Cursor.currentColor = new Color(127f/255f, 0, 1);
			break;
		case "Orange":
			Cursor.currentColor = new Color(1, 127f/255f, 0);
			break;
		case "Black":
			Cursor.currentColor = Color.black;
			break;
		case "Red":
			Cursor.currentColor = new Color(195f/255f, 4f/255f, 4f/255f);
			break;
		default:
			print("No color \"" + name + "\" found in ColorPicker");
			break;
		}
	}
}
