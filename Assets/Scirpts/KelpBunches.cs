using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpBunches : Cycle
{ 

  public KelpBunch[] Kelps;

  public TransformBuffer pullers;


   

  public override void Create(){
    SafeInsert( pullers );
    for( int i = 0; i < Kelps.Length; i++ ){
      SafeInsert(Kelps[i]);
    }

  }

  public override void Bind(){

    for( int i = 0; i < Kelps.Length; i++){
      Kelps[i].kelp.collision.BindForm("_Pullers",pullers);
    }
  }

}
