using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindGPUCollision : Binder
{

  public override void Bind(){
    data.BindGPUCollision( toBind );
  }

}
