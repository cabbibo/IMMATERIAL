using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : Cycle
{
    public MeshRenderer[] renderers;

    public float rotateSpeed;
    public float rotateSize;

    public bool inOut;
    float fadeValue;
    public void fadeIn(){
      inOut = true;
      data.tween.AddTween( 4, fadeMaterials);
    }


    public void fadeOut(){
      inOut = false;
      data.tween.AddTween( 4, fadeMaterials);
    }


    public void fadeMaterials(float v){

      if( !inOut ){ v = 1-v; }
      for( int i = 0; i < renderers.Length; i ++ ){
        renderers[i].sharedMaterial.SetFloat("_Fade" , v);
      }

    }

    public override void WhileLiving( float v ){
      //transform.localRotation = Quaternion.AngleAxis( Mathf.Sin( Time.time * rotateSpeed ) * rotateSize, Vector3.up );
    }


}
