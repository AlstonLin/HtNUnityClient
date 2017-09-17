using UnityEngine;
public abstract class StateChangeListener: MonoBehaviour {
  virtual public void onBlockAdded(Block block){}
  virtual public void onBlockRemoved(Block block){}

  virtual public void onPlayerAdded(Player player){}
  virtual public void onPlayerMove(Player player){}
  virtual public void onPlayerRemove(Player player){}
}
