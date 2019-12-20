﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotate : Cycle
{

  public float speed;
  public float angle;

  public SampleSynth synth;

  public float oAngle;
  public float playTime;

  public void WhileDown( Vector2 d ){
    speed += d.x * .01f;

  }



  public override void WhileLiving(float v){

    float camAngle = Vector3.Angle( Vector3.forward , Camera.main.transform.forward);
    speed -= (angle-camAngle)  * .04f;
    speed *= .9f;
    angle += speed;

    if( Mathf.Abs( angle - oAngle ) > 2.4f  && Time.time - playTime > .1f * Random.Range(.5f , 1.5f) ){
      synth.PlayGrain();
      oAngle =angle;
      playTime = Time.time;
    }

    transform.rotation = Quaternion.AngleAxis(angle, Camera.main.transform.up);
  }


}
