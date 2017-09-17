using UnityEngine;
using System.Collections.Generic;

public class RenderOtherPlayers: StateChangeListener {

  public GameObject PlayerPrefab;
  
  Dictionary<string, GameObject> playerMap;
    

  void Start() {
    State state = (State) GameObject.Find("FirebaseConnector").GetComponent("State");
    state.addListener(this);

    playerMap = new Dictionary<string, GameObject>();
  } 

  public override void onPlayerAdded(Player player){
    Debug.Log("Player added");
    playerMap.Add(
      player.id,
      Instantiate(PlayerPrefab, player.location, player.rotation)
    );
  }

  public override void onPlayerRemove(Player player){
    GameObject p;
    playerMap.TryGetValue(player.id, out p);
    if(p != null){
      Destroy(p);
      playerMap.Remove(player.id);
    }
  }

  public override void onPlayerMove(Player player){
    GameObject p;
    playerMap.TryGetValue(player.id, out p);
    if(p != null){
      p.transform.position = player.location;
      p.transform.rotation = player.rotation;
    }
  }
}
