using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookStory : Cycle
{

  public Page[] pages;
  public int currentPage;
  public GameObject prefab;

  public Vector2 uv;


  private float dif;
  private float oDif;
  public bool started;

  public override void Create(){

    for( int i = 0; i < pages.Length; i ++ ){
      SafeInsert(pages[i]);
    }



  }

  public override void OnBirthed(){
    for( int i = 0; i < pages.Length; i ++ ){
      pages[i].frameMPB.SetFloat("_Cutoff" , 1);
      pages[i].frame.borderLine.SetPropertyBlock( pages[currentPage].frameMPB );
    }



  }

  public void NextPage(){

    if( started ){

      currentPage ++;
      
      if( currentPage < pages.Length ){
        SetActivePage();
      }else{
        currentPage = 0;
        Release();
      }

    }

  }

  public void PreviousPage(){
    
    if( started ){

      currentPage --;
      
      if( currentPage >= 0 ){
        SetActivePage();
      }else{
        currentPage = 0;
        Release();
      }
    }

  }

  public void SetActivePage(){
    data.Text.Set( pages[currentPage].text );
    data.Text.PageStart();
  }   

  public void Release(){
    started = false;
    data.Text.Release();
  }



}
