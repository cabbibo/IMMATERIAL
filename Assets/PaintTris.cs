using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PaintTris : IndexForm {

  [ HideInInspector ] public int width;
  [ HideInInspector ] public int length;
  [ HideInInspector ] public int numTubes;
  public PaintVerts verts;

  public override void SetCount(){
    toIndex = verts;
    count = (verts.width-1) * (verts.width-1)  * 3 * 2;
  }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;
    for( int i = 0; i < verts.width -1; i++ ){
    for( int j = 0; j < verts.width -1; j++   ){
        int id1 = i * verts.width  + j + 0 ;
        int id2 = i * verts.width  + j + 1 ;
        int id3 = i * verts.width  + j + verts.width ;
        int id4 = i * verts.width  + j + verts.width  + 1;

        values[ index ++ ] = id1;
        values[ index ++ ] = id4;
        values[ index ++ ] = id2;
        values[ index ++ ] = id1;
        values[ index ++ ] = id3;
        values[ index ++ ] = id4;

    }
    }
    SetData(values);
  }

}

