using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryAudio : Cycle
{

  public AudioClip[] loopClips;

   public bool[] clipsOn;
   public bool[] oldClipsOn;

   public override void Create(){

    if( loopClips.Length != clipsOn.Length || clipsOn == null || oldClipsOn == null || loopClips.Length != oldClipsOn.Length ){
      clipsOn = new bool[ loopClips.Length ];
      oldClipsOn = new bool[ loopClips.Length ];
    }

   }

   public override void Activate(){

    if( data.audio.loopSources.Length < loopClips.Length ){
      DebugThis("NOT ENOUGH SOURCES IN THE AUDIO LOOP SOURCES");
    }

    for( int i = 0; i < loopClips.Length; i++ ){
      data.audio.loopSources[i].clip = loopClips[i];
    }

    data.audio.NewLoop();

   }

   public override void WhileLiving( float v ){

    for( int i = 0; i < loopClips.Length; i++ ){

      if( clipsOn[i] != oldClipsOn[i] ){
        FadeClip(i);
        oldClipsOn[i] = clipsOn[i];
      }
    } 
   }


   public  void FadeClip(int i){
    data.audio.FadeLoop(i,clipsOn[i]);
    //data.audio.loopSources[i].volume = clipsOn[i] ? 1 : 0;
   }



}
