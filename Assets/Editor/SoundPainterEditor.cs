 using UnityEngine;
 using System.Collections;
 using System.Reflection;
 using UnityEditor;
 
 [CustomEditor(typeof(SoundPainter))]
 public class SoundPainterEditor : Editor {
     //private static bool m_editMode = false;
     //private static bool m_editMode2 = false;
     
     void OnSceneGUI()
     {

    // Debug.Log("test");
         SoundPainter test = (SoundPainter)target;
 HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

             if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
             {

              //print("mouse");
                 Vector2 mousePos = Event.current.mousePosition * 1.5f;
                 mousePos.y = Camera.current.pixelHeight - mousePos.y;
                 Ray ray = Camera.current.ScreenPointToRay(mousePos);
                 test.WhileDown(ray);

             }



             if (Event.current.type == EventType.MouseUp)
             {
              test.Save();
 Debug.Log("Event Type : "  + Event.current.type );
 
             }
         
         
     }
     public override void OnInspectorGUI()
     {
        SoundPainter test = (SoundPainter)target;
 EditorGUILayout.Space(); EditorGUILayout.Space();

        GUILayout.Label("PaintSize : " + test.paintSize);
        test.paintSize = GUILayout.HorizontalSlider(test.paintSize, 0.0F, 100.0F);
        EditorGUILayout.Space(); EditorGUILayout.Space();
     
        GUILayout.Label("Paint Opacity: " + test.paintOpacity);
        test.paintOpacity = GUILayout.HorizontalSlider(test.paintOpacity, 0.0F, 1.0F);

 EditorGUILayout.Space(); EditorGUILayout.Space(); EditorGUILayout.Space();



        if (GUILayout.Button("Reset To Original"))
        {
          test.ResetToOriginal();
          test.Save();
        }

        if (GUILayout.Button("Set To Current"))
        {
          test.UltraSave();
        }

         /*if (GUILayout.Button("Reset To Flat"))
        {
          test.ResetToFlat();
          test.Save();
        }*/

        if (GUILayout.Button("UNDO"))
        {
          test.Undo();
        }

        if (GUILayout.Button("REDO"))
        {
          test.Redo();
        }

       

  test.raiseLowerFlatten = EditorGUILayout.Popup("Label", test.raiseLowerFlatten, test.options); 
  EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();EditorGUILayout.Space();

        DrawDefaultInspector ();
       
     }
 }