using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class PlayerMovement : MonoBehaviour 
{
	//===============================Strategy=Move================
	internal Vector3? oponent;
	internal Vector3 newPosition;
	internal float rot, abs_rot;
	private void BotTime()
	{
		if (current_action == null)
			return;
		if (!current_action.isWork)
			return;
		//if(oponent==null)
		oponent = Time_for_Tangency ();
		if(!oponent.HasValue)
		{
			//MonoBehaviour.print(number_of_Opponent);
			newPosition = current_action.getCurrentStrategyPoint ();
		}
		else
		{
			//MonoBehaviour.print(newPosition.ToString());
			//MonoBehaviour.print(number_of_Opponent);
			newPosition = current_action.getCurrentStrategyPoint (oponent.Value);
			//if(Vector3.Distance(transform.position,(Vector3)oponent)>contr.OPTIONS.min_radius_of_Tangency)
			{
				newPosition=getPoints_of_Tangency(oponent.Value,transform.position)[0];
			}
		}
		rot=takeRot (newPosition);
		abs_rot=Mathf.Abs(rot);
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
	internal List<Armature> list_of_Opponent=new List<Armature>();
	public void Strategy_Move_Enter(Collider other)
	{
		if (other.tag != Tags.player)
			return;
		Armature player = other.GetComponent<Armature> ();
		number_of_Opponent++;
		MonoBehaviour.print (number_of_Opponent);
		list_of_Opponent.Add (player);
	}
	public void Strategy_Move_Exit(Collider other)
	{
		if (other.tag != Tags.player)
			return;
		Armature player = other.GetComponent<Armature> ();
		if(number_of_Opponent==0)
		{
			MonoBehaviour.print("Something wrong: number_of_opponent<0");
			return;
		}
		number_of_Opponent--;
		MonoBehaviour.print (number_of_Opponent);
		list_of_Opponent.Remove (player);
	}
	public Vector3[] getPoints_of_Tangency(Vector3 O,Vector3 A)
	{
		Vector2 o, a;
		o.x = O.x;
		o.y = O.z;
		a.x = A.x;
		a.y = A.z;
		Vector3[] result=new Vector3[]{new Vector3(),new Vector3()};
		Vector2[] _result = _getPoints_of_Tangency (o, a, Game_Controller.current_contr.OPTIONS.min_radius_of_Tangency);
			result[0].x=_result[0].x;
			result[0].z=_result[0].y;
			result[1].x=_result[1].x;
			result[1].z=_result[1].y;
		return result;
	}
	internal float[] C=new float[6];
	Vector2[] _getPoints_of_Tangency(Vector2 O,Vector2 A,float r)
		/*/
		 * Пошук точок перетину кола, з центром О та радіосом r, та двох прамих, що є дотичними до коло та проходять через точку A.
		/*/
	{
		C [0] = A.x - O.x;
		C [1] = A.y - O.y;
		C [4] = C [0] / C [1];
		//MonoBehaviour.print (C [4]);
		C [0] = C[0]*C[0]+C[1]*C[1];
		C [0] = 2*r * r - C [0];
		C [3] = (A.y - O.y) / 2;
		/*/
		MonoBehaviour.print ("==================================");
		MonoBehaviour.print (O.ToString ());
		MonoBehaviour.print (A.ToString ());
		MonoBehaviour.print ("==================================");
		/*/
		if(C[3]==0)
		{
			MonoBehaviour.print("Magic!!!!!");
			C[3]=0.0001f;
		}
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
			//return new Vector2[]{new Vector2(),new Vector2()};
			C[0]=-C[0];
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
	Vector3? Time_for_Tangency()
	{
		for(int i=0;i<list_of_Opponent.Count;i++)
		{
			if(Vector3.Distance(transform.position,list_of_Opponent[i].position)<contr.OPTIONS.max_radius_of_Tangency)
			{
				return (Vector3?) list_of_Opponent[i].position;
			}
		}
		return null;
	}
}
