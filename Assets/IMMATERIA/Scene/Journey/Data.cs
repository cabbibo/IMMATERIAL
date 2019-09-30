﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
  
  This Script is going to be a relatively massive singleton
  that is going to hold all the data and helpers for binding
  that data throughout the project! 

  I'm hoping this will help me get away from the horrific
  spagetti code I'm used to!


*/
public class Data : Cycle
{
    

  
  public Transform camera;
  public Transform player;

  public Journey journey;
  public State state;
  public Tween tween;

  public CameraController cameraControls;
  public Character playerControls;
  public InputEvents inputEvents;
  public Land land;
  public LandTiler tiler;
  public SceneCircle sceneCircle;
  public Book book;

  public Framer framer;

  public TextParticles textParticles;
  public GuideParticles guideParticles;
  public GuideParticles monolithParticles;
  public TerrainTap  terrainTap;

  public Painter painter;
  public Helper helper;

  //public Terrain terrain;


  public Vector3 cameraForward;
  public Vector3 cameraUp;
  public Vector3 cameraRight;
  public Vector3 cameraPosition;


  public Vector3 playerForward;
  public Vector3 playerUp;
  public Vector3 playerRight;
  public Vector3 playerPosition;
  public Vector3 playerSoul;

  public Transform soul;


  public override void Create(){
    
    if( framer != null ){ SafePrepend(framer); }
    if( journey != null ){ SafePrepend(journey); }
    if( tween != null ){ SafePrepend(tween); }
    if( textParticles != null ){ SafePrepend(textParticles); }
    if( cameraControls != null ){ SafePrepend(cameraControls); }
    if( playerControls != null ){ SafePrepend(playerControls); }
    if( inputEvents != null ){ SafePrepend(inputEvents); }
    if( sceneCircle != null ){ SafePrepend(sceneCircle); }
    if( land != null ){ SafePrepend(land); }
    if( book != null ){ SafePrepend(book); }
    if( guideParticles != null ){ SafePrepend(guideParticles); }
    if( monolithParticles != null ){ SafePrepend(monolithParticles); }
    if( tiler != null ){ SafePrepend(tiler); }
    if( terrainTap != null ){ SafePrepend(terrainTap); }
    if( state != null ){ SafePrepend(state); }
    if( painter != null ){ SafePrepend(painter); }
    if( helper != null ){ SafePrepend(helper); }

  }


  public void BindPlayerData(Life toBind){

    toBind.BindVector3("_PlayerForward",  () => this.playerForward    );
    toBind.BindVector3("_PlayerUp",       () => this.playerUp         );
    toBind.BindVector3("_PlayerRight",    () => this.playerRight      );
    toBind.BindVector3("_PlayerPosition", () => this.playerPosition   );
    toBind.BindVector3("_PlayerSoul",     () => this.playerSoul       );

  }

  public void BindCameraData(Life toBind){

    toBind.BindVector3("_CameraForward",  () => this.cameraForward  );
    toBind.BindVector3("_CameraUp",       () => this.cameraUp       );
    toBind.BindVector3("_CameraRight",    () => this.cameraRight    );
    toBind.BindVector3("_CameraPosition", () => this.cameraPosition );

  }




  public void BindRayData(Life toBind){

    toBind.BindVector3("_RO", () => inputEvents.RO    );
    toBind.BindVector3("_RD", () => inputEvents.RD    );
    toBind.BindFloat("_DOWN", () => inputEvents.Down  );

  }

  public void BindLandData(Life toBind){

    land.BindData(toBind);

  }

  public void BindAllData(Life life){
    BindPlayerData( life );
    BindLandData(life);
    BindPlayerData(life);
    BindRayData(life);
  }

  public override void WhileLiving( float v ){


    if( camera != null ){
      cameraForward = camera.forward;
      cameraUp = camera.up;
      cameraRight = camera.right;
      cameraPosition = camera.position;
    }

    if( player != null ){
      playerForward = player.forward;
      playerUp = player.up;
      playerRight = player.right;
      playerPosition = player.position;
    }
    
    if( soul != null ){
      playerSoul = soul.position;
    }

    Shader.SetGlobalVector("_PlayerForward"   , playerForward );
    Shader.SetGlobalVector("_PlayerUp"        , playerUp );
    Shader.SetGlobalVector("_PlayerRight"     , playerRight );
    Shader.SetGlobalVector("_PlayerPosition"  , playerPosition );
    Shader.SetGlobalVector("_PlayerSoul"      , playerSoul );

  }

}
