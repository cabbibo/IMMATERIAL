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
    public MaterialPropertyBlock frameMPB;
    public bool locked;
    public bool mustContinue;
    public float fade;

    public float baseHue;

    public EventTypes.BaseEvent  OnStartEnter;
    public EventTypes.BaseEvent  OnStartExit;

    public EventTypes.BaseEvent  OnEndEnter;
    public EventTypes.BaseEvent  OnEndExit;


    public EventTypes.FloatEvent  FadeIn;
    public EventTypes.FloatEvent  FadeOut;

    public override void Create(){
      frame = GetComponent<Frame>();
      text = GetComponent<TextAnchor>();

      if( frameMPB == null ){ frameMPB = new MaterialPropertyBlock(); }
      
      SafeInsert(frame);
      SafeInsert(text);
    }

    public void Lock(){

    }



}
