using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : StateChangeListener {
	const float PLANE_WIDTH = 0.01f;

	public static Color currentColor = Color.white;
	public static int currentShape = Shapes.SQUARE_FACE;
	public static bool hudNotOpen = true;

	// Caches
	Vector3 lastClickTarget;

	// Removal stuff
	GameObject lastHighlightedTarget = null;
	GameObject lastSelectedTarget = null;
	Color lastHighlightedColor;

	GameObject plane = null;
	State state;

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
		state = (State) GameObject.Find("FirebaseConnector").GetComponent("State");

		state.addListener(this);
	}

	public override void onBlockAdded(Block block) {
		GameObject obj;
		switch (block.shape) {
		case Shapes.SQUARE_FACE:
			obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			break;
		case Shapes.CENTER_TRIANGLE:
			obj = createCenterTrianglePrim ();
			break;
		case Shapes.SLANT_SQUARE:
			obj = createSlantedSquarePrim ();
			break;
		default:
			obj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			break;
		}
		switch (block.color) {
		case FaceColors.WHITE:
			obj.GetComponent<Renderer> ().sharedMaterial.color = Color.white;
			break;
		case FaceColors.BLUE:
			obj.GetComponent<Renderer> ().sharedMaterial.color = Color.blue;
			break;
		case FaceColors.YELLOW:
			obj.GetComponent<Renderer> ().sharedMaterial.color = Color.yellow;
			break;
		case FaceColors.RED:
			obj.GetComponent<Renderer> ().sharedMaterial.color = Color.red;
			break;
		default:
			obj.GetComponent<Renderer> ().sharedMaterial.color = Color.white;
			break;
		}
		obj.transform.position = block.location;
		obj.transform.rotation.Set (block.rotation.x, block.rotation.y, block.rotation.z, block.rotation.w);
		obj.transform.localScale = block.scale;
		ActiveBlocksDictionary.addToDict (block.id, obj);
	}
	public override void onBlockRemoved(Block block) {
		Destroy (ActiveBlocksDictionary.getObj (block.id));
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
			} else if (depth >= 270) {
				depth = (depth - 180) / 60;
			} else {
				depth = 3;
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
				switch (currentShape) {
				case Shapes.SQUARE_FACE:
					createSquare (delta, target, max);
					break;
				case Shapes.CENTER_TRIANGLE:
					createCenterTriangle (delta, target, max);
					break;
				case Shapes.SLANT_SQUARE:
					createSlantedSquare (delta, target, max);
					break;
				default: 
					createSquare (delta, target, max);
					break;
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
				if (numClicks > 0 || Input.GetMouseButtonDown(0)) {
					betweenTouchTime += Time.deltaTime;
					if (betweenTouchTime > DOUBLE_CLICK_BETWEEN_TIME || Input.GetMouseButtonDown(0)) {
						if (numClicks == 1 || Input.GetMouseButtonDown(0)) {
							if (removeMode && lastSelectedTarget) {
								Destroy (lastSelectedTarget);
								Block block = new Block (lastSelectedTarget.transform.rotation, lastSelectedTarget.transform.position,
									lastSelectedTarget.transform.localScale, 0, 0, lastSelectedTarget.name);
								state.removeBlockFromState (block);
								lastSelectedTarget = null;
							} else if (!removeMode) {
								plane.transform.position = lastClickTarget;
								GameObject newPlane = GameObject.Instantiate (plane);
								newPlane.name = HashGenerator.generateHash ();
								ActiveBlocksDictionary.addToDict (newPlane.name, newPlane);

								int intColor;
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
									
								Block block = new Block (newPlane.transform.rotation, newPlane.transform.position, 
									newPlane.transform.localScale, intColor, currentShape, newPlane.name);
								state.addBlockToState (block);
							}
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

	private GameObject createCenterTrianglePrim() {
		GameObject plane = new GameObject ();
		plane.AddComponent<MeshFilter>();
		plane.AddComponent<MeshRenderer>();
		Mesh mesh = plane.GetComponent<MeshFilter> ().mesh;
		mesh.vertices = new Vector3[] {
			new Vector3 (0, 0, 0), // 0
			new Vector3 (0.5f, 1, 0.5f), // 1
			new Vector3 (0, 0, 1), // 2
			new Vector3 (0+PLANE_WIDTH, 0, 0), // 3
			new Vector3 (0.5f+PLANE_WIDTH, 1, 0.5f), // 4
			new Vector3 (0+PLANE_WIDTH, 0, 1)  // 5
		};
		mesh.triangles = new int[] { 0, 1, 2,  0, 3, 1, 1, 3, 4,  0, 2, 3, 2, 5, 3,  1, 4, 2, 4, 5, 2,  3, 5, 4 };
		return plane;
	}

	private void createCenterTriangle(Vector3 delta, Vector3 target, float max) {
		plane = createCenterTrianglePrim ();
		plane.GetComponent<Renderer> ().material.color = currentColor;
		// unlike Cube, the center of the mesh is on the sides
		plane.transform.position = new Vector3 (Mathf.Floor (target.x), Mathf.Floor (target.y), Mathf.Floor (target.z));
		if (max == Mathf.Abs (delta.x)) {
			// Creates a highlight plane on y-z axis
			if (delta.x >= 0) {
				Debug.Log ("x-axis +");
				plane.transform.rotation = Quaternion.AngleAxis (0, Vector3.up);
			} else {
				Debug.Log ("x-axis -");
				plane.transform.rotation = Quaternion.AngleAxis (180, Vector3.up);
			}
		} else if (max == Mathf.Abs (delta.y)) {
			// Creates a highlight plane on x-z axis
			if (delta.y >= 0) {
				Debug.Log ("y-axis +");
				plane.transform.rotation = Quaternion.AngleAxis (90, Vector3.up);
			} else {
				Debug.Log ("y-axis -");
				plane.transform.rotation = Quaternion.AngleAxis (270, Vector3.up);
			}
		} else { // delta.z is max
			// Creates a highlight plane on x-y axis
			if (delta.z >= 0) {
				Debug.Log ("z-axis +");
				plane.transform.rotation = Quaternion.AngleAxis (270, Vector3.up);
			} else {
				Debug.Log ("z-axis -");
				plane.transform.rotation = Quaternion.AngleAxis (90, Vector3.up);
			}
		}
	}

	private GameObject createSlantedSquarePrim() {
		GameObject plane = new GameObject ();
		plane.AddComponent<MeshFilter>();
		plane.AddComponent<MeshRenderer>();
		Mesh mesh = plane.GetComponent<MeshFilter> ().mesh;
		mesh.vertices = new Vector3[] {
			new Vector3 (0, 0, 0), // 0
			new Vector3 (1, 1, 0), // 1
			new Vector3 (1, 1, 1), // 2
			new Vector3 (0, 0, 1), // 3

			new Vector3 (0+PLANE_WIDTH, 0, 0), // 0
			new Vector3 (1+PLANE_WIDTH, 1, 0), // 1
			new Vector3 (1+PLANE_WIDTH, 1, 1), // 2
			new Vector3 (0+PLANE_WIDTH, 0, 1), // 3
		};
		mesh.triangles = new int[] { 0, 1, 3, 3, 1, 2,  6, 5, 7, 7, 5, 4 };
		return plane;
	}

	private void createSlantedSquare(Vector3 delta, Vector3 target, float max) {
		plane = createSlantedSquarePrim ();
		plane.GetComponent<Renderer> ().material.color = currentColor;
		plane.transform.position = new Vector3 (Mathf.Floor (target.x), Mathf.Floor (target.y), Mathf.Floor (target.z));
		if (max == Mathf.Abs (delta.x)) {
			// Creates a highlight plane on y-z axis
			if (delta.x >= 0) {
				Debug.Log ("x-axis +");
				plane.transform.rotation = Quaternion.AngleAxis (0, Vector3.up);
			} else {
				Debug.Log ("x-axis -");
				plane.transform.rotation = Quaternion.AngleAxis (180, Vector3.up);
			}
		} else if (max == Mathf.Abs (delta.y)) {
			// Creates a highlight plane on x-z axis
			if (delta.y >= 0) {
				Debug.Log ("y-axis +");
				plane.transform.rotation = Quaternion.AngleAxis (90, Vector3.up);
			} else {
				Debug.Log ("y-axis -");
				plane.transform.rotation = Quaternion.AngleAxis (270, Vector3.up);
			}
		} else { // delta.z is max
			// Creates a highlight plane on x-y axis
			if (delta.z >= 0) {
				Debug.Log ("z-axis +");
				plane.transform.rotation = Quaternion.AngleAxis (270, Vector3.up);
			} else {
				Debug.Log ("z-axis -");
				plane.transform.rotation = Quaternion.AngleAxis (90, Vector3.up);
			}
		}
	}
}
