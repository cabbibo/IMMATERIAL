using System.Collections;
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
    //print(data);
    //width = data.land.heightMap.width;
    //print( width );
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


      values[ index ++ ] = (((float)x + .5f)/(float)width) / data.land.size;
      values[ index ++ ] = c.r * data.land.height;
      values[ index ++ ] = (((float)y + .5f)/(float)width) / data.land.size;

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



  public override float[] GetDNA(){

    float[] DNA = new float[ count * 4 ];
    float[] values = GetData();

    for( int i = 0; i < count; i ++ ){
      DNA[i* 4 + 0 ] = values[ i * structSize + 1 ];
      DNA[i* 4 + 1 ] = values[ i * structSize + 6 ];
      DNA[i* 4 + 2 ] = values[ i * structSize + 8 ];
      DNA[i* 4 + 3 ] = values[ i * structSize + 11 ];
      
    }

    return DNA;

  }


  public override void SetDNA(float[] DNA){

    float[] values = new float[ count * structSize ];

    for( int i = 0; i < count; i ++ ){

      int x = i % width;
      int y = i / width;

      values[i* structSize + 0 ] = (((float)x + .5f)/(float)width) / data.land.size;
      values[i* structSize + 1 ] = DNA[ i * 4 + 0 ];
      values[i* structSize + 2 ] = (((float)y + .5f)/(float)width) / data.land.size;
      values[i* structSize + 6 ] = DNA[ i * 4 + 1 ];
      values[i* structSize + 8 ] = DNA[ i * 4 + 2 ];
      values[i* structSize + 11] = DNA[ i * 4 + 3 ];
      
    }

    SetData(values);

  }

}
