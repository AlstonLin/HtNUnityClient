using UnityEngine;

public class Block {
  [SerializeField]
  private int[] shapes = new int[6];
  
  [SerializeField]
  private int[] colors = new int[6];

  private readonly int DEFAULT_FACE = FaceShapes.EMPTY_FACE;
  private readonly int DEFAULT_COLOR = FaceColors.WHITE;

  public Block () {
    for(int x = 0; x < shapes.Length; x ++){
      shapes[x] = DEFAULT_FACE;
      colors[x] = DEFAULT_COLOR;
    }
  }

  public Block(int[] shapes, int[] colors) {
    this.shapes = shapes;
    this.colors = colors;
  }

  public Block changeFace(int face, int faceShape, int faceColor){
    if(face < 0 || face > shapes.Length){
      throw new System.ArgumentException("Face cannot be less than 0 or more than " + shapes.Length);
    }
    Block block = new Block();
    
    for(int x = 0; x < shapes.Length; x ++){
      if(x == face){
        block.shapes[x] = faceShape;
        block.colors[x] = faceColor;
      } else {
        block.shapes[x] = shapes[x];
        block.colors[x] = colors[x];
      }
    }

    return block;
  }

  public Face getFace(int face){
    return new Face(shapes[face], colors[face]);
  }

  public int[] getFaces(){
    int[] newArray = new int[shapes.Length];
    
    shapes.CopyTo(newArray, 0);

    return newArray;
  }

}
