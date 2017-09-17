using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	const float PLANE_WIDTH = 0.01f;

	public static Color currentColor = Color.white;
	public static bool hudNotOpen = true;

	// Caches
	Vector3 lastClickTarget;

	// Removal stuff
	GameObject lastHighlightedTarget = null;
	GameObject lastSelectedTarget = null;
	Color lastHighlightedColor;

	GameObject plane = null;

	// Clicks
	const float LONG_CLICK_TIME = 0.3f;
	const float DOUBLE_CLICK_BETWEEN_TIME = 0.3f;
	const float MAX_REMOVAL_DIST = 5f;
	float touchTime = 0f;
	float betweenTouchTime = 0f;
	float depth = 3f;
	int numClicks = 0;

	// Removal
	public static bool removeMode = false;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (plane) Destroy (plane);
		GameObject hitObject = null;
		if (lastHighlightedColor != null && lastHighlightedTarget != null) {
			lastHighlightedTarget.GetComponent<Renderer> ().material.color = lastHighlightedColor;
		}
		//if hud is not open, render cursor block
		if (hudNotOpen) {
			//calculate depth from camera rotation
			depth = Camera.main.transform.eulerAngles.z;
			if (depth <= 90) {
				depth = (depth + 180) / 60;
			} else if(depth >= 270) {
				depth = (depth - 180) / 60;
			}	
			// The direction vector * 3
			Vector3 delta = Camera.main.transform.forward.normalized * depth;
			Vector3 target = Camera.main.gameObject.transform.position + delta;
			// Finds if a plane is selected
			if (removeMode) {
				RaycastHit hit;
				Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity);
				hitObject = hit.transform.gameObject;
				lastHighlightedColor = hitObject.GetComponent<Renderer> ().material.color;
				hitObject.GetComponent<Renderer> ().material.color = Color.red;
				lastHighlightedTarget = hitObject;
			} else {
				// Finds the approximate axis that is being looked from
				float max = Mathf.Max (Mathf.Max (Mathf.Abs (delta.x), Mathf.Abs (delta.y)), Mathf.Abs (delta.z));
				plane = GameObject.CreatePrimitive (PrimitiveType.Cube);
				plane.GetComponent<Renderer> ().material.color = currentColor;
				if (max == Mathf.Abs (delta.x)) {
					// Creates a highlight plane on y-z axis
					plane.transform.position = new Vector3 ((float)Mathf.Floor (target.x), Mathf.Ceil (target.y) - 0.5f, Mathf.Ceil (target.z) - 0.5f);
					plane.transform.localScale = new Vector3 (PLANE_WIDTH, 1, 1);
				} else if (max == Mathf.Abs (delta.y)) {
					// Creates a highlight plane on x-z axis
					plane.transform.position = new Vector3 (Mathf.Ceil (target.x) - 0.5f, (float)Mathf.Floor (target.y), Mathf.Ceil (target.z) - 0.5f);
					plane.transform.localScale = new Vector3 (1, PLANE_WIDTH, 1);
				} else { // delta.z is max
					// Creates a highlight plane on x-y axis
					plane.transform.position = new Vector3 (Mathf.Ceil (target.x) - 0.5f, Mathf.Ceil (target.y) - 0.5f, (float)Mathf.Floor (target.z));
					plane.transform.localScale = new Vector3 (1, 1, PLANE_WIDTH);
				}
			}
			// Touch detected
			if (Input.touchCount > 0) {
				touchTime += Input.GetTouch (0).deltaTime;
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					if (touchTime < LONG_CLICK_TIME) { // Click
						numClicks += 1;
						if (!removeMode) {
							lastClickTarget = plane.transform.position;
						} else {
							lastSelectedTarget = hitObject;
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
						if (numClicks == 1) {
							if (removeMode && lastSelectedTarget) {
								Destroy (lastSelectedTarget);
								lastSelectedTarget = null;
							} else if (!removeMode) {
								plane.transform.position = lastClickTarget;
								GameObject.Instantiate (plane);
							}
						}
						numClicks = 0;
						betweenTouchTime = 0;
					}
				}
			}
		}
	}
}
