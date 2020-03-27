using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monolith : Cycle
{

    public GameObject storyMarkerPrefab;
    public Transform monolith;
    public Transform cameraLerp;
    //public Story story;

    public MeshRenderer renderer;

    public bool isBook;

    public float fade;

    public GameObject[] storyMarkers;
    public int whichStory;

    public float ratio;
    public MaterialPropertyBlock mpb;

    public bool dynamicPositions;

    public void DestroyMe(){
     // print( storyMarkers.Length );
      for( int i = 0; i < storyMarkers.Length; i++ ){

        if( storyMarkers[i] ){
        Cycles.Remove(storyMarkers[i].GetComponent<StoryMarker>());
        DestroyImmediate(storyMarkers[i]);
      }
      }

      Cycles.Clear();
    }


 public override void Destroy(){
      data.inputEvents.OnTap.RemoveListener( CheckHit );
    }


    public override void Create(){

      if( cameraLerp == null ){ 
        GameObject go = new GameObject();
        cameraLerp = go.transform;
      }

      data.inputEvents.OnTap.RemoveListener( CheckHit );
      data.inputEvents.OnTap.AddListener( CheckHit );

//      print( "WHA : " + this.gameObject );


      ratio = (float)Screen.width / (float) Screen.height;

      ratio =  ratio;

     // ratio = 1/.5f;

      //transform.position = story.transform.position;
      //transform.rotation = story.transform.rotation;

      float size = 2;



      monolith.localScale = new Vector3( size, size * ratio , size / 7 );
      
      if( !isBook ){ monolith.localPosition = Vector3.up * size * ratio * .4f; }



      if( data.journey.monoSetters.Length != storyMarkers.Length  || storyMarkers == null ){


      DestroyMe();
      storyMarkers = new GameObject[data.journey.monoSetters.Length];
      for( int i = 0; i < data.journey.monoSetters.Length; i++ ){
          storyMarkers[i] = Instantiate( storyMarkerPrefab);
          
          storyMarkers[i].transform.parent = transform;
          storyMarkers[i].transform.localPosition = Vector3.zero;
          storyMarkers[i].transform.localPosition += monolith.localScale.x * (Vector3.right) * ( (-.5f + data.journey.monoSetters[i].uv.x) * .7f ); 
          
          storyMarkers[i].transform.localPosition += monolith.localScale.y * (Vector3.up)  * ( (data.journey.monoSetters[i].uv.y) * .7f + .1f );
          storyMarkers[i].transform.localPosition -= monolith.localScale.z * (Vector3.forward)  * .5f;

      }
    }


      /*

        Setting the locations of the markers in the shader
        w
      */

      mpb = new MaterialPropertyBlock();

      Vector4[] positions = new Vector4[data.journey.monoSetters.Length ];
      for(int i = 0; i < positions.Length; i++ ){
        positions[i] = storyMarkers[i].transform.position;
      }

      mpb.SetVectorArray("_StoryPositions" , positions );
      mpb.SetInt("_NumStories" , positions.Length );
      mpb.SetInt("_WhichStory" , whichStory );
      mpb.SetFloat("_Fade", fade);

      renderer = monolith.GetComponent<MeshRenderer>();
      renderer.SetPropertyBlock( mpb );
      
     // if( isBook ){ transform.rotation = Quaternion.AngleAxis(90,Vector3.right);}
      cameraLerp.transform.parent = monolith;

      cameraLerp.localRotation = Quaternion.Euler(0, 0, 90);

      cameraLerp.localPosition = Vector3.forward * -8;
      //cameraLerp.rotation = monolith.rotation;
      //cameraLerp.Rotate( monolith.forward * 90 );


    }

    public void OnDone(){

    data.inputEvents.OnEdgeSwipeLeft.RemoveListener(  OnDone );
    data.inputEvents.OnEdgeSwipeRight.RemoveListener(  OnDone );

    data.cameraControls.SetFollowTarget();
    }

    public void CheckHit(){

      if( data.inputEvents.hit.collider == monolith.GetComponent<Collider>() && data.state.inPages == false ){
        data.cameraControls.SetLerpTarget( cameraLerp , 1 );
        data.inputEvents.OnEdgeSwipeLeft.AddListener( OnDone );
        data.inputEvents.OnEdgeSwipeRight.AddListener(  OnDone );
      }

      for(int i = 0; i < storyMarkers.Length; i++ ){
//      print(data.inputEvents.hitTag);
        if( data.inputEvents.hitTag == "StartNode" && data.inputEvents.hit.collider == storyMarkers[i].GetComponent<Collider>() ){
         // print("HELLO1");
          data.state.PlaySelection();
          data.state.ConnectMonolith( i );
        }
      }

    }

    public void FadeIn(){ 
      print( "hnhiiii");
      gameObject.SetActive(true);
      data.tween.AddTween(1 , tweenIn ); 
    }

    public void FadeOut(){ print( "had"); data.tween.AddTween(1 , tweenOut , end ); }

    public void tweenIn(float v){ fade = v; }
    public void tweenOut(float v){ fade = v; }
    public void end(){gameObject.SetActive(false); }


  public override void WhileLiving(float v ){
    if( dynamicPositions ){

      Vector4[] positions = new Vector4[data.journey.monoSetters.Length ];
      for(int i = 0; i < positions.Length; i++ ){
        positions[i] = storyMarkers[i].transform.position;
      }

      mpb.SetVectorArray("_StoryPositions" , positions );

      
      renderer.SetPropertyBlock( mpb );

    }
  }
    
}
