using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Framer : Cycle
{

  public bool doAudio;
    public FrameBuffer[] frames;


    public AudioMixer mixer;


    public int currentFrame;
    public int totalFrames;

    public SampleSynth instrument;

    public float minPlayTime;
    public float maxDist;


    public int oClosest;
    public int closest;

    public float distance;

    public float minFilterDist;
    public float maxFilterDist;
    public float minFilterVal;

    public Collider frameCollider;

    public override void Create(){
      for( int i = 0; i < frames.Length; i++ ){
        SafeInsert( frames[i] );
        frames[i].closeButton.GetComponent<ToggleFrame>().framer = this;
      }
      totalFrames = 0;

    }


    public void Set(Page page){

      frames[currentFrame].Release();

      //frames[currentFrame].closeButton.gameObject.GetComponent<FadeMaterial>().FadeOut();
      //if( frameCollider != null ){ frameCollider.enabled = false; }
      if( frames[currentFrame].currentPage == page ){ frames[currentFrame].ImmediateDeath(); }
      currentFrame ++;
      totalFrames ++;
      currentFrame %= frames.Length;
      frames[currentFrame].Set(page);
      distance = page.frame.distance;
      //frames[currentFrame].closeButton.gameObject.GetComponent<FadeMaterial>().FadeIn();
      //frameCollider = page.frame.collider;
      //frameCollider.enabled = true;
      
      frames[currentFrame].corners.body.mpb.SetInt("_TotalFrames", totalFrames );

    }

    public void Release(){

      frames[currentFrame].Release();
      currentFrame ++;
      currentFrame %= frames.Length;
      
    }




    public override void WhileLiving(float f){

      oClosest = closest;
      closest = (int)(frames[currentFrame].checkClosest.closestID);
      float m =  frames[currentFrame].checkClosest.closest.magnitude / distance;
  

      if( closest != oClosest && Time.time - instrument.lastTime > minPlayTime + Random.Range(0,minPlayTime * .2f) && doAudio && m < maxDist ){
        //instrument.location = Random.Range(0 ,10.5f);

  
        float v = (maxDist - m ) / maxDist;

        minPlayTime = (1-v) * .3f;
        instrument.pitch = Mathf.Clamp( v + data.inputEvents.vel.magnitude * .1f , .1f , 10.5f);
        instrument.volume = .3f * v;
        //print( instrument.volume );
        instrument.PlayGrain();
      }


      float v2 = Mathf.Clamp( ((m/distance) - minFilterDist) / ( maxFilterDist - minFilterDist) , 0 , 1);
      v2 = 1-v2;
      v2 *= 3000 - minFilterVal;
      v2 *= data.inputEvents.downTween2;


      //mixer.SetFloat("LowpassCutoff" , 22000 - v2);
      //mixer.SetFloat("HighpassCutoff" ,  v2);
      float f1;

      mixer.GetFloat("HighpassCutoff", out f1);
      //print(f1);

    }


    public override void OnBirthed(){
      shown = false;
      Toggle();
    }
    public bool shown = true;
    public void Toggle(){

      shown = !shown;

      if( !shown ){
        for( int i = 0; i < frames.Length; i++ ){
          frames[i].transfer.showBody = false;
          frames[i].corners.showBody = false;
          doAudio = false;
          data.state.frameShown = false;
          data.textParticles.body.active = false;
          data.textParticles.doAudio =false;
        }
      }else{
        for( int i = 0; i < frames.Length; i++ ){
          frames[i].transfer.showBody = true;
          frames[i].corners.showBody = true;
          data.state.frameShown = true;
          data.textParticles.body.active = true;
          doAudio = true;
          data.textParticles.doAudio =true;
        }
      }

    }

}
