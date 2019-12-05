using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageAudioData : Cycle
{
  public bool[] clipsOn;
  public StoryAudio storyAudio;
  public override void Create(){
    if( clipsOn.Length != storyAudio.clipsOn.Length){
      clipsOn = new bool[storyAudio.clipsOn.Length];
    }
  }

  public void turnOn(){
    for( int i = 0; i < storyAudio.clipsOn.Length; i++ ){
      storyAudio.clipsOn[i] = clipsOn[i];
    }
  }
}
