using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindBones : Binder
{

  public Bones bones;

  public override void Bind(){
    toBind.BindForm("_SkeletonBuffer",bones);
  }


}
