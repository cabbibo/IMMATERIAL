using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Cycle
{


    public MeshRenderer clickBackground;
    public MeshRenderer clickText;

    public MeshRenderer swipeLeftPageText;
    public MeshRenderer swipeRightPageText;
    
    public MeshRenderer swipeLeftTurnText;
    public MeshRenderer swipeRightTurnText;
    public MeshRenderer swipeForwardMoveText;


    public MeshRenderer tapMoveText;


    public MeshRenderer currentRenderer;
    public bool inOrOut;


    public override void Create(){

      if( data.state.DOFULL ){

        clickBackground.enabled = false;
        clickText.enabled = false;

        swipeLeftPageText.enabled = false;
        swipeRightPageText.enabled = false;
        swipeLeftTurnText.enabled = false;
        swipeRightTurnText.enabled = false;
        swipeForwardMoveText.enabled = false;
        tapMoveText.enabled = false;
      }

    }


    public void AddSwipeListeners(){

    }

    public void RemoveSwipeListeners(){

    }

    public void swipeLeftPageTurnIn(){

      swipeLeftPageText.enabled = true;
      inOrOut = true;
      currentRenderer = swipeLeftPageText;
       currentRenderer.sharedMaterial.SetColor("_Color" , new Color(0,0,0,0) );
      data.tween.AddTween( 1 , tweenMaterial  );
    }


    public void swipeLeftPageTurnOut(){

      inOrOut = false;
      currentRenderer = swipeLeftPageText;
      data.tween.AddTween( 1 , tweenMaterial  );
    }



    public void swipeRightPageTurnIn(){

      swipeRightPageText.enabled = true;
      inOrOut = true;
      currentRenderer = swipeRightPageText;

       currentRenderer.sharedMaterial.SetColor("_Color" , new Color(0,0,0,0) );
      data.tween.AddTween( 1 , tweenMaterial  );
    }


    public void swipeRightPageTurnOut(){
      inOrOut = false;
      currentRenderer = swipeRightPageText;
      data.tween.AddTween( 1,  tweenMaterial  );
    }



    public void ShowTurnTutorials(){

      swipeLeftTurnText.enabled = true;
      swipeRightTurnText.enabled = true;
      swipeForwardMoveText.enabled = true;
      tapMoveText.enabled = true;


      inOrOut = true;

      currentRenderer = swipeLeftTurnText; tweenMaterial(0);
      currentRenderer = swipeRightTurnText; tweenMaterial(0);
      currentRenderer = swipeForwardMoveText; tweenMaterial(0);
      currentRenderer = tapMoveText; tweenMaterial(0);


      currentRenderer = swipeLeftTurnText;
      data.tween.AddTween( .5f , tweenMaterial  );
      data.inputEvents.OnSwipeLeft.AddListener(  switchToRightTurn );
    }

    public void switchToRightTurn(){

      data.inputEvents.OnSwipeLeft.RemoveListener(  switchToRightTurn );
      inOrOut = false;
      currentRenderer = swipeLeftTurnText;
      data.tween.AddTween(.5f,tweenMaterial,rightTurnAdd);
    }

    public void rightTurnAdd(){
      inOrOut = true;
      currentRenderer = swipeRightTurnText;
      data.tween.AddTween( .5f , tweenMaterial  );
      data.inputEvents.OnSwipeRight.AddListener(  switchToForwardTurn );
    }

    public void switchToForwardTurn(){

      data.inputEvents.OnSwipeRight.RemoveListener(  switchToForwardTurn );
      inOrOut = false;
      currentRenderer = swipeRightTurnText;
      data.tween.AddTween(.5f,tweenMaterial,forwardTurnAdd);
    }

    public void forwardTurnAdd(){
      inOrOut = true;
      currentRenderer = swipeForwardMoveText;
      data.tween.AddTween( .5f , tweenMaterial  );
      data.inputEvents.OnSwipeDown.AddListener(  switchToTap );
    }

    public void switchToTap(){

      data.inputEvents.OnSwipeDown.RemoveListener(  switchToTap );
      inOrOut = false;
      currentRenderer = swipeForwardMoveText;
      data.tween.AddTween(.5f,tweenMaterial,tapAdd);
    }

    public void tapAdd(){
      inOrOut = true;
      currentRenderer = tapMoveText;
      data.tween.AddTween( .5f , tweenMaterial  );
      data.inputEvents.OnTap.AddListener(  endTutorial );
    }

    public void endTutorial(){

      data.inputEvents.OnTap.RemoveListener(  endTutorial );
      inOrOut = false;
      currentRenderer = tapMoveText;
      data.tween.AddTween(.5f,tweenMaterial,onTweenOutEnd);
    }


    
    public void tweenMaterial(float v){


      Material m = currentRenderer.sharedMaterial;
      if( Application.isPlaying ){ m = currentRenderer.material; }
      // Dont jump in if we already are in!
      if( !inOrOut ){ 
          v = 1-v; 
          if( m.HasProperty("_Color") ) v = Mathf.Min( v , m.GetColor("_Color").a );
        }else{
          if( m.HasProperty("_Color") )  v = Mathf.Max( v , m.GetColor("_Color").a );
        }

      if( Application.isPlaying ){
        m.SetColor("_Color" , new Color(v,v,v,v) );
      }else{
        m.SetColor("_Color" , new Color(v,v,v,v) );
      }
    }


    public void ShowClickTut(){
      clickBackground.enabled = true;
      clickText.enabled = true;
      data.tween.AddTween( 1 , tweenInClick  , onClickTweenIn );
    } 


    public void tweenInClick(float v){
      clickBackground.sharedMaterial.SetColor("_Color" , new Color(0,0,0,0));
      clickBackground.sharedMaterial.SetColor("_EmissionColor" , new Color(v,v,v,v));
      clickText.sharedMaterial.SetColor("_Color" , new Color(1-v,1-v,1-v,v));
    }


    public void tweenOutClick(float v){
      clickBackground.sharedMaterial.SetColor("_Color" , new Color(0,0,0,0));
      clickBackground.sharedMaterial.SetColor("_EmissionColor" , new Color(1-v,1-v,1-v,1-v));
      clickText.sharedMaterial.SetColor("_Color" , new Color(v,v,v,1-v));
    }

    public void onClickTweenIn(){
      data.inputEvents.OnTap.AddListener(checkForHit);
    }

    public void checkForHit(){
      if( data.inputEvents.hitTag == "Frame" ){
        HideClickTut();
      }
    }


    public void onTweenOutEnd(){


      swipeLeftPageText.enabled = false;
      swipeRightPageText.enabled = false;
      swipeLeftTurnText.enabled = false;
      swipeRightTurnText.enabled = false;
      swipeForwardMoveText.enabled = false;
      tapMoveText.enabled = false;

    }

    public void onClickOutEnd(){

      clickBackground.enabled = false;
      clickText.enabled = false;
    }

    public void HideClickTut(){
        data.tween.AddTween(1 , tweenOutClick , onClickOutEnd );
        clickBackground.GetComponent<Collider>().enabled = false;
    } 



    public override void WhileLiving(float v){

    }

    public bool sLP = false;
    public bool sRP = false;

   /* public void Set( TutorialSetter setter ){
      if( sLP != setter.swipeLeftPage ){
        if( setter.swipeLeftPage          swipeLeftPageTurnIn();
        }else{
          swipeLeftPageTurnOut();
        }
      }

      if( sRP != setter.swipeRightPage ){
        if( setter.swipeRightPage ){
          swipeRightPageTurnIn();
        }else{
          swipeRightPageTurnOut();
        }
      }
    }*/



}
