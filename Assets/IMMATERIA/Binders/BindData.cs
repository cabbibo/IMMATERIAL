using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindData : Binder
{
    
  public override void Bind() {
    data.BindCameraData(toBind);
    data.BindPlayerData(toBind);
    data.BindRayData(toBind);
  }
}
