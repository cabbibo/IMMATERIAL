using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindHairBuffer : Binder
{
  public Hair hair;
  public override void Bind(){

    toBind.BindForm( "_HairBuffer" , hair );
    toBind.BindInt( "_NumVertsPerHair" , () => hair.numVertsPerHair );
  }
}
