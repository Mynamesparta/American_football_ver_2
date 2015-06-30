using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	internal int number_of_Opponent=0;
	internal List<Transform> list_of_Opponent;
	public void Strategy_Move_Enter(Collider other)
	{
		MonoBehaviour.print (other.name);
		return;
		number_of_Opponent++;
		if(list_of_Opponent==null)
		{
			list_of_Opponent=new List<Transform>();
		}
		list_of_Opponent.Add (other.transform);
	}
	public void Strategy_Move_Exit(Collider other)
	{
		MonoBehaviour.print (other.name);
		return;
		if(number_of_Opponent==0)
		{
			MonoBehaviour.print("Something wrong: number_of_opponent<0");
			return;
		}
		number_of_Opponent--;
		list_of_Opponent.Remove (other.transform);
	}
	internal float[] C=new float[6];
	public Vector2[] getPoints_of_Tangency(Vector2 O,Vector2 A,float r)
		/*/
		 * Пошук точок перетину кола, з центром О та радіосом r, та двох прамих, що є дотичними до коло та проходять через точку A.
		/*/
	{
		C [0] = A.x - O.x;
		C [1] = A.y - O.y;
		C [4] = C [0] / C [1];
		C [0] = C[0]*C[0]+C[1]*C[1];
		C [0] = r * r - C [0];

		C [3] = (A.y - O.y) / 2;
		C [2] = (C [0] + A.x * A.x - O.x * O.x) / (4 * C [3]);
		C [0] = C [2] + C [3];
		C [1] = C [2] - C [3];
		C [5] = C [2] + (A.y + O.y) / 2;

		C [2] = C [4];
		C [3] = C [2] * (C [0] + C [1]) - (O.x + A.x);
		C [0] = C [3] * C [3] - 4 * (C [0] * C [1] - O.x * A.x)*(C [2] * C [2] - 1);
		if(C[0]<0)
		{
			MonoBehaviour.print("Something wrong in function getPoints_of_Tangency: D<0");
			return new Vector2[]{new Vector2(),new Vector2()};
		}
		C [2] = 2 * (C [2] * C [2] - 1);
		C [1] = Mathf.Sqrt (C [0]) / C [2];
		C [3] = C [3] / C [2];

		Vector2[] result = new Vector2[]{new Vector2(),new Vector2()};
		result [0].x = C [3] + C [1];
		result [1].x = C [3] - C [1];
		result [0].y = C [5] + result [0].x * C [4];
		result [1].y = C [5] + result [1].x * C [4];
		return result;

	}
}
