using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class CycleInfo : MonoBehaviour
{
  public Cycle cycle;
  public bool isForm;
  public bool isBinder;
  public bool isLife;
  public List<CycleInfo> siblings;
  public List<CycleInfo> children;
  public CycleInfo parent;
  public int count;
  public int level;
  public Vector3 vel;
  public LineRenderer lr;


}
