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

}
