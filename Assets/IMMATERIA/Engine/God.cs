 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class God : Cycle {

public bool AllInEditMode;
private static God _instance;
  
public bool started;

public override void Create(){

    if( data != null ){
        SafePrepend(data);
    }else{
        print("DUDE WHERE'S MY DATA");
    }

    Application.targetFrameRate = 60;


}


public void OnRenderObject(){
    if( created ){ _WhileDebug(); }
}

public void LateUpdate(){

    if( started == false ){ 
        _OnLive(); 
        started = true;
    }
    
    if( birthing ){ _WhileBirthing(1);}
    if( living ){ _WhileLiving(1); }
    if( dying ){ _WhileDying(1); }
}




public void OnLevelWasLoaded(){
    print("WASDass");
}


public void OnEnable(){

    started = false;


    if( _instance == null ){ _instance = this; }

    #if UNITY_EDITOR 
        EditorApplication.update += Always;

         Reset();
        _Destroy(); 
        _Create(); 
        _OnGestate();
        _OnGestated();
        _OnBirth(); 
        _OnBirthed();

    #else

    

    print("god enambles");


    if( Application.isPlaying ){
    
        Reset();
        _Destroy(); 
        _Create(); 
        _OnGestate();
        _OnGestated();
        _OnBirth(); 
        _OnBirthed();

    }

    #endif


}



public void OnDisable(){


  //  print("god disabblee");
    #if UNITY_EDITOR 
        EditorApplication.update -= Always;
        _Destroy();   

    #else
     
    if( Application.isPlaying ){
        _Destroy();   
    }
    #endif
}


 
void Always(){    
  #if UNITY_EDITOR 
  if( AllInEditMode ){
    EditorApplication.QueuePlayerLoopUpdate();
  }
  #endif
}



public void Rebuild(){
    Reset();
    OnDisable();
    OnEnable();
}



}
