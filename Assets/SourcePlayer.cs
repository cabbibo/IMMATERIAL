using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourcePlayer : MonoBehaviour
{
      
  public float pitch;
  public AudioSource source;
  public float length;
  public float inOut;
  public float targetVolume;
  public float startTime;

  public void Update(){

    float t = Time.time - startTime;
    t = t / length;
    float currVol = Mathf.Clamp( Mathf.Min( t * 10 , (1-t) * 10 ) , 0 , 1);
    source.volume = currVol;
  }

}
