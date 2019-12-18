using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
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
      target = t.position;
    }

    public void Update(){

      if( Time.time - shakeTime > 4 ){
        shake = false;
      }

      //transform.position = Vector3.Lerp( transform.position , target , .03f );
      
      if( shake == true ){
        float v = (Time.time - shakeTime)/ 4;
        v= Mathf.Min( v * 4  , 1-v);
        transform.position += v*new Vector3( Mathf.Sin(Time.time * 20) * .4f , Mathf.Sin(Time.time * 15 + 20) * .4f , Mathf.Sin(Time.time * 10 + 10) * .4f );
      }

    }

    public void SetShake(){
      shake = true;
      shakeTime = Time.time;
    }

}
