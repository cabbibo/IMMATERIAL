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


//    print("CRIATOSZqq");
    SafeInsert(body);
    SafeInsert(transfer);
    SafeInsert( parallel );

  }

  public override void Bind(){

  //  print("hmmmss");
    transfer.BindPrimaryForm("_VertBuffer", verts);
    transfer.BindForm("_SkeletonBuffer", skeleton); 


    TrailRibbonVerts v = (TrailRibbonVerts)verts;
    transfer.BindInt( "_RibbonLength" , () => v.length );

    TrailParticles s = (TrailParticles)skeleton;
    transfer.BindInt( "_NumVertsPerHair" , () => s.particlesPerTrail);

    parallel.BindPrimaryForm("_ParticleBuffer" , particles);
    parallel.BindForm("_VertBuffer" , verts );

    parallel.BindInt( "_RibbonLength"     ,   () => v.length  );
    parallel.BindInt( "_NumVertsPerHair"  ,   () => v.length  );

    data.BindCameraData( parallel );

    parallel.BindFloat( "_Radius" ,  () => this.radius );

  }

}