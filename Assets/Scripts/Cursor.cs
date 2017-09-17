using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : StateChangeListener {
	const float PLANE_WIDTH = 0.01f;

	public static Color currentColor = Color.white;
	public static string currentShape = "Square";
	public static bool hudNotOpen = true;

	Vector3 lastClickTarget;
	GameObject plane = null;
	State state;

	// Clicks
	const float LONG_CLICK_TIME = 0.3f;
	const float DOUBLE_CLICK_BETWEEN_TIME = 0.3f;
	float touchTime = 0f;
	float betweenTouchTime = 0f;
	float depth = 3f;
	int numClicks = 0;

	// Use this for initialization
	void Start () {
		state = (State) GameObject.Find("FirebaseConnector").GetComponent("State");

		state.addListener(this);
	}

	public override void onBlockAdded(Block block) {
		GameObject obj;
		switch (block.shape) {
		case Shapes.SQUARE_FACE:
			obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			break;
		default:
			obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			break;
		}
		switch (block.color) {
		case FaceColors.WHITE:
			obj.GetComponent<Renderer> ().material.color = Color.white;
			break;
		case FaceColors.BLUE:
			obj.GetComponent<Renderer> ().material.color = Color.blue;
			break;
		case FaceColors.YELLOW:
			obj.GetComponent<Renderer> ().material.color = Color.yellow;
			break;
		case FaceColors.RED:
			obj.GetComponent<Renderer> ().material.color = Color.red;
			break;
		default:
			obj.GetComponent<Renderer> ().material.color = Color.white;
			break;
		}
		obj.transform.position.Set (block.location.x, block.location.y, block.location.z);
		obj.transform.rotation.Set (block.rotation.x, block.rotation.y, block.rotation.z, block.rotation.w);
		obj.transform.localScale.Set (block.scale.x, block.scale.y, block.scale.z);
	}
	public override void onBlockRemoved(Block block) {
		Debug.Log("Removed");
		Debug.Log(block);
	}

	// Update is called once per frame
	void Update () {
		if (plane) Destroy (plane);

		//if hud is not open, render cursor block
		if (hudNotOpen) {
			//calculate depth from camera rotation
			depth = Camera.main.transform.eulerAngles.z;
			if (depth <= 90) {
				depth = (depth + 180) / 60;
			} else if (depth >= 270) {
				depth = (depth - 180) / 60;
			} else {
				depth = 3;
			}
			// The direction vector * 3
			Vector3 delta = Camera.main.transform.forward.normalized * depth;
			Vector3 target = Camera.main.gameObject.transform.position + delta;
			// Finds the approximate axis that is being looked from
			float max = Mathf.Max (Mathf.Max (Mathf.Abs (delta.x), Mathf.Abs (delta.y)), Mathf.Abs (delta.z));

			switch (currentShape) {
			case "Square":
				createSquare (delta, target, max);
				break;
			default: 
				createSquare (delta, target, max);
				break;
			}

			// Touch detected
			if (Input.touchCount > 0) {
				touchTime += Input.GetTouch (0).deltaTime;
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					if (touchTime < LONG_CLICK_TIME) { // Click
						numClicks += 1;
						lastClickTarget = plane.transform.position;
					} else {
						numClicks = 0;
						betweenTouchTime = 0;
					}
					touchTime = 0;
				}
			} else {
				if (numClicks > 0 || Input.GetMouseButtonDown(0)) {
					betweenTouchTime += Time.deltaTime;
					if (betweenTouchTime > DOUBLE_CLICK_BETWEEN_TIME || Input.GetMouseButtonDown(0)) {
						if (numClicks == 1 || Input.GetMouseButtonDown(0)) {
							plane.transform.position = lastClickTarget;
							GameObject.Instantiate (plane);

							int intColor;
							int intShape;
							if (currentColor == Color.white) {
								intColor = FaceColors.WHITE;
							} else if(currentColor == Color.blue) {
								intColor = FaceColors.BLUE;
							} else if(currentColor == Color.red) {
								intColor = FaceColors.RED;
							} else if(currentColor == Color.yellow) {
								intColor = FaceColors.YELLOW;
							} else {
								intColor = FaceColors.WHITE;
							}

							switch (currentShape) {
							case "Square":
								intShape = Shapes.SQUARE_FACE;
								break;
							default:
								intShape = Shapes.SQUARE_FACE;
								break;
							}

							Block block = new Block (plane.transform.rotation, plane.transform.position, plane.transform.localScale, intColor, intShape);
							state.addBlockToState (block);
						}
						numClicks = 0;
						betweenTouchTime = 0;
					}
				}
			}
		}
	}

	private void createSquare(Vector3 delta, Vector3 target, float max) {
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
}
