using System.Collections;
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

  [HideInInspector]public Transform cam;
  [HideInInspector]public Vector3 right;
  [HideInInspector]public Vector3 up;
  [HideInInspector]public Vector3 forward;

  // Use this for initialization
  public override void Create(){


    transformArray = new float[16];
    cam = Camera.main.transform;

    Cycles.Insert(0,body);
    Cycles.Insert(1,transfer);


  }

  public override void _Bind(){
    transfer.BindPrimaryForm("_VertBuffer", verts);
    transfer.BindForm("_SkeletonBuffer", skeleton); 

    transfer.BindAttribute("_CameraRight" , "right" , this); 
    transfer.BindAttribute("_CameraUp" , "up" , this); 
    transfer.BindAttribute("_CameraForward" , "forward" , this); 
    transfer.BindAttribute("_Radius" , "radius" , this); 

    transfer.BindAttribute("_TransformBase","transformArray", this);
    
    Bind();
  }

  public virtual void BindAttributes(){}

  public override void WhileLiving(float v){


    if( active == true ){


      transformArray = HELP.GetMatrixFloats( transform.localToWorldMatrix );
    

      right = cam.right;
      up = cam.up;
      forward = cam.forward;

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
