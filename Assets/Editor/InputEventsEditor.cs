 using UnityEngine;
 using System.Collections;
 using System.Reflection;
 using UnityEditor;
 
[CustomEditor(typeof(InputEvents))]
public class InputEventsEditor : CycleEditor{


  public bool down;
   void OnSceneGUI(){

      InputEvents events = (InputEvents)target;
      HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
((UnityEditor.SceneView)SceneView.sceneViews[0]).pivot = events.MainCamera.transform.position;
((UnityEditor.SceneView)SceneView.sceneViews[0]).rotation = events.MainCamera.transform.rotation;
//((UnityEditor.SceneView)SceneView.sceneViews[0]).cameraDistance = 0;//events.MainCamera.transform.rotation;

SceneView.CameraSettings cameraSettings = new SceneView.CameraSettings();
cameraSettings.fieldOfView = events.MainCamera.GetComponent<Camera>().fieldOfView;
cameraSettings.nearClip = events.MainCamera.GetComponent<Camera>().nearClipPlane;
cameraSettings.farClip = events.MainCamera.GetComponent<Camera>().farClipPlane;
    

((UnityEditor.SceneView)SceneView.sceneViews[0]).cameraSettings = cameraSettings;


       // Camera.current.transform.position =events.MainCamera.transform.position;
      events.oP = events.p;
      events.oDown = events.Down;
    

      if( Event.current.type == EventType.MouseDown  ){
        down = true;
        events.Down = 1;
        Vector2 mousePos = Event.current.mousePosition * 1.5f;
        mousePos.y = Camera.current.pixelHeight - mousePos.y;
        events.p = mousePos;
         Ray ray =Camera.current.ScreenPointToRay(mousePos);

        events.RayOrigin = ray.origin;//Camera.current.ScreenToWorldPoint( new Vector3( events.p.x , events.p.y , Camera.current.nearClipPlane ) );
        events.RayDirection = ray.direction;//(Camera.current.transform.position - events.RayOrigin).normalized;
   
      }

      if( Event.current.type == EventType.MouseUp ){
        down = false;
        events.Down = 0;
        events.oP = events.p;
      }
      
      if (Event.current.type == EventType.MouseDrag && down ) {
        Vector2 mousePos = Event.current.mousePosition* 1.5f;
        mousePos.y = Camera.current.pixelHeight - mousePos.y;
        events.p = mousePos;
        Ray ray =Camera.current.ScreenPointToRay(mousePos);

        events.RayOrigin = ray.origin;//Camera.current.ScreenToWorldPoint( new Vector3( events.p.x , events.p.y , Camera.current.nearClipPlane ) );
        events.RayDirection = ray.direction;//(Camera.current.transform.position - events.RayOrigin).normalized;
      }


    events.ray.origin = events.RayOrigin;
    events.ray.direction = events.RayDirection;//.normalized;


    events.RO = events.ray.origin;
    events.RD = events.ray.direction;



    // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast( events.ray , out events.hit, Mathf.Infinity))
        {
          events.hitTag = events.hit.collider.tag;
        }else{
          events.hitTag = "Untagged";
        }




      if( events.Down == 1 && events.oDown == 0 ){
          events.JustDown = 1;
          events.touchID ++;
          events.startTime = Time.time;
          events.startPos = events.p;

          if( events.startPos.x <  (float)Screen.width * events.swipeInCutoff ){
            events.canEdgeSwipe = 1;
          }else if( events.startPos.x >  (float)Screen.width - (float)Screen.width * events.swipeInCutoff ){
            events.canEdgeSwipe = 2;
          }else{
            events.canEdgeSwipe = 0;
          }

          Shader.SetGlobalFloat("_CanEdgeSwipe" , events.canEdgeSwipe );

          events.whileDown();
          events.onDown();
      }


      if( events.Down == 1 && events.oDown == 1 ){
        events.JustDown = 0;

        //if( Time.time - startTime > tapSpeed ){
          events.whileDown();
          events.whileDownDelta();
        //} 
      }




      if( events.Down == 0 && events.oDown == 1 ){
        events.JustUp = 1;
        //Debug.Log("Just Up");
        events.endTime = Time.time;
        events.endPos = events.p;
        events.canEdgeSwipe = 0;


          Shader.SetGlobalFloat("_CanEdgeSwipe" , events.canEdgeSwipe );
        events.onUp();
      }      

      if( events.Down == 0 && events.oDown == 0 ){
        events.JustDown = 0;
      }

      if( events.JustDown == 1 ){ events.oP = events.p; }
      events.vel = events.p - events.oP;

        //print("mouse"); 
      
           
   }


    public override void OnInspectorGUI()
    {

      DrawDefaultInspector();

    }
     

}
