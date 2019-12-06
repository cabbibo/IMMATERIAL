using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LinkVolumeToDown : MonoBehaviour
{
   public SampleSynth synth;
   public InputEvents events;

   void Update(){
      synth.volume = events.downTween2;
      synth.speed = 10/events.vel.magnitude;
   }
}
