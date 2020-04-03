using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindParticleBuffer : Binder
{
  public Particles particles;
  public string nameInBuffer;

  public override void Bind(){
    toBind.BindForm( nameInBuffer , particles );
  }
}
