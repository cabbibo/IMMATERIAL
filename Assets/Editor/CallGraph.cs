using UnityEngine;
using UnityEditor;

public class CallGraph : EditorWindow
{



public void Update()
{
   // This is necessary to make the framerate normal for the editor window.

   Repaint();
}




    public Camera camera;
   
    public RenderTexture renderTexture;
    public BuildTree buildTree;


    float viewWidth;
    float startHeight;
    float distance = 10;

    public Vector3 target;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/CallGraph")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CallGraph window = (CallGraph)EditorWindow.GetWindow(typeof(CallGraph));
       // renderTexture = Resources.Load("CallGraph") as RenderTexture;
        window.Show();
    }

  

    void OnSceneGUI(SceneView sceneView){

  
    }
    public bool dragging;
    Vector2 angle = new Vector2();
    Vector2 angleForce = new Vector2();
    void OnGUI()
    {

        Event e = Event.current;

        ShowRender();
   
        //Debug.Log( screenPos );
        if( allAssigned  && EditorWindow.focusedWindow == this ){

         // Debug.Log( viewWidth );
         // Debug.Log( camera.pixelHeight );
         Vector2 mousePos = e.mousePosition * camera.pixelHeight/viewWidth ;
          mousePos.y = camera.pixelHeight - mousePos.y;//* (camera.pixelHeight/viewWidth);
          Ray ray = camera.ScreenPointToRay( mousePos );
          
          buildTree._RO = ray.origin;
          buildTree._RD = ray.direction;

      

          if( e.type == EventType.ScrollWheel ){
            distance += e.delta.y;
          }

          if( e.type == EventType.MouseDrag ){
            dragging = true;
            angleForce += e.delta;

          }
          if( e.type == EventType.MouseUp ){
            if( !dragging ) buildTree.WindowMouseUp();
            dragging = false;
          }

          GetInfo();

          
        }


    }

    bool allAssigned{
      get { return (renderTexture != null && camera != null && buildTree != null ); }
    }
  

    void ShowRender(){
      

      if( buildTree == null) buildTree = (BuildTree)EditorGUILayout.ObjectField(buildTree,typeof(BuildTree),true);
      
      if( buildTree != null ){
        camera = buildTree.camera;
        renderTexture = camera.targetTexture;
      }

          Rect rect = EditorGUILayout.GetControlRect(false, 100 );

        if( rect.x > startHeight ){
          startHeight = rect.x;
        }
        //Debug.Log( Screen.width );
        rect.width = Screen.width / 1.5f;
        rect.height = Screen.width / 1.5f;
        viewWidth = rect.height;


        //Debug.Log( viewWidth );
        EditorGUI.DrawPreviewTexture( rect , renderTexture);


    }

    void GetInfo(){


      if( allAssigned ){

        angle += angleForce * .005f;
        angleForce *= .95f;

        Quaternion q = Quaternion.Euler(angle.y,angle.x,0);
        Vector3 direction;// = new Vector3( Mathf.Sin(angle.x),0 , -Mathf.Cos(angle.x) );
        direction = q * Vector3.forward;

        target = Vector3.Lerp( target , buildTree.target , .1f );
        camera.transform.position = target + direction * distance;

        camera.transform.LookAt(target);
    
      }


    }


    
  
}