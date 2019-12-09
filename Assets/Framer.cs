using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Framer : Cycle
{
    public FrameBuffer[] frames;

    public int currentFrame;

    public SampleSynth instrument;


    public int oClosest;
    public int closest;

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


    public override void WhileLiving(float f){

      oClosest = closest;
      closest = (int)(frames[currentFrame].checkClosest.closestID);

      if( closest != oClosest && Time.time - instrument.lastTime > .1f  ){
        instrument.location = Random.Range(0 ,2.5f);
        instrument.pitch = data.inputEvents.vel.magnitude;
        instrument.volume = (.3f + 20 * frames[currentFrame].checkClosest.closest.magnitude);
        print( instrument.volume );
        instrument.PlayGrain();
      }


    }
}
