using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeToFrame : MonoBehaviour
{
    public Frame frame;
    
    public void Scale(){

      transform.localScale = new Vector3( frame.width , frame.height , frame.height / 20 );
    }


}
