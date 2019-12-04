 using UnityEngine;
 using UnityEditor;
 [CustomEditor(typeof(Test))]
 public class TestEditor : Editor {
     void OnGUI() {
         Event e = Event.current;
         Debug.Log( e );
         switch (e.type) {
             case EventType.KeyDown:
                 break;
         }
     }
 }