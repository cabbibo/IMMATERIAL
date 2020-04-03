using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.SceneManagement;

public class BuildTree :Cycle
{

    public Camera cam;
    public AudioClip[] hoverClips;
    public AudioClip[] selectClips;

    public Transform vectorRep;

    public TextMesh typeLabel;
    public TextMesh goLabel;
    public TextMesh idLabel;
    public TextMesh extraLabel;

    public GameObject nameGO;
    public GameObject goToGO;
    public GameObject scriptGO;
    public GameObject moreInfoGO;
    public GameObject goToStoryGO;

    public int selectedVert;
    public int oSelectedVert;
    public float selectedDist;
    public float oSelectedDist;
    public HELP.CycleInfo activeCycleInfo;
    
    public BuildTreeVerts verts;
    public BuildTreeInfo info;
    public BuildTreeConnections connections;

    public Life sim;
    public Life resolve;
    public ClosestLife closest;

    public int activeVert;

    public float lastPlayTime;

    public Ray ray;
    public Vector3 _RO;
    public Vector3 _RD;

    public bool holdCenter;
    public Vector3 target;
    public override void Create(){
      lastPlayTime = 0;
      SafeInsert(info);
      SafeInsert(verts);
      SafeInsert(connections);
      SafeInsert(sim);
      SafeInsert(resolve);
      SafeInsert(closest);
      AddBinders();
    }

    public override void Bind(){
      sim.BindPrimaryForm("_VertBuffer",verts);
      sim.BindForm("_InfoBuffer",info);
      sim.BindForm("_ConnectionBuffer",connections);


      sim.BindVector3("_RO" , () => _RO);
      sim.BindVector3("_RD" , () => _RD);
      sim.BindInt("_SelectedVert" , () => selectedVert );
      sim.BindInt("_ActiveVert" , () => activeVert );

      closest.BindPrimaryForm("_VertBuffer",verts);
      closest.BindVector3("_RO" , () => _RO);
      closest.BindVector3("_RD" , () => _RD);


      resolve.BindInt("_SelectedVert" , () => selectedVert );
      resolve.BindPrimaryForm("_VertBuffer",verts);
      resolve.BindForm("_ConnectionBuffer",connections);


    }

    public override void WhileLiving( float v ){
      if( holdCenter ){ target = transform.position; }

      oSelectedVert = selectedVert;
      oSelectedDist = selectedDist;

      vectorRep.position = _RO + _RD * 10;
      vectorRep.LookAt(cam.transform);
      int id  = (int)closest.value.w;
      selectedDist = (new Vector3(closest.value.x , closest.value.y , closest.value.z )).magnitude;
      info.selectedVert = selectedVert;
     
      if( oSelectedVert != id && selectedDist < 1){
        OnNewVert(id);
          }

      if( selectedDist > 1 ){
        OnNoVert();

      }


    

    }

    public void OnNewVert(int id){

      selectedVert = id;

      if( Time.time - lastPlayTime > .1f){
        //data.audio.Play( hoverClips[Random.Range(0,hoverClips.Length)]  , 2.01f , .3f );
        lastPlayTime = Time.time;
      }

       typeLabel.text  = ""   + info.allCyclesInfo[selectedVert].name;
        goLabel.text    = "" + info.allCyclesInfo[selectedVert].goName;
        idLabel.text    = "" + info.allCyclesInfo[selectedVert].id;
        extraLabel.text = "" + info.allCyclesInfo[selectedVert].siblingCount;
   

    }

    public void OnNoVert(){
      selectedVert = activeVert;


        typeLabel.text  = "";
        goLabel.text    = "";
        idLabel.text    = "";
        extraLabel.text = "";
    }

    public void WindowMouseUp(){

      Ray ray = new Ray( _RO , _RD );
      RaycastHit hit;
      if( Physics.Raycast( ray , out hit ) ){
        
        if( hit.collider.gameObject == goToGO ){
          DoGoTo();
        }else if( hit.collider.gameObject == scriptGO ){
          DoScript();
        }else if( hit.collider.gameObject == moreInfoGO ){
          DoMoreInfo();
        }else if( hit.collider.gameObject == goToStoryGO ){
          GoToStory();
        }
      }else{

      if(selectedDist < .5f ){
        ActivateVert();
      }

      }

      
    }

    public void WindowMouseDown(){

    }

    public void WindowMouseDrag(){

    }

    public void ActivateVert(){
      activeCycleInfo = info.allCyclesInfo[selectedVert];
      activeVert = activeCycleInfo.id;
      string title = "" + activeCycleInfo.goName + " || " + activeCycleInfo.name;
      nameGO.GetComponent<TextMesh>().text  = title; 
      HighlightGameObject();

      //data.audio.Play( selectClips[Random.Range(0,selectClips.Length)]  , 1.01f, .6f);

      float [] d = new float[verts.structSize];
      verts._buffer.GetData( d , 0 , (int)activeVert * verts.structSize , verts.structSize );
      //GetData(Array data, int managedBufferStartIndex, int computeBufferStartIndex, int count);

      target = new Vector3(d[0] , d[1] , d[2] );

      if( activeCycleInfo.type == -1 ){
        moreInfoGO.SetActive( false );
      }else if( activeCycleInfo.type == -2 ){
        moreInfoGO.SetActive( false );
      }else if( activeCycleInfo.type ==  0 ){
        moreInfoGO.SetActive( false );
      }else if( activeCycleInfo.type ==  1 ){
        moreInfoGO.SetActive( true );
        moreInfoGO.GetComponent<TextMesh>().text =  "Save Form";
      }else if( activeCycleInfo.type == 2 ){
        moreInfoGO.SetActive( true );
        moreInfoGO.GetComponent<TextMesh>().text =  "Open Compute Shader";
      }else if( activeCycleInfo.type == 6 ){
        moreInfoGO.SetActive( true );
        moreInfoGO.GetComponent<TextMesh>().text =  "Open Shader";
      }else{
        moreInfoGO.SetActive( false );
      }
    }

    public void HighlightGameObject(){
      GameObject go = activeCycleInfo.go;
      #if UNITY_EDITOR
      EditorGUIUtility.PingObject( go );    

      Selection.activeTransform = activeCycleInfo.go.transform;
    #endif
      
    }

    public void DoGoTo(){
      #if UNITY_EDITOR
      SceneView scene = (SceneView) SceneView.sceneViews[0];
      scene.pivot =  activeCycleInfo.go.transform.position;
      scene.rotation =  activeCycleInfo.go.transform.rotation;
      #endif
    }

    public void DoScript(){
      #if UNITY_EDITOR
      var script = MonoScript.FromMonoBehaviour(activeCycleInfo.cycle); // gets script as an asset
      AssetDatabase.OpenAsset(script); // opens script in your predefined script editor
      #endif
    }

    public void DoSave(){
      RecursiveSave( activeCycleInfo.cycle );
    }

    public void RecursiveSave( Cycle cycle ){

      if( cycle is Form ){ Saveable.Save((Form)cycle); }
      foreach( Cycle c in cycle.Cycles ){
        RecursiveSave(c);
      }

    }

    public void GoToStory(){
      StepUp(activeCycleInfo.cycle);
    }

    public void StepUp(Cycle c){
      if( c is StorySetter ){
//        int story = 0;

        for( int i = 0; i < data.journey.setters.Length; i++ ){
          print( i );
          if( data.journey.setters[i] == c ){
            data.state.startInStory = true;
            data.state.startInPages = true;
            data.state.startStory = i;
            data.state.startPage = 0;
            data.god.Rebuild();
            break;
          }
        }
      }else{
        if( c.parent != null ){
          StepUp(c.parent);
        }else{
          print("NO STASODOD");
        }
      }
    }


  public void DoMoreInfo(){
#if UNITY_EDITOR
     if( activeCycleInfo.type == -1 ){
        moreInfoGO.GetComponent<TextMesh>().text =  "THIS A STORY BRUV";
      }else if( activeCycleInfo.type == -2 ){
        data.god.godPause = !data.god.godPause;
      }else if( activeCycleInfo.type ==  0 ){
        moreInfoGO.GetComponent<TextMesh>().text =  "boring cycle";
      }else if( activeCycleInfo.type ==  1 ){
        DoSave();
      }else if( activeCycleInfo.type == 2 ){
        // var script = MonoScript.FromMonoBehaviour(((Life)activeCycleInfo.cycle).shader); // gets script as an asset
        AssetDatabase.OpenAsset(((Life)activeCycleInfo.cycle).shader); // opens script in your predefined script editor
      }else if( activeCycleInfo.type == 6 ){
        AssetDatabase.OpenAsset(((Body)activeCycleInfo.cycle).material.shader); // opens script in your predefined script editor
      }
      #endif

  }    



}
