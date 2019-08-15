﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Land : Cycle {

  public float size;
  public float height;
  public float tileSize;
  public Texture2D heightMap;

  public Texture2D startTexture;

  public int traceSteps;
  public float traceDist;

  public Transform camRay;
  

  public override void Create(){

    heightMap = new Texture2D(1024,1024);

    heightMap.wrapMode = TextureWrapMode.Clamp;
    Graphics.CopyTexture( startTexture , heightMap);

   // heightMap.Apply();

    Shader.SetGlobalTexture("_HeightMap", heightMap);
    Shader.SetGlobalFloat("_TerrainSize", size);
    Shader.SetGlobalFloat("_TerrainHeight", height);
    Shader.SetGlobalFloat("_MapSize", size);
    Shader.SetGlobalFloat("_MapHeight", height);

  
  }



  // Makes sure we are using the correct values for heightmap, terrainSize etc.
  // OPTIMIZATION: won't need 'SetGlobalTexture' all the time!
  public override void WhileLiving (float l) {

    Shader.SetGlobalTexture("_HeightMap", heightMap);
    Shader.SetGlobalFloat("_TerrainSize", size);
    Shader.SetGlobalFloat("_TerrainHeight", height);
    Shader.SetGlobalFloat("_MapSize", size);
    Shader.SetGlobalFloat("_MapHeight", height);

    camRay.position = Trace( data.cameraPosition , data.cameraForward );

  }

  public float SampleHeight( Vector2 v ){
    float posX = (v.x-.5f) * size + .5f/(float)size;
    float posZ = (v.y-.5f) * size + .5f/(float)size;
    
    Color c =  heightMap.GetPixelBilinear(posX , posZ);
    return c.r * height;
  }


  public float SampleHeight( Vector3 v ){
    float posX = (v.x-.5f) * size  - (.5f / (float)heightMap.width);
    float posZ = (v.z-.5f) * size  - (.5f / (float)heightMap.width);
    Color c =  heightMap.GetPixelBilinear(posX , posZ);
    return c.r * height;
  }

  public Vector3 NewPosition( Vector3 v ){
    return new Vector3( v.x , SampleHeight( v ), v.z);
  }

  public Vector3 SampleNormal( Vector3 v ){
    float eps =1;

    Vector3 h1 = NewPosition( v + Vector3.forward * eps);
    Vector3 h2 = NewPosition( v - Vector3.forward * eps);
    Vector3 h3 = NewPosition( v + Vector3.right  * eps);
    Vector3 h4 = NewPosition( v - Vector3.right  * eps);

    return (Vector3.Cross( (h1-h2).normalized , (h3-h4).normalized )).normalized;
  }

  public Vector3 Trace( Vector3 ro , Vector3 rd ){

    for( int i = 0; i < traceSteps; i++ ){

      Vector3 pos = ro +rd * i *traceDist;
      float h = SampleHeight( pos );

      if( pos.y < h ){

       // print( h);
        return pos;
//        break;
      }

    }

    return ro + rd * 40;

  }


  public Vector3 Trace( Vector3 ro , Vector3 rd , out bool hit ){

    hit = false;
    for( int i = 0; i < traceSteps; i++ ){

      Vector3 pos = ro +rd * i *traceDist;
      float h = SampleHeight( pos );

      if( pos.y < h ){
        hit = true;
       // print( h);
        return pos;
//        break;
      }

    }

    return ro + rd * 40;

  }


  public void BindData(Life life){

    life.BindAttribute("_HeightMap", "heightMap" , this );
    life.BindAttribute("_MapSize"  , "size" , this );
    life.BindAttribute("_MapHeight", "height" , this );

  }


}
