using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : Cycle
{
  public Transform toCopy;

  public override void WhileLiving( float v ){
    transform.position = toCopy.position;
  }

}
