using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTap : Cycle
{
   
   public void Tap(){

    print("helllllo");
      transform.position = data.land.Trace( data.inputEvents.ray.origin , data.inputEvents.ray.direction );
      data.playerControls.SetMoveTarget( transform.position );
   }

}
