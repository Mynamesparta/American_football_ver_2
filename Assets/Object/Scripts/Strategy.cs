using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class Strategy : MonoBehaviour 
{
	Game_Controller contr;
	public static Canvas canvas;
	public delegate void nullMethod();
	public event nullMethod destroy;
	public event nullMethod update;
	public Options OPTIONS;
	public Colors COLORS;
	public Clone_of_Object CLONE;

	List<Activities> List_of_Acrivities=new List<Activities>();
	Activities current_Activities;
	void Awake()
	{
		contr = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<Game_Controller> ();
		Activities.strategy = this;
		Line.strategy = this;
		Action.strategy = this;
		Vector.strategy = this;
	}
	void DestroyStrategy()
	{
		if(destroy!=null)
		{
			destroy();
			destroy=null;
		}
		List_of_Acrivities.Clear ();
	}
	public void Ini()
	{
		DestroyStrategy ();
		List<PlayerMovement> players = contr.getCurrentTeam ();
		for(int i=0;i<players.Count;i++)
		{
			List_of_Acrivities.Add(new Activities(players[i]));
		}
	}
	public void fromString(List<string> list)
	{
		Ini ();
		int j=0;
		List<string> list_2=new List<string>();
		for(int i=0;i<list.Count;i++)
		{
			if(list[i]=="newA")
			{
				//MonoBehaviour.print("new Activities:");
				if(list_2!=null&&list_2.Count!=0)
				{
					List_of_Acrivities[j].fromString(list_2);
					j++;
				}
				list_2=new List<string>();
			}
			else
			{
				list_2.Add(list[i]);
				//MonoBehaviour.print(list[i]);
			}

		}
		if(list_2!=null&&list_2.Count!=0)
		{
			List_of_Acrivities[j].fromString(list_2);
			j++;
		}
	}
	public List<string> toString()
	{
		List<string> list = new List<string> ();
		List<string> list_2;
		int j;
		for(int i=0;i<List_of_Acrivities.Count;i++)
		{
			list.Add("newA");
			//MonoBehaviour.print("newA");
			list_2=List_of_Acrivities[i].toString();
			for(j=0;j<list_2.Count;j++)
			{
				list.Add(list_2[j]);
				//MonoBehaviour.print(list_2[j]);
			}
		}
		return list;
	}
	public void SetCurrentActivities(PlayerMovement player)
	{
		if(current_Activities!=null)
		{
			EndBuildTime();
		}
		current_Activities = player.getAction ();
		//MonoBehaviour.print ("hello:" + player.name);
	}
	public void Update()
	{
		if(update!=null)
		{
			update();
		}
	}
	//=====================================Button======================
	public void setPanel_in_Scene (bool b)
	{
		canvas.inScene (name_of_Button.Editor_Panel, b);
	}
	public void MoveAction()
	{
		MonoBehaviour.print ("Hello MoveAction");
		if (current_Activities == null) 
		{
			MonoBehaviour.print("hello");
			return;
		}
		Move move = Activities.createAction (Action.name_of_Action.Move) as Move;
		current_Activities.Add (move);
	}
	public void StopAction()
	{
		if (current_Activities == null)
			return;
		MonoBehaviour.print ("Hello StopAction");
		Action action = Activities.createAction (Action.name_of_Action.Stop) as Action;
		current_Activities.Add (action);

	}
	public void VectorMoveAction()
	{
		if (current_Activities == null)
			return;
		MonoBehaviour.print ("Hello VectorMoveAction");
		Action action = Activities.createAction (Action.name_of_Action.VectorMove) as Action;
		current_Activities.Add (action);
	}
	public void BallAction()
	{
		if (current_Activities == null)
			return;
		current_Activities.Last ().WhereIsMyBall ();
	}
	public void Back()
	{
		if (current_Activities == null)
			return;
		current_Activities.Back ();
		//current_Activities.Back ();
	}
	public void Remove_Action()
	{
		if (current_Activities == null)
			return;
		current_Activities.RemoveLast ();
	}
	public void EndBuildTime()
	{
		if (current_Activities == null)
			return;
		if (current_Activities.getLenght() == 0)
			return;
		current_Activities.Last ().BuildTime (false);
	}
	public void EndEditInputField(string number)
	{
		if (current_Activities == null)
			return;
		if (number == "" || number == " ")
			number = "0";
		float num = float.Parse (number);
		MonoBehaviour.print (number + "==" + num.ToString ());
		current_Activities.Last ().updateValue (num);
	}

	//========================================Actiction========================================
	public class Activities
	{
		public static Counter counter;
		public PlayerMovement player;
		public Vector3 startPosition;
		public static Strategy strategy;
		List<Action> list_of_Action = new List<Action> ();
		bool _isWork=false;
		int currentIndex_of_Action;
		public bool isWork
		{
			get{ return _isWork;}
		}
		public bool Action_With_Ball
		{
			get{return currentAction.WithBall;}
		}
		public Action currentAction
		{
			get
			{
				MonoBehaviour.print(currentIndex_of_Action);
				return list_of_Action[currentIndex_of_Action];
			}
		}
		public Activities(PlayerMovement p)
		{
			strategy.destroy+=Destroy;
			player=p;
			player.SetAction(this);
			startPosition=player.transform.position;
			player.OPTION.isBlockPickUp=false;
			/*/
			Move move=new Move();
			list_of_Action.Add(move);
			/*/
		}
		public void Destroy()
		{
			for(int i=0;i<list_of_Action.Count;i++)
			{
				list_of_Action[i].Destroy();
			}
			player.DestroyAction ();
			MonoBehaviour.print ("Destroy");
		}
		public List<string> toString()
		{
			List<string> list = new List<string> ();
			//MonoBehaviour.print (player.transform.position.ToString ());
			int j;
			List<string> list_2;
			for(int i=0;i<list_of_Action.Count;i++)
			{
				list.Add("newAc");
				list.Add(list_of_Action[i].getName());
				list.Add(list_of_Action[i].WithBall.ToString());
				list_2=list_of_Action[i].toString();
				for(j=0;j<list_2.Count;j++)
				{
					list.Add(list_2[j]);
				}
			}
			return list;
		}
		public void fromString(List<string> list)
		{
			//MonoBehaviour.print (player.transform.position.ToString ());
			List<string> list_2 = new List<string> ();
			Action action=new Move();
			for(int i=0;i<list.Count;i++)
			{
				if(list[i]=="newAc")
				{
					if(list_2!=null&&list_2.Count!=0)
					{
						if(action!=null)
							action.fromString(list_2);
					}
					if(i+2>=list.Count)
					{
						break;
					}
					i++;
					action=createAction(list[i]);
					action.player=player;
					i++;
					if(list[i]=="true"||list[i]=="True")
					{
						action.WhereIsMyBall();
					}
					list_of_Action.Add(action);
					list_2=new List<string>();
				}
				else
				{
					list_2.Add(list[i]);
				}
			}
			if(list_2!=null&&list_2.Count!=0)
			{
				if(action!=null)
					action.fromString(list_2);
			}
			Start_Game ();
			//player.transform.position = list_of_Action [0].startPos;
		}
		public Action Last()
		{
			return list_of_Action [list_of_Action.Count - 1];
		}
		public void Add(Action act)
		{
			Action last; 
			if(getLenght()!=0)
			{
				last = Last ();
				last.BuildTime (false);
				act.startPos=last.endPos;
			}
			else
			{
				act.startPos=player.transform.position;
			}
			//MonoBehaviour.print(act.startPos);
			act.player = player;
			list_of_Action.Add (act);
			act.BuildTime (true);
			player.OPTION.isBlockPickUp = true;
		}
		public void RemoveLast()
		{
			if (list_of_Action.Count == 0) 
			{
				return;
			}
			Last ().Destroy ();
			list_of_Action.RemoveAt (list_of_Action.Count - 1);
			
			if (list_of_Action.Count == 0) 
			{
				player.OPTION.isBlockPickUp=false;
			}
		}
		public int getLenght()
		{
			return list_of_Action.Count;
		}
		public void Back()
		{
			if (list_of_Action.Count == 0)
				return;
			Last ().Back ();
		}
		//=====================================Game========================
		bool _newAction;
		public bool newAction
		{
			get
			{
				if(_newAction)
				{
					_newAction=false;
					return true;
				}
				return false;
			}
		}
		public void Time_to_Next_Action()
		{
			if (!_isWork)
				return;
			currentAction.WithBall = false;
			if(currentAction.getName_of_Action()!=Action.name_of_Action.Move)
			{
				if(!_Next_Action())
				{
					return;
				}
			}
		}
		public void Start_Game()
		{
			MonoBehaviour.print ("start");
			currentIndex_of_Action = 0;
			if(currentIndex_of_Action>=list_of_Action.Count)
			{
				return;
			}
			_isWork = true;
			player.transform.position =  list_of_Action[0].startPos;
			list_of_Action [currentIndex_of_Action].StartPlay ();
			_newAction = true;
		}
		public Vector3 getCurrentStrategyPoint()
		{
			if(currentIndex_of_Action>=list_of_Action.Count)
			{
				_isWork=false;
				//MonoBehaviour.print("hello Exit");
				return new Vector3();
			}
			Vector3 strategyPoint = currentAction.getCurrentStrategyPoint ();
			//MonoBehaviour.print ("strategyPoint:"+strategyPoint.ToString ());
			if(currentAction.endPlay)
			{
				if(!_Next_Action())
				{
					return new Vector3();
				}
				return getCurrentStrategyPoint();
			}

			return strategyPoint;
		}
		public  Vector3 getCurrentStrategyPoint(Vector3 other_player)
		{
			if(currentIndex_of_Action>=list_of_Action.Count)
			{
				_isWork=true;
				return new Vector3();
			}
			Vector3 strategyPoint = currentAction.getCurrentStrategyPoint (other_player);
			if(currentAction.endPlay)
			{
				if(!_Next_Action())
				{
					return new Vector3();
				}
				return getCurrentStrategyPoint(other_player);
			}
			return strategyPoint;
		}
		private bool _Next_Action()
		{
			MonoBehaviour.print ("Next Action:" + player.name);
			currentIndex_of_Action++;
			if(currentIndex_of_Action>=getLenght())
			{
				_isWork=false;
				//MonoBehaviour.print("end Activitis..");
				return false;
			}
			else
			{
				_newAction=true;
				currentAction.StartPlay();
			}
			return true;
		}
		//=====================================Builder=====================
		public static Action createAction(string _name)
		{
			return createAction(Action.fromStringToName(_name));
		}
		public static Action createAction(Action.name_of_Action _name)
		{
			switch(_name)
			{
			case Action.name_of_Action.Move:
			{
				Action action = new Move();
				//action.player=
				return action;
			}
			case Action.name_of_Action.Stop:
			{
				Action action= new Stop();
				return action;
			}
			case Action.name_of_Action.VectorMove:
			{
				Action action= new VectorMove();
				return action;
			}
			}
			return null;
			//strategy.update += action;
		}


		
	}
	[System.Serializable]
	public struct Options
	{
		public float Lenght_of_line;
		public float Zone_of_remove_point;
		public float TimePause_before_paint;
	}
	[System.Serializable]
	public struct Clone_of_Object
	{
		public GameObject Line;
		public GameObject Stop;
		public GameObject Vector;
	}
	[System.Serializable]
	public struct Colors
	{
		public Color Ignore_Ball;
		public Color Action_With_Ball;
	}
	public void AwakeStaticDate()
	{
		Move.Clone_of_line = CLONE.Line;
		Stop.Clone_of_Stop = CLONE.Stop;
		MonoBehaviour.print ("Awake Static Date");
	}
	//======================================Action==============================================
	public abstract class Action
	{
		public abstract void Destroy();
		public abstract List<string> toString();
		public abstract void fromString(List<string> text);
		public abstract void BuildTime(bool b);
		public abstract void Back ();
		public abstract void updateStartPos ();
		public abstract void updateValue(float num);
		public abstract Vector3 getCurrentStrategyPoint();
		public abstract Vector3 getCurrentStrategyPoint(Vector3 other_player);

		protected abstract void updateWithBall();
		protected abstract void _StartPlay();

		public static Strategy strategy;
		public static Counter counter;
		public PlayerMovement player;
		name_of_Action name;
		protected Vector3 _startPos;
		protected Vector3 _endPos;
		protected bool _WithBall=false;
		protected bool _endPlay;
		public bool endPlay
		{
			get{ return _endPlay;}

		}
		public bool WithBall
		{
			get{return _WithBall;}
			set
			{ 
				_WithBall = value;
				updateWithBall();
			}
		}

		public Vector3 startPos { 
			get { return _startPos; } 
			set
			{
				_startPos = value;
				updateStartPos();
			}
		}
		public Vector3 endPos{ get { return _endPos; } }
		
		public static name_of_Action fromStringToName(string _name)
		{
			return (name_of_Action)Enum.Parse (typeof(name_of_Action), _name);
		}
		public void SetName(name_of_Action _name)
		{
			name = _name;
		}
		public void SetName(string _name)
		{
			name = fromStringToName (_name);
		}
		public string getName()
		{
			return name.ToString ();
		}
		public name_of_Action getName_of_Action()
		{
			return name;
		}
		public void WhereIsMyBall ()
		{
			WithBall = !WithBall;
			updateWithBall ();
		}
		public void StartPlay()
		{
			_endPlay = false;
			_StartPlay ();
		}
		public enum name_of_Action{Move,Stop,VectorMove};
	}
	public partial class Move: Strategy.Action
	{
	}
	public partial class Stop: Strategy.Action
	{
	}
	public partial class VectorMove: Strategy.Action
	{
	}
}
