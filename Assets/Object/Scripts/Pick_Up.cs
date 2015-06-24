using UnityEngine;
using System.Collections;

public class Pick_Up : MonoBehaviour 
{
	public Transform Center;
	public Material mat;
	public Color[] color;

	bool isLeftSide;
	bool isTimeToPickUp=false;
	PlayerMovement _Zenor;
	public PlayerMovement Zenor
	{
		get{ return _Zenor;}
		set
		{
			if (!isTimeToPickUp)
				return;
			if(value.OPTION.isBlockPickUp)
				return;
			if(_Zenor!=null)
			{
				_Zenor.setPick(false);
			}
			_Zenor=value;
			_Zenor.setPick(true);
			Block=0;
			isLeftSide=Zenor.transform.position.x<Center.position.x;
		}
	}
	public Camera strategyCamera;
	public float Radius;
	bool _isWork;
	Animator anim;
	int _Block;
	int Block
	{
		get{return _Block;}
		set
		{
			//MonoBehaviour.print(_Block+"="+value);
			if(_Block==0)
			{
				setColor(1);
			}
			_Block=value;
			//MonoBehaviour.print(_Block);
			if(_Block==0&&!Block_Side)
			{
				setColor(0);
			}
		}
	}
	public bool isWork
	{
		get{ return _isWork;}
		set
		{
			_isWork=value;
			//MonoBehaviour.print("hello:"+value.ToString());
			anim.SetBool("PickTime",value);
			if(!_isWork&&_Zenor!=null)
			{
				_Zenor.setPick(false);
				_Zenor=null;
			}
		}
	}
	delegate void Action();
	event Action onMouseOver;
	event Action onMouseExit;

	void Awake()
	{
		anim = GetComponent<Animator> ();
		setColor (0);
	}
	bool Block_Side;
	public void Update()
	{
		if (!isTimeToPickUp)
			return;
		if (_Zenor == null||_Zenor.OPTION.isBlockPickUp)
			return;
		transform.position = strategyCamera.ScreenToWorldPoint (Input.mousePosition);
		transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
		if(transform.position.x<Center.position.x!=isLeftSide)
		{
			setColor(1);
			Block_Side=true;
		}
		else
		{
			if(_Block==0)
			{
				setColor(0);
			}
			Block_Side=false;
		}
		if (_isWork&&Zenor!=null&&Block==0&&!Block_Side) 
		{
			//MonoBehaviour.print( "Block:"+Block);
			Zenor.transform.position = transform.position;
		}
	}
	public void TimeToPickUp(bool b)
	{
		isTimeToPickUp = b;
		isWork = false;
	}
	void OnTriggerEnter(Collider other)
	{
		if (!isTimeToPickUp)
			return;
		if (_Zenor == null)
			return;
		if (other.tag==Tags.player&&!other.GetComponentInParent<PlayerMovement>().isPick()) 
		{
			/*/
			MonoBehaviour.print ("===============================");
			MonoBehaviour.print ("HellO:" + other.tag+" "+other.name);
			MonoBehaviour.print ("Zenor:" + Zenor.tag+" "+Zenor.name);
			MonoBehaviour.print("Welcome");
			MonoBehaviour.print ("===============================");
			/*/
			/*/
			Zenor.transform.position=Vector3.MoveTowards(Zenor.transform.position,
			                                      (2*Zenor.transform.position-other.transform.position)*50,
			                                      Radius-Vector3.Distance(Zenor.transform.position,other.transform.position));
			/*/
			Block++;
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (!isTimeToPickUp)
			return;
		if (_Zenor == null)
			return;
		if (other.tag==Tags.player&&!other.GetComponentInParent<PlayerMovement>().isPick()) 
		{
			/*/
			MonoBehaviour.print ("===============================");
			MonoBehaviour.print ("HellO:" + other.tag+" "+other.name);
			MonoBehaviour.print ("Zenor:" + Zenor.tag+" "+Zenor.name);
			MonoBehaviour.print ("Bye");
			MonoBehaviour.print ("===============================");
			/*/
			Block--;
			if(Block<0)
				Block=0;
		}
	}
	bool is_in_Zone(Vector3 a, Vector3 b)
	{
		return Vector3.Distance (a, b) < Radius;
	}
	void setColor(Color col)
	{
		mat.color = col;
	}
	void setColor(int i)
	{
		mat.color=color[i];
		//MonoBehaviour.print ("color:" + i);
	}


}
