using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionDiffusionRenderer : Cycle
{
    [HideInInspector]
  public Texture Result;

  [SerializeField]
  Material reactionDiffusionMaterial;

  [SerializeField]
  Material destMaterial;

  [SerializeField]
  Vector4 resolution;

  [SerializeField]
  float feedRate = 0.05f;

  [SerializeField]
  float deathRate = 0.062f;

  [SerializeField]
  public float diffusionRateA = 1.5f;

  [SerializeField]
  float diffusionRateB = 0.5f;

  [SerializeField]
  float timeMultiplier = 1.0f;

  [SerializeField]
  float texelMultiplier = 0.75f;

  [SerializeField]
  int passesPerFrame = 50;

  void Start() {
    Cursor.visible = false;
    Application.runInBackground = true;
    InitRenderTextures();
  }

  void OnDestroy() {
    RenderTexture.Destroy(Result);
  }

  void Update () {
    
    RenderTexture tempTex = RenderTexture.GetTemporary((int)resolution.x, (int)resolution.y);

    reactionDiffusionMaterial.SetFloat("_FeedRate", feedRate);
    reactionDiffusionMaterial.SetFloat("_KillRate", deathRate);
    reactionDiffusionMaterial.SetFloat("_DiffusionRateA", diffusionRateA);
    reactionDiffusionMaterial.SetFloat("_DiffusionRateB", diffusionRateB);
    reactionDiffusionMaterial.SetFloat("_Multiplier", timeMultiplier);
    reactionDiffusionMaterial.SetVector("_Resolution", resolution);
    reactionDiffusionMaterial.SetVector("_TexelSize", new Vector4(1f / resolution.x, 1f / resolution.y, 0f, 0f) * texelMultiplier);

    for (int i = 0; i < passesPerFrame; i++) {
      Graphics.Blit(Result, tempTex, reactionDiffusionMaterial);
      reactionDiffusionMaterial.SetTexture("_LastTex", tempTex);
    }

    destMaterial.SetTexture("_MainTex", tempTex);
    RenderTexture.ReleaseTemporary(tempTex);
  }

  void InitRenderTextures() {
    Result = new RenderTexture((int)resolution.x, (int)resolution.y, 0, RenderTextureFormat.Default);
  }
}
