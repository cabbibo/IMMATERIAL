using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Cycle
{
  

    public Transform CameraHolder;

    public Vector3 targetPos;
    public Vector3 targetLookPos;

    public float angle;
    public bool swipeFollow;
    public Vector3 pos;
    public Vector3 lookPos;

    public float lerpSpeed;
    public Vector3 startLerpPosition;
    public Quaternion startLerpRotation;
    public float lerpStartTime;
    public Transform lerpTarget;

    public float startFollowSpeed;
    public bool following;
    public bool lerping;
    public float startFollowTime;

    public float followMoveSpeed;
    public float followRotateSpeed;

    public Transform followTarget;

    public float heightAbove;
    public float radius;

    public override void Create(){
      //lerping = false;
    }

    public override void WhileLiving( float v ){


      if( lerping ){
        DoLerp();
      }


      if( following ){
        DoFollow();
      }

    }

    public void ChangeAngle( Vector2 dif ){
      angle += dif.x * .001f;
    }

    public void  DoFollow(){

      Vector3 xy = Vector3.left * Mathf.Sin(angle) - Vector3.forward * Mathf.Cos(angle);

      if( !swipeFollow ){
       xy = -followTarget.forward;
      }

      Vector3 targetPosition = followTarget.position + Vector3.up* heightAbove + xy * radius;

      float h = data.land.SampleHeight( targetPosition );

      if( targetPosition.y < h + 2 ){ targetPosition = new Vector3( targetPosition.x , h + 2 , targetPosition.z);}
      Quaternion targetRotation = Quaternion.LookRotation( (followTarget.position - CameraHolder.position + Vector3.up) );

      float steal = Mathf.Clamp( (Time.time - startFollowTime) / startFollowSpeed , 0 , 1);

      float tDelta = Time.deltaTime * steal * steal;



      CameraHolder.position = Vector3.Lerp( CameraHolder.position , targetPosition , followMoveSpeed * tDelta );//subject.position  + Vector3.up * (height - hDelta * 3);
      CameraHolder.rotation = Quaternion.Slerp(CameraHolder.rotation, targetRotation ,  followRotateSpeed * tDelta );
  
    }

  
    public void DoLerp(){
        
        float v = Mathf.Clamp( (Time.time - lerpStartTime) / lerpSpeed ,0,1);
        
        // smoothing function;
        v = v * v * (3 - 2 * v);
        
        CameraHolder.position = Vector3.Lerp( startLerpPosition , lerpTarget.position, v);
        CameraHolder.rotation = Quaternion.Slerp( startLerpRotation , lerpTarget.rotation, v);
    
    }

  
    /*
  
      Setting this will mean that the camera will flow to a specific location from its
      starting location IN A SPECIFIC TIME!

    */
    public void SetLerpTarget( Transform t , float speed ){

      following = false;
      lerping = true;
      lerpTarget = t;
      lerpSpeed = speed;
      startLerpPosition = CameraHolder.position;
      startLerpRotation = CameraHolder.rotation;
      lerpStartTime = Time.time;

    }

    public void ReleaseLerp(){
      lerping = false;
    }

    public void SetNewLocationTarget( Vector3 v ){
      targetPos = v;
    }

    public void SetNewLookTarget( Vector3 v ){
      targetLookPos = v;
    }


    public void SetFollowTarget(Transform t , float speed ){

      following = true;
      lerping = false;
      followTarget = t;
      startFollowSpeed = speed;
      startFollowTime = Time.time;

      Vector3 t1 = CameraHolder.position - Vector3.up * CameraHolder.position.y; 
      Vector3 t2 = t.position - Vector3.up * t.position.y; 
      angle = Vector3.Angle(t1,t2);

    }

    public void SetFollowTarget(){
       following = true;
      lerping = false;
      
      startFollowTime = Time.time;

       Vector3 t1 = CameraHolder.position - Vector3.up * CameraHolder.position.y; 
      Vector3 t2 = followTarget.position - Vector3.up * followTarget.position.y; 
      angle = Vector3.Angle(t1,t2);
    }


}
