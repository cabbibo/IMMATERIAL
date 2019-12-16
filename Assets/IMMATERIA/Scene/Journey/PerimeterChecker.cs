﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PerimeterChecker : Cycle
{

  public int id;

  public float innerRadius;
  public float outerRadius;

  public bool insideInner;
  public bool insideOuter;

  public EventTypes.BaseEvent OnEnterOuter;
  public EventTypes.BaseEvent OnEnterInner;
  public EventTypes.BaseEvent OnExitOuter;
  public EventTypes.BaseEvent OnExitInner;
  public EventTypes.FloatEvent OnDoFade;


  public float dif;
  public float oDif;
  public bool started;

  public bool hardCoded;


  public override void OnGestated(){

    ExitInner();
    ExitOuter();

    oDif = 100000;
  
  }



  public override void WhileLiving( float v){
    
   
    if( !hardCoded ){ 

      

      dif = (transform.position - data.player.position).magnitude;

      if( dif < outerRadius && oDif >= outerRadius &&  !insideOuter ){
        EnterOuter();
      }

      if( dif < innerRadius && oDif >= innerRadius && !insideInner ){
        EnterInner();
      }

      if( dif >= outerRadius && oDif < outerRadius && insideOuter ){
        ExitOuter();
      }

      if( dif >= innerRadius && oDif < innerRadius && insideInner){
        ExitInner();
      }
      
      if( insideOuter && !insideInner ){
        DoFade( 1-((dif - innerRadius) / (outerRadius-innerRadius)));
      }

      oDif = dif;

    }


  
  }

  public void EnterOuter(){

    data.state.lastTimeStoryVisited = Time.time;
    data.state.inStory = true;

    insideOuter=true;
    data.sceneCircle.Set( this );

    OnEnterOuter.Invoke();

  }

  public void EnterInner(){

    insideInner=true;
    OnEnterInner.Invoke();


  }

  public void ExitOuter(){
    insideOuter=false;
    data.sceneCircle.Unset( this );
    OnExitOuter.Invoke();
  }


  public void ExitInner(){

    data.state.lastTimeStoryVisited = Time.time;
    data.state.inStory = false;

    insideInner=false;
    OnExitInner.Invoke();
  }

  public void DoFade( float v ){
    OnDoFade.Invoke(v);
  }



}