using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindTerrainData : Binder
{
  
  public override void Bind() {
    data.land.BindData(toBind);
  }
}
