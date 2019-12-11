using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryAudio : Cycle
{

  


   public AudioClip[] loopClips;

   public int[] audioInfo;
   public int[] oAudioInfo;

   public override void Create(){

    if( loopClips == null ){
      loopClips = new AudioClip[0];
    }

    if( audioInfo == null || oAudioInfo == null ){
      audioInfo = new int[ loopClips.Length ];
      oAudioInfo = new int[ loopClips.Length ];
    }
    
    if( loopClips.Length != audioInfo.Length || 
        loopClips.Length != oAudioInfo.Length ){

      audioInfo = new int[ loopClips.Length ];
      oAudioInfo = new int[ loopClips.Length ];
    }


   }

   public override void Activate(){

    if( data.audio.loopSources.Length < loopClips.Length ){
      DebugThis("NOT ENOUGH SOURCES IN THE AUDIO LOOP SOURCES");
    }

    print("ACTIVADO");
    for( int i = 0; i < loopClips.Length; i++ ){
      data.audio.loopSources[i].clip = loopClips[i];
    }

    data.audio.NewLoop();

   }

   public override void WhileLiving( float v ){

    for( int i = 0; i < loopClips.Length; i++ ){

      if( audioInfo[i] != oAudioInfo[i] ){
        print("NEW DATZA");
        FadeLoop(i);
        oAudioInfo[i] = audioInfo[i];
      }
    } 
   }


  public  void FadeLoop(int i){
    data.audio.FadeLoop(i,audioInfo[i]);
  }


}
