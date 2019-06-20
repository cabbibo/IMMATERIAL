
using UnityEngine;
using UnityEditor;

 [ExecuteInEditMode]
public class CopyScenePosition : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
      EditorApplication.update += Always;
    }

    void OnDisable(){
      EditorApplication.update -= Always;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation =  SceneView.lastActiveSceneView.camera.transform.rotation;
        transform.position =  SceneView.lastActiveSceneView.camera.transform.position;
        SceneView.lastActiveSceneView.camera.nearClipPlane = Camera.main.nearClipPlane;
        SceneView.lastActiveSceneView.camera.farClipPlane = Camera.main.farClipPlane;
        SceneView.lastActiveSceneView.camera.fieldOfView = Camera.main.fieldOfView;
    }

    void Always(){
      EditorApplication.QueuePlayerLoopUpdate();
    }


}
