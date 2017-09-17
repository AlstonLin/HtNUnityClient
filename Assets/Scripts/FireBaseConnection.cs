using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using System;
using System.Linq;

public class FireBaseConnection {

	public const int ADD_BLOCK = 0;
	public const int REMOVE_BLOCK = 1;

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

		world.ChildAdded += (object s, ChildChangedEventArgs c) => 
			Handle(ADD_BLOCK, s, c);

		world.ChildRemoved += (object s, ChildChangedEventArgs c) => 
			Handle(REMOVE_BLOCK, s, c);

		this.onUpdate = onUpdate;

		// importCurrentWorld();
	}

	// public void importCurrentWorld(){
	// 	Debug.Log("Import current world");
	// 	world
	// 		.GetValueAsync()
	// 		.ContinueWith(task => {
	// 			if(task.IsFaulted){
	// 				Debug.Log("Import faulted");
	// 				return;
	// 			};

	// 			if(task.IsCompleted){
	// 				Debug.Log("Fetched world Data");

	// 				Dictionary<string, string> blockDict = JsonUtility.FromJson<Dictionary<string, string>>(task.Result.GetRawJsonValue());
					
	// 				foreach(KeyValuePair<string, string> b in blockDict){
	// 					Debug.Log(b.Value);
	// 				}

	// 				Debug.Log(blockDict.Keys.Count);

	// 			}
	// 		});
	// }

	public void addBlock(Block block){
		world.Child(block.id).SetRawJsonValueAsync(block.toJson());
	}

	public void removeBlock(Block block){
		world.Child(block.id).RemoveValueAsync();
	}

	public void Handle(int eventAction, object sender, ChildChangedEventArgs args){
		string json = args.Snapshot.GetRawJsonValue();
		Block block = new Block(json);
		
		onUpdate(eventAction, block);
	}
}
