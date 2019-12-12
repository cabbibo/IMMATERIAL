using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLooper : Cycle
{

  public AudioClip[] clips;

  public float[] clipVolumes;
  public float fadeOutSpeed;
  public float fadeInSpeed;

  public bool on;

  public override void Create(){
    if( clipVolumes == null || clipVolumes.Length != clips.Length ){
      clipVolumes = new float[clips.Length];
    }
  }

  public void FadeOut(){

      on = false;
  }

  public void FadeIn(){
      on = true;
    
  }

  public override void WhileLiving( float v ){
    for( int i = 0; i< clips.Length; i++){

      data.audio.globalLoopSources[i].clip = clips[i];
      if( on ){
        data.audio.globalLoopSources[i].volume = Mathf.Lerp(data.audio.globalLoopSources[i].volume,clipVolumes[i], .001f);
      }else{
        data.audio.globalLoopSources[i].volume = Mathf.Lerp(data.audio.globalLoopSources[i].volume,0, .1f);
      }

    }
  }

}
