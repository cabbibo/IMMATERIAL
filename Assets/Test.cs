using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteAlways]
public class Test : MonoBehaviour
{
    public void OnGUI(){
      //print(Input.mousePosition);
    }

  /* static EditorWindow gameView = null;
 
    public static void GameView_Repaint()
    {
        if( gameView == null )
        {
            System.Reflection.Assembly assembly = typeof( UnityEditor.EditorWindow ).Assembly;
            System.Type type = assembly.GetType( "UnityEditor.GameView" );
            gameView = EditorWindow.GetWindow( type );
        }
 
        if( gameView != null )
            gameView.Repaint();
    }*/
}
