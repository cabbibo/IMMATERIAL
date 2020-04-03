using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameTris : IndexForm
{
  

  public FrameVerts verts;
  public override void SetCount(){
    count = ((verts.size-1) * 3 * 2) * 4;
  }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;

    // 1-0
    // |/|
    // 3-2
    for( int i = 0; i < 4; i++ ){
      for( int j= 0; j < verts.size-1; j++ ){
        
        int bID = i * verts.size * 2;
        bID += j * 2;

        values[ index ++ ] = bID + 0;
        values[ index ++ ] = bID + 1;
        values[ index ++ ] = bID + 3;
        values[ index ++ ] = bID + 0;
        values[ index ++ ] = bID + 3;
        values[ index ++ ] = bID + 2;
      }
    }
    SetData(values);
  }



}
