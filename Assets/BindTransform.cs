﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindTransform : Binder
{

  public Matrix4x4 transformMatrix;
  public override void Bind(){
    toBind.BindAttribute("_Transform", "transformMatrix" , this );
  }


public override void WhileLiving( float v){
 transformMatrix = transform.localToWorldMatrix;
}
}
