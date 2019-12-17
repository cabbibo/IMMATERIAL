using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Framer : Cycle
{

  public bool doAudio;
    public FrameBuffer[] frames;

    public int currentFrame;

    public SampleSynth instrument;

    public float minPlayTime;
    public float maxDist;


    public int oClosest;
    public int closest;

    public Collider frameCollider;

    public override void Create(){
      for( int i = 0; i < frames.Length; i++ ){
        SafeInsert( frames[i] );
      }
    }


    public void Set(Page page){
      frames[currentFrame].Release();
      //if( frameCollider != null ){ frameCollider.enabled = false; }
      if( frames[currentFrame].currentPage == page ){ frames[currentFrame].ImmediateDeath(); }
      currentFrame ++;
      currentFrame %= frames.Length;
      frames[currentFrame].Set(page);
      //frameCollider = page.frame.collider;
      //frameCollider.enabled = true;

    }

    public void Release(){

      frames[currentFrame].Release();
      currentFrame ++;
      currentFrame %= frames.Length;
      
    }


    public override void WhileLiving(float f){

      oClosest = closest;
      closest = (int)(frames[currentFrame].checkClosest.closestID);
      float m =  frames[currentFrame].checkClosest.closest.magnitude;
      if( closest != oClosest && Time.time - instrument.lastTime > minPlayTime + Random.Range(0,minPlayTime * .2f) && doAudio && m < maxDist ){
        //instrument.location = Random.Range(0 ,10.5f);

        float v = (maxDist - m ) / maxDist;

        minPlayTime = (1-v) * .3f;
        instrument.pitch = Mathf.Clamp( v + data.inputEvents.vel.magnitude * .1f , .1f , 10.5f);
        instrument.volume = .3f * v;
        //print( instrument.volume );
        instrument.PlayGrain();
      }


    }
}
