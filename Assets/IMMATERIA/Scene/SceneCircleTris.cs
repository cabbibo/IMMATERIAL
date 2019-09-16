using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCircleTris : IndexForm
{

  private SceneCircleVerts scv;

  public override void SetCount(){
    scv = (SceneCircleVerts)toIndex;
    count = (scv.rows-1) * (scv.cols-1) * 3 * 2;
  }

  public override void Embody(){

    int[] values = new int[count];
    int index = 0;

    for( int i = 0; i < (scv.cols-1); i++ ){
      for( int j = 0; j < (scv.rows-1); j++ ){

        int id1 = i * scv.rows + j;
        int id2 = ((i+1) ) * scv.rows + j;
        int id3 = i * scv.rows + j+1;
        int id4 = ((i+1)) * scv.rows + j+1;

        values[index++] = id1;
        values[index++] = id2;
        values[index++] = id4;
        values[index++] = id1;
        values[index++] = id4;
        values[index++] = id3;

      }
    }

    SetData( values );
  }

}
