using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBook : Cycle
{

    public Transform bookHoldLocation;
    public Transform toBeBook;

    public void PickUp(){
      toBeBook.parent = bookHoldLocation;
      toBeBook.rotation = Quaternion.identity;
      toBeBook.position = Vector3.zero;
      print("HELLOOOOO");
    }



}
