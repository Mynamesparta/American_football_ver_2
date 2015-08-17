
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class PlayerMovement : MonoBehaviour 
{
	//===============================Strategy=Move================
	internal object _oponent_;
	internal Transform oponent=null;
	internal Vector3 newPosition;
	internal float rot, abs_rot;
	internal bool b = false;
	private int _number_of_Enemy_Team=0;
	public int Number_of_Enemy_Team
	{
		get{return _number_of_Enemy_Team;}
	}
	private void BotTime()
	{
		if (current_action == null)
			return;
		if (!current_action.isWork)
		{
			MovementManagement(0,0);
			return;
		}
		if(OPTION.pass_time)
		{
			//MonoBehaviour.print("Pass Time!");
			if(Ideal_player_for_pass!=null)
			{
				//MonoBehaviour.print(name+":pass time");
				Vector3 vec=Ideal_player_for_pass.transform.position-transform.position;
				//MonoBehaviour.print(name+"->"+Ideal_player_for_pass.name);
				if(Vector3.Angle(vec,transform.forward)<contr.OPTIONS_FOR_PLAYERS.min_angle_for_Rot)
				{
					//transform.LookAt(Ideal_player_for_pass.transform);
					if(OPTION.have_ball&&b)
					{
						Ideal_player_for_pass.Pass_Time (this);
						Pass_to_another_PlayerB(Ideal_player_for_pass.transform.position);
						b=false;
					}
				}
				MonoBehaviour.print(name+":"+takeRot (transform.position+vec));
				MovementManagement(takeRot (transform.position+vec),0);
			}
			return;
		}
		//MonoBehaviour.print ("okkkk....................");
		//if(oponent==null)
		_oponent_ = Time_for_Tangency ();
		if(_oponent_==null)
		{
			//MonoBehaviour.print(number_of_Opponent);
			newPosition = current_action.getCurrentStrategyPoint ();
		}
		else
		{
			oponent=(Transform)_oponent_;
			newPosition = current_action.getCurrentStrategyPoint (oponent.position);
			if(isTimetoTangency(oponent.position,newPosition-transform.position,transform.position))
			{
				newPosition=getPoints_of_Tangency(oponent,transform.position);
			}
			else
			{
				_oponent_=null;
			}
		}
		if(current_action.newAction)
		{
			MonoBehaviour.print("new Action");
			Time_for_New_ActionSP();
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
		//,qua_1=Quaternion.FromToRotation (point - transform.position,transform.forward);
		/*/
		MonoBehaviour.print ("========================");
		MonoBehaviour.print (qua.ToString ());
		MonoBehaviour.print (qua_1.ToString ());
		MonoBehaviour.print ("========================");
		/*/
		//qua = Mathf.Abs( qua.y) < Mathf.Abs(qua_1.y) ? qua : qua_1;
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
		//MonoBehaviour.print (number_of_Opponent);
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
		//MonoBehaviour.print (number_of_Opponent);
		list_of_Opponent.Remove (player);
	}
	public Vector3 getPoints_of_Tangency(Transform O,Vector3 A)
	{
		Vector2[] _result = _getPoints_of_Tangency (toVector2(O.position),toVector2( A), contr.OPTIONS.min_radius_of_Tangency);
		Vector3[] result=new Vector3[2];
		result[0]=toVector3(_result[0]);
		result[1]=toVector3(_result[1]);
		/*/
		MonoBehaviour.print ("=================================");
		MonoBehaviour.print (result [0].ToString ());
		MonoBehaviour.print (result [1].ToString ());
		MonoBehaviour.print (O.position.ToString ());
		MonoBehaviour.print(Quaternion.FromToRotation(O.forward,result[0]-O.position).y);
		MonoBehaviour.print(Quaternion.FromToRotation(O.forward,result[1]-O.position).y);
		MonoBehaviour.print ("=================================");
		/*/
		return result [1];
		if(Quaternion.FromToRotation(O.forward,result[0]-O.position).y<Quaternion.FromToRotation(O.forward,result[1]-O.position).y)
		{
			return result[0];
		}
		else
		{
			return result[1];
		}
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
			C[0]=0;
		}
		C [2] = 2 * (C [2] * C [2] - 1);
		C [1] = Mathf.Sqrt (C [0]) / C [2];
		C [3] = C [3] / C [2];

		Vector2[] result = new Vector2[]{new Vector2(),new Vector2()};
		result [0].x = C [3] + C [1];
		result [1].x = C [3] - C [1];
		result [0].y = C [5] + result [0].x * C [4];
		result [1].y = C [5] + result [1].x * C [4];
		/*/
		MonoBehaviour.print ("------------");
		MonoBehaviour.print (result [0].ToString ());
		MonoBehaviour.print (result [1].ToString ());
		MonoBehaviour.print ("------------");
		/*/
		return result;

	}
	object Time_for_Tangency()
	{
		for(int i=0;i<list_of_Opponent.Count;i++)
		{
			if(Vector3.Distance(transform.position,list_of_Opponent[i].position)<contr.OPTIONS.max_radius_of_Tangency)
			{
				return (object) list_of_Opponent[i].transform;
			}
		}
		return oponent;
	}
	static public Vector2 toVector2(Vector3 vec)
	{
		Vector2 res = new Vector2 ();
		res.x = vec.x;
		res.y = vec.z;
		return res;
	}
	static public Vector3 toVector3(Vector2 vec)
	{
		Vector3 res = new Vector3 ();
		res.x = vec.x;
		res.z = vec.y;
		return res;
	}
	static public float distance(Vector3 point,Vector3 line,Vector3 point_in_line)
	{
		return distance (toVector2 (point), toVector2 (line), toVector2 (point_in_line));
	}
	static public float distance(Vector2 point, Vector2 line,Vector3 point_in_line)
	{
		float D=Mathf.Sqrt(line.x*line.x+line.y*line.y);
		float c=-line.x*point_in_line.x-line.y*point_in_line.y;
		return Mathf.Abs ((point.x*line.x+point.y*line.y+c)/D);
	}
	public bool isTimetoTangency(Vector3 point,Vector3 line,Vector3 point_in_line)
	{
		Vector3 deltaPoint = transform.position + transform.forward;
		if (Vector3.Distance (point, deltaPoint) > Vector3.Distance (point, transform.position))
			return false;
		return distance (point, line, point_in_line) < contr.OPTIONS.min_radius_of_Tangency;
	}
	//======================================Strategy=Action=====================================================
	void Time_for_New_ActionSP()//StrategyPass
	{
		MonoBehaviour.print (name+":Time_for_New_ActionSP");
		if(!OPTION.have_ball)
		{
			if(OPTION.search_ball&&!current_action.Action_With_Ball)
			{
				contr.Remove_Player_AWB(this);
			}
			OPTION.search_ball=current_action.Action_With_Ball;
			if(OPTION.search_ball)
			{
				contr.Add_Player_AWB(this);
			}
		}
		else
		{
			OPTION.try_to_pass=current_action.Action_With_Ball;
			if(OPTION.try_to_pass)
			{
				List<PlayerMovement> players=contr.getPlayer_AWB();
				if(Time_Strategy_Pass(players))//;
				//
				{
					contr.Clear_Player_AWB();
					b=true;
					//cont
				}
				//
			}
		}
	}
	public void NextAction()
	{
		current_action.Time_to_Next_Action();
	}
	internal PlayerMovement Ideal_player_for_pass;
	bool Time_Strategy_Pass(List<PlayerMovement> players)
	{
		if (players.Count == 0)
			return false;
		int i = 0;
		for(;i<players.Count;i++)
		{
			if(I_Can_Pass(players[i].transform.position))
			{
				Ideal_player_for_pass=players[i];
				break;
			}
		}
		if (i >= players.Count)
			return false;
		for(;i<players.Count;i++)
		{
			if(players[i].Number_of_Enemy_Team < Ideal_player_for_pass.Number_of_Enemy_Team
			   &&I_Can_Pass(players[i].transform.position))
			{
				Ideal_player_for_pass=players[i];
			}
		}
		OPTION.pass_time = true;
		return true;

	}
	public void new_Players_Strategy_Pass(List<PlayerMovement> players)
	{
		if(Time_Strategy_Pass(players))//;
		//
		{
			contr.Clear_Player_AWB();
			b=true;
			//current_action.Time_to_Next_Action();
			//cont
		}
		//
	}
	public void Pass_Time(PlayerMovement player)
	{
		Ideal_player_for_pass = player;
		OPTION.pass_time = true;
	}
	public void Time_To_Next_Action()
	{
		if(current_action!=null)
			current_action.Time_to_Next_Action ();
	}
	void Number_of_ET_Enter(Collider other)
	{
		if(other.tag!=Tags.player)
			return;
		if(!other.GetComponent<Armature>().itsMyTeam(team))
		{
			_number_of_Enemy_Team++;
		}
	}
	void Number_of_ET_Exit(Collider other)
	{
		if(other.tag!=Tags.player)
			return;
		if(!other.GetComponent<Armature>().itsMyTeam(team))
		{
			_number_of_Enemy_Team--;
		}
		if(_number_of_Enemy_Team<0)
		{
			MonoBehaviour.print("Something wrong:_number_of_Enemy_Team < 0");
			_number_of_Enemy_Team=0;
		}
	}
}
