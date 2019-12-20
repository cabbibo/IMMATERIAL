using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoLogo : Cycle
{
  
  public float fadeVal;

  public SeaOfStars[] stars;

  public Body[] textBodies;
  public TransferLifeForm[] gooeyText;

  public Renderer quad;


  public float activationTime;
  public float dieTime;
  public override void Activate(){
    activationTime = Time.time;

    quad.enabled = true;
  }

  public  void Die(){
    dieTime = Time.time;
  }

  public override void Deactivate(){

    quad.enabled = false;
  }
  

  public override void Create(){
    for(int i = 0; i < stars.Length; i++ ){
      SafeInsert(stars[i]);
    }
    for(int i = 0; i < textBodies.Length; i++ ){
      SafeInsert(textBodies[i]);
    }
    for(int i = 0; i < gooeyText.Length; i++ ){
      SafeInsert(gooeyText[i]);
    }
  }

}
