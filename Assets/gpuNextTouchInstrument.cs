using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gpuNextTouchInstrument : Cycle
{

  public AudioClip[] clips;
  ClosestLife c;
  public float lastPlayTime;

  public float minPlayTime;
  public float maxDist;

  public float randomness;

  public override void OnBirthed(){
    c = data.gpuCollisions.life;
    lastPlayTime = 0;
  }

  public override void WhileLiving( float v ){
    bool inAndNewClosest = ((c.closestID != c.oClosestID) && (c.closest.magnitude < maxDist));
    bool nowIn = ((c.closest.magnitude < maxDist) && (c.oClosest.magnitude >= maxDist));
    bool overPlayTime = ((Time.time - lastPlayTime) > minPlayTime+ randomness * Random.Range(-.99f,.99f));
    //print( (c.closestID != c.oClosestID) );
    if( ( inAndNewClosest || nowIn ) && overPlayTime ){

    
      data.audio.Play( clips[Random.Range(0,clips.Length)]  ,  2.01f , .4f );
      lastPlayTime = Time.time;
    }

  }
}
