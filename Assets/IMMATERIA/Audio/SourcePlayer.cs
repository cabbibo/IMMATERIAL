using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[ExecuteAlways]
public class SourcePlayer : MonoBehaviour
{
      
  public float pitch;
  public AudioSource source;
  public float length;
  public float inOut;
  public float targetVolume;
  public float startTime;

  public bool playing;

    void Update(){
    if( playing ){
      float t = (Time.time - startTime);
      t = t / length;
      if( t > 1 ){ playing = false; }
    //  print( t );
      float currVol = Mathf.Clamp( Mathf.Min( t * 20 , (1-t) * 5 ) , 0 , 1);
      //print( currVol );
      source.volume = currVol;
    }

  }


  public void Play( AudioClip clip ){

    startTime = Time.time;
    source.volume = 0;
    playing = true;
    length = clip.length;
    source.clip = clip;
    source.Play();

  }

  public void Play( AudioClip clip , float start, float len ){

    startTime = Time.time;
    source.volume = 0;
    playing = true;
    length = len;
    source.time = start;
    source.clip = clip;
    source.Play();
    source.SetScheduledEndTime( AudioSettings.dspTime + length );
    
  }


  public void Play( AudioClip clip , float start, float len , AudioMixer mixer, string group){
     

    startTime = Time.time;
    source.volume = 0;
    playing = true;
    length = len;
    source.time = start;
    source.clip = clip;

    source.outputAudioMixerGroup = mixer.FindMatchingGroups(group)[0];
    source.Play();
    source.SetScheduledEndTime( AudioSettings.dspTime + length );

    
  }


}
