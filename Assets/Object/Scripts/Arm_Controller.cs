using UnityEngine;
using System.Collections;

public class Arm_Controller : MonoBehaviour {

	public Transform[] Arm;

	Vector3 current_pos_of_ball;
	bool ball_in_zone=false;

	public Vector3 postion_of_ball
	{
		set
		{
			if(value!=null)
			{
				current_pos_of_ball=value;
				ball_in_zone=true;
			}
			else
			{
				ball_in_zone=false;
			}
		}
	}
	public void setMyPresions(bool b)
	{
		ball_in_zone = b;
	}
	void Awake()
	{
	}
	void LateUpdate()
	{
		if(Arm.Length>=3&&current_pos_of_ball!=null&&ball_in_zone)
		{
			for(int i=0;i<Arm.Length;i++)
			{
				Arm[i].localRotation=Quaternion.Euler(new Vector3());
			}
			//transform.LookAt (current_pos_of_ball);
			//transform.rotation = Quaternion.FromToRotation(Vector3.up, transform.forward);
			//transform.forward = Arm [2].position - Arm [0].position;
			//transform.LookAt (current_pos_of_ball);
			transform.rotation = Quaternion.FromToRotation (Arm[2].position-Arm[0].position,current_pos_of_ball-Arm[0].position)*transform.rotation;
		}
	}

}
