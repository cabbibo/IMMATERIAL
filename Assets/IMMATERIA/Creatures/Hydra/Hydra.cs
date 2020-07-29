using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : Cycle
{

  public float speed = 1;
  public float dampening = .9f;

   public TransformBuffer stalk;
   public TransformBuffer tips;

   public HairBasic stalkRope;
   public HairBasic tipRope;

   public Vector3[] vels;

   public Transform target;

   public Vector3 torsoVel;

   public override void Create(){

     //tentacles.numHairs = tips.transforms.Length + 1;
//

    vels = new Vector3[tips.transforms.Length / 2 ];
    
    SafeInsert( tips );
    SafeInsert( tipRope);


    SafeInsert( stalk );
    SafeInsert( stalkRope );
 

   
   }

   public override void Bind(){

   }

   public override void OnLive(){

    stalkRope.Set();
    tipRope.Set();
  
   }


   public override void WhileLiving(float v){

    if( active ){

    Vector3 force = new Vector3();

    Vector3 targetPos = stalk.transforms[0].position + Vector3.up * 6;

    force += (targetPos - stalk.transforms[1].position) * .001f * speed;
    force += (target.position - stalk.transforms[1].position) * .001f * speed;


      torsoVel += force;

      stalk.transforms[1].position += torsoVel;
      stalk.transforms[1].LookAt( target.position );
      stalk.transforms[1].transform.Rotate( 90, 0, 0 );

      torsoVel *= .9f * dampening;

      for( int i = 0; i< vels.Length; i++ ){

        force = new Vector3();


  
        float a = ((float)i / vels.Length) * (2 * Mathf.PI);

        float r = 2;

        Vector3 offset = Mathf.Sin( a ) * r * stalk.transforms[1].right +  Mathf.Cos( a ) * r * stalk.transforms[1].forward ;
        
        targetPos = stalk.transforms[1].position + offset;

        force += (targetPos - tips.transforms[i*2].position) * .001f  * (1.6f+Mathf.Sin( i * 100 + Time.time * .3f ));

        vels[i] += force;

        tips.transforms[i*2].position += vels[i];
        tips.transforms[i*2].LookAt( target.position );tips.transforms[i*2].transform.Rotate( -90, 0, 0 );

        vels[i] *= .9f;

      }

    }


   }



}
