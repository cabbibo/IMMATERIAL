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



    public void Set(){


      data.audio.Play(epiphanyClip);
      

      _Activate();
      setting = true;

      startTime = Time.time;
      body.active = true;
      
      body.mpb.SetFloat("_StartTime", Time.time );

      circle.body.mpb.SetFloat("_StartTime" , Time.time );
      circle.body.mpb.SetFloat("_Setting" , 1 );

      for( int i = 0; i < rerenderers.Length; i++ ){

        rerenderers[i].mpb.SetFloat("_ID",i+1);
        rerenderers[i].active = true;

        rerenderers[i].mpb.SetFloat("_StartTime", Time.time );
        rerenderers[i].mpb.SetVector("_SetPosition", transform.position );
      }

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
      if( Time.time -startTime > 20 && setting ){
        circle._Deactivate();
        _Deactivate();
      }
    }


    public void UnSet(){
      print("UNSET");
      circle._Activate();
    }

    public override void Activate(){
      //print("ACTIVATOD");
    }

}
