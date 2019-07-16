
using UnityEngine;
using UnityEditor;

 [ExecuteInEditMode]
public class CopyScenePosition : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

      #if UNITY_EDITOR 
        transform.rotation =  SceneView.lastActiveSceneView.camera.transform.rotation;
        transform.position =  SceneView.lastActiveSceneView.camera.transform.position;
        SceneView.lastActiveSceneView.camera.nearClipPlane = Camera.main.nearClipPlane;
        SceneView.lastActiveSceneView.camera.farClipPlane = Camera.main.farClipPlane;
        SceneView.lastActiveSceneView.camera.fieldOfView = Camera.main.fieldOfView;

      #endif
    }



}
