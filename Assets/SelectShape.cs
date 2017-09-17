using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectShape : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onSelectShape(BaseEventData data) {
		string name = data.selectedObject.name;
		gameObject.transform.parent.gameObject.GetComponent<HUDController> ().close ();

		// name is name of object under "ShapePicker"
		switch (name) {
		case "Square": 
			Cursor.currentShape = Shapes.SQUARE_FACE;
			break;
		case "CenterTriangle":
			Cursor.currentShape = Shapes.CENTER_TRIANGLE;
			break;
		case "SlantSquare":
			Cursor.currentShape = Shapes.SLANT_SQUARE;
			break;
		default:
			print("No shape \"" + name + "\" found in ShapePicker");
			break;
		}
	}
}
