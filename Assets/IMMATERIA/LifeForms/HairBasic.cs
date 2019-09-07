
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
    constraint0.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", Hair );

    constraint1.BindInt("_Pass" , 1 );
    constraint1.BindPrimaryForm("_VertBuffer", Hair);
    constraint1.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", Hair );

    set.BindAttribute( "_HairLength"  , "length", Hair );
    set.BindAttribute( "_HairVariance"  , "variance", Hair );
    set.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", Hair );

    // Don't need to bind for all of them ( constraints ) because same shader
    collision.BindAttribute( "_HairLength"  , "length", Hair );
    collision.BindAttribute( "_HairVariance"  , "variance", Hair );
    collision.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", Hair );
    collision.BindAttribute( "_Transform" , "transformArray" , this );

    data.BindCameraData(collision);

  }


  public override void OnBirth(){
    set.Live();
    set.active = false;
  }

  public override void Activate(){
    set.Live();
    set.active = false;
  }

  public override void WhileLiving(float v){
    
    //set.active = false;
    transformArray = HELP.GetMatrixFloats( transform.localToWorldMatrix );
  }

  public void Set(){
    set.YOLO();
  }


}