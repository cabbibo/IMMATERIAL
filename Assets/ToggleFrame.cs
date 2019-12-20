using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFrame : Cycle
{

    public Framer framer;
    public Collider collider;

    public void Toggle(){
      //print("hhsm");
      //print( data.inputEvents.hit.collider );
      //print( collider );
      if( collider == null ){ collider = GetComponent<Collider>(); }
      if( data.inputEvents.hit.collider == collider ) framer.Toggle();
    }
}
