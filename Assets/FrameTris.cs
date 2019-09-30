using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTris : IndexForm
{
  

  public FrameVerts verts;
  public override void SetCount(){

    print("setting count");

    count = (((verts.count/8)-1) * 3 * 2) * 4;

    print(""+count);
    
  }


}
