using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraParticlesBinder : Binder
{

    public Body body;
    public TransferLifeForm transfer; 

    public float _SpawnRingRadius;
    public float _DeathSpeed;
    public float _CurlNoiseSize;
    public float _CurlNoiseSpeed;
    public float _CurlNoiseStrength;
    public Vector3 _Gravity;

    public Texture2D _ColorMap;
    public Texture2D _ColorMap_DEFAULT;


    public float _Radius;   public float _Radius_DEFAULT;
    public float _HueStart; public float _HueStart_DEFAULT;
    public float _HueSize;  public float _HueSize_DEFAULT;



    
    public float _SpawnRingRadius_DEFAULT;
    public float _DeathSpeed_DEFAULT;
    public float _CurlNoiseSize_DEFAULT;
    public float _CurlNoiseStrength_DEFAULT;
    public float _CurlNoiseSpeed_DEFAULT;
    public Vector3 _Gravity_DEFAULT;

   public override void Bind(){
       toBind.BindFloat("_SpawnRingRadius",()=>_SpawnRingRadius);
       toBind.BindFloat("_DeathSpeed",()=>_DeathSpeed);
       toBind.BindFloat("_CurlNoiseSize",()=>_CurlNoiseSize);
       toBind.BindFloat("_CurlNoiseSpeed",()=>_CurlNoiseSpeed);
       toBind.BindFloat("_CurlNoiseStrength",()=>_CurlNoiseStrength);
       toBind.BindVector3("_Gravity",()=>_Gravity);
   }




    public void Set(ExtraParticlesSetter toSet){

        _SpawnRingRadius = toSet._SpawnRingRadius;
        _DeathSpeed = toSet._DeathSpeed;
        _CurlNoiseSize = toSet._CurlNoiseSize;
        _CurlNoiseStrength = toSet._CurlNoiseStrength;
        _CurlNoiseSpeed = toSet._CurlNoiseSpeed;
        _Gravity = toSet._Gravity;
        _Radius = toSet._Radius;
        _HueStart = toSet._HueStart;
        _HueSize = toSet._HueSize;
        _ColorMap = toSet._ColorMap;

        UpdateMaterials();
      
   }

    public void Reset(){
        _SpawnRingRadius    = _SpawnRingRadius_DEFAULT;
        _DeathSpeed         = _DeathSpeed_DEFAULT;
        _CurlNoiseSize      = _CurlNoiseSize_DEFAULT;
        _CurlNoiseStrength  = _CurlNoiseStrength_DEFAULT;
        _CurlNoiseSpeed     = _CurlNoiseSpeed_DEFAULT;
        _Gravity            = _Gravity_DEFAULT;
        _Radius             = _Radius_DEFAULT;
        _HueStart           = _HueStart_DEFAULT;
        _HueSize            = _HueSize_DEFAULT;
        _ColorMap           = _ColorMap_DEFAULT;

        UpdateMaterials();
    }


   public void UpdateMaterials(){
       transfer.radius = _Radius;
       body.mpb.SetFloat("_HueSize", _HueSize);
       body.mpb.SetFloat("_HueStart", _HueStart);
       body.mpb.SetTexture("_ColorMap",_ColorMap);
   }
}
