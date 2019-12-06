using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[ExecuteAlways]
public class StrokeSynth : MonoBehaviour
{
   
    public int[] steps;
  //public AudioClip clip;
  public AudioClip[] clips;
  public AudioPlayer player;
  public InputEvents events;
    public float distToPlay;
    public AudioMixer mixer;
    public string group;

    public Vector2 lastPosition;
    

    // Update is called once per frame
    void Update()
    {
      if( (events.p - lastPosition ).magnitude  > distToPlay){

        int step = steps[Random.Range(0, steps.Length)];
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        player.Play( clip , 12 , 1 * events.vel.magnitude, mixer,  group);
        lastPosition = events.p;
      }    
    }
}
