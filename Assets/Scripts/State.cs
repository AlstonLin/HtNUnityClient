using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class State: MonoBehaviour {
  /**
    * ^ Y (height)
    * |     / Z (length)
    * |   / 
    * | /
    * |-------- X (width)
   */
  
  public Boolean OptimisticPlacing = true;


  // Keeps track of local block state
  List<Block> blocks;
  List<Player> players;

  Player client;

  Transform cameraTransform;

  // Keep track of listeners
  List<StateChangeListener> listeners = new List<StateChangeListener>();

  FireBaseConnection fireBaseConnection;

  void Start() {
    client = new Player();

    fireBaseConnection = new FireBaseConnection (
      (int eventCode, Block block) => updateBlockStateFromFirebase(eventCode, block),
      (int eventCode, Player player) => updatePlayerStateFromFirebase(eventCode, player)
    );

    blocks = new List<Block>();
    players = new List<Player>();

    cameraTransform = (Transform) Camera.main.transform;
  }

  public void addListener(StateChangeListener listener){
    this.listeners.Add(listener);
  }

  public void updateBlockStateFromFirebase(int eventCode, Block block){
    switch(eventCode){
		case FireBaseConnection.ADD:
			if (OptimisticPlacing) {
				int i = blocks.FindIndex ((b) => b.id == block.id);
				if (i >= 0)
					return; // Don't broadcast to client if the state is already matched
			}
			blocks.Add (block);
        listeners.ForEach((listener) => listener.onBlockAdded(block));
        return;
      case FireBaseConnection.REMOVE:
        int idx = blocks.FindIndex((b) => b.id == block.id);
        if(idx >= 0) blocks.RemoveAt(idx);
        listeners.ForEach((listener) => listener.onBlockRemoved(block));
        return;
      default: return;
    }
  }

  public void addBlockToState(Block block){
    if(OptimisticPlacing)
      blocks.Add(block);

    fireBaseConnection.addBlock(block);
  }

  public void removeBlockFromState(Block block){
    int idx = blocks.FindIndex((b) => b.id == block.id);

    if(idx >= 0) blocks.RemoveAt(idx);

    fireBaseConnection.removeBlock(block);
  }

  public List<Block> getBlocks(){
    return blocks;
  }

  /**
    ============================ Players =========================
   */


  public void updatePlayerStateFromFirebase(int eventCode, Player player) {

    if(player.id == this.client.id) return;

    Predicate<Player> findPlayerById = (p) => (p.id == player.id);
    
    switch(eventCode) {
      case FireBaseConnection.ADD:
        int idx = players.FindIndex(findPlayerById);
        if(idx >= 0) return; // Don't add a player we already have
        players.Add(player);
        listeners.ForEach(l => l.onPlayerAdded(player));
        return;
      case FireBaseConnection.REMOVE:
        int i = players.FindIndex(findPlayerById);
        players.RemoveAt(i);
        listeners.ForEach(l => l.onPlayerRemove(player));
        return;
      case FireBaseConnection.CHANGE:
        int index = players.FindIndex(findPlayerById);
        if(index < 0) {
          Debug.Log("Index" + index);
          updatePlayerStateFromFirebase(FireBaseConnection.ADD, player);
          return;
        }
        players.RemoveAt(index);
        players.Add(player);
        listeners.ForEach(l => l.onPlayerMove(player));
        return;
      default: return;
    }
  }

  public void updatePlayer(Player player) {
    fireBaseConnection.changePlayer(player);
  }

  void Update() {
    // Update current client location
    if(cameraTransform.rotation.Equals(client.rotation) && 
      cameraTransform.position.Equals(client.location)) return;
    
    client.rotation = new Quaternion(
      cameraTransform.rotation.x,
      cameraTransform.rotation.y,
      cameraTransform.rotation.z,
      cameraTransform.rotation.w
    );

    client.location = new Vector3(
      cameraTransform.position.x,
      cameraTransform.position.y,
      cameraTransform.position.z
    );

    fireBaseConnection.changePlayer(client);
  }

  void OnDisable() {
    fireBaseConnection.removePlayer(client);
  }

  
}
