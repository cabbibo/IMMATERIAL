
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CanEditMultipleObjects]
[CustomEditor(typeof(Page))]
public class PageEditor : CycleEditor
{

  bool showEvents = true;
  bool showProperties = true;

  // PropertiesToSet
  //SerializedProperty frame;
  //SerializedProperty text;
  SerializedProperty lerpSpeed    ;
  SerializedProperty moveTarget   ;
  SerializedProperty lerpTarget   ;
  SerializedProperty locked       ;
  SerializedProperty mustContinue ;
  SerializedProperty fade         ;
  SerializedProperty baseHue      ;

  // Events
  SerializedProperty startEnter;
  SerializedProperty startExit ;
  SerializedProperty endEnter  ;
  SerializedProperty endExit   ;

void OnEnable(){

    lerpSpeed    = serializedObject.FindProperty("lerpSpeed"   );
    moveTarget   = serializedObject.FindProperty("moveTarget"  );
    lerpTarget   = serializedObject.FindProperty("lerpTarget"  );
    locked       = serializedObject.FindProperty("locked"      );
    mustContinue = serializedObject.FindProperty("mustContinue");
    fade         = serializedObject.FindProperty("fade"        );
    baseHue      = serializedObject.FindProperty("baseHue"     );

    startEnter  = serializedObject.FindProperty("OnStartEnter");
    startExit   = serializedObject.FindProperty("OnStartExit");
    endEnter    = serializedObject.FindProperty("OnEndEnter");
    endExit     = serializedObject.FindProperty("OnEndExit");
}

 
    public override void OnInspectorGUI()
    {

      //GUI.skin.font = (Font)Resources.Load("Fonts/RobotoCondensed-Bold");;
        base.OnInspectorGUI();
        showProperties = EditorGUILayout.Foldout(showProperties, "Properties");

        if( showProperties ){

GUILayout.BeginVertical();

            EditorGUILayout.PropertyField(lerpSpeed    );
            EditorGUILayout.PropertyField(moveTarget   );
            EditorGUILayout.PropertyField(lerpTarget   );
            EditorGUILayout.PropertyField(locked       );
            EditorGUILayout.PropertyField(mustContinue );
            EditorGUILayout.PropertyField(fade         );
            EditorGUILayout.PropertyField(baseHue      );


            GUILayout.EndVertical();
    
        }

        showEvents = EditorGUILayout.Foldout(showEvents, "Events");
        if( showEvents ){
            EditorGUILayout.PropertyField(startEnter);
            EditorGUILayout.PropertyField(startExit );
            EditorGUILayout.PropertyField(endEnter  );
            EditorGUILayout.PropertyField(endExit   );
        }
        serializedObject.ApplyModifiedProperties();

       


    EditorUtility.SetDirty( target );

    }
}
