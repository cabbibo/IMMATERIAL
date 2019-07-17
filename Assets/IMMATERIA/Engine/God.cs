using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class God : Cycle {


private static God _instance;
  


public override void Create(){

    if( data != null ){
        SafePrepend(data);
    }else{
        print("DUDE WHERE'S MY DATA");
    }
}

public void OnRenderObject(){
    if( created ){ _WhileDebug(); }
}

public void LateUpdate(){


    if( birthing ){ _WhileBirthing(1);}
    if( living ){ _WhileLiving(1); }
    if( dying ){ _WhileDying(1); }
}



public void OnEnable(){

    #if UNITY_EDITOR 
        EditorApplication.update += Always;
    #endif
    if( _instance == null ){ _instance = this; }
    Reset();
    _Create(); 
    _OnGestate();
    _OnGestated();
    _OnBirth(); 
    _OnBirthed();
    _OnLive(); 
}

public void OnDisable(){

    #if UNITY_EDITOR 
        EditorApplication.update -= Always;
    #endif
        
    _Destroy();   
}


 
    void Always(){    
    #if UNITY_EDITOR 
      EditorApplication.QueuePlayerLoopUpdate();
    #endif
    }



public void Rebuild(){
    Reset();
    OnDisable();
    OnEnable();
}



}
