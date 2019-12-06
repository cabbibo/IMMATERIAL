using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(State))]
public class StateEditor : CycleEditor 
{  
    public override void OnInspectorGUI()
    {
  
        State state = (State)target;
        if(GUILayout.Button("Set Start Story / Pages")){
            state.SetStartToCurrentStory();
        }

      DrawDefaultInspector();

    }
     
}
