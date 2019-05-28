using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintLife : Life{
  public override void GetNumGroups(){
    numGroups = Mathf.Max( (primaryForm.count/2+((int)numThreads-1))/(int)numThreads,1);
  }
}


