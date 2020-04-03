using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToFade : Cycle
{

  public float waitTime;
  public float startTime;
  public float currTime;
  public bool hasFired;
  public EventTypes.BaseEvent Fire;

  public override void Create(){
    hasFired = false;
  }
  public override void Activate(){
    startTime = Time.time;
  }

  public override void WhileLiving( float v ){
    if( !hasFired ){
      currTime = Time.time- startTime;
      if( currTime > waitTime ){
        Fire.Invoke();
        hasFired = true;
        active = false;
      }
    }
  }

}
