using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeVerts: Form {

  public Hair hair;
  public int width;
  public int length;
  public int numTubes;

  public override void SetStructSize(){ structSize = 16; }

  public override void SetCount(){
    numTubes = hair.numHairs;
    count = numTubes * width * length;
  }

}





