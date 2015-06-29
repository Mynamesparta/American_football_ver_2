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
		float rot=takeRot (newPosition);
		float abs_rot=Mathf.Abs(rot);
		if (Vector3.Distance( newPosition, transform.position)<contr.OPTIONS.min_distance)
		{
			MovementManagement(0,0);
			return;
		}
		else
		{
			//MonoBehaviour.print("MovementManagement("+rot+","+(1-abs_rot)+")");
			MovementManagement(rot,1-abs_rot*contr.OPTIONS.minSpeed_for_StM);
		}

	}
	public float takeRot(Vector3 point)
	{
		Quaternion qua = Quaternion.FromToRotation (transform.forward, point - transform.position);
		float rot = qua.y / contr.OPTIONS.max_angul_for_StM;
		rot = rot > 1 ? 1 : rot;
		rot = rot < -1 ? -1 : rot;
		return rot;
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
