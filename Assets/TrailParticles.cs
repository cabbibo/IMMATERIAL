﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailParticles : Particles
{

  public Form head;
  public int particlesPerTrail;

  public override void SetCount(){ count = head.count * particlesPerTrail; }
}
