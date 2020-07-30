using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LandDataVerts : Form
{

  public int width;
  public float size;
  public override void SetStructSize(){
    structSize = 20;
  }


  public override void SetCount(){
    count = width * width;
    size = data.land.size;
  }



  public override void Embody(){

    int index = 0;

    float[] values = new float[count*structSize];
    for( int i = 0; i < count; i ++ ){

      int x = i % width;
      int y = i / width;



      Color c =  data.land.heightMap.GetPixelBilinear((float)x / (float)width , (float)y / (float)width);

      if( i % 10000 == 0 ){
        //  print(data.land.heightMap);
        //  print( c.r );
      }

      values[ index ++ ] = (((float)x + .5f)/(float)width) / data.land.size;
      values[ index ++ ] = c.r * data.land.height;
      values[ index ++ ] = (((float)y + .5f)/(float)width) / data.land.size;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = .5f;
    
    }

    SetData( values );

  }

}

