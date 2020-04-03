using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arp : MonoBehaviour
{

  public AudioPlayer player;

  public AudioClip clip;
  public int[] steps;
  public float speed;
  public float speedRandomness;
  public int baseStep;

  public bool randomize;
  public bool up;

  public float lastTime;
  public int currentStep;
  public int currentStepID;

  public float randomOffset;

  public void Start(){
    currentStep = 0;
    currentStepID = 0;
    lastTime = 0;
    randomOffset = 0;
  }


  public void Update(){

    if( Time.time - lastTime  > speed + randomOffset ){
      lastTime = Time.time;

      randomOffset = speedRandomness * Random.Range( -.5f, .5f);

      if( randomize ){
        currentStepID = Random.Range( 0, steps.Length );
      }else{

        if( up ){
          currentStepID += 1;
        }else{
          currentStepID -= 1;
        }

        if( currentStepID < 0 ){ currentStepID += steps.Length; }
        currentStepID %= steps.Length;

      }

      currentStep = steps[currentStepID];
      player.Play( clip , currentStep + baseStep , 1 );


    } 

  }



}
