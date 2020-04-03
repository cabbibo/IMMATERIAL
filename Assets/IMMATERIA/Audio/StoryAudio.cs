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

   public float[] audioInfo;
   public float[] oAudioInfo;

   public bool playing;

   public override void Create(){


    if( loopClips == null ){
      loopClips = new AudioClip[0];
    }

    if( audioInfo == null || oAudioInfo == null ){
      audioInfo = new float[ loopClips.Length ];
      oAudioInfo = new float[ loopClips.Length ];
    }
    
    if( loopClips.Length != audioInfo.Length || 
        loopClips.Length != oAudioInfo.Length ){

      audioInfo = new float[ loopClips.Length ];
      oAudioInfo = new float[ loopClips.Length ];
    }


   }

   public override void Activate(){


//    print("Activate");

    if( data.sound.loopSources.Length < loopClips.Length ){
      DebugThis("NOT ENOUGH SOURCES IN THE AUDIO LOOP SOURCES");
    }
    for( int i = 0; i < data.sound.loopSources.Length; i++ ){
      data.sound.loopSources[i].volume = 0;
    }
    for( int i = 0; i < loopClips.Length; i++ ){
      data.sound.loopSources[i].clip = loopClips[i];
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
    data.sound.FadeLoop(i , audioInfo[i] , setter.CS.transitionSpeed );
  }

  public void Enter(){

    
    playing = true;
    data.sound.globalLooper.FadeOut();

    data.sound.NewLoop();

    for( int i = 0; i < audioInfo.Length; i++ ){
      data.sound.FadeLoop(i , audioInfo[i] , data.sound.globalLooper.fadeOutSpeed );
    }
  }

  public void Exit(){

    playing = false;
    data.sound.globalLooper.FadeIn();
    for( int i = 0; i < audioInfo.Length; i++ ){
      data.sound.FadeLoop(i , 0 , data.sound.globalLooper.fadeInSpeed );
    }
  }


}
