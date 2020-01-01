using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextParticles : LifeForm{

  public Particles particles;
  public TextAnchor anchor;

  public Body body;

  public Vector3 emitterPosition;

  public Life setAnchor;
  public Life setGlyph;
  public Life setPage;

  public Life simulate;
  public Life transfer;
  public ClosestLife closest;
  private float pageStart;

  public float radius;

  public float scale;

  public int currentMax;
  public int currentMin;

  public SampleSynth synth;

  public void HideShowParticles(bool val){
    body.active = val;
  }

  public override void _Create(){
    SafeInsert(particles);
    SafeInsert(setAnchor);
    SafeInsert(setGlyph);
    SafeInsert(setPage);
    SafeInsert(simulate);
    SafeInsert(transfer);
    SafeInsert(body);
    SafeInsert(closest);
    DoCreate();
    currentMin = 0;
    currentMax = 0;

  }

  public override void OnBirthed(){
    float[] values = new float[ particles.count * particles.structSize ];
    particles.SetData( values );

    values = new float[ body.verts.count * body.verts.structSize  ];
    body.verts.SetData( values );

    closest.Set( particles );

  }

  public override void Bind(){

    setGlyph.BindForm("_TransferBuffer",body.verts);
    setGlyph.BindForm("_VertBuffer",particles);
    setGlyph.BindPrimaryForm("_AnchorBuffer",anchor);

    setAnchor.BindForm("_VertBuffer",particles);
    setAnchor.BindPrimaryForm("_AnchorBuffer",anchor);

    setPage.BindForm("_AnchorBuffer",anchor);
    setPage.BindForm("_VertBuffer",particles);

    simulate.BindPrimaryForm("_VertBuffer",particles);
    simulate.BindForm("_TransferBuffer",body.verts);

    transfer.BindPrimaryForm("_TransferBuffer",body.verts);
    transfer.BindForm("_VertBuffer",particles);
    



    setPage.BindVector3("_EmitPos" , () => this.emitterPosition );

    setGlyph.BindFloat("_Radius" , () => this.radius );//"radius",this);//.BindForm("_VertBuffer",particles);
    setGlyph.BindFloat("_Scale"  , () => this.scale  );//.BindForm("_VertBuffer",particles);


  /*  var scaleGetter = () -> { return this.scale; };
    setGlyph.BindAttribute("_Scale", () -> { return this.scale; }, float); */

    setGlyph.BindInt(  "_BaseID"  , () => this.currentMin );
    setAnchor.BindInt( "_BaseID"  , () => this.currentMin );
    setPage.BindInt(   "_BaseID"  , () => this.currentMin );
    simulate.BindInt(  "_BaseID"  , () => this.currentMin );
    transfer.BindInt(  "_BaseID"  , () => this.currentMin );

    setGlyph.BindInt(  "_TipID" , () => this.currentMax );
    setAnchor.BindInt( "_TipID" , () => this.currentMax );
    setPage.BindInt(   "_TipID" , () => this.currentMax );
    simulate.BindInt(  "_TipID" , () => this.currentMax );
    transfer.BindInt(  "_TipID" , () => this.currentMax );

setGlyph.BindFloat(  "_FontWidth" , () => Arial.width );
setGlyph.BindFloat(  "_FontHeight" , () => Arial.height );
setAnchor.BindFloat(  "_FontSize" , () => Arial.size );

setAnchor.BindFloat(  "_FontWidth" , () => Arial.width );
setAnchor.BindFloat(  "_FontHeight" , () => Arial.height );
setAnchor.BindFloat(  "_FontSize" , () => Arial.size );
   setAnchor.BindFloat("_Radius" , () => this.radius );//"radius",this);//.BindForm("_VertBuffer",particles);
    setAnchor.BindFloat("_Scale"  , () => this.scale  );//.BindForm("_VertBuffer",particles);




   /* simulate.BindAttribute("_Active","pageActive",story);
    
    simulate.BindAttribute("_Up","up",story.ursula);
    simulate.BindAttribute("_CameraForward","forward",touch);
    simulate.BindAttribute("_CameraUp","up",touch);
    simulate.BindAttribute("_PageAlive","pageAlive",story);
    simulate.BindAttribute("_UrsulaPos","position" , story.ursula );
    
    simulate.BindAttribute("_Fade","fade" , story );

    simulate.BindAttribute("_RayOrigin", "RayOrigin",touch);
    simulate.BindAttribute("_RayDirection", "RayDirection",touch);
    simulate.BindAttribute("_Scale","scale",this);*/




  }

  public void Set(TextAnchor t){

    currentMin = currentMax;
    currentMax = currentMin + t.count; 
    
    anchor = t;
    scale = t.scale;

    setGlyph.RebindPrimaryForm("_AnchorBuffer",anchor);
    setAnchor.RebindPrimaryForm("_AnchorBuffer",anchor);

    setAnchor.shader.SetVector("_FrameTopLeft", t.frame.topLeft );
    setAnchor.shader.SetFloat("_FrameWidth", t.frame.width );
    setAnchor.shader.SetFloat("_FrameHeight", t.frame.height );
    setAnchor.shader.SetVector("_FrameUp", t.frame.up );
    setAnchor.shader.SetVector("_FrameRight", t.frame.right );
    
    setPage.RebindPrimaryForm( "_AnchorBuffer" , anchor );

    setAnchor.YOLO();
    setGlyph.YOLO();
  }


  public void Release(){

    currentMin = currentMax;

  }


  public override void OnLive(){
    if( anchor != null ){ Set( anchor ); }
  }


  public void PageStart(){
    emitterPosition = data.soul.position;
    setPage.YOLO();
  }

  public void SpawnFromCamera(){

    //emitterPosition = data.GetComponent<Camera>().position;
    setPage.YOLO();

  }



  private int closestID;
  private int oClosestID;

  public float minPlayTime;
  public float maxDist;
  public bool doAudio;
    public override void WhileLiving(float f){

      oClosestID = closestID;
      closestID = (int)closest.closestID;
      float m =  closest.closest.magnitude / scale;

      
      if( closestID != oClosestID &&  doAudio && m < maxDist ){
        //synth.location = Random.Range(0 ,10.5f);

        float v = (maxDist - m ) / maxDist;

        minPlayTime = (1-v) * .3f;
        synth.pitch = Mathf.Clamp( v + data.inputEvents.vel.magnitude * .1f , .1f , 10.5f);
        synth.volume = .2f * v;
        synth.PlayGrain();
      }

    
    }


}
