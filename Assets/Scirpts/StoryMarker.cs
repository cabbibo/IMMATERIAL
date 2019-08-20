﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryMarker : Cycle
{

    public TextMesh text;
    public int id;

    public override void Destroy(){
      data.inputEvents.OnTap.RemoveListener( CheckHit );
    }


    public override void Create(){

      data.inputEvents.OnTap.RemoveListener( CheckHit );
      data.inputEvents.OnTap.AddListener( CheckHit );

     
    }

    public void CheckHit(){
      
//      print(data.inputEvents.hitTag);
      if( data.inputEvents.hitTag == "StartNode" ){

        print("instorio");
        if( data.inputEvents.hit.collider == this.GetComponent<Collider>()){
          print("IM TOUCHED");

          data.journey.ConnectMonolith( id );
        }
      }
    
    }

    

}