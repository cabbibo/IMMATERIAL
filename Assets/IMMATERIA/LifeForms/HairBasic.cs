﻿
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HairBasic : LifeForm {

  public Life set;
  public Life collision;
  
  public ConstraintLife constraint0;
  public ConstraintLife constraint1;

  public Form Base;
  public Hair Hair;

  public float[] transformArray;

  public override void Create(){

    transformArray = new float[16];

    
    /*  
      All of this info should be visualizable!
    */

    SafePrepend( set );
    SafePrepend( collision );
    SafePrepend( constraint0 );
    SafePrepend( constraint1 );
    SafePrepend( Hair );

    //Cycles.Insert( 4 , Base );


  }


  public override void Bind(){

    set.BindPrimaryForm("_VertBuffer", Hair);
    set.BindForm("_BaseBuffer", Base );

    collision.BindPrimaryForm("_VertBuffer", Hair);
    collision.BindForm("_BaseBuffer", Base ); 

    constraint0.BindInt("_Pass" , 0 );
    constraint0.BindPrimaryForm("_VertBuffer", Hair);
    constraint0.BindInt( "_NumVertsPerHair" , () => Hair.numVertsPerHair );

    constraint1.BindInt("_Pass" , 1 );
    constraint1.BindPrimaryForm("_VertBuffer", Hair);
    constraint1.BindInt( "_NumVertsPerHair" ,  () => Hair.numVertsPerHair );

    set.BindFloat( "_HairLength"  , () => Hair.length);
    set.BindFloat( "_HairVariance"  , () => Hair.variance);
    set.BindInt( "_NumVertsPerHair" , () => Hair.numVertsPerHair );

    // Don't need to bind for all of them ( constraints ) because same shader
    collision.BindFloat( "_HairLength"  , () => Hair.length );
    collision.BindFloat( "_HairVariance"  , () => Hair.variance );
    collision.BindInt( "_NumVertsPerHair" , () => Hair.numVertsPerHair );
    collision.BindFloats( "_Transform" , () => this.transformArray );

    data.BindCameraData(collision);

  }


  public override void OnBirth(){
    set.active = true;
  }

  public override void Activate(){
    set.active = true;
  }

  public override void WhileLiving(float v){
    
    //set.active = false;
    transformArray = HELP.GetMatrixFloats( transform.localToWorldMatrix );
  }

  public void Set(){
    set.YOLO();
  }


}