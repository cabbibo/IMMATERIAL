using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EpiphanyRing : Cycle
{
    public AudioClip epiphanyClip;

    public SceneCircleVerts verts;
    public SceneCircleTris tris;

    public Body body;

    //public Life life;

    public ReRender[] rerenderers;

    public CircleOnTerrain circle;


    public Renderer scanRenderer;




    public float startTime;

    public bool setting;

    public override void Create(){
      setting = false;

      //_Activate();
      SafeInsert(body);
      //SafeInsert(life);

      for( int i = 0; i < rerenderers.Length; i++ ){
        SafeInsert( rerenderers[i] );
      }

    }



  public void SetGlowCircle(){

      circle.body.mpb.SetFloat("_StartTime" , Time.time );
      circle.body.mpb.SetFloat("_Setting" , 1 );

      circle.body.mpb.SetFloat("_ScanTime" ,0 );


      print( circle.body.mpb.GetFloat("_ScanTime"));
  }

    public void Set(){

      print("SETTING");

      data.sound.Play(epiphanyClip,1.0f,.3f);
      

      _Activate();
      setting = true;

      startTime = Time.time;

      SetGlowCircle();

      for( int i = 0; i < rerenderers.Length; i++ ){

        rerenderers[i].mpb.SetFloat("_ID",i+1);
        rerenderers[i].active = true;

        rerenderers[i].mpb.SetFloat("_StartTime", Time.time );
        rerenderers[i].mpb.SetVector("_SetPosition", transform.position );
      }


      scanRenderer.enabled = true;
      data.sourceParticles.EmitOn();
      print("ON");

    }

    public override void Bind(){
      
      //life.BindFloat("_StartTime", () => this.startTime );
      //life.BindVector3("_SetLocation", () => transform.position );

      body.mpb.SetVector("_SetPosition", transform.position );
      body.mpb.SetFloat("_StartTime", 10000*Time.time  );
      body.active = false;

      for( int i = 0; i < rerenderers.Length; i++ ){
        rerenderers[i].mpb.SetFloat("_ID",i+1);

        rerenderers[i].active = false;
        rerenderers[i].mpb.SetFloat("_StartTime", 10000*Time.time );
        rerenderers[i].mpb.SetVector("_SetPosition", transform.position );
      }
    } 

    public override void WhileLiving( float v ){

      float scanTime = Time.time - startTime;

      scanTime /= 10;
      scanRenderer.sharedMaterial.SetFloat("_ScanTime", scanTime);
      circle.body.mpb.SetFloat("_ScanTime" ,scanTime );

      if( scanTime > .1 && setting ){

        data.sourceParticles.EmitOff();
      }
      if( scanTime > 1 && setting ){

        print("HERE IT IS");
        circle._Deactivate();
        _Deactivate();
        scanRenderer.enabled = false;
      }
    }


    public void UnSet(){
      //circle._Activate();
      _Deactivate();

        scanRenderer.enabled = false;
        data.sourceParticles.EmitOff();
    }

    public override void Activate(){
      //print("ACTIVATOD");
    }

}
