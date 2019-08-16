using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTileVertsOuter : Particles
{

  public int dimensionsHigh;
  public int dimensionsLow;
  public float size;
  public Vector3 position;

  public override void SetCount(){
    count = dimensionsHigh-1 * 4;
  }

  public override void Embody(){
/*

    float[] values = new float[count*structSize];

    int index = 0;


    Vector3 pos;
    Vector3 uv;
    Vector3 tan;
    Vector3 nor;
    int baseTri;

    for( int i = 0; i < count; i ++ ){

      float x = (((float)(i % dimensions)+.5f) / ((float)dimensions-1)) * size;
      float z = ((Mathf.Floor((float)(i / dimensions)) +.5f) / ((float)dimensions-1)) * size;

      pos = new Vector3( x, 0, z);
      values[ index ++ ] = transform.position.x  +  pos.x - size/2;
      values[ index ++ ] = 0;
      values[ index ++ ] = transform.position.z  +  pos.z- size/2;

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = 0;
      values[ index ++ ] = 1;
      values[ index ++ ] = 0;

      values[ index ++ ] = pos.x - size/2;
      values[ index ++ ] = 0;
      values[ index ++ ] = pos.z- size/2;

      values[ index ++ ] = x;
      values[ index ++ ] = z;

      values[ index ++ ] = i;
      values[ index ++ ] = i/count;

    }

//    print("embodyo");

    SetData( values );


 // public Form Verts;
  //public IntForm Tris;*/

  }


  
}
