using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActiveBlocksDictionary : MonoBehaviour
{

	static Dictionary<string, GameObject> m_instanceMap = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static void addToDict(GameObject obj) {
		m_instanceMap.Add (obj.GetInstanceID().ToString(), obj);
	}

	public static GameObject getObj(string id) {
		return m_instanceMap [id];
	}
}

