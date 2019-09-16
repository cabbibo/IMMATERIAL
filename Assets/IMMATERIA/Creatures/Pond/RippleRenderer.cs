using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleRenderer : Cycle {


  public  Material reactionDiffusionMaterial;
  public Renderer render;

  public Body body;

  public Vector2 resolution;
  public int flip;

  [SerializeField]
  float texelMultiplier = 0.75f;

  [SerializeField]
  int passesPerFrame = 5;


  private RenderTexture t1;
  private RenderTexture t2;

  public override void Create() {

    SafeInsert(body);
    InitRenderTextures();
  }

  public override void Destroy() {
    RenderTexture.DestroyImmediate(t1);
    RenderTexture.DestroyImmediate(t2);
  }

  public override void WhileLiving( float v ) {
    
    //RenderTexture tempTex = RenderTexture.GetTemporary((int)resolution.x, (int)resolution.y);

    reactionDiffusionMaterial.SetVector("_Resolution", resolution);
    reactionDiffusionMaterial.SetVector("_TexelSize", new Vector4(1f / resolution.x, 1f / resolution.y, 0f, 0f) * texelMultiplier);
    reactionDiffusionMaterial.SetMatrix("_Transform", transform.localToWorldMatrix);

    if( data.inputEvents.hitTag == "Pond" && data.inputEvents.Down ==1 ){
      reactionDiffusionMaterial.SetVector("_HitUV",data.inputEvents.hit.textureCoord );
      reactionDiffusionMaterial.SetInt("_Down", 1 );
    }else{
      reactionDiffusionMaterial.SetInt("_Down", 0 );
    }

    for( int i = 0; i < passesPerFrame; i++ ){
      Flip();
    }


  }


  public void Flip(){

    flip ++;
    flip %= 2;
    if( flip == 0){
      reactionDiffusionMaterial.SetTexture("_LastTex", t1);
      Graphics.Blit(t1, t2, reactionDiffusionMaterial);
      body.mpb.SetTexture("_MainTex", t2);
      render.sharedMaterial.SetTexture("_MainTex", t2);
    }else{
      reactionDiffusionMaterial.SetTexture("_LastTex", t2);
      Graphics.Blit(t2, t1, reactionDiffusionMaterial);
      body.mpb.SetTexture("_MainTex", t1);
      render.sharedMaterial.SetTexture("_MainTex", t1);
    }

  }

  void InitRenderTextures() {
    t1 = new RenderTexture((int)resolution.x, (int)resolution.y, 0, RenderTextureFormat.ARGBFloat);
    t2 = new RenderTexture((int)resolution.x, (int)resolution.y, 0, RenderTextureFormat.ARGBFloat);
  }
}
