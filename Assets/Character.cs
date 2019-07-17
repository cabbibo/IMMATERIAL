using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Character : Cycle {

  public Animator animator;

  public bool doTerrain;

  public Vector3 moveTarget;
  
  public float runMultiplier;
  public float maxSpeed;
  public float forwardCutoff;
  
  public Vector3 velocity;
  public Vector3 force;

  public float moveForce;
  public float dampening;

  private Vector3 tmpPos;
  private Quaternion tmpRot;


  private Vector3 oPos;
  private Quaternion oRot;

  private Vector3 deltaPos;
  private Quaternion deltaRot;

  public override void Create () {

    oRot = Quaternion.identity;
    deltaRot = Quaternion.identity;
    oPos = Vector3.zero;
    velocity = Vector3.zero;
  }

  public override void WhileLiving (float v) {
    DoMovement();   
  }





/*
  
  Movement Stuff

*/

  void DoMovement(){


    oPos = transform.position;
    oRot = transform.rotation;

    force = Vector3.zero;


    //force +=  m_Move  * moveForce * .1f  * runForce;
      Vector3 dif = moveTarget-transform.position;

    force += dif * .01f * moveForce * (velocity.magnitude+.13f);
    
    velocity += force;
    velocity *= dampening;

    if( velocity.magnitude > maxSpeed ){
      velocity = velocity.normalized * maxSpeed;
    }

    Vector3 m = transform.InverseTransformDirection(velocity);
    float turn = Mathf.Atan2(m.x, m.z);
    float forward = m.z;

    Rotate(forward , turn);
    animator.SetFloat("Turn", turn, 0.1f, Time.deltaTime);
  
    if( forward < forwardCutoff ){ forward = 0; }
    animator.SetFloat("Forward", forward*runMultiplier, 0.1f, Time.deltaTime);


if( doTerrain ){
    float h = data.Island.SampleHeight( transform.position );
    transform.position = new Vector3( transform.position.x , h , transform.position.z);
}

 //animator.Update(Time.deltaTime);

    deltaPos = transform.position - oPos;

    velocity = deltaPos;
  }


  public void SetMoveTarget( Vector3 p ){
    moveTarget = p;
  }






  void Rotate(float f  , float t){

    // help the character turn faster (this is in addition to root rotation in the animation)
    float turnSpeed = Mathf.Lerp(180, 360, f);
    transform.Rotate(0, t * turnSpeed * Time.deltaTime, 0);
  
  }




}
