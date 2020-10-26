using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVerts : Particles
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
        
        float rowNor = (float)j / (float)rows;

        p = new Vector3( rowNor - .5f , 0 , colNor - .5f);

        // position
        values[index++] = p.x;
        values[index++] = p.y;
        values[index++] = p.z;

        
        // nor
        values[index++] = p.x;
        values[index++] = p.y; 
        values[index++] = p.z;

        // vel
        values[index++] = 0;
        values[index++] = 1;
        values[index++] = 0;
        
        // tan
        values[index++] = 0;
        values[index++] = 1;
        values[index++] = 0;

        // uv
        values[index++] = rowNor;
        values[index++] = colNor;

        // debug
        values[index++] = 1;  
        values[index++] = 1;
      
      }
    }

    SetData(values);
  }

}
