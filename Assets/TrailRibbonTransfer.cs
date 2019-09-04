using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRibbonTransfer : Cycle {
  
  public Life parallel;
  public Form particles;

  public Form verts;
  public IndexForm triangles; 
  public Life transfer;

   public Body body;
  public bool showBody;
  public Form skeleton;
  public float radius;


    public override void Destroy(){
    Cycles.Remove( body );
    Cycles.Remove( transfer );
    Cycles.Remove( parallel );
  }


  public override void Create(){


    print("CRIATOSZqq");
    SafeInsert(body);
    SafeInsert(transfer);
    SafeInsert( parallel );

  }

  public override void Bind(){

    print("hmmmss");
      transfer.BindPrimaryForm("_VertBuffer", verts);
    transfer.BindForm("_SkeletonBuffer", skeleton); 

    transfer.BindAttribute( "_RibbonLength" , "length" , verts );
    transfer.BindAttribute( "_NumVertsPerHair" , "particlesPerTrail" , skeleton );

    parallel.BindPrimaryForm("_ParticleBuffer" , particles);
    parallel.BindForm("_VertBuffer" , verts );

    parallel.BindAttribute( "_RibbonLength" , "length" , verts );
    parallel.BindAttribute( "_NumVertsPerHair" ,  "length" , verts  );

    data.BindCameraData( parallel );
    parallel.BindAttribute( "_Radius" ,  "radius" , this );

  }

}