using UnityEngine;
using System.Collections;

public partial class PlayerMovement : MonoBehaviour 
{
	//===============================Strategy=Move================
	private void BotTime()
	{
		if (current_action == null)
			return;
		if (!current_action.isWork)
			return;
		Vector3 newPosition = current_action.getCurrentStrategyPoint ();

		Quaternion qua = Quaternion.FromToRotation (transform.forward, newPosition - transform.position);
		float angle = qua.y;
		//angle
		//MonoBehaviour.print ("angle:" + angle);
		float rot = angle / contr.OPTIONS.max_angul_for_StM;
		rot = rot > 1 ? 1 : rot;
		rot = rot < -1 ? -1 : rot;
		float abs_rot=Mathf.Abs(rot);
		if(false&&!isBot)
		{
			MonoBehaviour.print("=========================================================");
			MonoBehaviour.print ("qua:" + qua.ToString ());
			MonoBehaviour.print ("rot:" + rot);
			MonoBehaviour.print ("angle:" + angle);
			MonoBehaviour.print("=========================================================");
		}
		if (Vector3.Distance( newPosition, transform.position)<contr.OPTIONS.min_distance)
		{
			MovementManagement(0,0);
			return;
		}
			MonoBehaviour.print("MovementManagement("+rot+","+(1-abs_rot)+")");
			MovementManagement(rot,1-abs_rot*contr.OPTIONS.minSpeed_for_StM);
		/*/
		if(Mathf.Abs( angle)<contr.OPTIONS.max_angul_for_StM)
		{
			if(contr.OPTIONS.Gods_of_Rotation||Mathf.Abs(angle)<contr.OPTIONS.delta_angul_for_StM)
			{
				transform.LookAt(newPosition);
				//MonoBehaviour.print(newPosition.ToString());
				MovementManagement(0,1);
			}
			else
			{

				if(!Strategy.Move.it_in_Left_side(transform.position+transform.forward,transform.position,newPosition))//isRightRotation(transform.position,transform.position+transform.forward,newPosition))
				{
					MovementManagement(rot,1-rot);
				}
				else
				{
					MovementManagement(-rot,1-rot);
				}
			}

		}
		else
		{
			MovementManagement(rot,0);
			//
			if(Mathf.Abs(angle)<contr.OPTIONS.delta_angul_for_StM)
			{
				transform.LookAt(newPosition);
				//MovementManagement(0,1);
			}
			else
			{
				//
				if(isRightRotation(transform.position,transform.position+transform.forward,newPosition))
				{
					MovementManagement(rot,0);
				}
				else
				{
					MovementManagement(-rot,0);
				}
				//
			}
			//
		}
		/*/
	}
	public void StartPlay()
	{
		inPlay = true;
	}
	public void EndPlay()
	{
		inPlay = false;
	}
	bool isRightRotation(Vector3 last,Vector3 begin,Vector3 end)
	{
		Vector3 a = begin - last;
		Vector3 b = end - begin;
		//print ("angle:"+(a.x +"*"+ b.y +"-"+ a.y +"*"+ b.x));
		return a.x * b.y - a.y * b.x <= 0;
	}
	
	public void Strategy_Move_Stay(Collider other)
	{
	}
	public void Strategy_Move_Exit(Collider other)
	{
	}
}
