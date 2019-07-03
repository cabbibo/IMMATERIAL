using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMoveTarget : MonoBehaviour
{

  public Character character;
  public Transform marker;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        character.SetMoveTarget( clickedPosition );

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit , 100)){
              character.SetMoveTarget(hit.point);
              marker.position = hit.point;
            }
        
    }
}
