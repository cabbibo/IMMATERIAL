
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Cycle))]
public class CycleEditor : Editor
{
 public override bool RequiresConstantRepaint()
     {
         return true;
     }
  bool showCycleInfo = true;
  SerializedProperty debug  ;
  SerializedProperty active ;
  SerializedProperty data   ;
  SerializedProperty cycles ;

void OnEnable(){
    debug  = serializedObject.FindProperty("debug");
    active = serializedObject.FindProperty("active");
    data   = serializedObject.FindProperty("data");
    cycles = serializedObject.FindProperty("Cycles");
}

 
    public override void OnInspectorGUI()
    {


    debug  = serializedObject.FindProperty("debug");
    active = serializedObject.FindProperty("active");
    data   = serializedObject.FindProperty("data");
    cycles = serializedObject.FindProperty("Cycles");
    
        showCycleInfo = EditorGUILayout.Foldout(showCycleInfo, "CycleInfo");
        if( showCycleInfo ){
          var indent = EditorGUI.indentLevel;
          EditorGUI.indentLevel = indent + 1;

            //EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((target.TypeOf) target), typeof(target.TypeOf), false);
    
          
            EditorGUILayout.PropertyField(debug  );
            EditorGUILayout.PropertyField(active );
            EditorGUILayout.PropertyField(data   );

            EditorGUILayout.PropertyField(cycles ,true);
            EditorGUILayout.Space();
            GuiLine();

          EditorGUI.indentLevel = indent;
        }
        serializedObject.ApplyModifiedProperties();
    }

    public void GuiLine( int i_height = 1 )

   {

       Rect rect = EditorGUILayout.GetControlRect(false, i_height );

       rect.height = i_height;

       EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );

   }
}
