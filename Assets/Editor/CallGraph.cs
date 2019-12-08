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



    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/CallGraph")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CallGraph window = (CallGraph)EditorWindow.GetWindow(typeof(CallGraph));
       // renderTexture = Resources.Load("CallGraph") as RenderTexture;
        window.Show();
    }



    void OnGUI()
    {

        Vector2 screenPos = Event.current.mousePosition;
        //Debug.Log( screenPos );
        ShowRender();
   
    }

    bool allAssigned{
      get { return (renderTexture != null && camera != null && buildTree != null ); }
    }
  

    void ShowRender(){
      buildTree = (BuildTree)EditorGUILayout.ObjectField(buildTree,typeof(BuildTree),true);
      if( buildTree != null ){
        camera = buildTree.camera;
        renderTexture = camera.targetTexture;
      }

      if( allAssigned ){

        camera.transform.LookAt(buildTree.transform);
        Rect rect = EditorGUILayout.GetControlRect(false, 10 );

       rect.height = rect.width;

        EditorGUI.DrawPreviewTexture( rect , renderTexture);
      }
    }

  
}