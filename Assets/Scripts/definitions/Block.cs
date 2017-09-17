using UnityEngine;

public class Block {
  public Quaternion rotation;
  public Vector3 location;
  public int color;
  public int shape;
  public string id;

  public Block(Quaternion rotation, Vector3 location, int color, int shape, string id) {
    this.rotation = rotation;
    this.location = location;
    this.color = color; 
    this.shape = shape; 
    this.id = id; 
  }

  public Block(Quaternion rotation, Vector3 location, int color, int shape):
    this(rotation, location, color, shape, GenerateHash.generateHash()){}

  public Block(Block block){
    this.rotation = block.rotation;
    this.location = block.location;
    this.color = block.color;
    this.shape = block.color;
    this.id = block.id;
  }

  public Block(string json): this(JsonUtility.FromJson<Block>(json)){}

  public string toJson(){
    return JsonUtility.ToJson(this);
  }
}

public class GenerateHash {
  static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  static System.Random random = new System.Random();

  public static string generateHash (){
  char[] stringChars = new char[8];

  for (int i = 0; i < stringChars.Length; i++)
  {
      stringChars[i] = chars[random.Next(chars.Length)];
  }

  return new string(stringChars);
  }
}
