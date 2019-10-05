using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Framer : Cycle
{
    public FrameBuffer[] frames;

    public int currentFrame;


    public override void Create(){
      for( int i = 0; i < frames.Length; i++ ){
        SafeInsert( frames[i] );
      }
    }


    public void Set(Page page){
      frames[currentFrame].Release();
      currentFrame ++;
      currentFrame %= frames.Length;
      frames[currentFrame].Set(page);
    }

}
