﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintVerts : Form
{

  public int width;
  public float size;
  public override void SetStructSize(){
    structSize = 12;
  }

  public override void SetCount(){
    print(data);
    width = data.Island.heightMap.width;
    print( width );
    count = width * width;
    size = data.Island.size;
  }

  public override void Embody(){

    int index = 0;

    float[] values = new float[count*structSize];
    for( int i = 0; i < count; i ++ ){

      int x = i % width;
      int y = i / width;

      Color c =  data.Island.heightMap.GetPixelBilinear((float)x / (float)width , (float)y / (float)width);


      values[ index ++ ] = ((float)x/(float)width) / data.Island.size;
      values[ index ++ ] = c.r * data.Island.height;
      values[ index ++ ] = ((float)y/(float)width) / data.Island.size;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = c.g * 2 -1;
      values[ index ++ ] = 0;
      values[ index ++ ] = c.b* 2 -1;

      values[ index ++ ] = x;
      values[ index ++ ] = y;

      values[ index ++ ] = c.a;
    
    }

    SetData( values );

  }


}