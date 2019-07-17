using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTap : Cycle
{
   
   public void Tap(){

      transform.position = data.Island.Trace( data.Events.ray.origin , data.Events.ray.direction );
      data.PlayerControls.SetMoveTarget( transform.position );
   }

}
