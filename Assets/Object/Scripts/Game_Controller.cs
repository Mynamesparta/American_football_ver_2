using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game_Controller : MonoBehaviour {
	public Scripts SCRIPTS;
	public Objects OBJECTS;
	public Clone_of_Object CLONE;
	public Options OPTIONS;
	public Options_for_Players OPTIONS_FOR_PLAYERS;
	public Anim ANIM;
	public PlayerMovement Player;
	public GameObject forTest;
	public State_of_Game_Controller state;
	public PlayerMovement.Team currentTeam;
	public Side currentSide;
	public static Game_Controller current_contr;
	public delegate void  nullFunction();
	public event nullFunction StartPlay;
	public event nullFunction EndPlay;
	public event nullFunction StartDown;
	public event nullFunction EndDown;

	private Canvas canvas;
	private List<PlayerMovement> BlackTeam,WhiteTeam;
	private List<PlayerMovement> Players_Action_WB;//люди, які чекають на подію м'яча
	private PlayerMovement current_player;
	private PlayerMovement current_player_WB;// ігрок з м'ячем
	// Use this for initialization
	void Awake () 
	{
		current_contr = this;
		//Player.setBot(true);
		//currentTeam = PlayerMovement.Team.White;
		SCRIPTS.main_camera.setRotation_by_Mouse (false);
		canvas = SCRIPTS.canvas.GetComponent<Canvas> ()as Canvas;
		BlackTeam = new List<PlayerMovement> ();
		WhiteTeam = new List<PlayerMovement> ();
		OPTIONS.Black_transform_gc=new Transform_GC[OPTIONS.size_of_team];
		OPTIONS.White_transform_gc=new Transform_GC[OPTIONS.size_of_team];
		Players_Action_WB = new List<PlayerMovement> ();
		int a;
		for(int i=0;i<OPTIONS.size_of_team;i++)
		{
			a=i+1;
			if((i+1)%2==1)
			{
				a=-a/2*OPTIONS.lenght;
				if(!OPTIONS.oneTeamInField||currentTeam==PlayerMovement.Team.Black)
				{
					OPTIONS.Black_transform_gc[i].position=new Vector3(-10,0 ,a);
					OPTIONS.Black_transform_gc[i].rotation=new Vector3(  0,90,0);
				}
				if(!OPTIONS.oneTeamInField||currentTeam==PlayerMovement.Team.White)
				{
					OPTIONS.White_transform_gc[i].position=new Vector3(10,0  ,a);
					OPTIONS.White_transform_gc[i].rotation=new Vector3( 0,-90,0);
				}
			}
			else
			{
				a=a/2*OPTIONS.lenght;
				if(!OPTIONS.oneTeamInField||currentTeam==PlayerMovement.Team.Black)
				{
					OPTIONS.Black_transform_gc[i].position=new Vector3(-10, 0,a);
					OPTIONS.Black_transform_gc[i].rotation=new Vector3(  0,90,0);
				}
				if(!OPTIONS.oneTeamInField||currentTeam==PlayerMovement.Team.White)
				{
					OPTIONS.White_transform_gc[i].position=new Vector3(10,  0,a);
					OPTIONS.White_transform_gc[i].rotation=new Vector3( 0,-90,0);
				}
			}
			if(!OPTIONS.oneTeamInField||currentTeam==PlayerMovement.Team.Black)
				BlackTeam.Add(Add(PlayerMovement.Team.Black,OPTIONS.Black_transform_gc[i]));
			if(!OPTIONS.oneTeamInField||currentTeam==PlayerMovement.Team.White)
				WhiteTeam.Add(Add(PlayerMovement.Team.White,OPTIONS.White_transform_gc[i]));
		}
		PlayerMovement.strategyCamera = OBJECTS.Strategy_Camera;
		Line.contr = this;
		Vector.contr = this;
		PlayerMovement.contr = this;
		Strategy.canvas = canvas;
		SCRIPTS.strategy.AwakeStaticDate ();
		SCRIPTS.strategy.Ini ();
	}
	void OnApplicationQuit()
	{

		if (!OPTIONS.destroy_before_Quit)
			return;
		//
		Object.Destroy (OBJECTS.WhiteTeam);
		Object.Destroy (OBJECTS.BlackTeam);
		/*/
		for(int i=0;i<WhiteTeam.Count;i++)
		{
			MonoBehaviour.Destroy(WhiteTeam[i].gameObject);
		}
		for(int i=0;i<BlackTeam.Count;i++)
		{
			MonoBehaviour.Destroy(BlackTeam[i].gameObject);
		}
		/*/
		WhiteTeam.Clear ();
		BlackTeam.Clear ();
		
		MonoBehaviour.Destroy (OBJECTS.Main_Camera.gameObject);
		MonoBehaviour.Destroy (OBJECTS.Strategy_Camera.gameObject);
		MonoBehaviour.Destroy (OBJECTS.Strategy_Focus);
		Destroy (SCRIPTS.canvas.gameObject);
		Destroy (SCRIPTS.counter.gameObject);
		Destroy (SCRIPTS.files.gameObject);
		Destroy (SCRIPTS.main_camera.gameObject);
		Destroy (SCRIPTS.pickUp.gameObject);
		Destroy (SCRIPTS.strategy.gameObject);
		System.GC.Collect();

		MonoBehaviour.print ("Bye");
	}
	bool First_Update=true;
	void Update () 
	{
		switch(state)
		{
		case State_of_Game_Controller.Edit:
		{
			if(Input.GetMouseButtonUp(0))
			{
				SCRIPTS.pickUp.isWork=false;
			}
			break;
		}
		}
		if(OPTIONS.Test_Down_End)
		{
			OPTIONS.Test_Down_End=false;
			if(EndDown!=null)
				EndDown();
		}
	} 
	void Start()
	{
		if (First_Update) 
		{
			canvas.gameObject.SetActive(true);
			MonoBehaviour.print("First_Update");
			First_Update=false;
			if(WhiteTeam.Count>0)
			{
				setCurrentPlayer (WhiteTeam [0]);
				if(OPTIONS.Player_who_have_ball!=-1)
					WhiteTeam[OPTIONS.Player_who_have_ball].getBall();
			}
			if(!OPTIONS.isTime_for_Test_Play)
				return;
			SCRIPTS.files.Read (OPTIONS.name_of_File);
			StartGame();
			Time_to_Play(true);
		}
	}
	public void StartGame()
	{
		if (OPTIONS.TestDestroy) 
		{
			OnApplicationQuit ();
			return;
		}
		SCRIPTS.counter.float_counter_1 =	OPTIONS.Start_Pos_of_Yellow_Line;
		SCRIPTS.pickUp.TimeToPickUp (false);
		MonoBehaviour.print ("Start Game");
		setCurrentPlayer (WhiteTeam [0]);
		//SCRIPTS.main_camera.setRotation_by_Mouse (true);
		canvas.inScene (name_of_Button.Menu, false);
		OBJECTS.Main_Camera.depth = 1f;
		OBJECTS.Strategy_Camera.depth = 0f;
		state = State_of_Game_Controller.Play;
		if(StartPlay!=null)
		{
			StartPlay();
		}
		else
		{
			//MonoBehaviour.print ("Something Wrong: StartPlay==null");
		}
		canvas.inScene (name_of_Button.FileList, true);
		canvas.inScene (name_of_Button.Counter, true);
		ChoseStrategy (true);

	}
	public void Time_to_Play(bool b)
	{
		if (b) 
		{
			if(StartPlay!=null)
			{
				StartPlay();
			}
			else
			{
				MonoBehaviour.print ("Something Wrong: StartPlay==null");
			}
		}
		else
		{
			if(EndPlay!=null)
			{
				EndPlay();
			}
			else
			{
				MonoBehaviour.print ("Something Wrong: EndPlay==null");
			}
		}
	}
	public void StartEditStrategy()
	{
		SCRIPTS.pickUp.TimeToPickUp (true);
		ChoseStrategy (false);
		MonoBehaviour.print ("Start Edit");
		canvas.inScene (name_of_Button.Menu, false);
		canvas.inScene (name_of_Button.FileList, true);
		canvas.inScene (name_of_Button.Editor, true);
		OBJECTS.Main_Camera.depth = 0f;
		OBJECTS.Strategy_Camera.depth = 1f;
		state = State_of_Game_Controller.Edit;
		//SCRIPTS.pickUp
		//Camera
		//SCRIPTS.main_camera.setCurrentObject (OBJECTS.Strategy_Focus);
		//SCRIPTS.main_camera.setState (Camera_controller.State_of_Camera.Strategy);
	}
	private void ChoseStrategy(bool b)
	{
		if (ANIM.ChoseStrategy == null) 
		{
			ANIM.ChoseStrategy = canvas.getAnimator (name_of_Button.File_Panel);
		}
		ANIM.ChoseStrategy.SetBool ("inGame", b);
		canvas.inScene (name_of_Button.FileList, b);
	}
	int Index_of_BlackTeam=0;
	int Index_of_WhiteTeam=0;
	public PlayerMovement Add(PlayerMovement.Team team,Transform_GC tran)
	{
		PlayerMovement player = (Object.Instantiate (CLONE.Zenor,tran.position,Quaternion.Euler( tran.rotation))as GameObject).GetComponent<PlayerMovement> ();
		player.team = team;
		StartPlay += player.StartPlay;
		EndPlay += player.EndPlay;
		switch(team)
		{
		case PlayerMovement.Team.Black:
		{
			player.transform.SetParent(OBJECTS.BlackTeam.transform);
			player.Name="Black_Team_Zenor_"+Index_of_BlackTeam;
			Index_of_BlackTeam++;
			break;
		}
		case PlayerMovement.Team.White:
		{
			player.transform.SetParent(OBJECTS.WhiteTeam.transform);
			player.Name="White_Team_Zenor_"+Index_of_WhiteTeam;
			Index_of_WhiteTeam++;
			break;
		}
		}
		return player;
	}
	public void PickMe(PlayerMovement Zenor)
	{
		if (!SCRIPTS.pickUp.isTimeToPickUp)
			return;
		if (Zenor.team != currentTeam)
			return;
		if (SCRIPTS.pickUp.Zenor != null) 
		{
			MonoBehaviour.print (SCRIPTS.pickUp.Zenor.name + "==" + Zenor.name);
			if (SCRIPTS.pickUp.Zenor.name == Zenor.name)
				return;
		}
		SCRIPTS.pickUp.Zenor = Zenor;
		SCRIPTS.pickUp.isWork = true;
		SCRIPTS.strategy.SetCurrentActivities (Zenor);
	}
	void setCurrentPlayer(PlayerMovement player)
	{
		if (current_player != null) 
		{
			current_player.setBot (true);
		}
		current_player = player;
		current_player.setBot(false);
		SCRIPTS.main_camera.setCurrentObject (current_player.gameObject);
		//current_player.setPick
	}
	public void setCurrentPlayerWB(PlayerMovement player)
	{
		current_player_WB = player;
	}
	//==========================================Strategy===========================================
	public List<PlayerMovement> getCurrentTeam()
	{
		switch(currentTeam)
		{
		case PlayerMovement.Team.Black:
		{
			return BlackTeam;
		}
		case PlayerMovement.Team.White:
		{
			return WhiteTeam;
		}
		}
		return null;
	}
	public Vector3 getCurrentMousePosition()
	{
		Vector3 pos = Input.mousePosition;
		pos=OBJECTS.Strategy_Camera.ScreenToWorldPoint (pos);
		//MonoBehaviour.print ("Hello:"+pos.ToString ());
		pos.y = 0;
		return pos;
	}
	public void Add_Player_AWB(PlayerMovement player)
	{

		if (player.team != currentTeam)
			return;
		Players_Action_WB.Add (player);
		if(current_player_WB!=null&&current_player_WB.is_Try_to_Pass())
		{
			current_player_WB.new_Players_Strategy_Pass(Players_Action_WB);
		}
	}
	public void Remove_Player_AWB(PlayerMovement player)
	{
		if (current_player_WB==null||player.team != current_player_WB.team)
			return;
		Players_Action_WB.Remove (player);
	}
	public List<PlayerMovement> getPlayer_AWB()
	{
		return Players_Action_WB;
	}
	public void Clear_Player_AWB()
	{
		for(int i=0;i<Players_Action_WB.Count;i++)
		{
			//Players_Action_WB[i].NextAction();
		}
		Players_Action_WB.Clear ();
	}
	//===============================struct======================
	[System.Serializable]
	public enum State_of_Game_Controller{Play,Edit};
	[System.Serializable]
	public struct Scripts
	{
		public GameObject canvas;
		public Camera_controller main_camera;
		public Camera_controller editor_camera;
		public Pick_Up pickUp;
		public Strategy strategy;
		public Counter counter;
		public File_Controller files;
	}
	[System.Serializable]
	public struct Objects
	{
		public GameObject Strategy_Focus;
		public Camera Main_Camera;
		public Camera Strategy_Camera;
		public GameObject WhiteTeam;
		public GameObject BlackTeam;
	}
	[System.Serializable]
	public struct Clone_of_Object
	{
		public GameObject Zenor;
	}
	[System.Serializable]
	public struct Options
	{
		public int size_of_team;
		public float Start_Pos_of_Yellow_Line;
		public int lenght;
		public bool oneTeamInField;
		public Transform_GC[] Black_transform_gc;
		public Transform_GC[] White_transform_gc;
		public string name_of_File;
		public bool All_Bot;
		public float max_angul_for_StM;
		public float delta_angul_for_StM;
		public bool Gods_of_Rotation;
		public float min_distance;
		public bool isTime_for_Test_Play;
		public bool Stasis;
		public float minSpeed_for_StM;
		public bool destroy_before_Quit;
		public bool TestDestroy;
		public float min_radius_of_Tangency;
		public float max_radius_of_Tangency;
		[Range(-1, 10)]
		public int Player_who_have_ball;
		public bool Test_Down_End;
	}
	[System.Serializable]
	public struct Options_for_Players
	{
		public int[] speeds_of_Ball;
		public int[] speeds_of_Ball_Kick_off;
		public PlayerMovement test_Zenor;
		public PlayerMovement.Angle_Pass angle;
		public bool Time_to_Strategy;
		public float min_angle_for_Rot;
		[Range(0f, 1f)]
		public float max_fow_AWB;
	}
	[System.Serializable]
	public struct Transform_GC
	{
		public Vector3 position;
		public Vector3 rotation;
	}
	[System.Serializable]
	public struct Anim
	{
		public Animator ChoseStrategy;
	}
	[System.Serializable]
	public enum Side{Left=-1,Right=1};

}
