using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))] 
public class IKGetBall : MonoBehaviour {
	protected Animator animator;
	
	public bool ikActive = false;
	public Transform rightHandObj = null;
	
	void Start () 
	{
		animator = GetComponent<Animator>();
	}
	
	//a callback for calculating IK
	void OnAnimatorIK()
	{
		print("hello");
		if(animator) {
			print("hello2");
			
			//if the IK is active, set the position and rotation directly to the goal. 
			if(ikActive) {
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1.0f);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1.0f);
				//set the position and the rotation of the right hand where the external object is
				if(rightHandObj != null) {
					animator.SetIKPosition(AvatarIKGoal.RightHand,rightHandObj.position);
					animator.SetIKRotation(AvatarIKGoal.RightHand,rightHandObj.rotation);
				}                   
				
			}
			
			//if the IK is not active, set the position and rotation of the hand back to the original position
			else {          
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0);             
			}
		}
	}
}
