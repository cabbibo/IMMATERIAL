using UnityEngine;
using UnityEditor;

public class GlobalEditWindow : EditorWindow
{

    public GUISkin skin;

    // CONTROLS
    private bool showControls;
    public bool lockToGameCamera;
    public bool doInputEvents;

    // CONTROLS

    private bool showState;
    public bool startInPages;
    public bool startInStory;
    public int startPage;
    public int startStory;


    //LINKED OBJECTS
    private bool showObjects;
    public God god;
    public Data data;
    public InputEvents events;
    public State state;



    //FORM WORK
    private bool showForms;
    private bool showLives;

    private Vector2 formScroll;

    private int indent;
    private GUIContent label;
    private bool down;
    private GUISkin tSkin;
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Global Edit Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        GlobalEditWindow window = (GlobalEditWindow)EditorWindow.GetWindow(typeof(GlobalEditWindow));
        window.Show();
    }

     // Window has been selected
 void OnFocus() {
     // Remove delegate listener if it has previously
     // been assigned.
     SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
     // Add (or re-add) the delegate.
     SceneView.onSceneGUIDelegate += this.OnSceneGUI;
 }
 
 void OnDestroy() {
     // When the window is destroyed, remove the delegate
     // so that it will no longer do any drawing.
     SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
 }





    /*  


              
         ▒█████   ███▄    █      ██████  ▄████▄  ▓█████  ███▄    █ ▓█████      ▄████  █    ██  ██▓
        ▒██▒  ██▒ ██ ▀█   █    ▒██    ▒ ▒██▀ ▀█  ▓█   ▀  ██ ▀█   █ ▓█   ▀     ██▒ ▀█▒ ██  ▓██▒▓██▒
        ▒██░  ██▒▓██  ▀█ ██▒   ░ ▓██▄   ▒▓█    ▄ ▒███   ▓██  ▀█ ██▒▒███      ▒██░▄▄▄░▓██  ▒██░▒██▒
        ▒██   ██░▓██▒  ▐▌██▒     ▒   ██▒▒▓▓▄ ▄██▒▒▓█  ▄ ▓██▒  ▐▌██▒▒▓█  ▄    ░▓█  ██▓▓▓█  ░██░░██░
        ░ ████▓▒░▒██░   ▓██░   ▒██████▒▒▒ ▓███▀ ░░▒████▒▒██░   ▓██░░▒████▒   ░▒▓███▀▒▒▒█████▓ ░██░
        ░ ▒░▒░▒░ ░ ▒░   ▒ ▒    ▒ ▒▓▒ ▒ ░░ ░▒ ▒  ░░░ ▒░ ░░ ▒░   ▒ ▒ ░░ ▒░ ░    ░▒   ▒ ░▒▓▒ ▒ ▒ ░▓  
          ░ ▒ ▒░ ░ ░░   ░ ▒░   ░ ░▒  ░ ░  ░  ▒    ░ ░  ░░ ░░   ░ ▒░ ░ ░  ░     ░   ░ ░░▒░ ░ ░  ▒ ░
        ░ ░ ░ ▒     ░   ░ ░    ░  ░  ░  ░           ░      ░   ░ ░    ░      ░ ░   ░  ░░░ ░ ░  ▒ ░
            ░ ░           ░          ░  ░ ░         ░  ░         ░    ░  ░         ░    ░      ░ 


    */

    void OnSceneGUI(SceneView sceneView){

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
      

        if( lockToGameCamera ) LockCamera(sceneView);
        if( doInputEvents ) DoInputEvents(sceneView);
        
    

    }

    void LockCamera(SceneView sceneView){
      sceneView.pivot = events.MainCamera.transform.position;
      sceneView.rotation = events.MainCamera.transform.rotation;
      //((UnityEditor.SceneView)SceneView.sceneViews[0]).cameraDistance = 0;//events.MainCamera.transform.rotation;

      SceneView.CameraSettings cameraSettings = new SceneView.CameraSettings();
      cameraSettings.fieldOfView = events.MainCamera.GetComponent<Camera>().fieldOfView;
      cameraSettings.nearClip = events.MainCamera.GetComponent<Camera>().nearClipPlane;
      cameraSettings.farClip = events.MainCamera.GetComponent<Camera>().farClipPlane;
    }



    void DoInputEvents( SceneView sceneView ){
        // Camera.current.transform.position =events.MainCamera.transform.position;
      events.oP = events.p;
      events.oDown = events.Down;
    

      if( Event.current.type == EventType.MouseDown  ){
        down = true;
        events.Down = 1;
        Vector2 mousePos = Event.current.mousePosition * 1.5f;
        mousePos.y = sceneView.camera.pixelHeight - mousePos.y;
        events.p = mousePos;
         Ray ray =sceneView.camera.ScreenPointToRay(mousePos);

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

    }






    /*  



               ██▓ ███▄    █ ▄▄▄█████▓▓█████  ██▀███    █████▒▄▄▄       ▄████▄  ▓█████       ▓█████▄  ██▓  ██████  ██▓███   ██▓    ▄▄▄     ▓██   ██▓
        ▓██▒ ██ ▀█   █ ▓  ██▒ ▓▒▓█   ▀ ▓██ ▒ ██▒▓██   ▒▒████▄    ▒██▀ ▀█  ▓█   ▀       ▒██▀ ██▌▓██▒▒██    ▒ ▓██░  ██▒▓██▒   ▒████▄    ▒██  ██▒
        ▒██▒▓██  ▀█ ██▒▒ ▓██░ ▒░▒███   ▓██ ░▄█ ▒▒████ ░▒██  ▀█▄  ▒▓█    ▄ ▒███         ░██   █▌▒██▒░ ▓██▄   ▓██░ ██▓▒▒██░   ▒██  ▀█▄   ▒██ ██░
        ░██░▓██▒  ▐▌██▒░ ▓██▓ ░ ▒▓█  ▄ ▒██▀▀█▄  ░▓█▒  ░░██▄▄▄▄██ ▒▓▓▄ ▄██▒▒▓█  ▄       ░▓█▄   ▌░██░  ▒   ██▒▒██▄█▓▒ ▒▒██░   ░██▄▄▄▄██  ░ ▐██▓░
        ░██░▒██░   ▓██░  ▒██▒ ░ ░▒████▒░██▓ ▒██▒░▒█░    ▓█   ▓██▒▒ ▓███▀ ░░▒████▒      ░▒████▓ ░██░▒██████▒▒▒██▒ ░  ░░██████▒▓█   ▓██▒ ░ ██▒▓░
        ░▓  ░ ▒░   ▒ ▒   ▒ ░░   ░░ ▒░ ░░ ▒▓ ░▒▓░ ▒ ░    ▒▒   ▓▒█░░ ░▒ ▒  ░░░ ▒░ ░       ▒▒▓  ▒ ░▓  ▒ ▒▓▒ ▒ ░▒▓▒░ ░  ░░ ▒░▓  ░▒▒   ▓▒█░  ██▒▒▒ 
         ▒ ░░ ░░   ░ ▒░    ░     ░ ░  ░  ░▒ ░ ▒░ ░       ▒   ▒▒ ░  ░  ▒    ░ ░  ░       ░ ▒  ▒  ▒ ░░ ░▒  ░ ░░▒ ░     ░ ░ ▒  ░ ▒   ▒▒ ░▓██ ░▒░ 
         ▒ ░   ░   ░ ░   ░         ░     ░░   ░  ░ ░     ░   ▒   ░           ░          ░ ░  ░  ▒ ░░  ░  ░  ░░         ░ ░    ░   ▒   ▒ ▒ ░░  
         ░           ░             ░  ░   ░                  ░  ░░ ░         ░  ░         ░     ░        ░               ░  ░     ░  ░░ ░     
                                                                 ░                      ░                                             ░ ░    


    */

    void OnGUI()
    {

      GUILayout.TextField("Global Editor",skin.GetStyle("Label"));

      ShowControls();
      ShowState();
      ShowLinkedObjects();
      ShowAllForms();
 
    }
    void ShowControls(){

      showControls = EditorGUILayout.Foldout(showControls, "Controls");
      if( showControls ){

        StartGroup();

        lockToGameCamera = EditorGUILayout.Toggle ("Lock To Game Camera", lockToGameCamera);
        doInputEvents = EditorGUILayout.Toggle ("Do InputEvents", doInputEvents);

        EndGroup();

      }
    }

     void ShowState(){

      showState= EditorGUILayout.Foldout(showState, "showState");
      if( showState ){

        
        StartGroup();

        startInStory = EditorGUILayout.Toggle ("StartInStory", startInStory);
        startInPages = EditorGUILayout.Toggle ("StartInPages", startInPages);
        startStory = EditorGUILayout.IntField ("StartStory", startStory);
        startPage = EditorGUILayout.IntField ("StartPage", startPage);

        Debug.Log(startInStory);

        state.startStory = startStory;
        state.startPage = startPage;
        state.startInStory = startInStory;
        state.startInPages = startInPages;

        EndGroup();

      }
    }

    void ShowLinkedObjects(){
      showObjects = EditorGUILayout.Foldout(showObjects, "Linked Objects");
      if( showObjects ){
        StartGroup();

          label.text = "God";
          god = (God)EditorGUILayout.ObjectField(label,god,typeof(God),true);

          label.text = "Data";
          data = (Data)EditorGUILayout.ObjectField(label,data,typeof(Data),true);

          label.text = "Events";
          events = (InputEvents)EditorGUILayout.ObjectField(label,events,typeof(InputEvents),true);
   
          label.text = "State";
          state = (State)EditorGUILayout.ObjectField(label,state,typeof(State),true);
      
          label.text = "Skin";
          skin = (GUISkin)EditorGUILayout.ObjectField(label,skin,typeof(GUISkin),true);
  
        EndGroup();
      }
    }


    void ShowAllForms(){


      showForms = EditorGUILayout.Foldout(showForms, "All Forms");
      if( showForms ){

         formScroll = EditorGUILayout.BeginScrollView (formScroll,
                                                      false,
                                                      false);
 
        GUILayout.BeginVertical();
        foreach( Form f in god.forms ){
          GUILayout.BeginHorizontal();
          if(GUILayout.Button(""+f.gameObject.name , skin.GetStyle("GO_Button"))){
            EditorGUIUtility.PingObject( f.gameObject);
            Selection.activeObject = f.gameObject;
            //+ " : " + f.GetType() +  " : " + f.count * f.structSize
          }

          GUILayout.Label( " : " +  f.count * f.structSize, skin.GetStyle("CountVal") );
          GUILayout.Label( " : " + f.GetType() );
          GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();;
        EditorGUILayout.EndScrollView();

      }
    }

    void StartGroup(){
      EditorGUILayout.BeginVertical();
      indent = EditorGUI.indentLevel;
      EditorGUI.indentLevel = indent + 1;
    }

    void EndGroup(){
      EditorGUI.indentLevel = indent ;
      EditorGUILayout.EndVertical();
    }
}