using UnityEngine;
using System.Collections;

public partial class PlayerMovement : MonoBehaviour {
	public SpeedOptions SPEED;
	public Option OPTION;
	public Children_Transform CHILDREN;
	public Action_Collider COLLIDER;
	public Scripts SCRIPTS;
	public State_of_Player state=State_of_Player.Normal;
	public Team team;
	public float Forse;
	//public int maxForseWhenRun=5;
	public int mouseIndex=0;
	public Transform[] ball_position;
	public static Game_Controller contr;
	public string Name
	{
		get{return gameObject.name;}
		set
		{
			_Name=value;
			if(OPTION.isBot)
			{
				gameObject.name=_Name;
			}
		}
	}

	private Animator anim;              // Reference to the animator component.
	private Hash_id hash;               // Reference to the HashIDs.
	private float speed;
	private float angular_speed;
	private float jumpspeed;
	private float timer;
	private BallController ball_con;
	private int indexBallpos=0;
	private Arm_Controller[] arms;
	private Strategy.Activities current_action;
	private bool inPlay=false;
	private Oponents oponents;
	private string _Name;
	private static int[] speeds_of_Ball;

	//private bool Take_Ball = false;
	
	void Awake ()
	{
		setBallPosition (1);
		// Setting up the references.
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<Hash_id>();
		ball_con = GameObject.FindGameObjectWithTag (Tags.ball).GetComponent<BallController> ();
		SCRIPTS.contr = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<Game_Controller> ();
		arms = GetComponentsInChildren<Arm_Controller> ();
		timer = 0;
		// Set the weight of the shouting layer to 1.
		anim.SetLayerWeight(1, 1f);

		COLLIDER.Ball.Stay += Ball_Stay;
		COLLIDER.Ball.Exit += Ball_Exit;
		COLLIDER.Fall.Stay += Fall_Exit;
		COLLIDER.Fall.Exit += Fall_Exit;
		COLLIDER.Navigation.Enter += Strategy_Move_Enter;
		COLLIDER.Navigation.Exit += Strategy_Move_Exit;

		OPTION.isBot=true;
		OPTION.have_ball=false;
	}
	void Start()
	{
		Strategy.Move.r = contr.OPTIONS.max_radius_of_Tangency;
		/*/
		Vector2[] result=getPoints_of_Tangency(new Vector2(),new Vector2(1,-3),2);
		MonoBehaviour.print (result [0].ToString ());
		MonoBehaviour.print (result [1].ToString ());
		/*/
		speeds_of_Ball = contr.OPTIONS_FOR_PLAYERS.speeds_of_Ball;
	}
	
	
	void FixedUpdate ()
	{
		// Cache the inputs.
		if(!inPlay)
		{
			return;
		}
		if(!contr.OPTIONS.All_Bot&&!OPTION.isBot)
		{
			float rot = Input.GetAxis("Rotation");
			float fow = Input.GetAxis("Forward");
			MovementManagement(rot, fow);
			/*/
			Vector3 newPosition = current_action.getCurrentStrategyPoint ();
			
			Quaternion qua = Quaternion.FromToRotation (transform.forward, newPosition - transform.position);
			MonoBehaviour.print ("qua:" + qua.ToString ());
			MonoBehaviour.print("newPostion:"+newPosition);
			/*/
		}
		else
		{
			BotTime();
			/*/
			anim.SetFloat (hash.float_speed, 0f, SPEED.speedDampTime, Time.deltaTime);
			anim.SetFloat (hash.float_angular_speed, 0f, SPEED.speedDampTime, Time.deltaTime);
			ChangePosition();
			ChangeRotation();
			/*/
		}
		if(OPTION.have_ball)
		{
			ball_con.SetPosition(ball_position[indexBallpos]);
			ball_con.SetRotation(transform);
		}
	}
	//===========================================Pick=Up===================================================
	static public Camera strategyCamera;
	void OnMouseOver()
	{
		//MonoBehaviour.print("Welcome");
		if(Input.GetMouseButton(OPTION.Index_Of_Mouse_Button))
		{
			SCRIPTS.contr.PickMe(this);
		}
	}
	void OnMouseEnter()
	{
		return;
		MonoBehaviour.print ("Enter");
	}
	bool Pick=false;
	public bool isPick()
	{
		return Pick;
	}
	public void setPick(bool b)
	{
		Pick = b;
	}
	//==================================================================================================================
	void Update()
	{

		if(Vector3.Distance(strategyCamera.ScreenToWorldPoint( Input.mousePosition),transform.position)<OPTION.MouseLock)
		{
			MonoBehaviour.print("Hello");
		}
	}
	void MovementManagement (float rot, float fow)
	{
		//MonoBehaviour.print(Name+"_angularspeed:"+rot+"*"+SPEED.maxAngularSpeed);
		if(contr.OPTIONS.Stasis)
			fow = 0;
		//rot = Mathf.Sign (rot);
		//MonoBehaviour.print ("for:" + fow + "rot:" + rot);
		switch(state)
		{
		case State_of_Player.Normal:
		{
			ChangeRotation ();
			if (rot != 0f) //
			{
				anim.SetFloat (hash.float_angular_speed, rot*SPEED.maxAngularSpeed, SPEED.speedDampTime*0.5f, Time.deltaTime);
			}
			else
			{
				anim.SetFloat (hash.float_angular_speed, 0f, SPEED.speedDampTime, Time.deltaTime);
			}
			if (fow != 0f) 
			{
				// ... set the players rotation and set the speed parameter to 5.5f.
				ChangePosition ();
				anim.SetFloat (hash.float_speed, fow*SPEED.maxSpeed, SPEED.speedDampTime, Time.deltaTime);
			} 
			else 
			{		
				ChangePosition ();
				anim.SetFloat (hash.float_speed, 0f, SPEED.speedDampTime, Time.deltaTime);
			}
			//=============================to=Jump=========
			if(Input.GetButtonDown ("Jump"))
			{
				/*/
				timer+=Time.deltaTime;
				if(timer> maxSpaceTimer)
				{
				}
				/*/
				if(anim.GetInteger(hash.int_jump)==0)
					anim.SetInteger(hash.int_jump,1);
				state=State_of_Player.Jump;
			}
			//=============================to=Pass=========
			if(Input.GetMouseButtonDown(mouseIndex)&&anim.GetBool(hash.bool_ball))
			{
				state=State_of_Player.Pass;
				anim.SetBool(hash.bool_pass,true);
			}
			break;
		}
		case State_of_Player.Jump:
		{
			anim.SetFloat (hash.float_speed, jumpspeed, SPEED.speedDampTimeJump, Time.deltaTime);
			anim.SetFloat (hash.float_angular_speed, 0f, SPEED.speedDampTime, Time.deltaTime);
			//print(anim.GetFloat(hash.float_speed));
			ChangePosition();
			ChangeRotation();
			break;
		}
		case State_of_Player.Pass:
		{
			anim.SetFloat (hash.float_speed, 0f, SPEED.speedDampTime, Time.deltaTime);
			anim.SetFloat (hash.float_angular_speed, 0f, SPEED.speedDampTime, Time.deltaTime);
			ChangePosition();
			ChangeRotation();
			break;
		}
		case State_of_Player.Fall:
		{
			break;
			if(CHILDREN.Armature!=null)
			{
				transform.position=CHILDREN.Armature.position;
				CHILDREN.Armature.localPosition=new Vector3();
			}
			break;
		}
		}
		
	}

	void ChangePosition ()
	{
		speed = anim.GetFloat (hash.float_speed)* Time.deltaTime;
		//
		//print ("rigidbody:"+rigidbody.rotation.y);
		float rot = (GetComponent<Rigidbody>().rotation.eulerAngles.y)* Mathf.Deg2Rad;
		GetComponent<Rigidbody>().MovePosition(new Vector3(GetComponent<Rigidbody>().position.x+speed*Mathf.Sin(rot) ,
		                                   GetComponent<Rigidbody>().position.y,
		                                   GetComponent<Rigidbody>().position.z+speed*Mathf.Cos(rot) ));
		//
		//print (rigidbody.rotation.y+" "+rot+" "+Mathf.Sin(rot) );
		//print (rigidbody.rotation.eulerAngles);
	}
	void ChangeRotation()
	{
		angular_speed=anim.GetFloat (hash.float_angular_speed)* Time.deltaTime;
		//Quaternion deltaRotation = Quaternion.Euler(angular_speed*eulerAngleVelocity * Time.deltaTime);
		//rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
		GetComponent<Rigidbody>().angularVelocity = new Vector3 (0f, angular_speed*10, 0f);
		//print (rigidbody.rotation.y);
	}
	public void setBot(bool setbot)
	{
		OPTION.isBot = setbot;
		if(OPTION.isBot)
		{
			gameObject.name=_Name;
		}
		else
		{
			gameObject.name="Player";
		}
	}
	//======================Jump==================================
	void setJumpSpeed(float jspeed)
	{
		if(anim.GetFloat (hash.float_speed)>1f)
			jumpspeed = jspeed;
		if(jspeed==2f)
			anim.SetFloat(hash.float_speed,2f);
	}
	void isSecondJump()
	{
		if (Input.GetButton("Jump")) 
		{
			anim.SetFloat (hash.float_speed,1f);
			anim.SetInteger(hash.int_jump,2);
		}
		//Input.getB
	}
	void endJump()
	{
		state=State_of_Player.Normal;
		anim.SetInteger (hash.int_jump, 0);
	}
	//======================Pass===================================
	private int IndexOfPassForse=0;
	void isTimetoPass()
	{
		IndexOfPassForse++;
		//print ("isTimetoPass: IndexOfPassForse=" + IndexOfPassForse);
		if(!Input.GetMouseButton(mouseIndex))
			anim.SetBool(hash.bool_pass,false);
		/*/spe
		else
			if(IndexOfPassForse>=maxForseWhenRun&&state!=State_of_Player.Pass)
				anim.SetBool(hash.bool_pass,false);
		/*/
	}
	void Pass()
	{
		if(false&&!OPTION.isBot)
		{
			if(IndexOfPassForse-1>=contr.OPTIONS_FOR_PLAYERS.speeds_of_Ball.Length)
			{
				MonoBehaviour.print("Something wrong: IndexOfPassForse("+(IndexOfPassForse-1)+")>= speeds_of_Ball.Length("
				                    +contr.OPTIONS_FOR_PLAYERS.speeds_of_Ball.Length);
			}
			else
			{
				MonoBehaviour.print("current speeds_of_Ball:"+speeds_of_Ball[IndexOfPassForse-1]);
				ball_con.Pass(Camera_controller.main_Camera.transform.forward*speeds_of_Ball[IndexOfPassForse-1]);
			}
		}
		else
		{
			float distance=Vector3.Distance(transform.position,contr.OPTIONS_FOR_PLAYERS.test_Zenor.transform.position);
			float min_speed=getMinSpeed(distance);
			min_speed*=1.5f;
			ball_con.Pass(getVector_of_Speed(contr.OPTIONS_FOR_PLAYERS.test_Zenor.transform.position-transform.position,
			                                 min_speed,
			                                 getAngel(distance,min_speed)));
		}
		IndexOfPassForse = 0;
		//print ("Pass");
		OPTION.have_ball = false;
		anim.SetBool(hash.bool_ball, false);
		anim.SetBool(hash.bool_pass,false);
	}
	void endPass()
	{
		//print ("End Pass");
		state = State_of_Player.Normal;
	}
	public float getMinSpeed(float l)
	{
		return Mathf.Sqrt(l*9.8f);
	}
	public float getMinSpeed(Vector3 target)
	{
		return Mathf.Sqrt(Vector3.Distance(transform.position,target)*9.8f);
	}
	public Vector3 getVector_of_Speed(Vector3 direction,float speed, float angle)
	{
		MonoBehaviour.print ("tan(" + angle + ")=" + Mathf.Tan (angle));
		Vector3 result = new Vector3 (direction.x, Mathf.Sqrt (direction.x * direction.x + direction.z * direction.z)*Mathf.Tan(angle), direction.z);
		result = result * (speed / Mathf.Sqrt (result.x*result.x+result.y*result.y+result.z*result.z));
		return result;
	}
	internal float getAngel(float l,float speed)
	{
		C[0] = l * 9.8f/(2*speed*speed);
		switch(contr.OPTIONS_FOR_PLAYERS.angle)
		{
		case Angle_Pass.Long_Flight:
		{
			return (Mathf.PI/2-Mathf.Asin(2*C[0])/2);
			break;
		}
		case Angle_Pass.Short_Flight:
		{
			return (Mathf.Asin(2*C[0])/2);
			break;
		}
		}
		return 0;
	}
	//=====================Fall======================================
	public void Fall_Stay(Collider other)
	{
		Transform player = other.transform;
		MonoBehaviour.print ("Fall_Stay...s" +
			":" + player.gameObject.name);
		//state = State_of_Player.Fall;
		MonoBehaviour.print ((player != null).ToString()+" "+ OPTION.isBot.ToString()+" " + Input.GetButtonDown ("Test").ToString());
		if(player!=null&&OPTION.isBot&&Input.GetButtonDown("Test"))
		{
			float _y=Quaternion.FromToRotation(transform.forward,player.forward).eulerAngles.y;
			_y+=30;
			if(_y>=360)
				_y-=360;
			//MonoBehaviour.print(_y);
			int Index=((int)_y/60+1);
			MonoBehaviour.print("Index of fall:"+Index);
			anim.SetInteger(hash.int_fall,Index);
		}
	}
	public void Fall_Exit(Collider other)
	{
		Transform player = other.transform;
		//MonoBehaviour.print ("Fall_Exit:" + player.gameObject.name);
	}
	//===============================================================
	void Ball_Stay(Collider other)
	{
			//print ("onTriggerStay:"+other.name);
		if (!OPTION.have_ball && other.tag == Tags.ball) 
		{
			for(int i=0;i<arms.Length;i++)
			{
				arms[i].postion_of_ball=other.transform.position;
			}
			if (ball_con.getState () != State_of_Ball.Player && Input.GetMouseButton (1)) 
			{
				getBall();
			}
		}
	}
	void getBall()
	{
		ball_con.Take ();
		OPTION.have_ball = true;
		anim.SetBool (hash.bool_ball, true);
		for(int i=0;i<arms.Length;i++)
		{
			arms[i].setMyPresions(false);
		}
	}
	void Ball_Exit(Collider other)
	{
		if (!OPTION.have_ball && other.tag == Tags.ball)
		{
			for(int i=0;i<arms.Length;i++)
			{
				arms[i].setMyPresions(false);
			}
		}
	}
	void setBallPosition(int Index)
	{
		if(Index>=0&&Index<ball_position.Length)
			indexBallpos=Index;
		else
			print ("Error function setBallPosition in PlayerMovement.cs Index? ");
	}

	public Transform takeBallTransform(int Index)
	{
		if(Index>=0&&Index<ball_position.Length)
			return ball_position[Index];
		else
			return null;
	}
	//==========================================================Strategy======================================================
	public void SetAction(Strategy.Activities act)
	{
		current_action = act;
	}
	public Strategy.Activities getAction()
	{
		return current_action;
	}
	public void DestroyAction()
	{
		OPTION.isBlockPickUp = false;
		current_action = null;
	}

	//==========================================================Options=struct===============================================
	[System.Serializable]
	public enum State_of_Player {Normal,Jump,Pass,Fall};
	[System.Serializable]
	public enum Team{Black,White}

	[System.Serializable]
	public struct SpeedOptions
	{
		public float turnSmoothing ;   // A smoothing value for turning the player.
		public float speedDampTime ;  // The damping for the speed parameter
		public float speedDampTimeJump;//0.15f;
		public float maxSpeed ;
		public float maxAngularSpeed;
		public float maxSpaceTimer;
		public Vector3 eulerAngleVelocity ;
	}
	
	[System.Serializable]
	public struct Option
	{
		public PlayerMovement Test;
		public bool isPickUp;
		public bool isBlockPickUp;
		public int Index_Of_Mouse_Button;
		public float MouseLock;
		public bool isBot;
		public bool have_ball;
	}
	[System.Serializable]
	public struct Children_Transform
	{
		public Transform Armature;
	}
	[System.Serializable]
	public struct Action_Collider
	{
		public Collider_Action Ball;
		public Collider_Action Fall;
		public Collider_Action Navigation;
	}
	[System.Serializable]
	public struct Scripts
	{
		public Game_Controller contr;
	}
	[System.Serializable]
	public enum Angle_Pass{Long_Flight,Short_Flight};
	public class Oponents
	{
		public Transform Left_Oponent;
		public Transform Right_Oponent;
		public float distanceLeft;
		public float distanceRight;
		public void Update()
		{
			distanceLeft = Lenght (Left_Oponent);
			distanceRight = Lenght (Right_Oponent);
		}
		public float Lenght(Transform oponent)
		{
			return 0;
			/*/
			Vector3 point_1 = transform.position;
			Vector3 point_2 = point_1 + transform.forward;
			float delta_x = point_2.x - point_1.x;
			float delta_z = point_1.z - point_2.z;
			return ((delta_z)*oponent.position.x+(delta_x)*oponent.position.z+(point_1.x*point_2.z+point_2.x*point_1.z))/
				(Mathf.Sqrt(delta_x*delta_x+delta_z*delta_z));
			/*/
		}
	}
}
/*/
public float turnSmoothing = 1f;   // A smoothing value for turning the player.
public float speedDampTime = 0.5f;  // The damping for the speed parameter
public float speedDampTimeJump=0.25f;//0.15f;
public float maxSpeed =	8f;
public float maxAngularSpeed = 5f;
public float maxSpaceTimer = 1f;
public Vector3 eulerAngleVelocity = new Vector3(0, 500, 0);
/*/
