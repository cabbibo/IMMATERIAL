using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CollapseAll
{
  
  private static int wait = 0;
  private static int undoIndex;

  [MenuItem( "Assets/Collapse Folders", priority = 1000 )]
  public static void CollapseFolders()
  {
    DirectoryInfo[] rootDirectories = new DirectoryInfo( Application.dataPath ).GetDirectories();
    List<Object> rootDirectoriesList = new List<Object>( rootDirectories.Length );
    for( int i = 0; i < rootDirectories.Length; i++ )
    {
      Object directoryObj = AssetDatabase.LoadAssetAtPath<Object>( "Assets/" + rootDirectories[i].Name );
      if( directoryObj != null )
        rootDirectoriesList.Add( directoryObj );
    }

    if( rootDirectoriesList.Count > 0 )
    {
      Undo.IncrementCurrentGroup();
      Selection.objects = Selection.objects;
      undoIndex = Undo.GetCurrentGroup();

      EditorUtility.FocusProjectWindow();

      Selection.objects = rootDirectoriesList.ToArray();

      EditorApplication.update -= CollapseHelper;
      EditorApplication.update += CollapseHelper;
    }
  }

  public static void CollapseGameObjects()
  {
    EditorApplication.update -= CollapseGameObjects;
    CollapseGameObjects( new MenuCommand( null ) );
  }

  [MenuItem( "GameObject/Collapse All", priority = 40 )]
  private static void CollapseGameObjects( MenuCommand command )
  {
    // This happens when this button is clicked via hierarchy's right click context menu
    // and is called once for each object in the selection. We don't want that, we want
    // the function to be called only once
    if( command.context )
    {
      EditorApplication.update -= CollapseGameObjects;
      EditorApplication.update += CollapseGameObjects;

      return;
    }

    List<GameObject> rootGameObjects = new List<GameObject>();
    int sceneCount = SceneManager.sceneCount;
    for( int i = 0; i < sceneCount; i++ )
      rootGameObjects.AddRange( SceneManager.GetSceneAt( i ).GetRootGameObjects() );

    if( rootGameObjects.Count > 0 )
    {
      Undo.IncrementCurrentGroup();
      Selection.objects = Selection.objects;
      undoIndex = Undo.GetCurrentGroup();

      Selection.objects = rootGameObjects.ToArray();

      EditorApplication.update -= CollapseHelper;
      EditorApplication.update += CollapseHelper;
    }
  }

  private static void CollapseHelper()
  {
    if( wait < 1 ) // Increase the number if script doesn't always work
      wait++;
    else
    {
      EditorApplication.update -= CollapseHelper;
      wait = 0;

      EditorWindow focusedWindow = EditorWindow.focusedWindow;
      if( focusedWindow != null )
        focusedWindow.SendEvent( new Event { keyCode = KeyCode.LeftArrow, type = EventType.KeyDown, alt = true } );

      Undo.RevertAllDownToGroup( undoIndex );
    }
  }
}