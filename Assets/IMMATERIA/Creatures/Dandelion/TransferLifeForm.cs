﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferLifeForm : LifeForm {


  public Form verts;
  public IndexForm triangles; 
  public Life transfer;
  public Body body;
  public bool showBody;
  public Form skeleton;
  public float radius;

    public float[] transformArray;


  public override void Destroy(){
    Cycles.Remove( body );
    Cycles.Remove( transfer );
  }
  // Use this for initialization
  public override void _Create(){

    _Destroy();


    transformArray = new float[16];

    SafeInsert(body);
    SafeInsert(transfer);


    DoCreate();

  }

  public override void _Bind(){
    transfer.BindPrimaryForm("_VertBuffer", verts);
    transfer.BindForm("_SkeletonBuffer", skeleton); 

    data.BindCameraData( transfer );
    
    transfer.BindFloat("_Radius" , () => this.radius ); 
    transfer.BindFloats("_TransformBase", () => this.transformArray);
    
    Bind();
  }

  public virtual void BindAttributes(){}

  public override void WhileLiving(float v){


    if( active == true ){


      transformArray = HELP.GetMatrixFloats( transform.localToWorldMatrix );
  

      if( showBody == true ){
        body.active = true;
      }else{
        body.active = false;
      }

      
    }

  }

  public override void Activate(){
    showBody = true;
  }

  public override void Deactivate(){
    showBody = false;
  }

}
