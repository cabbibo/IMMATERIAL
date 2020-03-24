using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Frame : Cycle {

  public float distance;
  public float border;

  public float borderLeft;
  public float borderRight;
  public float borderTop;
  public float borderBottom;

  public bool constant;

  private float _ratio;

  //public LineRenderer borderLine;

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
   
    if( constant ){ SetFrame(); }
   
    
  }

  public void SetFrame(){

    _ratio = (float)Screen.width / (float)Screen.height;

    Camera cam = Camera.main;

    Vector3  tmpP = cam.transform.position;
    Quaternion tmpR = cam.transform.rotation;

    cam.transform.position = transform.position;
    cam.transform.rotation = transform.rotation;

    bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3( borderLeft ,_ratio *borderBottom,distance));  
    bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1- borderRight,_ratio *borderBottom ,distance));
    topLeft = Camera.main.ViewportToWorldPoint(new Vector3(borderLeft,1-_ratio * borderTop,distance));
    topRight = Camera.main.ViewportToWorldPoint(new Vector3(1-borderRight,1-_ratio * borderTop,distance));


    center = Camera.main.ViewportToWorldPoint(new Vector3( .5f , .5f , distance )); 

    normal = transform.forward;


    up = -(bottomLeft - topLeft).normalized;
    right = -(bottomLeft - bottomRight).normalized;


    width = (bottomLeft - bottomRight).magnitude;
    height = (bottomLeft - topLeft).magnitude;
    
    cam.transform.position = tmpP;
    cam.transform.rotation = tmpR;
  
    //print("setting collider");
    collider.transform.rotation = this.transform.rotation;
    collider.transform.position = center;
    collider.transform.localScale = new Vector3( (bottomLeft - bottomRight).magnitude , (bottomLeft - topLeft).magnitude , .001f);

  }


}