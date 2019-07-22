using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journey : Cycle
{

    public Story[] stories;


   public override void Create(){

      for( int i = 0; i < stories.Length; i++ ){
        SafeInsert( stories[i] );
      }

   }


}
