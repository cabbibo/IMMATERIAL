using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(God))]
public class GodEditor : Editor 
{  
   public override void OnInspectorGUI()
    {
       

        God god = (God)target;
        if(GUILayout.Button("Rebuild"))
        {
            god.Rebuild();
        }

         DrawDefaultInspector();


    }


   


}