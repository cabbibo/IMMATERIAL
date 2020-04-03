using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loopy : MonoBehaviour
{
  public AudioSource source;
  public AudioClip clip;
  public float speed;
  public float speedRandomness;
  public float speedLerp;


  public AudioClip oClip;


  public void Update(){


    if( oClip != clip ){ source.clip = clip; source.Play(); }
    oClip= clip;
    float fSpeed = Mathf.Lerp( source.pitch , speed + speedRandomness*Random.Range(-.5f,.5f), speedLerp);
    source.pitch = fSpeed;
  }


}
