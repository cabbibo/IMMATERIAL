using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class God : Cycle {


private static God _instance;


public void OnRenderObject(){
    if( created ){ _WhileDebug(); }
}

public void LateUpdate(){


    if( birthing ){ _WhileBirthing(1);}
    if( living ){ _WhileLiving(1); }
    if( dying ){ _WhileDying(1); }
}


public void OnEnable(){

    _instance = this;
    _Create(); 
    _OnGestate();
    _OnGestated();
    _OnBirth(); 
    _OnBirthed();
    _OnLive(); 
}

public void OnDisable(){

    _Destroy();   
}

public void Rebuild(){
    OnDisable();
    OnEnable();
}


  // Add a menu item named "Do Something with a Shortcut Key" to MyMenu in the menu bar
    // and give it a shortcut (ctrl-g on Windows, cmd-g on macOS).
    [MenuItem("IM||MATERIA/Rebuid Scene %b")]
    static void RebuildScene()
    {
        _instance.Rebuild();//Debug.Log("Doing something with a Shortcut Key...");
    }



}
