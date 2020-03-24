using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentCollisionCalculator : Cycle
{
  public ClosestLife life;

  public Form ToBind;

  public override void Create(){
    SafeInsert(life);
  }

  public override void Bind(){
    data.BindAllData(life);
  }

  public void BindNewForm( Form f ){
    ToBind = f;
    life.Set(f);
  }

  public void Unbind(){
    ToBind = null;
    life.Unset();
  }

  public override void WhileLiving( float v ){
    if( life.primaryForm == null ){
      life.active = false;
    }



    if( life.oClosestID  != life.closestID ){
        Shader.SetGlobalFloat("_ClosestGPUCollisionID", life.closestID );
        Shader.SetGlobalVector("_ClosestGPUCollision", life.closest );

        Shader.SetGlobalFloat("_ClosestGPUCollisionTime", life.closestTime );
      }
  }
}
