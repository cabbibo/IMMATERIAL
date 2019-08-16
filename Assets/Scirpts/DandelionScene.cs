using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionScene : Cycle
{

  public MeshVerts  planeVerts;
  public MeshTris   planeTris;

  public Dandelion dandelions;

  public Life SetDandelionsOnPlane;
  public Life Simulation;

  public bool inScene;
  public bool _Releasing;
  public bool _ViolentWind;


  public override void Create(){
    SafeInsert( SetDandelionsOnPlane );
  }


  public override void Bind(){

    data.land.BindData( SetDandelionsOnPlane );
  

  }



  public override void Activate(){

    SetDandelionsOnPlane.YOLO();
    Simulation.Activate();

  }

  public override void WhileLiving( float v ){

    if( active == true ){
    }

  }



}
