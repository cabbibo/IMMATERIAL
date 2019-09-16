using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCircleVerts : Particles
{
  public int rows;
  public int cols;


  public override void SetCount(){
    count = rows * cols;
  }

  public override void Embody(){


    float[] values = new float[count * structSize];
    int index = 0;
    Vector3 p;
    for(int i = 0; i < cols; i++ ){

      float colNor = (float)i / ((float)cols-1);

      for( int j = 0; j < rows; j++){
        
        float rowNor = .0001f + (float)j / (float)rows;

        float angle = colNor * 2 * Mathf.PI;//6.28f;
        float x = Mathf.Cos( angle )  * rowNor;
        float y = -Mathf.Sin( angle )  * rowNor;

        p = new Vector3( x , 0 , y);

        // position
        values[index++] = p.x;
        values[index++] = p.y;
        values[index++] = p.z;

        // vel
        values[index++] = 0;
        values[index++] = 0;
        values[index++] = 0;

        // nor
        values[index++] = 0;
        values[index++] = 1; 
        values[index++] = 0;

        // tan
        values[index++] = p.x;
        values[index++] = p.y;
        values[index++] = p.z;

        // uv
        values[index++] = colNor;
        values[index++] = rowNor;

        // debug
        values[index++] = 1;  
        values[index++] = 1;
      
      }
    }

    SetData(values);
  }

}
