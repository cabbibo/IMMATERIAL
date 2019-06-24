using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : Cycle
{


    public Frame frame;
    public TextAnchor text;

    public override void Create(){
      frame = GetComponent<Frame>();
      text = GetComponent<TextAnchor>();
      
      SafeInsert(frame);
      SafeInsert(text);
    }



    public override void Activate(){

     // data.text.Set( text );
     // data.text.PageStart();
    }




}
