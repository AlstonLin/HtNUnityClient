using UnityEngine;
public abstract class StateChangeListener: MonoBehaviour {
  abstract public void onBlockAdded(Block block);
  abstract public void onBlockRemoved(Block block);
}
