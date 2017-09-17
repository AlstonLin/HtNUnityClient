using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using System;
using System.Linq;

public class FireBaseConnection {

	public const int ADD = 0;
	public const int REMOVE = 1;
	public const int CHANGE = 2;

	DatabaseReference db;
	DatabaseReference world;
	DatabaseReference players;
	Action<int, Block> onUpdateBlock;
	Action<int, Player> onUpdatePlayer;
	public FireBaseConnection (Action<int, Block> onUpdateBlock, Action<int, Player> onUpdatePlayer) {
		// Start the Firebase db instance
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(
			"https://hackthenorth-a4e06.firebaseio.com/"
		);

		// Set references
		db = FirebaseDatabase.DefaultInstance.RootReference;
		world = db.Child("world");
		players = db.Child("players");
		

		world.ChildAdded += (object s, ChildChangedEventArgs c) => 
			HandleBlockChange(ADD, s, c);

		world.ChildRemoved += (object s, ChildChangedEventArgs c) => 
			HandleBlockChange(REMOVE, s, c);

		players.ChildAdded += (object s, ChildChangedEventArgs c) =>
			HandlePlayerChange(ADD, s, c);
		
		players.ChildChanged += (object s, ChildChangedEventArgs c) =>
			HandlePlayerChange(CHANGE, s, c);
		
		players.ChildRemoved += (object s, ChildChangedEventArgs c) =>
			HandlePlayerChange(REMOVE, s, c);


		this.onUpdateBlock = onUpdateBlock;
		this.onUpdatePlayer = onUpdatePlayer;

		// importCurrentWorld();
	}
	/**
		====================== Block Methods ===================
	 */
	public void addBlock(Block block){
		world.Child(block.id).SetRawJsonValueAsync(block.toJson());
	}

	public void removeBlock(Block block){
		world.Child(block.id).RemoveValueAsync();
	}

	public void HandleBlockChange(int eventAction, object sender, ChildChangedEventArgs args){
		string json = args.Snapshot.GetRawJsonValue();
		Block block = new Block(json);
		
		onUpdateBlock(eventAction, block);
	}

	/**
		========================= Player Methods ==================
	 */

	 public void addPlayer(Player player) {
		 players.Child(player.id).SetRawJsonValueAsync(player.toJson());
	 }

	 public void removePlayer(Player player) {
		 players.Child(player.id).RemoveValueAsync();
	 }

	public void HandlePlayerChange(int eventAction, object sender, ChildChangedEventArgs args){

		string json = args.Snapshot.GetRawJsonValue();
		Player player = new Player(json);

		onUpdatePlayer(eventAction, player);

	}

	public void changePlayer(Player player) {
		players.Child(player.id).SetRawJsonValueAsync(player.toJson());
	}
	
}
