using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBookTouchAndDrag : Cycle
{
  

  public Monolith monolith;

  public SampleSynth synth;

  public override void Activate(){  
    monolith = GetComponent<Monolith>();
    data.inputEvents.WhileDownDelta.AddListener(WhileDown);
    
    data.inputEvents.OnDown.AddListener( OnDown );
    data.inputEvents.OnUp.AddListener( OnUp );
    data.extraParticles.GetComponent<ExtraParticlesBinder>().Set( GetComponent<ExtraParticlesSetter>() );
  }

  public override void Deactivate(){ 
    
    data.inputEvents.OnDown.RemoveListener( OnDown );
    data.inputEvents.OnUp.RemoveListener( OnUp );
    data.inputEvents.WhileDownDelta.RemoveListener(WhileDown); 
    
    data.extraParticles.GetComponent<ExtraParticlesBinder>().Reset();
    
  }



private Vector3 delta;
private Vector3 oPos;


public void OnDown(){
  if(data.inputEvents.hitTag == "Book" || data.inputEvents.hitTag == "Monolith"){
    synth.active = true;
    print("HMMM");
    
      data.extraParticles.EmitOn();
  }
}

public void OnUp(){
  synth.active = false;
  
      data.extraParticles.EmitOff();
}


  public void WhileDown( Vector2 d ){

    if(data.inputEvents.hitTag == "Book" || data.inputEvents.hitTag == "Monolith"){

      delta = data.inputEvents.hit.point - oPos;
      oPos = data.inputEvents.hit.point;

      monolith.mpb.SetVector( "_HitPoint" , data.inputEvents.hit.point );

      monolith.renderer.SetPropertyBlock( monolith.mpb);

      data.extraParticles.SetEmitterPosition( data.inputEvents.hit.point );
      
      data.extraParticles.EmitOn();
      synth.active = true;
    }else{
      data.extraParticles.EmitOff();
      synth.active = false;
    }
  }




  public void ExtraParticles(){

  }

  public void ReleaseExtraParticles(){

  }

}
