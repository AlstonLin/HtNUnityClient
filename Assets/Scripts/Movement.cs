using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	// Clicks
	const float LONG_CLICK_TIME = 0.2f;
	const float DOUBLE_CLICK_BETWEEN_TIME = 0.3f;
	float touchTime = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Touch detected
		if (Input.touchCount > 0) {
			touchTime += Input.GetTouch(0).deltaTime;
			if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				touchTime = 0;
			}
			if (touchTime >= LONG_CLICK_TIME) {
				transform.Translate (Camera.main.transform.forward * 0.1f);
			}
		}
	}
}
