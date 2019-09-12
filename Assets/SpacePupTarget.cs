using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class SpacePupTarget : MonoBehaviour
{
    Vector3 target;

    public Transform firstTransform;

    public void OnEnable(){
      SetTarget( firstTransform );
    }


    public void SetTarget(Transform t){
      target = t.position;
    }

    public void Update(){
      transform.position = Vector3.Lerp( transform.position , target , .03f );
    }

}
