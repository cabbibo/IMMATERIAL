using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCamera : Cycle
{
    public float xVel;
    public float yVel;
    public float zVel;
    public float xVal;
    public float yVal;
    public float zVal;
    public float scrollVel;

    public void WhileDown( Vector2 delta ){
      xVel += delta.x * .001f;
      yVel += -delta.y * .001f;
    }

    public override void WhileLiving( float v){
      scrollVel -= Input.mouseScrollDelta.y * .1f;

      xVal += xVel * .1f * zVal;
      yVal += yVel * .1f * zVal;
      zVal += scrollVel;
      zVal = Mathf.Clamp(zVal , 1 , 40);
      transform.position=new Vector3(xVal,yVal,zVal);

      xVel *= .9f;
      yVel *= .9f;
      scrollVel *= .9f;
    }
}
