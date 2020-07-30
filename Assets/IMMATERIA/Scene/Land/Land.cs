using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

using System.Runtime.Serialization.Formatters.Binary;


public class Land : Cycle {

  public float size;
  public float height;
  public float tileSize;
  public Texture2D heightMap;

  public Texture2D startTexture;

  public int traceSteps;
  public float traceDist;

  public Transform terrainHole;
  public Transform camRay;

  public Texture2D[] infoTextures;

  public override void Create(){



    heightMap = new Texture2D(1024,1024,TextureFormat.RGBAFloat,false);

    heightMap.wrapMode = TextureWrapMode.Clamp;
    heightMap.filterMode = FilterMode.Trilinear;
    heightMap.mipMapBias = 10000;

  infoTextures = new Texture2D[4];

  for( int i = 0; i < 4; i++ ){
    infoTextures[i] =  new Texture2D(512,512,TextureFormat.RGBAFloat,false);

    infoTextures[i].wrapMode = TextureWrapMode.Clamp;
    infoTextures[i].filterMode = FilterMode.Trilinear;
    infoTextures[i].mipMapBias = 10000;
  }

    
    //Graphics.CopyTexture( startTexture , heightMap);

   // heightMap.Apply();

    Shader.SetGlobalTexture("_HeightMap", heightMap);
    Shader.SetGlobalTexture("_TerrainInfo1", infoTextures[0]);
    Shader.SetGlobalTexture("_TerrainInfo2", infoTextures[1]);
    Shader.SetGlobalTexture("_TerrainInfo3", infoTextures[2]);
    Shader.SetGlobalTexture("_TerrainInfo4", infoTextures[3]);
    Shader.SetGlobalFloat("_TerrainSize", size);
    Shader.SetGlobalFloat("_TerrainHeight", height);
    Shader.SetGlobalFloat("_MapSize", size);
    Shader.SetGlobalFloat("_MapHeight", height);
    Shader.SetGlobalVector("_TerrainHole", terrainHole.position );


    LoadFromFile();

   // if( data.painter == null ){ LoadFromFile(); };

  
  }



  // Makes sure we are using the correct values for heightmap, terrainSize etc.
  // OPTIMIZATION: won't need 'SetGlobalTexture' all the time!
  public override void WhileLiving (float l) {


    //Shader.SetGlobalTexture("_HeightMap", heightMap);
    Shader.SetGlobalFloat("_TerrainSize", size);
    Shader.SetGlobalFloat("_TerrainHeight", height);
    Shader.SetGlobalFloat("_MapSize", size);
    Shader.SetGlobalFloat("_MapHeight", height);
    
    Shader.SetGlobalVector("_TerrainHole", terrainHole.position );

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


  public Color SampleTexture( Vector3 v , int t ){
    float posX = (v.x-.5f) * size  - (.5f / (float)heightMap.width);
    float posZ = (v.z-.5f) * size  - (.5f / (float)heightMap.width);
    Color c =  infoTextures[t].GetPixelBilinear(posX , posZ);
    return c;
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

      Vector3 pos = ro + rd * i *traceDist;
      float h = SampleHeight( pos );

      if( pos.y < h ){
        return pos;
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
        return pos;
      }

    }

    return ro + rd * 40;

  }


  public void BindData(Life life){

    life.BindTexture("_HeightMap" , () => this.heightMap  );
    life.BindFloat("_MapSize"     , () => this.size       );
    life.BindFloat("_MapHeight"   , () => this.height     );

  }

  public void BindInfoData( Life life ){
    
    life.BindTexture("_InfoTexture1" , () => this.infoTextures[0]  );
    life.BindTexture("_InfoTexture2" , () => this.infoTextures[1]  );
    life.BindTexture("_InfoTexture3" , () => this.infoTextures[2]  );
    life.BindTexture("_InfoTexture4" , () => this.infoTextures[3]  );

  }


  public void LoadFromFile(){

    string name = "Terrain/safe";

    BinaryFormatter bf = new BinaryFormatter();


    FileStream stream = File.OpenRead(Application.streamingAssetsPath+ "/"+name+".dna");
    float[] data = bf.Deserialize(stream) as float[];
    stream.Close();


    Color[] colors =  new Color[data.Length/4];
    for( int i = 0; i < data.Length / 4; i ++ ){
      
      // extracting height
      float h = data[ i * 4 + 0 ] / height;
      
      // extracting flow verts
      float x = data[ i * 4 + 1 ] * .5f + .5f;
      float z = data[ i * 4 + 2 ] * .5f + .5f;


      float a2 =  data[ i * 4+3 ];

      colors[i] = new Color( h,x,z,a2);

    }

    heightMap.SetPixels(colors,0);
    heightMap.Apply(true);





    name = "Terrain/terrainData";
    stream = File.OpenRead(Application.streamingAssetsPath+ "/"+name+".dna");
    data = bf.Deserialize(stream) as float[];
    stream.Close();

    print( data.Length / 20 );


    Color[] colors1 =  new Color[data.Length/20];
    Color[] colors2 =  new Color[data.Length/20];
    Color[] colors3 =  new Color[data.Length/20];
    Color[] colors4 =  new Color[data.Length/20];

    float r; float g; float b; float a;

    for( int i = 0; i < data.Length / 20; i ++ ){
      
      r = data[ i * 20 + 3 ];
      g = data[ i * 20 + 4 ];
      b = data[ i * 20 + 5 ];
      a = data[ i * 20 + 6 ];
      colors1[i] = new Color( r,g,b,a);

      r = data[ i * 20 + 7 ];
      g = data[ i * 20 + 8 ];
      b = data[ i * 20 + 9 ];
      a = data[ i * 20 + 10 ];
      colors2[i] = new Color( r,g,b,a);


      r = data[ i * 20 + 11 ];
      g = data[ i * 20 + 12 ];
      b = data[ i * 20 + 13 ];
      a = data[ i * 20 + 14 ];
      colors3[i] = new Color( r,g,b,a);

      r = data[ i * 20 + 15 ];
      g = data[ i * 20 + 16 ];
      b = data[ i * 20 + 17 ];
      a = data[ i * 20 + 18 ];
      colors4[i] = new Color( r,g,b,a);

    }


    infoTextures[0].SetPixels(colors1,0);
    infoTextures[0].Apply(true);


    infoTextures[1].SetPixels(colors2,0);
    infoTextures[1].Apply(true);

    infoTextures[2].SetPixels(colors3,0);
    infoTextures[2].Apply(true);


    infoTextures[3].SetPixels(colors4,0);
    infoTextures[3].Apply(true);
    

  }


}
