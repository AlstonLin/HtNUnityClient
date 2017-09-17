using UnityEngine;

public class Player {
  public Quaternion rotation;
  public Vector3 location;
  
  public int color;
  public string id;

  public Player(Quaternion rotation, Vector3 location, int color):
    this(rotation, location, color, HashGenerator.generateHash()){}

  public Player(Quaternion rotation, Vector3 location, int color, string id){
    this.rotation = rotation;
    this.location = location;
    this.color = color;
    this.id = id;
  }
  public Player(Player player) {
    this.rotation = player.rotation;
    this.location = player.location;
    this.color = player.color;
    this.id = player.id;
  }
  
  public Player() {
    string id = HashGenerator.generateHash();
    int color = (int)Mathf.Floor(Random.Range(0, 4));

    this.id = id;
    this.color = color;
  }

  public Player(string json): this(JsonUtility.FromJson<Player>(json)){}

  public string toJson(){
    return JsonUtility.ToJson(this);
  }
}
