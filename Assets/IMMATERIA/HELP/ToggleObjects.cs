using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject[] meshes;
    public Cycle[] cycles;

    public void TurnOff(){
      for(int i = 0; i < meshes.Length; i++ ){
        meshes[i].SetActive(false);
      }

      for( int i = 0; i < cycles.Length; i++ ){
        cycles[i]._Deactivate();
      }


     // print("TURNING OFF");
    }

    public void TurnOn(){
      for(int i = 0; i < meshes.Length; i++ ){
        meshes[i].SetActive(true);
      }
    

      for( int i = 0; i < cycles.Length; i++ ){

        cycles[i]._Activate();
      }
    
      //print("TURNING ON");
    }
}
