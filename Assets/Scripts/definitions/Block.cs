using UnityEngine;

public class Block {
  public Quaternion rotation;
  public Vector3 location;
  public Vector3 scale;
  public int color;
  public int shape;
  public string id;

  public Block(Quaternion rotation, Vector3 location, Vector3 scale, int color, int shape, string id) {
    this.rotation = rotation;
    this.location = location;
	this.scale = scale;
    this.color = color; 
    this.shape = shape; 
    this.id = id; 
  }

	public Block(Quaternion rotation, Vector3 location, Vector3 scale, int color, int shape):
    this(rotation, location, scale, color, shape, HashGenerator.generateHash()){}

  public Block(Block block){
    this.rotation = block.rotation;
    this.location = block.location;
	this.scale = block.scale;
    this.color = block.color;
    this.shape = block.shape;
    this.id = block.id;
  }

  public Block(string json): this(JsonUtility.FromJson<Block>(json)){}

  public string toJson(){
    return JsonUtility.ToJson(this);
  }
}
