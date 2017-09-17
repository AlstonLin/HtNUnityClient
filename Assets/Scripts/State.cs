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

  // Keep track of listeners
  List<StateChangeListener> listeners = new List<StateChangeListener>();

  FireBaseConnection fireBaseConnection;

  void Start() {
    fireBaseConnection = new FireBaseConnection (
      (int eventCode, Block block) => updateStateFromFirebase(eventCode, block)
    );

    blocks = new List<Block>();

    // test
    Block testBlock = new Block(new Quaternion(1,2,3,4), new Vector3(1,2,3), 1,1);
    addBlockToState(testBlock);
  }

  public void addListener(StateChangeListener listener){
    this.listeners.Add(listener);
  }

  public void updateStateFromFirebase(int eventCode, Block block){
    switch(eventCode){
      case FireBaseConnection.ADD_BLOCK:
        if(OptimisticPlacing){
          int i = blocks.FindIndex((b) => b.id == block.id);
          if(i >= 0) return; // Don't broadcast to client if the state is already matched
        }
        blocks.Add(block);
        listeners.ForEach((listener) => listener.onBlockAdded(block));
        return;
      case FireBaseConnection.REMOVE_BLOCK:
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

  public List<Block> getBlocks(){
    return blocks;
  }
}
