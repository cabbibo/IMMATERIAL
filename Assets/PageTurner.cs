using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageTurner : Cycle
{
  public Page[] pages;
  private Vector3 ro;
  private Vector3 rd;

  public override void Create(){

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

      RaycastHit hit;
      if( Physics.Raycast(ro,rd, out hit, Mathf.Infinity)){
       
        if( hit.collider.gameObject.tag == "Frame"){

          print( hit.collider.transform.parent.GetComponent<Page>());
          print( data.text );
          data.text.Set( hit.collider.transform.parent.GetComponent<Page>().text );
          data.text.PageStart();
        }
      }
    }

  }

  }

}
