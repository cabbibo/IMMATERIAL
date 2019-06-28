using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : Cycle
{


    public Frame frame;
    public TextAnchor text;
    public float lerpSpeed;

    public override void Create(){
      frame = GetComponent<Frame>();
      text = GetComponent<TextAnchor>();
      
      SafeInsert(frame);
      SafeInsert(text);
    }


    public void SetActivePage(){
      data.text.Set( text );
      data.text.PageStart();

      data.camControls.SetLerpTarget( transform ,lerpSpeed);
    
    }


    public override void Activate(){

     // data.text.Set( text );
     // data.text.PageStart();
    }




}
