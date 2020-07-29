using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class SpacePupTarget : MonoBehaviour
{
    Vector3 target;

    public Transform firstTransform;
    public bool shake;
    public float shakeTime;

    public void OnEnable(){
      SetTarget( firstTransform );
    }


    public void SetTarget(Transform t){

      
        print( "setting pos");
        print( t.name );
              target = t.position;
    }

    public void Update(){


      
      if( Time.time - shakeTime > 8 ){
        shake = false;
      }

      transform.position = Vector3.Lerp( transform.position , target , .03f );
      
      if( shake == true ){
        float v = (Time.time - shakeTime)/ 8;
        v= Mathf.Min( v * 8  , 1-v);
        float t = Time.time * .7f;
        transform.position += v*new Vector3( Mathf.Sin(t * 20) * 1 , Mathf.Sin(t * 15 + 20) * 1 , Mathf.Sin(t * 10 + 10) * 1 );
      }

    }

    public void SetShake(){
      shake = true;
      shakeTime = Time.time;
    }

}
