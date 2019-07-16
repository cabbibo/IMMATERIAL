using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Cycle
{

    public Frame frame;
    public override void Create(){
      frame = GetComponent<Frame>();  
      SafeInsert(frame);
    }
}
