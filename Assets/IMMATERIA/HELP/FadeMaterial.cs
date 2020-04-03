using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMaterial : Cycle
{
    public float fadeTime;
    public Renderer r;

    public override void Create(){
      if( r == null ){ r = GetComponent<Renderer>(); }
    }


     public void FadeIn(){
      Ac();
      r.sharedMaterial.SetFloat("_Fade" , 0);
      data.tween.AddTween( fadeTime , tweenIn );
    }

    public void FadeOut(){
      r.sharedMaterial.SetFloat("_Fade" , 1);
      data.tween.AddTween( fadeTime , tweenOut, De );
    }

    public void tweenIn(float v){
      r.sharedMaterial.SetFloat("_Fade" , v);
    }

    public void tweenOut(float v){
      r.sharedMaterial.SetFloat("_Fade" , 1-v);
    }

    public void De(){
      r.enabled = false;
    }

    public void Ac(){
      r.enabled = true;
    }
}
