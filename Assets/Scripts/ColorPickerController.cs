using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerController : MonoBehaviour {

	private bool open;

	public GameObject colorPicker;

	// Use this for initialization
	void Start () {
		open = false;
		colorPicker.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			open = !open;
			colorPicker.SetActive (open);
			colorPicker.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
			colorPicker.transform.rotation = Camera.main.transform.rotation;
		}

	}

	public void selectColor(Color color) {
		
	}
}
