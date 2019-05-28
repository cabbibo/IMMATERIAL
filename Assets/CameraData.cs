using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraData : MonoBehaviour
{
    
    Vector3 up;
    Vector3 right;
    Vector3 forward;
    Vector3 position;

    public void LateUpdate(){
      up = transform.up;
      right = transform.right;
      forward = transform.forward;
      position = transform.position;
    }


}
