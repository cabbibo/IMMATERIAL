using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : Cycle
{


    public Frame frame;
    public TextAnchor text;
    public float lerpSpeed;
    public Transform moveTarget;
    public Transform lerpTarget;
    public bool locked;
    public bool mustContinue;

    public float fade;
    public float baseHue;

    public MaterialPropertyBlock frameMPB;

    public EventTypes.BaseEvent  OnStartEnter;
    public EventTypes.BaseEvent  OnStartExit;

    public EventTypes.BaseEvent  OnEndEnter;
    public EventTypes.BaseEvent  OnEndExit;

    public EventTypes.FloatEvent  FadeIn;
    public EventTypes.FloatEvent  FadeOut;

    public StorySetter setter;
    public Story        story;

    public int[] audioInfo;

    public override void Create(){
      frame = GetComponent<Frame>();
      text = GetComponent<TextAnchor>();

      if( frameMPB == null ){ frameMPB = new MaterialPropertyBlock(); }
      
      SafeInsert(frame);
      SafeInsert(text);

        
    }

    public override void OnGestate(){
        
        // currently no setter on the book projection pages!
        if( setter != null ){
            
            if( audioInfo == null ){ audioInfo = new int[setter.audio.audioInfo.Length]; }

            if( audioInfo.Length != setter.audio.audioInfo.Length){
              audioInfo = new int[setter.audio.audioInfo.Length];
            }

        }
    }

    public void Lock(){

    }



}
