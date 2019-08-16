using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindTransformBuffer : Binder
{
  public TransformBuffer transformBuffer;
  
  public override void Bind(){
    toBind.BindForm("_TransformBuffer", transformBuffer );
  }
}
