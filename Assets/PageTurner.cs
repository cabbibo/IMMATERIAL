using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTurner : Cycle
{
  public Page[] pages;
  private Vector3 ro;
  private Vector3 rd;

  private float switchTime;
  public override void Create(){

    switchTime = 0;
    for( int i = 0; i < pages.Length; i ++ ){
      SafeInsert(pages[i]);
    }


  }

  public override void OnLive(){

  }

  
  // Update is called once per frame
  public override void WhileLiving( float v) {

    if( Application.isPlaying ){


      Vector2 p =  Input.mousePosition;///Input.GetTouch(0).position;
      ro = Camera.main.ScreenToWorldPoint( new Vector3( p.x , p.y , Camera.main.nearClipPlane ) );
      rd = -(Camera.main.transform.position - ro).normalized;
      
      if( Input.GetMouseButtonDown(0) ){

      print("helllooooooo");
        RaycastHit hit;
        if( Physics.Raycast(ro,rd, out hit, Mathf.Infinity)){


      print("helllooooooo2");
         
          if( hit.collider.gameObject.tag == "Frame"){

            //hit.collider.transform.parent.GetComponent<Page>().SetActivePage();
          

          }
        }
      }

    }else{


      /*switchTime += 1;
      if( switchTime > 400 ){

        print("swartch");

        switchTime -= 400;
        pages[Random.Range( 0, pages.Length )].SetActivePage();
        
      }*/



    }

  }

}
