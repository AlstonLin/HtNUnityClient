using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

	private bool hudOpen;

	public GameObject colorPicker;
	public GameObject HUD;
	public GameObject shapePicker;

	// Clicks
	const float LONG_CLICK_TIME = 0.2f;
	const float DOUBLE_CLICK_BETWEEN_TIME = 0.3f;
	float touchTime = 0f;
	float betweenTouchTime = 0f;
	int numClicks = 0;

	// Use this for initialization
	void Start () {
		close ();
	}

	// Update is called once per frame
	void Update () {
		// Touch detected
		if (Input.touchCount > 0) {
			touchTime += Input.GetTouch (0).deltaTime;
			if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				if (touchTime < LONG_CLICK_TIME) { // Click
					numClicks += 1;
					if (numClicks == 2) {
						if (!hudOpen && !colorPicker.activeInHierarchy && !shapePicker.activeInHierarchy) {
							hudOpen = true;
							Cursor.hudNotOpen = false;
							HUD.SetActive (hudOpen);
							HUD.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
							HUD.transform.rotation = Camera.main.transform.rotation;
						} else {
							close ();
						}
					}
				} else {
					numClicks = 0;
					betweenTouchTime = 0;
				}
				touchTime = 0;
			}
		} else {
			if (numClicks > 0) {
				betweenTouchTime += Time.deltaTime;
				if (betweenTouchTime > DOUBLE_CLICK_BETWEEN_TIME) {
					numClicks = 0;
					betweenTouchTime = 0;
				}
			}
		}
	}

	public void openColorPicker() {
		colorPicker.SetActive (true);
		colorPicker.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
		colorPicker.transform.rotation = Camera.main.transform.rotation;
		hudOpen = false;
		Cursor.hudNotOpen = false;
		HUD.SetActive (false);
	}

	public void openShapePicker() {
		shapePicker.SetActive (true);
		shapePicker.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
		shapePicker.transform.rotation = Camera.main.transform.rotation;
		hudOpen = false;
		Cursor.hudNotOpen = false;
		HUD.SetActive (false);
	}

	public void close() {
		Cursor.hudNotOpen = true;
		hudOpen = false;
		colorPicker.SetActive (false);
		shapePicker.SetActive (false);
		HUD.SetActive (false);
	}
}
