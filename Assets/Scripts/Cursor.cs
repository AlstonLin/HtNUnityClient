using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {
	const float PLANE_WIDTH = 0.01f;

	public static Color currentColor = Color.white;

	GameObject plane = null;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (plane) Destroy (plane);
		// The direction vector * 3
		Vector3 delta = Camera.main.transform.forward.normalized * 3;
		Vector3 target = Camera.main.gameObject.transform.position + delta;
		// Finds the approximate axis that is being looked from
		float max = Mathf.Max(Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y)), Mathf.Abs(delta.z));
		plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
		plane.GetComponent<Renderer> ().material.color = currentColor;
		if (max == Mathf.Abs(delta.x)) {
			// Creates a highlight plane on y-z axis
			plane.transform.position = new Vector3 ((float) Mathf.Floor (target.x), Mathf.Ceil (target.y) - 0.5f, Mathf.Ceil (target.z) - 0.5f);
			plane.transform.localScale = new Vector3 (PLANE_WIDTH, 1, 1);
		} else if (max == Mathf.Abs(delta.y)) {
			// Creates a highlight plane on x-z axis
			plane.transform.position = new Vector3 (Mathf.Ceil (target.x) - 0.5f, (float) Mathf.Floor (target.y), Mathf.Ceil (target.z) - 0.5f);
			plane.transform.localScale = new Vector3 (1, PLANE_WIDTH, 1);
		} else { // delta.z is max
			// Creates a highlight plane on x-y axis
			plane.transform.position = new Vector3 (Mathf.Ceil (target.x) - 0.5f, Mathf.Ceil (target.y) - 0.5f, (float) Mathf.Floor(target.z));
			plane.transform.localScale = new Vector3 (1, 1, PLANE_WIDTH);
		}

		//place block
		if (Input.touchCount > 0 || Input.GetMouseButtonDown (0)) {
			GameObject.Instantiate (plane);
		}
	}
}
