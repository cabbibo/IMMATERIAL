using UnityEngine;
using UnityEditor;

public class CallGraph : EditorWindow
{



  public void Update()
 {
     // This is necessary to make the framerate normal for the editor window.
     Repaint();
 }

   
    public RenderTexture renderTexture;
    public BuildCycleVerts verts;



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

   
        ShowRender();
   
    }
  

    void ShowRender(){
if( renderTexture == null ) renderTexture = (RenderTexture)EditorGUILayout.ObjectField(renderTexture,typeof(RenderTexture),true);

      if( renderTexture != null ){
        Rect rect = EditorGUILayout.GetControlRect(false, 10 );

       rect.height = rect.width;

        EditorGUI.DrawPreviewTexture( rect , renderTexture);
      }
    }

  
}