using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryMarker : Cycle
{

    public TextMesh text;
    public int id;
    public string storyName;

    public override void Destroy(){
      data.inputEvents.OnTap.RemoveListener( CheckHit );
    }


    public override void Create(){

      data.inputEvents.OnTap.RemoveListener( CheckHit );
      data.inputEvents.OnTap.AddListener( CheckHit );

     
    }

    public void CheckHit(){
      
//      print(data.inputEvents.hitTag);
      if( data.inputEvents.hitTag == "StartNode" && data.inputEvents.hit.collider == this.GetComponent<Collider>() ){
        data.state.ConnectMonolith( id );
        print( storyName );
      }
    
    }

    

}
