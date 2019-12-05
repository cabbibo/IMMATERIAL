
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

Texture2D t1 = (Texture2D)Resources.Load("EditorStuff/bar1");//
Texture2D t = new Texture2D(256,1);
for(int i = 0; i < 256; i++ ){
t.SetPixel(i,0,Color.HSVToRGB(((Time.time ) + ((float)i / 1024))%1,.5f,1));
}
t.Apply();
GUIStyle gsTest = new GUIStyle();
gsTest.normal.background = t;
//gsTest.font = (Font)Resources.Load("Fonts/RobotoCondensed-Bold");
//gsTest.fontSize = 20;
Font tmp = EditorStyles.label.font;
        if( showProperties ){

GUILayout.BeginVertical(gsTest);
    Rect rect = EditorGUILayout.GetControlRect(false, 10 + Mathf.Sin(Time.time) * 7 );
       rect.height = 10 + Mathf.Sin(Time.time) * 7;
EditorGUI.DrawPreviewTexture(rect, t1);

Font f1 = (Font)Resources.Load("Fonts/RobotoCondensed-Bold");
Font f2 = (Font)Resources.Load("Fonts/LiberationSans");

EditorStyles.label.font = f1;
EditorStyles.label.fontSize = (int)((Mathf.Sin(Time.time) +1 ) * 10);
            EditorGUILayout.PropertyField(lerpSpeed    );

EditorStyles.label.fontSize = (int)((Mathf.Sin(Time.time * .7f+2) +1 ) * 10);
            EditorGUILayout.PropertyField(moveTarget   );
EditorStyles.label.font = f2;

EditorStyles.label.fontSize = (int)((Mathf.Sin(Time.time * .4f+2) +1 ) * 10);


            EditorGUILayout.PropertyField(lerpTarget   );

EditorStyles.label.fontSize = (int)((Mathf.Sin(Time.time * .5f+2) +1 ) * 8);
            EditorGUILayout.PropertyField(locked       );
            EditorStyles.label.fontSize = (int)((Mathf.Sin(Time.time * .7f+12) +1 ) * 8);
            EditorGUILayout.PropertyField(mustContinue );

            EditorStyles.label.fontSize = (int)((Mathf.Sin(Time.time * 1.7f+12) +1 ) * 8);
            EditorGUILayout.PropertyField(fade         );

            EditorStyles.label.fontSize = (int)((Mathf.Sin(Time.time * 2.7f+12) +1 ) * 8);
            EditorGUILayout.PropertyField(baseHue      );
            EditorStyles.label.fontSize = 12;

        rect = EditorGUILayout.GetControlRect(false, 10 + Mathf.Sin(Time.time * 2) * 7 );
       rect.height = 10 + Mathf.Sin(Time.time * 2) * 7;
   //rect = EditorGUILayout.GetControlRect(false, 10 );
     //  rect.height = 10;
EditorGUI.DrawPreviewTexture(rect, t1);
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
