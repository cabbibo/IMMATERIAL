﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Frame : Cycle {

  public float distance;
  public float border;

  private float _ratio;

  public LineRenderer borderLine;

  public Vector3 bottomLeft;
  public Vector3 bottomRight;
  public Vector3 topLeft;
  public Vector3 topRight;
  public Vector3 center;

  public float width;
  public float height;
  public Vector3 normal;
  public Vector3 up;
  public Vector3 right;

  public Collider collider;


  // Use this for initialization
  public override void Create() {
///    this.gameObject.tag = "Frame";
    borderLine = GetComponent<LineRenderer>();
    border = .02f;

    if( collider != null ){
      DestroyImmediate( collider.gameObject);
    }

    GameObject cubeInfo = (GameObject)Resources.Load("Prefabs/CubeCollider", typeof(GameObject));
      
    GameObject cube = Instantiate(cubeInfo);
    cube.tag = "Frame";
    collider = cube.GetComponent<Collider>();
    collider.transform.parent = this.transform;

    SetFrame();
  }
  
  // Update is called once per frame
  public override void WhileLiving(float v) {
    SetFrame();
    collider.transform.rotation = this.transform.rotation;
    collider.transform.position = center;
    collider.transform.localScale = new Vector3( (bottomLeft - bottomRight).magnitude , (bottomLeft - topLeft).magnitude , .001f);
  }

  public void SetFrame(){

    _ratio = (float)Screen.width / (float)Screen.height;

    Camera cam = Camera.main;

    Vector3  tmpP = cam.transform.position;
    Quaternion tmpR = cam.transform.rotation;

    cam.transform.position = transform.position;
    cam.transform.rotation = transform.rotation;

    bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3( border ,_ratio *border,distance));  
    bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1- border,_ratio *border,distance));
    topLeft = Camera.main.ViewportToWorldPoint(new Vector3(border,1-_ratio * border,distance));
    topRight = Camera.main.ViewportToWorldPoint(new Vector3(1-border,1-_ratio * border,distance));


    center = Camera.main.ViewportToWorldPoint(new Vector3( .5f , .5f , distance )); 

    //print("setting");

    borderLine.SetPosition( 0 , bottomLeft );
    borderLine.SetPosition( 1 , bottomRight );
    borderLine.SetPosition( 2 , topRight );
    borderLine.SetPosition( 3 , topLeft );


    //borderLine.SetPosition( 4 , bottomLeft );

    //transform.localPosition = new Vector3( 0, 0, distance);
    // /transform.localRotation = Quaternion.identity;
    normal = transform.forward;


    up = -(bottomLeft - topLeft).normalized;
    right = -(bottomLeft - bottomRight).normalized;


   // transform.localScale = new Vector3( (bottomLeft - bottomRight).magnitude , (bottomLeft - topLeft).magnitude , .1f );

    width = (bottomLeft - bottomRight).magnitude;
    height = (bottomLeft - topLeft).magnitude;
    

    cam.transform.position = tmpP;
    cam.transform.rotation = tmpR;

  }


}