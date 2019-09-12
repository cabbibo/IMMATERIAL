using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpTips : Simulation
{

  public Hair hair ;
  public TransformBuffer pullers;
  public Form baseParticles;

  public override void Create(){
    form.count = baseParticles.count;
  }


  public override void Bind(){

    data.land.BindData( life );
    data.BindPlayerData( life );
    data.BindCameraData( life );

    life.BindInt( "_NumVertsPerHair" , () => hair.numVertsPerHair );
    life.BindForm( "_HairBuffer" ,hair );
    life.BindForm( "_PullBuffer" ,pullers );

  }
}
