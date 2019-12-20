using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoLogo : Cycle
{
  
  public float fadeVal;

  public SeaOfStars[] stars;

  public Body[] textBodies;
  public TransferLifeForm[] gooeyText;

  public gpuNextTouchInstrument instrument;

  public Renderer quad;

  public bool growing;

  public float liveSpeed;
  public float dieSpeed;


  public float activationTime;
  public float dieTime;

  public float fade;
  public override void Activate(){
    growing = true;
    activationTime = Time.time;
    quad.enabled = true;
    data.gpuCollisions.BindNewForm( gooeyText[0].verts );
  }

  public  void Die(){
    growing = false;
  }

  public override void Deactivate(){

    quad.enabled = false;
    data.gpuCollisions.Unbind();
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

  public override void Bind(){
    for(int i = 0; i < stars.Length; i++ ){
      stars[i].life.BindFloat("_Fade", () => fade);
      stars[i].body.transfer.BindFloat("_Fade", () => fade);
    }

     for(int i = 0; i < gooeyText.Length; i++ ){
      gooeyText[i].transfer.BindFloat("_Fade", () => fade);
    }
  }

  public override void WhileLiving( float v ){

    fade = Time.time - activationTime;

    if( growing ) fade /= liveSpeed;
    if( !growing ) fade /= dieSpeed;

    fade = Mathf.Clamp( fade , 0 , 1 );

    if( !growing ) fade = 1-fade;


    for(int i = 0; i < stars.Length; i++ ){
      stars[i].body.body.mpb.SetFloat("_Fade",fade);
    }
    for(int i = 0; i < gooeyText.Length; i++ ){
      gooeyText[i].body.mpb.SetFloat("_Fade",fade);
    }

    quad.sharedMaterial.SetFloat("_Fade",fade);


  }

}
