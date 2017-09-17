using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using System;
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
	}

	public void importCurrentWorld(){
		
	}

	public void addBlock(Block block){
		world.Child(block.id).SetRawJsonValueAsync(block.toJson());
	}

	public void Handle(int eventAction, object sender, ChildChangedEventArgs args){
		string json = args.Snapshot.GetRawJsonValue();
		Block block = new Block(json);
		
		onUpdate(eventAction, block);
	}
}
