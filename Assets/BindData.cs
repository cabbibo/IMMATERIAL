using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindData : Binder
{
    
  public override void Bind() {

    print("hmmmm");
    data.BindCameraData(toBind);
    data.BindPlayerData(toBind);
  }
}
