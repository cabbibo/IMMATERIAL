using UnityEngine;
using UnityEditor;

public class GlobalEditWindow : EditorWindow
{



  public void Update()
 {
     // This is necessary to make the framerate normal for the editor window.
     Repaint();
 }

    public GUISkin skin;

    // CONTROLS
    private bool showControls;
    public bool lockToGameCamera;
    public bool doInputEvents;
    public bool godPause;

    // CONTROLS

    private bool showState;
    public bool startInPages;
    public bool startInStory;
    public int startSetter;
    public int startStory;
    public int startPage;
    public bool fast;


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

        Debug.Log( GameObject.Find("God") );
       //GameObject g = GameObject.Find("God");

       //god = g.GetComponent<God>();
       //data = g.GetComponent<Data>();
       //events = data.inputEvents;
       //state = data.state;

       //skin = (GUISkin)Resources.Load("GlobalEditSkin");
       
       // Debug.Log(skin);

    }



    void Assign(){

       skin = (GUISkin)Resources.Load("GlobalEditSkin");
      if( god == null ){
         GameObject g = GameObject.Find("God");

       god = g.GetComponent<God>();
       data = g.GetComponent<Data>();
       events = data.inputEvents;
       state = data.state;
       skin = (GUISkin)Resources.Load("GlobalEditSkin");
      }
    }

     // Window has been selected
 void OnFocus() {
     // Remove delegate listener if it has previously
     // been assigned.
     SceneView.duringSceneGui -= this.OnSceneGUI;
     // Add (or re-add) the delegate.
     SceneView.duringSceneGui += this.OnSceneGUI;
 }
 
 void OnDestroy() {
     // When the window is destroyed, remove the delegate
     // so that it will no longer do any drawing.
     SceneView.duringSceneGui -= this.OnSceneGUI;
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
      Assign();
      
        if(god != null ){

        if( lockToGameCamera ) LockCamera(sceneView);
        if( doInputEvents ){

          HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
          DoInputEvents(sceneView);
        }
      }
        
    

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

      Assign();

        
      GUILayout.TextField("Global Editor",skin.GetStyle("Label"));
      
      ShowLinkedObjects();

      if( god != null ){
        ShowControls();
        ShowState();
        ShowAllForms();
        ShowSaveButtons();
      }
 
    }
    void ShowControls(){

      showControls = EditorGUILayout.Foldout(showControls, "Controls");
      if( showControls ){

        StartGroup();

        lockToGameCamera = EditorGUILayout.Toggle ("Lock To Game Camera", lockToGameCamera);
        doInputEvents = EditorGUILayout.Toggle ("Do InputEvents", doInputEvents);
        godPause = EditorGUILayout.Toggle ("God Pause", godPause);
        god.godPause = godPause;
        EndGroup();

      }
    }

     void ShowState(){

      showState= EditorGUILayout.Foldout(showState, "showState");
      if( showState ){

        
        StartGroup();

        startInStory = EditorGUILayout.Toggle ("StartInStory", startInStory);
        startInPages = EditorGUILayout.Toggle ("StartInPages", startInPages);

        startSetter = EditorGUILayout.IntField ("StartSetter", startSetter);
        startStory = EditorGUILayout.IntField ("StartStory", startStory);
        startPage = EditorGUILayout.IntField ("StartPage", startPage);
        
        fast = EditorGUILayout.Toggle ("Fast", fast);

        state.startStory = startStory;
        state.startSetter = startSetter;
        state.startPage = startPage;
        state.startInStory = startInStory;
        state.startInPages = startInPages;
        state.fast = fast;

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
 GuiLine();
         formScroll = EditorGUILayout.BeginScrollView (formScroll,
                                                      false,
                                                      false);
 
        GUILayout.BeginVertical();
        foreach( Form f in god.forms ){
          GUILayout.BeginHorizontal();
          if(GUILayout.Button(""+f.gameObject.name , skin.GetStyle("GO_Button"))){
            EditorGUIUtility.PingObject( f.gameObject);
            Selection.activeTransform = f.gameObject.transform;
            //+ " : " + f.GetType() +  " : " + f.count * f.structSize
          }

          GUILayout.Label( " : " +  f.count * f.structSize, skin.GetStyle("CountVal") );
          GUILayout.Label( " : " + f.GetType() );
          GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        GuiLine();

      }
    }



    public bool recursiveSave;
    public bool recursiveLoad;

    public void SaveCurrentForm(GameObject g){

      if( g.GetComponent<Form>() != null ){
        Form f = g.GetComponent<Form>();
        Saveable.Save(f);
        Debug.Log("Saving Form : " + f.GetType() + " || On Game Object || " + g.name);
      }else{
        Debug.Log("No  Forms On Object");
      }

        if( recursiveSave ){
          Cycle[] cycles = g.GetComponents<Cycle>();
          for( int i = 0; i < cycles.Length; i++ ){
            for(int j =0; j < cycles[i].Cycles.Count; j++){
            if( cycles[i].Cycles[j].gameObject != g ){
              SaveCurrentForm( cycles[i].Cycles[j].gameObject );
            }}
          }
        }
      
    }


    public void LoadCurrentForm(GameObject g){

      if( g.GetComponent<Form>() != null ){
        Form f = g.GetComponent<Form>();
        Saveable.Load(f);
        Debug.Log("Saving Form : " + f.GetType() + " || On Game Object || " + g.name);
      }else{
        Debug.Log("No  Forms On Object");
      }

        if( recursiveLoad ){
          Cycle[] cycles = g.GetComponents<Cycle>();
          for( int i = 0; i < cycles.Length; i++ ){
            for(int j =0; j < cycles[i].Cycles.Count; j++){
            if( cycles[i].Cycles[j].gameObject != g ){
              LoadCurrentForm( cycles[i].Cycles[j].gameObject );
            }}
          }
        }
      
    }



    void ShowSaveButtons(){

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
         
        GUILayout.BeginHorizontal();
          if(GUILayout.Button("Save Current Form" , skin.GetStyle("SAVE_Button"))){
            if( Selection.activeTransform != null) SaveCurrentForm(Selection.activeTransform.gameObject);
          }
          recursiveSave = EditorGUILayout.Toggle(recursiveSave,GUILayout.Width(30));
        GUILayout.EndHorizontal();
    EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
          if(GUILayout.Button("Load Current Form" , skin.GetStyle("SAVE_Button"))){
            if( Selection.activeTransform != null) LoadCurrentForm(Selection.activeTransform.gameObject);
          }
          recursiveLoad = EditorGUILayout.Toggle(recursiveLoad,GUILayout.Width(30));
        GUILayout.EndHorizontal();
      
        GUILayout.EndVertical();

      
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

    void GuiLine( int i_height = 1 )

   {

       Rect rect = EditorGUILayout.GetControlRect(false, i_height );

       rect.height = i_height;

       EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );

   }
}