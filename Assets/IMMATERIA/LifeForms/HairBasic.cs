
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HairBasic : LifeForm {

  public Life SetHairPosition;
  public Life HairCollision;
  
  public ConstraintLife HairConstraint0;
  public ConstraintLife HairConstraint1;

  public Form Base;
  public Hair Hair;

  public float[] transformArray;

  public override void Create(){

    transformArray = new float[16];

    
    /*  
      All of this info should be visualizable!
    */

    SafePrepend( SetHairPosition );
    SafePrepend( HairCollision );
    SafePrepend( HairConstraint0 );
    SafePrepend( HairConstraint1 );
    SafePrepend( Hair );

    //Cycles.Insert( 4 , Base );


  }


  public override void Bind(){

    SetHairPosition.BindPrimaryForm("_VertBuffer", Hair);
    SetHairPosition.BindForm("_BaseBuffer", Base );

    HairCollision.BindPrimaryForm("_VertBuffer", Hair);
    HairCollision.BindForm("_BaseBuffer", Base ); 

    HairConstraint0.BindInt("_Pass" , 0 );
    HairConstraint0.BindPrimaryForm("_VertBuffer", Hair);

    HairConstraint1.BindInt("_Pass" , 1 );
    HairConstraint1.BindPrimaryForm("_VertBuffer", Hair);

    SetHairPosition.BindAttribute( "_HairLength"  , "length", Hair );
    SetHairPosition.BindAttribute( "_HairVariance"  , "variance", Hair );
    SetHairPosition.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", Hair );

    // Don't need to bind for all of them ( constraints ) because same shader
    HairCollision.BindAttribute( "_HairLength"  , "length", Hair );
    HairCollision.BindAttribute( "_HairVariance"  , "variance", Hair );
    HairCollision.BindAttribute( "_NumVertsPerHair" , "numVertsPerHair", Hair );
    HairCollision.BindAttribute( "_Transform" , "transformArray" , this );

    data.BindCameraData(HairCollision);

  }


  public override void OnBirth(){
    SetHairPosition.Live();
    SetHairPosition.active = false;
  }

  public override void Activate(){
    SetHairPosition.Live();
    SetHairPosition.active = false;
  }

  public override void WhileLiving(float v){
    
    SetHairPosition.active = false;
    transformArray = HELP.GetMatrixFloats( transform.localToWorldMatrix );
  }

  public void Set(){
    SetHairPosition.YOLO();
  }


}