using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Animator))]
public class IKController : Cycle
{
   
   public bool ikActive;
   public Transform Head;
   public Transform LeftHand;
   public Transform RightHand;
   public Transform LeftFoot;
   public Transform RightFoot;

   public float HeadWeight;
   public float LeftHandWeight;
   public float RightHandWeight;
   public float LeftFootWeight;
   public float RightFootWeight;


   public Animator animator;
   public override void Create(){
       animator  = GetComponent<Animator>();
   }

 void OnAnimatorIK()
  {


        if(animator) {  //if the IK is active, set the position and rotation directly to the goal. 
            if(ikActive) {

                // Set the look target position, if one has been assigned
                if(Head != null) {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(Head.position);
                }    

                // Set the right hand target position and rotation, if one has been assigned
                if(RightHand != null && RightHandWeight != 0 ) {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand,RightHandWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand,RightHandWeight);  
                    animator.SetIKPosition(AvatarIKGoal.RightHand,RightHand.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand,RightHand.rotation);
                }  


                 // Set the right hand target position and rotation, if one has been assigned
                if(LeftHand != null && LeftHandWeight != 0 ) {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,LeftHandWeight);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,LeftHandWeight);  
                    animator.SetIKPosition(AvatarIKGoal.LeftHand,LeftHand.position);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand,LeftHand.rotation);
                }      
                
            } else {

                // Set it all          
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
                animator.SetLookAtWeight(0);
            }
        }
    }

}
