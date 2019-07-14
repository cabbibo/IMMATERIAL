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
    
  public Transform Camera;
  public Transform Player;

  public TextParticles Text;
  public CameraController Controls;
  public InputEvents Events;

  //public Terrain terrain;

  public Vector3 CameraForward;
  public Vector3 CameraUp;
  public Vector3 CameraRight;
  public Vector3 CameraPosition;


  public Vector3 PlayerForward;
  public Vector3 PlayerUp;
  public Vector3 PlayerRight;
  public Vector3 PlayerPosition;
  public Vector3 PlayerSoul;

  public Transform Soul;


  public void BindPlayerData(Life toBind){

    toBind.BindAttribute("_PlayerForward", "PlayerForward" , this );
    toBind.BindAttribute("_PlayerUp", "PlayerUp" , this );
    toBind.BindAttribute("_PlayerRight", "PlayerRight" , this );
    toBind.BindAttribute("_PlayerPosition", "PlayerPosition" , this );
    toBind.BindAttribute("_PlayerSoul", "PlayerSoul" , this );

  }

  public void BindCameraData(Life toBind){

    toBind.BindAttribute("_CameraForward", "CameraForward" , this );
    toBind.BindAttribute("_CameraUp", "CameraUp" , this );
    toBind.BindAttribute("_CameraRight", "CameraRight" , this );
    toBind.BindAttribute("_CameraPosition", "CameraPosition" , this );

  }

  public override void WhileLiving( float v ){


    CameraForward = Camera.forward;
    CameraUp = Camera.up;
    CameraRight = Camera.right;
    CameraPosition = Camera.position;


    PlayerForward = Player.forward;
    PlayerUp = Player.up;
    PlayerRight = Player.right;
    PlayerPosition = Player.position;
    PlayerSoul = Soul.position;


  }

}
