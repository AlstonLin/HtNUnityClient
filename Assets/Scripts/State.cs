using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class State: MonoBehaviour {
  /**
    * ^ Y (height)
    * |     / Z (length)
    * |   / 
    * | /
    * |-------- X (width)
   */
  List<Block> blocks;
  public int worldWidth;
  public int worldHeight;
  public int worldLength;

  public bool resetWorldOnStart = false;

  FireBaseConnection fireBaseConnection;

  void Start() {
    blocks = new List<Block>(new Block[worldWidth * worldHeight * worldLength]);

    fireBaseConnection = new FireBaseConnection(
      (int index, Block block) => updateBlock(index, block)
    );

    if(resetWorldOnStart) resetWorld();
  }

  public void resetWorld(){
    for(int i = 0; i < worldWidth * worldHeight * worldLength; i ++){
      fireBaseConnection.updateBlock(i, new Block());
    }
  }

  int getIndex(int x, int y, int z){
    return y * (worldHeight * worldWidth) + (z * worldWidth) + x;
  }

  public Block getBlock(int x, int y, int z){
    int index = getIndex(x, y, z);
    return blocks[index];
  }

  private void updateBlock(int index, Block block){
    blocks[index] = block;
  }

  public void changeBlock(int x, int y, int z, Block block){
    int index = getIndex(x, y, z);

    updateBlock(index, block);
    fireBaseConnection.updateBlock(index, block);
  }

  public void changeBlock(int x, int y, int z, int face, int faceShape, int faceColor){
    int index = getIndex(x, y, z);
    Block newBlock = blocks[index].changeFace(face, faceShape, faceColor);

    updateBlock(index, newBlock);
    fireBaseConnection.updateBlock(index, newBlock);
  }
}
