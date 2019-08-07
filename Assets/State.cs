﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : Cycle
{

  public bool hasFallen;
  public bool hasPickedUpBook;

  public bool[] storiesVisited;

  public int currentStory;
  
  public bool inStory;
  public bool inBook;




  public override void Create(){
    if( storiesVisited.Length != data.journey.stories.Length ){
      storiesVisited = new bool[ data.journey.stories.Length ];
    }
  }
  

}