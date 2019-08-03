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

  public bool rotating;
  public float angleOffset;
  public float forwardOffset;

  public Transform moveTargetTransform;
  public bool movingTowardsTarget;

  public bool falling;

  public override void Create () {

    moveTarget = transform.position;
    oRot = Quaternion.identity;
    deltaRot = Quaternion.identity;
    oPos = Vector3.zero;
    velocity = Vector3.zero;
  }

  public override void WhileLiving (float v) {
    DoMovement();   
  }

  public void Fall(){
    falling = true;
  }




  public void SwipeTurn( Vector2 delta ){
    if( delta.magnitude > 10 ){
      movingTowardsTarget = false;
    }
    angleOffset += delta.x * .003f;
    forwardOffset += delta.y * .004f ;
    forwardOffset = Mathf.Max( forwardOffset, 0);
  
  }

/*
  
  Movement Stuff

*/

  void DoMovement(){


    oPos = transform.position;
    oRot = transform.rotation;

    force = Vector3.zero;


    if( falling ){
      animator.SetBool("Falling", true);
      if( movingTowardsTarget ){
        transform.position = Vector3.Lerp( transform.position , moveTarget , .1f);
        transform.rotation = Quaternion.Slerp( transform.rotation , moveTargetTransform.rotation , .1f);
      }

    }else{


      //force +=  m_Move  * moveForce * .1f  * runForce;
        

      //moveTarget += dif.magnitude * angleOffset * transform.right * .1f;

     // moveTarget = angleOffset * transform.right * .1f  * (forwardOffset + 1.3f) + forwardOffset * transform.forward * .1f;
     
    //  dif = moveTarget;

   //   if( dif.magnitude < .003f ){ dif = Vector3.zero; }

      //if( moveTargetTransform ){ moveTargetTransform.position = moveTarget + transform.position; }

      if( movingTowardsTarget ){
        Vector3 dif = moveTarget-transform.position;
        force =  dif * moveForce * (velocity.magnitude + .01f);
        angleOffset *= .5f;
        forwardOffset = Mathf.Lerp( forwardOffset  , force.magnitude * .01f , .1f );
      }else{

        if( Mathf.Abs(angleOffset) < .001f ){
          angleOffset = 0;
        }
        force = angleOffset  * transform.right   + forwardOffset * transform.forward;/// dif * .01f * moveForce;// * (velocity.magnitude+.13f);
      }
    
     // if( force.magnitude < .001f ){ force = Vector3.zero; }
      force *= .1f;
     // if( force.magnitude < .01f ){ force = Vector3.zero; }

    
      

      velocity += force;
      velocity *= dampening;

      if( velocity.magnitude > maxSpeed ){
        velocity = velocity.normalized * maxSpeed;
      }





      Vector3 m = transform.InverseTransformDirection(velocity);
      float turn = Mathf.Atan2(m.x, m.z) * m.magnitude;
      float forward = m.z;

      //turn += angleOffset;

      Rotate(forward , turn );
      animator.SetFloat("Turn", turn, 0.1f, Time.deltaTime);
    
      if( forward < forwardCutoff ){ forward = 0; }
      animator.SetFloat("Forward", forward*runMultiplier, 0.1f, Time.deltaTime);



      if( doTerrain ){
        float h = data.land.SampleHeight( transform.position );
       
        Vector3 normal = data.land.SampleNormal( transform.position );
        float d = Vector3.Dot( normal , Vector3.up );

        float h2 = data.land.SampleHeight( transform.position + transform.forward * .5f );

    //    print( (1-d) * 10);
        animator.SetFloat("Steepness", (1-d) * 10 );

        if( h2 > h ){
          animator.SetBool( "Uphill" , true);
        }else{
          animator.SetBool("Uphill", false);
        }


          transform.position = new Vector3( transform.position.x , h , transform.position.z);
      }

   //animator.Update(Time.deltaTime);

      deltaPos = transform.position - oPos;

      velocity = deltaPos;


      angleOffset *= .9f;
      forwardOffset *= .98f;

    }

  }


  public void SetMoveTarget( Vector3 p ){
    moveTarget = p;
    movingTowardsTarget = true;
  }

  public void SetMoveTarget( Transform p ){
    moveTargetTransform = p;
    moveTarget = p.position;
    movingTowardsTarget = true;
  }






  void Rotate(float f  , float t){

    // help the character turn faster (this is in addition to root rotation in the animation)
    float turnSpeed =  1000;//Mathf.Lerp(0, 360, f);
    transform.Rotate(0, t * turnSpeed * Time.deltaTime, 0);
  
  }




}
