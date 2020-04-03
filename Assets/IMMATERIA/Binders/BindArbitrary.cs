using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindArbitrary : Binder
{


    public GameObject go;

    public string script;
    public string property;

    public string nameInShader;


    private int storedInt;
    private float storedFloat;
    private Vector3 storedVec3;

    public override void Bind(){

      var component = go.GetComponent(script);
      var field = component.GetType().GetField(property);
      var val = field.GetValue(component);

      print( val );
      print( "" + val.GetType());//.TypeOf() );

      if( val.GetType() == typeof(int) ){
        toBind.BindInt(nameInShader, ()=> (int)val );
      }


      if( val.GetType() == typeof(float) ){
        toBind.BindFloat(nameInShader, ()=>(float)val);
      }

      if( val.GetType() == typeof(Vector3) ){
        toBind.BindVector3(nameInShader, ()=>(Vector3)val);
      }


    }


}
