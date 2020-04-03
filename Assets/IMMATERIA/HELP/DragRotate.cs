using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotate : Cycle
{

  public float speed;
  public float angle;

  public SampleSynth synth;

  public float oAngle;
  public float playTime;

  public float dragMultiplier;
  public float returnStrength;
  public float dampening;

  public float playSpeed;
  public float anglePlay;

  public float pitchMultiplier;
  public float pitchBase;

  public float volumeMultiplier;
  public float volumeBase;


public override void Activate(){
  playTime= Time.time;
}
  public void WhileDown( Vector2 d ){
    speed += d.x * dragMultiplier;

  }



  public override void WhileLiving(float v){

    float camAngle = Vector3.Angle( Vector3.forward , Camera.main.transform.forward);
    speed -= (angle-camAngle)  * returnStrength;
    speed *= dampening;
    angle += speed;

    if( Mathf.Abs( angle - oAngle ) > anglePlay  && Time.time - playTime > playSpeed * Random.Range(.5f , 1.5f)){
      synth.PlayGrain();
      synth.volume = Mathf.Abs(speed) * volumeMultiplier + volumeBase;
      synth.pitch = Mathf.Abs(speed) * pitchMultiplier + pitchBase;
      oAngle =angle;
      playTime = Time.time;
    }

    transform.rotation = Quaternion.AngleAxis(angle, Camera.main.transform.up);
  }


}
