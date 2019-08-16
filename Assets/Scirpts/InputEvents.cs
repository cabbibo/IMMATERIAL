using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class InputEvents: Cycle {

  public EventTypes.Vector2Event    OnSwipe;
  public EventTypes.FloatEvent      OnSwipeHorizontal;
  public EventTypes.BaseEvent       OnSwipeLeft;
  public EventTypes.BaseEvent       OnSwipeRight;
  public EventTypes.FloatEvent      OnSwipeVertical;
  public EventTypes.BaseEvent       OnSwipeUp;
  public EventTypes.BaseEvent       OnSwipeDown;
  public EventTypes.BaseEvent       OnTap;
  public EventTypes.BaseEvent       OnDown;
  public EventTypes.BaseEvent       OnUp;
  public EventTypes.RayEvent        WhileDown;
  public EventTypes.Vector2Event    WhileDownDelta;
  public EventTypes.Vector2Event    WhileDownDelta2;
  public EventTypes.BaseEvent       OnDebugTouch;

  public bool fakeSwipeLeft;
  public bool fakeSwipeRight;
  public bool fakeTapCenter;
  

  public Vector3 RayOrigin;
  public Vector3 RayDirection;
  
  public float Down;
  public float oDown;
  
  public float Down2;
  public float oDown2;

  public float JustDown;
  public float JustUp;
  public Vector2 startPos;
  public Vector2 endPos;

  public Ray ray;

  public float startTime;
  public float endTime;

  public float swipeSensitivity;
  public float tapSpeed;

  public float minSwipeTime;
  public float maxSwipeTime;
  // Use this for initialization

  public Vector2 p; 
  public Vector2 oP; 
  public Vector2 vel;

  public Vector2 p2;
  public Vector2 oP2;
  public Vector2 vel2;

  public int touchID = 0;

  public RaycastHit hit;
  public string hitTag;

  void Start(){}
  
   // Update is called once per frame
  public override void WhileLiving ( float v ){

    if( fakeSwipeLeft ){ 

      OnSwipeLeft.Invoke();
      fakeSwipeLeft = !fakeSwipeLeft;

    }

     if( fakeSwipeRight ){ 

      OnSwipeRight.Invoke();
      fakeSwipeRight = !fakeSwipeRight;

    }

    oP = p;
    oDown = Down;

    oP2 = p2;
    oDown2 = Down2;

    #if UNITY_EDITOR 
      MouseInput();
    #elif UNITY_STANDALONE
      MouseInput();
    #else
      TouchInput();
    #endif

    

    RayOrigin = Camera.main.ScreenToWorldPoint( new Vector3( p.x , p.y , Camera.main.nearClipPlane ) );
    RayDirection = (Camera.main.transform.position - RayOrigin).normalized;


    ray.origin = RayOrigin;
    ray.direction = -RayDirection;//.normalized;


    // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast( ray , out hit, Mathf.Infinity))
        {
          hitTag = hit.collider.tag;
        }else{
          hitTag = "Untagged";
        }




      if( Down == 1 && oDown == 0 ){
          JustDown = 1;
          touchID ++;
          startTime = Time.time;
          startPos = p;
          whileDown();
          onDown();
      }


      if( Down == 1 && oDown == 1 ){
        JustDown = 0;

        //if( Time.time - startTime > tapSpeed ){
          whileDown();
          whileDownDelta();
        //} 
      }

      if( Down2 == 1 && oDown2 == 1 ){
       // whileDown2();
        //if( Time.time - startTime > tapSpeed ){
          whileDownDelta2();
        //}
      }



      if( Down == 0 && oDown == 1 ){
        JustUp = 1;
        endTime = Time.time;
        endPos = p;
        onUp();
      }      

      if( Down == 0 && oDown == 0 ){
        JustDown = 0;
      }

      if( JustDown == 1 ){ oP = p; }
      vel = p - oP;

  }

  void MouseInput(){
      if (Input.GetMouseButton (0)) {
        Down = 1;
        p  =  Input.mousePosition;///Input.GetTouch(0).position;


      }else{
        Down = 0;
        oP = p;
      }

      if( Input.GetMouseButtonDown(0) &&   Input.GetKey("space") ){
        OnDebugTouch.Invoke();
      }

      if( Input.GetMouseButton(1)){
       
         p2 =   Input.mousePosition;
         Down2 = 1;
      }else{
        Down2 = 0;
        oP2 = oP;
      }
  }

  void TouchInput(){
      if (Input.touchCount == 1 ){
        Down = 1;
        p  =  Input.GetTouch(0).position;

        if( Input.touchCount > 2 ){
          if( Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began || Input.GetTouch(2).phase == TouchPhase.Began ){
            OnDebugTouch.Invoke();
          }
        }
      }else{
        Down = 0;
        oP = p;
      }

      if (Input.touchCount == 2 ){
        Down2 = 1;
        p2  =  Input.GetTouch(0).position;

        if( Input.touchCount > 2 ){
          if( Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began || Input.GetTouch(2).phase == TouchPhase.Began ){
            OnDebugTouch.Invoke();
          }
        }
      }else{
        Down2 = 0;
        oP2 = p2;
      }
  }

  void whileDown(){
    WhileDown.Invoke( ray );
  }

  void whileDownDelta(){
    WhileDownDelta.Invoke(p - oP);
  }

  void whileDownDelta2(){
    WhileDownDelta2.Invoke( p2 - oP2 );
  }


  void onDown(){
    OnDown.Invoke();

  }

  void onUp(){
    OnUp.Invoke();


    float difT = endTime - startTime;
    Vector2 difP = endPos - startPos;


    float ratio = .01f * difP.magnitude / difT;

  //  print( ratio );
//    print( difT );

    if( ratio > swipeSensitivity && difT > minSwipeTime && difT < maxSwipeTime ){
      
      OnSwipe.Invoke( difP ); 

      if( Mathf.Abs(difP.x) > Mathf.Abs(difP.y) ){
        OnSwipeHorizontal.Invoke(difP.x);
        
        if( difP.x < 0 ){
          OnSwipeLeft.Invoke();
        }else{
          OnSwipeRight.Invoke();
        }
      

      }else{
        OnSwipeVertical.Invoke(difP.y);
        
        if( difP.x < 0 ){
          OnSwipeUp.Invoke();
        }else{
          OnSwipeDown.Invoke();
        }
      } 


    }else{

      print(difP.magnitude);
      if( difT < tapSpeed && difP.magnitude < .1 ){
        OnTap.Invoke();
      }

    }


   //print( difT );
   //print( difP );
   //print( ratio );
  }


}