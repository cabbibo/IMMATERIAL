using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTileVerts : Particles
{

  public int dimensions;
  public float size;
  public Vector3 position;

  public override void SetCount(){
    count = dimensions * dimensions;  
  }

  public override void Embody(){


    float[] values = new float[count*structSize];

    int index = 0;


    Vector3 pos;
    //Vector3 uv;
    //Vector3 tan;
    //Vector3 nor;
//    int baseTri;

    for( int i = 0; i < count; i ++ ){


      float uvRX = (((float)(i % dimensions)) / ((float)dimensions-1));
      float uvRY = ((Mathf.Floor((float)(i / dimensions))) / ((float)dimensions-1));

      float uvX = (((float)(i % dimensions)+.5f) / ((float)dimensions-1));
      float uvY = ((Mathf.Floor((float)(i / dimensions)) +.5f) / ((float)dimensions-1));

      float x = transform.localScale.x * uvX * size;
      float z = transform.localScale.z * uvY * size;

      pos = new Vector3( x, 0, z);
      values[ index ++ ] = transform.position.x  +  pos.x - (transform.localScale.x*size/2);
      values[ index ++ ] =  transform.position.y;
      values[ index ++ ] = transform.position.z  +  pos.z- (transform.localScale.z*size/2);

      values[ index ++ ] = 0;
      values[ index ++ ] = 0;
      values[ index ++ ] = 0;

      values[ index ++ ] = 0;
      values[ index ++ ] = 1;
      values[ index ++ ] = 0;

      values[ index ++ ] = pos.x - size/2;
      values[ index ++ ] = 0;
      values[ index ++ ] = pos.z- size/2;

      values[ index ++ ] = uvRX;
      values[ index ++ ] = uvRY;

      values[ index ++ ] = i;
      values[ index ++ ] = i/count;

    }

//    print("embodyo");

    SetData( values );


 // public Form Verts;
  //public IntForm Tris;

  }


  
}
