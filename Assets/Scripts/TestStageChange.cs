using UnityEngine;

public class TestStageChange: StateChangeListener {
  void Start() {
    Debug.Log("test started");

    State state = (State) GameObject.Find("Script").GetComponent("State");

    state.addListener(this);
  } 

  public override void onBlockAdded(Block block) {
    Debug.Log("Added");
    Debug.Log(block);
  }
  public override void onBlockRemoved(Block block) {
    Debug.Log("Removed");
    Debug.Log(block);
  }
}
