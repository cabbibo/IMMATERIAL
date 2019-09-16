using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageChain : Cycle
{


   
  public Page[] pages;
  public int currentPage;
  private float switchTime;

  
  public override void Create(){

    for( int i = 0; i < pages.Length; i ++ ){
      SafeInsert(pages[i]);
    }

  }

  public void NextPage(){

    currentPage ++;
    
    if( currentPage < pages.Length ){
      SetActivePage();
    }else{
      Release();
    }

  }

  public void PreviousPage(){
    
    currentPage --;
    
    if( currentPage >= 0 ){
      SetActivePage();
    }else{
      Release();
    }

  }

  public void SetActivePage(){
    data.textParticles.Set( pages[currentPage].text );
    data.textParticles.PageStart();
    data.cameraControls.SetLerpTarget( pages[currentPage].transform ,pages[currentPage].lerpSpeed);
  }   


  public void Catch(){
   SetActivePage();
  }

  public void Release(){
    data.cameraControls.SetFollowTarget( data.player ,data.cameraControls.lerpSpeed);
  }


}
