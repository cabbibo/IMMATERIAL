using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionBunch : Cycle
{

  public CircleOnTerrain circle;
  public ParticlesOnCircle baseParticles;
  public HairBasic stalk;
  public TubeTransfer  stalkBody;
  public Dandelion tips;

  public bool constant;

  public int releasing;

  public override void Create(){
    SafeInsert(circle);
    SafeInsert(baseParticles);
    SafeInsert(stalk);
    SafeInsert(stalkBody);
    SafeInsert(tips);
  }

  public void Set(){
    circle.Set();
    baseParticles.Set();
    //kelp.Set();
  }

  public override void Activate(){
//    print("activado");
    Set();
    stalkBody.showBody = true;
  }


  public override void Deactivate(){
    stalkBody.showBody = false;
  }

  public override void WhileLiving(float v){
    if( constant ){ Set(); }
  }


  public override void Bind(){
    data.BindAllData( stalk.collision ); 
    stalk.collision.BindInt("_Releasing",()=>releasing);
  }


  public void SetRelease( int i ){
    releasing = i;
  }


}
