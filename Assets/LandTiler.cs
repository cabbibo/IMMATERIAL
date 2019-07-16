using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTiler : Cycle
{

  public LandTile[] Tiles;

  public float tileSize;
  public int numTiles;
  public int tileDimensions;

  public Life setTile;

  public GameObject landTilePrefab;



  public Vector3 _Offset;
  public int _ID;

  public GameObject[]  tileObjects;

  private float hT; // halfTile
  private float t; // tile

  public override void Destroy(){
    print("Destry");
    if( Tiles != null ){
    for( int i=0; i < Tiles.Length; i++ ){

          Cycles.Remove(Tiles[i]);
          DestroyImmediate(tileObjects[i]);


    }

    Cycles.Remove(setTile );
    Tiles = null;
  }
  }

  public override void Create(){

    print("OnCreate");


        Tiles = new LandTile[numTiles * numTiles ];
        tileObjects = new GameObject[numTiles * numTiles ];

        t = tileSize  * numTiles;
        hT = t/2;
  
        Tiles = new LandTile[numTiles * numTiles ];
        
            print("huhhh");
        for( int i = 0; i < numTiles; i++ ){
          for( int j = 0; j < numTiles; j++ ){

            int id = i * numTiles + j;
            GameObject g = Instantiate( landTilePrefab );
            g.transform.parent = transform;
            tileObjects[id] = g;

              Tiles[id]= g.GetComponent<LandTile>();
           Tiles[id].size = tileSize;
           Tiles[id].dimensions = tileDimensions;

           SafeInsert(Tiles[id]);

          }
        }
      

      SafeInsert(setTile );

        print("birder");
      for( int i = 0; i < Tiles.Length; i++ ){
       // _ID = i;

        _Offset = Vector3.left * (i%numTiles) * tileSize;
        _Offset += Vector3.forward * (float)(i/numTiles) * tileSize;

        tileObjects[i].transform.position = data.PlayerPosition + _Offset;
        //OffsetTile();
    }
  }

  public override void Bind(){
    data.BindLandData( setTile );
    setTile.BindAttribute("_Offset", "_Offset" , this );
    setTile.BindAttribute("_ID", "_ID" , this );

 
  }

  public override void OnBirthed(){

  }

    // Use this for initialization
  public override void WhileLiving (float l) {


    Vector3 oPos;

//    print(data.PlayerPosition);

    for( int i = 0; i < Tiles.Length; i++ ){



//  print("s");
       oPos = tileObjects[i].transform.position;

         _ID = i;
         _Offset = Vector3.zero;

      if( data.PlayerPosition.x - tileObjects[i].transform.position.x < -hT   ){     
      //print("hellooo") ; 
          _Offset += -Vector3.right * t;
         //_ID = i;

        tileObjects[i].transform.position += _Offset;
      }

      if( data.PlayerPosition.x - tileObjects[i].transform.position.x > hT   ){
          _Offset += Vector3.right * t;
        tileObjects[i].transform.position += _Offset;
      }


      if( data.PlayerPosition.z - tileObjects[i].transform.position.z < -hT   ){        
            _Offset += -Vector3.forward * t;
       

        tileObjects[i].transform.position += _Offset;
      }

      if( data.PlayerPosition.z - tileObjects[i].transform.position.z > hT   ){
      //   print("hellooo") ; 
         _Offset += Vector3.forward * t;
        tileObjects[i].transform.position += _Offset;
  
      }

      _Offset = tileObjects[i].transform.position;

      //_Offset = tileObjects[i].transform.position;
      //if( oPos != tileObjects[i].transform.position ){
        OffsetTile();
      //}


      /*if( data.PlayerPosition.z - tileObjects[i].position.z < -hT * tileSize ){
        tileObjects[i].position -= Vector3.forward * t* tileSize ;
      }

      if( data.PlayerPosition.z - tileObjects[i].position.z > hT * tileSize  ){
        tileObjects[i].position += Vector3.forward * t* tileSize ;
      }


  /// WHY THIS?
      if( oPos != Tiles[i].position ){
        Tiles[i].Set( (Tiles[i].transform.position-oPos) );
      }*/

      //waters[i].position = new Vector3( Tiles[i].position.x , Tiles[i].waterHeight,Tiles[i].position.z);
    }
    
  }

  public void OffsetTile(){
    setTile.RebindPrimaryForm( "_VertBuffer" , Tiles[_ID] );
    setTile.YOLO();
  }


}
