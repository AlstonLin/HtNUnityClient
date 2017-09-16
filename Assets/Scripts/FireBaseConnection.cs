using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using System;
public class FireBaseConnection {

	DatabaseReference db;
	DatabaseReference world;
	Action<int, Block> onUpdate;
	public FireBaseConnection (Action<int, Block> onUpdate) {
		// Start the Firebase db instance
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(
			"https://hackthenorth-a4e06.firebaseio.com/"
		);

		// Set references
		db = FirebaseDatabase.DefaultInstance.RootReference;
		world = db.Child("world");

		this.onUpdate = onUpdate;

		world.ChildChanged += HandleChildChanged;
	}
	
	public void updateBlock(int index, Block block){
		string json = JsonUtility.ToJson(block);

		world.Child(index + "").SetRawJsonValueAsync(json);
	}

	public void HandleChildChanged(object sender, ChildChangedEventArgs args){
		int key = Convert.ToInt16(args.Snapshot.Key);
		Dictionary<string, object> value = (Dictionary<string, object>) args.Snapshot.Value;

		object shapesObj;
		object colorsObj;

		value.TryGetValue("shapes", out shapesObj);
		value.TryGetValue("colors", out colorsObj);

		IList shapesList = (IList)shapesObj;
		IList colorsList = (IList)colorsObj;

		int[] shapes = new int[shapesList.Count];
		int[] colors = new int[colorsList.Count];

		Debug.Log(shapesList.Count);
		Debug.Log(colorsList.Count);

		for(int x = 0; x < shapesList.Count; x ++){
			Debug.Log(x);

			Debug.Log(shapesList[x]);
			Debug.Log(colorsList[x]);
		}

		onUpdate(key, new Block(shapes, colors));
	}




}
