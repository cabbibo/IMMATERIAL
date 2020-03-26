using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCycleTreeCPU : Cycle
{
      
    public GameObject CyclePrefab;
    public God god;

    public Transform transformHolder;

    public List<CycleInfoCPU> cycleInfo;

    public void Rebuild(){

        foreach( CycleInfoCPU ci in cycleInfo ){
          DestroyImmediate(ci.gameObject);
        }

       cycleInfo = new List<CycleInfoCPU>();

       CycleInfoCPU ci2 = GetComponent<CycleInfoCPU>();
       ci2.cycle = data; 


        Spawn( GetComponent<CycleInfoCPU>() , 1);


    }

    public override void OnBirthed(){
      Debug.Log( cycleInfo.Count );
      Debug.Log( god._cycles.Count );
      if( god._cycles.Count != (cycleInfo.Count+1) ){
        Rebuild();
      }

    }

    public void Spawn( CycleInfoCPU p_cycleInfo  , int currentLevel ){

      int siblingCount = p_cycleInfo.cycle.Cycles.Count;
      int id = 0;

      List<CycleInfoCPU> childList = new List<CycleInfoCPU>();


      foreach( Cycle c in p_cycleInfo.cycle.Cycles ){

        GameObject child = Instantiate( CyclePrefab );
        child.transform.parent = p_cycleInfo.transform;
        child.transform.localPosition = new Vector3( (int)id , -1 , Random.Range(-.99f,.99f) * 20 );
        child.transform.localScale = new Vector3( .6f , .6f, .6f );
        child.name ="" + c.gameObject.name + " || " + c.GetType();
        CycleInfoCPU ci = child.GetComponent<CycleInfoCPU>();



        childList.Add(ci);

        ci.parent = p_cycleInfo;
        ci.cycle = c;
        ci.isForm = c is Form;
        ci.isBinder = c is Binder;
        ci.isLife = c is Life;
        ci.count = (c is Form) ? ((Form)c).count : 0;
        ci.parent = p_cycleInfo;
        ci.level = currentLevel;
        //lr.S

        id ++;
        Spawn( ci , currentLevel+1);

        cycleInfo.Add(ci);
      
      }

      p_cycleInfo.children = childList;

      foreach( CycleInfoCPU ci in childList ){
        ci.siblings = childList;
      }



    }



    public override void WhileLiving(float v){


      //GameObject go;
      //GameObject parent;
      CycleInfoCPU ci;
      Vector3 force;
      Vector3 t1;

      for( int i = 0; i < cycleInfo.Count; i++ ){

        ci = cycleInfo[i];

        force = Vector3.zero;

//ci.vel = Vector3.zero;
       // force += Vector3.up * -3;
        t1 = ci.transform.position - ci.parent.transform.position;
        force += -t1 * .1f;

        foreach( CycleInfoCPU sibling in ci.siblings ){
          t1 = ci.transform.position - sibling.transform.position;
          force +=  (t1.normalized * ((100 / (1+4*(float)ci.level)) -t1.magnitude))  * .1f;
        }

         foreach( CycleInfoCPU sibling in cycleInfo ){
          t1 = ci.transform.position - sibling.transform.position;
          force += t1.normalized/(1+t1.magnitude) * .01f;//* ((100 / (1+(float)ci.level)) -t1.magnitude))  * .1f;
        }

        t1 = ci.transform.position - transform.position;
        //force += t1.normalized;


        ci.vel += force * .01f;
        ci.transform.position += ci.vel;
        ci.vel *= .95f;

        ci.lr.SetPosition( 0 , ci.transform.position );
        ci.lr.SetPosition(1 , ci.parent.transform.position );




      }
    }


}
