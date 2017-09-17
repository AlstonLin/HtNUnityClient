﻿using UnityEngine;
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

	public static void addToDict(string id, GameObject obj) {
		m_instanceMap.Add (id, obj);
	}

	public static GameObject getObj(string id) {
		return m_instanceMap [id];
	}

	public static void removeObj(string id) {
		m_instanceMap.Remove (id);
	}
}

