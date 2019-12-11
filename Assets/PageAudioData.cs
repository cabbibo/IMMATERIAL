using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageAudioData : Cycle
{
  public int[] audioInfo;
  public StoryAudio storyAudio;
  public override void Create(){
    if( audioInfo.Length != storyAudio.audioInfo.Length){
      audioInfo = new int[storyAudio.audioInfo.Length];
    }
  }

  public void turnOn(){
    for( int i = 0; i < storyAudio.audioInfo.Length; i++ ){
      storyAudio.audioInfo[i] = audioInfo[i];
    }
  }
}
