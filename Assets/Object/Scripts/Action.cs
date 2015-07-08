using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class Strategy : MonoBehaviour 
{
	public partial  class Move: Strategy.Action
	{
		public static GameObject Clone_of_line;
		public Line line;
		int current_Index;
		public override void Destroy()
		{
			MonoBehaviour.print ("Destroy Action");
			if(line!=null)
				GameObject.Destroy (line.gameObject);
		}
		public override List<string> toString()
		{
			List<string> list = line.toString ();
			return list;
		}
		public override void fromString(List<string> list)
		{
			line.fromString (list);
			_startPos = line.BeginPoint;
			_endPos = line.LastPoint;
		}
		public override void BuildTime(bool b)
		{
			MonoBehaviour.print ("Build Time:" + b.ToString ());
			line.setBuildTime (b);
			if(b)
			{
				line.setStartPoint(_startPos);
			}
			else
			{
				_endPos=line.LastPoint;
			}
		}
		public override void Back()
		{
			line.RemoveLast ();
		}
		public override void updateStartPos()
		{
		}
		public override void updateValue(float num)
		{
		}
		protected override void updateWithBall()
		{
			if(_WithBall)
			{
				line.setColor(strategy.COLORS.Action_With_Ball);
			}
			else
			{
				line.setColor(strategy.COLORS.Ignore_Ball);
			}
		}
		/*/
		//Vector3 temp;
		//float distance;
		//float current_way;
		//Vector3 lastPosition;
		Vector3 lastPoint;
		public override Vector3 getCurrentStrategyPoint()
		{
			//distance += Vector3.Distance (lastPosition, player.transform.position);
			//MonoBehaviour.print ("distance="+distance);
			while(Function(lastPoint,currentPoint,player.transform.position))//distance>=current_way||Vector3.Distance(currentPoint,player.transform.position)<=player.SPEED.maxSpeed*Time.deltaTime)
			{
				MonoBehaviour.print ("Hello Function");
				lastPoint=currentPoint;
				//distance=0;//Vector3.Distance(player.transform.position,lastPoint);
				//MonoBehaviour.print ("new distance="+distance);
				current_Index++;
				if(current_Index>=line.Count-1)
				{
					_endPlay=true;
					return new Vector3 ();
				}
				//current_way=Vector3.Distance(lastPoint,currentPoint);
			}
			//lastPosition=player.transform.position;
			//MonoBehaviour.print (currentPoint.ToString() + " " + player.transform.position.ToString ());
			return currentPoint;
		}
		/*/
		public override Vector3 getCurrentStrategyPoint()
		{
			while(Vector3.Distance(player.transform.position,currentPoint)<Game_Controller.current_contr.OPTIONS.min_distance)
			{
				current_Index++;
				if(current_Index>=line.Count-1)
				{
					_endPlay=true;
					MonoBehaviour.print("end Action");
					return new Vector3();
				}
			}
			//MonoBehaviour.print ("currentPoint:"+currentPoint.ToString ());
			return currentPoint;
		}
		//
		public static float r;
		public override Vector3 getCurrentStrategyPoint(Vector3 other_player)
		{
			while(Vector3.Distance(player.transform.position,currentPoint)<Game_Controller.current_contr.OPTIONS.min_distance)
			{
				current_Index++;
				if(current_Index>=line.Count-1)
				{
					_endPlay=true;
					MonoBehaviour.print("end Action");
					return new Vector3();
				}
			}
			int lastIndex = current_Index;
			for(;current_Index<line.Count-1;current_Index++)
			{
				if(Vector3.Distance(currentPoint,other_player)<r)
				{
					while(Vector3.Distance(currentPoint,other_player)<r)
					{
						current_Index++;
						if(current_Index>=line.Count-1)
						{
							_endPlay=true;
							//MonoBehaviour.print("end Action");
							return new Vector3();
						}
					}
					return currentPoint;
				}
			}
			current_Index = lastIndex;
			return currentPoint;
		}
		protected override void _StartPlay()
		{
			_endPlay = false;
			current_Index = 0;
			//distance = 0;
			//lastPoint = currentPoint;//2 * currentPoint - line.getVec (1);
			//lastPoint = lastPosition;
			//current_Index++;
		}
		public Move()
		{
			SetName(name_of_Action.Move);
			line=(GameObject.Instantiate(Clone_of_line)as GameObject).GetComponent<Line>();
			//r=PlayerMovement.contr.OPTIONS.min_radius_of_Tangency;
			updateWithBall();
			MonoBehaviour.print("Welcome to Action MOve");
		}
		public Vector3 currentPoint
		{
			get{return line.getVec(current_Index);}
		}
		//==================================================Some_Function==============================
		public static bool Function(Vector3 point_1, Vector3 point_2,Vector3 player_pos)
		{
			Vector3 vec = point_2 - point_1;

			Vector3 inverse_vec = new Vector3 ();
			//MonoBehaviour.print (point_1.ToString () + " " + point_2.ToString ());
			inverse_vec.x = -vec.z;
			inverse_vec.z = vec.x;
			//MonoBehaviour.print (inverse_vec.ToString ());
			
			Vector3 point = point_2 + vec;
			
			return it_in_Left_side (point_2,inverse_vec, player_pos) == it_in_Left_side (point_2,inverse_vec, point);
			
		}
		public static bool  it_in_Left_side(Vector3 ver_1,Vector3 vec,Vector3 ver)
		{
			if (vec.x != 0)
				if(vec.x>0)
					return ver.y > (ver_1.y + vec.y / vec.x * (ver.x - ver_1.x));
			else
				return ver.y < (ver_1.y + vec.y / vec.x * (ver.x - ver_1.x));
			else
				return ver.x <= ver_1.x;
		}

	}
	
	public partial class Stop: Strategy.Action
	{
		public static GameObject Clone_of_Stop;
		GameObject stop;
		Vector3 startPoint;
		Animator anim;
		float timePause;
		float timer;
		public override void Destroy()
		{
			if(Clone_of_Stop!=null)
			{
				GameObject.Destroy(stop);
			}
		}
		public override List<string> toString()
		{
			List<string> list = new List<string> ();
			list.Add (_startPos.ToString ());
			list.Add (timePause.ToString());
			return list;
		}
		public override void fromString(List<string> text)
		{
			Vector3 vec = Line.fromString_to_Vector (text [0]);
			timePause = float.Parse (text [1]);
			_startPos = vec+counter.center;
			_endPos = _startPos;
			stop.transform.position = _startPos;

		}
		public override void BuildTime(bool b)
		{
		}
		public override void Back ()
		{
		}
		protected override void  updateWithBall()
		{
			anim.SetBool ("Ball", _WithBall);
		}
		
		public override void updateStartPos()
		{
			MonoBehaviour.print ("_startPos"+_startPos.ToString ());
			_endPos = _startPos;
			stop.transform.position = _startPos;
			//if(Clone
		}
		public override void updateValue(float num)
		{
			timePause = num;
		}
		public override Vector3 getCurrentStrategyPoint()
		{
			if(timer>=timePause)
			{
				_endPlay=true;
				return new Vector3();
			}
			timer += Time.deltaTime;
			return startPos;
		}
		public override Vector3 getCurrentStrategyPoint(Vector3 other_player)
		{
			return getCurrentStrategyPoint();
		}
		protected override void _StartPlay()
		{
			timer = 0;
		}
		public Stop()
		{
			SetName(name_of_Action.Stop);
			stop=GameObject.Instantiate(Clone_of_Stop)as GameObject;
			anim=stop.GetComponent<Animator>();
		}
	}
	
	public partial class VectorMove: Strategy.Action
	{
		Vector vectorMove;
		float time;
		public override void Destroy()
		{
			GameObject.Destroy (vectorMove.gameObject);
		}
		public override List<string> toString()
		{
			List<string> list = new List<string> ();
			list.Add (_startPos.ToString());
			list.Add (vectorMove.getVector ().ToString ());
			list.Add (time.ToString ());
			return list;
		}
		public override void fromString(List<string> text)
		{
			Vector3 vec;
			vec = Line.fromString_to_Vector (text [0]);
			_startPos = vec+counter.center;
			updateStartPos ();
			vec = Line.fromString_to_Vector (text [1]);
			vectorMove.setVector (vec);
			time = float .Parse (text [2]);
		}
		public override void BuildTime(bool b)
		{
			vectorMove.setBuildTime (b);
		}
		public override void Back ()
		{
		}
		public override void updateStartPos ()
		{
			vectorMove.setStartPoint (_startPos);
			_endPos = _startPos;
		}
		public override void updateValue(float num)
		{
			time = num;
		}
		protected override void updateWithBall()
		{
			if(_WithBall)
			{
				vectorMove.setColor(strategy.COLORS.Action_With_Ball);
			}
			else
			{
				vectorMove.setColor(strategy.COLORS.Ignore_Ball);
			}
		}
		float timer;
		public override Vector3 getCurrentStrategyPoint()
		{
			if(timer>=time)
			{
				_endPlay=true;
				return new Vector3();
			}
			timer += Time.deltaTime;
			return player.transform.position+vectorMove.getVector();
		}
		public override Vector3 getCurrentStrategyPoint(Vector3 other_player)
		{
			return getCurrentStrategyPoint();
		}
		protected override void _StartPlay()
		{
			timer = 0;
		}
		public VectorMove()
		{
			vectorMove=(GameObject.Instantiate(strategy.CLONE.Vector)as GameObject).GetComponent<Vector>();
			SetName(name_of_Action.VectorMove);
		}

	}
}


