using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryAudio : Cycle
{

  
    public StorySetter setter;

    public AudioClip[] startClips;
    public AudioClip[] endClips;

    //public AudioClip baseLoop;
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


    print("Activate");

    if( data.audio.loopSources.Length < loopClips.Length ){
      DebugThis("NOT ENOUGH SOURCES IN THE AUDIO LOOP SOURCES");
    }
    for( int i = 0; i < data.audio.loopSources.Length; i++ ){
      data.audio.loopSources[i].volume = 0;
    }
    for( int i = 0; i < loopClips.Length; i++ ){
      data.audio.loopSources[i].clip = loopClips[i];
    }


    for( int i = 0; i < loopClips.Length; i++ ){
        FadeLoop(i);
        oAudioInfo[i] = audioInfo[i];
    } 

   }

   public override void Deactivate(){
    for( int i = 0; i < loopClips.Length; i++ ){
        audioInfo[i] = 0;
        FadeLoop(i);
        oAudioInfo[i] = audioInfo[i];
    } 

   }

   public override void WhileLiving( float v ){

    for( int i = 0; i < loopClips.Length; i++ ){

      if( audioInfo[i] != oAudioInfo[i] ){
        FadeLoop(i);
        oAudioInfo[i] = audioInfo[i];
      }
    } 
   }


  public void FadeLoop(int i){
    data.audio.FadeLoop(i , audioInfo[i] , setter.CS.transitionSpeed );
  }

  public void Enter(){

   // print("ENNTNT");

    data.audio.globalLooper.FadeOut();

    data.audio.NewLoop();

    for( int i = 0; i < audioInfo.Length; i++ ){
      data.audio.FadeLoop(i , audioInfo[i] , data.audio.globalLooper.fadeOutSpeed );
    }
  }

  public void Exit(){

    print("EXITS");
    data.audio.globalLooper.FadeIn();
    for( int i = 0; i < audioInfo.Length; i++ ){
      data.audio.FadeLoop(i , 0 , data.audio.globalLooper.fadeInSpeed );
    }
  }


}
