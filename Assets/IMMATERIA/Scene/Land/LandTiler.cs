using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTiler : Cycle
{

  public bool alwaysRespawn;
  public LandTile[] Tiles;

  public float tileSize;


  public Life setTile;



  public GameObject landTilePrefab;

  public GameObject midQualityTilePrefab;
  public GameObject lowQualityTilePrefab;



  public int numTiles;
  public int tileDimensions;
  public int lowTileDimensions;
  public int midTileDimensions;

  public int idX;
  public int idY;

  public int oIDX;
  public int oIDY;

  public int currentCenterX;
  public int currentCenterY;

  public Material terrainMaterial;



  public Vector3 _Offset;
  public int _ID;

  public GameObject[]  tileObjects;

  public bool constantUpdate;

  private float hT; // halfTile
  private float t; // tile

  public void DestroyMe(){
    if( Tiles != null ){
    for( int i=0; i < Tiles.Length; i++ ){

          Cycles.Remove(tileObjects[i].GetComponent<LandTile>());
          DestroyImmediate(tileObjects[i]);


    }

    Cycles.Remove(setTile );
    Tiles = null;
  }
  }



  public override void Create(){

    //print("OnCreate");
    //print( landTilePrefab );
    //print( landTilePrefab.GetComponent<LandTile>() );
    //print( landTilePrefab.GetComponent<LandTile>().verts );

    currentCenterX = 1;
    currentCenterY = 1;

    tileSize = 1/(numTiles *data.land.size);
    t = tileSize  * 3;
        hT = t/2;

    if( Tiles.Length != 3 * 3  || Tiles == null || alwaysRespawn ){
      
    Cycles.Clear();
      DestroyMe();

        Tiles = new LandTile[3 * 3 ];
        tileObjects = new GameObject[3 * 3 ];

        
  
        Tiles = new LandTile[3 * 3 ];
        
        
        for( int i = 0; i < 3; i++ ){
          for( int j = 0; j < 3; j++ ){

            int id = i * 3 + j;
            GameObject g = Instantiate( landTilePrefab );
            g.transform.parent = transform;
            tileObjects[id] = g;

            Tiles[id]= g.GetComponent<LandTile>();
            Tiles[id].size = tileSize;
            Tiles[id].dimensions = tileDimensions;
            Tiles[id].tiler = this;

            SafeInsert(g.GetComponent<LandTile>());

          }
        }
      

      SafeInsert(setTile);
   
  }   

  for( int i = 0; i < Tiles.Length; i++ ){
        _ID = i;

        _Offset = -Vector3.left * ((i%3)+.5f) * tileSize;
        _Offset += Vector3.forward * ((float)(i/3) + .5f) * tileSize;

        tileObjects[i].transform.position = Vector3.zero + _Offset;
    }

  }

  public override void Bind(){
    data.BindLandData( setTile );
    
    setTile.BindVector3("_Offset", () => this._Offset );
    setTile.BindInt("_ID", () => this._ID );
 
  }

  public override void OnLive(){

    for( int i = 0; i < Tiles.Length; i++ ){
      _Offset = tileObjects[i].transform.position;
      _ID = i;
      OffsetTile(i);
    }
  }

    // Use this for initialization
  public override void WhileLiving (float l) {


    //Vector3 oPos;

//    print(data.playerPosition);
    oIDX = idX;
    oIDY = idY;


    idX = (int)Mathf.Floor(data.playerPosition.x / tileSize );
    idY = (int)Mathf.Floor(data.playerPosition.z / tileSize );


    if( currentCenterX != idX ){
      if( idX > currentCenterX ){
        ShiftLeft();
      }else{
        ShiftRight();
      }
    }


     if( currentCenterY != idY ){
      if( idY > currentCenterY ){
        ShiftForward();
      }else{
        ShiftBack();
      }
    }





/*

    for( int i = 0; i < Tiles.Length; i++ ){



//  print("s");
       oPos = tileObjects[i].transform.position;

         _ID = i;
         _Offset = Vector3.zero;

      if( data.playerPosition.x - tileObjects[i].transform.position.x < -hT   ){     
        _Offset += -Vector3.right * t;
        tileObjects[i].transform.position += _Offset;
      }

      if( data.playerPosition.x - tileObjects[i].transform.position.x > hT   ){
        _Offset += Vector3.right * t;
        tileObjects[i].transform.position += _Offset;
      }


      if( data.playerPosition.z - tileObjects[i].transform.position.z < -hT   ){        
        _Offset += -Vector3.forward * t;
        tileObjects[i].transform.position += _Offset;
      }

      if( data.playerPosition.z - tileObjects[i].transform.position.z > hT   ){
         _Offset += Vector3.forward * t;
        tileObjects[i].transform.position += _Offset;
  
      }

      _Offset = tileObjects[i].transform.position;


      if( constantUpdate ){
        OffsetTile(i);
      }else{
        //_Offset = tileObjects[i].transform.position;
        if( oPos != tileObjects[i].transform.position ){
          OffsetTile(i);
        }
      }
    }*/
    
  }


  void ShiftLeft(){

currentCenterX ++;

    for( int i = 0; i < 3*3; i++ ){

       if( tileObjects[i].transform.position.x - data.player.position.x < -hT ){ 
        tileObjects[i].transform.position -= Vector3.left * t; 
        OffsetTile(i);
    
      }
    }


  }
  void ShiftRight(){

    currentCenterX --;

    for( int i = 0; i < 3*3; i++ ){

      if( tileObjects[i].transform.position.x - data.player.position.x > hT ){ 
        tileObjects[i].transform.position += Vector3.left * t; 
        OffsetTile(i);
      }
    }


  }

  void ShiftBack(){

currentCenterY --;

    for( int i = 0; i < 3*3; i++ ){

      if( tileObjects[i].transform.position.z - data.player.position.z > hT ){ 
        tileObjects[i].transform.position -= Vector3.forward * t; 
        OffsetTile(i);
      }
    }


  }

  void ShiftForward(){


currentCenterY ++;
     for( int i = 0; i < 3*3; i++ ){

      if( tileObjects[i].transform.position.z - data.player.position.z < -hT ){ 
        tileObjects[i].transform.position += Vector3.forward * t; 
        OffsetTile(i);
      }
    }


  }

  public void OffsetTile(int i ){
    Tiles[i].Set();
  }

  public void ToggleWater(){
    for( int i = 0; i < Tiles.Length; i++ ){
      Tiles[i].water.active = !Tiles[i].water.active;
    } 
  }


public void ToggleWaterOff(){
    for( int i = 0; i < Tiles.Length; i++ ){
      Tiles[i].water.active = false;
    } 
  }


public void ToggleWaterOn(){
    for( int i = 0; i < Tiles.Length; i++ ){
      Tiles[i].water.active = true;
    } 
  }

}
