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

	}
}
