using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocation : Cycle
{

    public GuideParticles particles;
    public Transform location;


    public override void Create(){
      particles.SetEmitterPosition( location.position );
    }
   



}
