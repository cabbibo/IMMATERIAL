using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class CycleInfoCPU : MonoBehaviour
{
  public Cycle cycle;
  public bool isForm;
  public bool isBinder;
  public bool isLife;
  public List<CycleInfoCPU> siblings;
  public List<CycleInfoCPU> children;
  public CycleInfoCPU parent;
  public int count;
  public int level;
  public Vector3 vel;
  public LineRenderer lr;


}
