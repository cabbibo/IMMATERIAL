using System.Collections;
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

  public CameraController cameraControls;
  public Character playerControls;
  public InputEvents inputEvents;
  public Land land;
  public SceneCircle sceneCircle;
  public Book book;

  public TextParticles textParticles;
  public GuideParticles guideParticles;
  public GuideParticles monolithParticles;

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
    if( journey != null ){ SafePrepend(journey); }
    if( textParticles != null ){ SafePrepend(textParticles); }
    if( cameraControls != null ){ SafePrepend(cameraControls); }
    if( playerControls != null ){ SafePrepend(playerControls); }
    if( inputEvents != null ){ SafePrepend(inputEvents); }
    if( sceneCircle != null ){ SafePrepend(sceneCircle); }
    if( land != null ){ SafePrepend(land); }
    if( book != null ){ SafePrepend(book); }
    if( guideParticles != null ){ SafePrepend(guideParticles); }
    if( monolithParticles != null ){ SafePrepend(monolithParticles); }
  }


  public void BindPlayerData(Life toBind){

    toBind.BindAttribute("_PlayerForward", "playerForward" , this );
    toBind.BindAttribute("_PlayerUp", "playerUp" , this );
    toBind.BindAttribute("_PlayerRight", "playerRight" , this );
    toBind.BindAttribute("_PlayerPosition", "playerPosition" , this );
    toBind.BindAttribute("_PlayerSoul", "playerSoul" , this );

  }

  public void BindCameraData(Life toBind){

    toBind.BindAttribute("_CameraForward", "cameraForward" , this );
    toBind.BindAttribute("_CameraUp", "cameraUp" , this );
    toBind.BindAttribute("_CameraRight", "cameraRight" , this );
    toBind.BindAttribute("_CameraPosition", "cameraPosition" , this );

  }

  public void BindLandData(Life toBind){

    land.BindData(toBind);

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
