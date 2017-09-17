using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPickerController : MonoBehaviour {

	private bool open;

	public GameObject colorPicker;

	// Clicks
	const float LONG_CLICK_TIME = 0.2f;
	const float DOUBLE_CLICK_BETWEEN_TIME = 0.3f;
	float touchTime = 0f;
	float betweenTouchTime = 0f;
	int numClicks = 0;

	// Use this for initialization
	void Start () {
		open = false;
		colorPicker.SetActive (false);
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
						open = !open;
						colorPicker.SetActive (open);
						colorPicker.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
						colorPicker.transform.rotation = Camera.main.transform.rotation;
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

	public void close() {
		open = false;
		colorPicker.SetActive (false);
	}
}
