using UnityEngine;
using System.Collections;
public class Camera_controller : MonoBehaviour {
	public Option OPTION;
	public GameObject current_object;
	public GameObject BallObj;
	public float speed=2f;
	public float angularspeed=100f;
	public RectCameraZone Zone;
	public float Rad_Rot;
	public Camera mainCamera;
	public State_of_Camera _state;
	public static Camera main_Camera;
	private float startTime;
	private float journeyLength;
	private Vector3 last_mouse_position;
	private float rot_Left_Right=0,rot_Up_Down=0;
	private Vector3 next_rotation;
	void Start () 
	{
		last_mouse_position = Input.mousePosition;
		main_Camera = mainCamera;
		//offset = transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if (current_object != null) 
		{
			if(OPTION.rotation_by_mouse)
			{
				Rotation_By_Mouse ();
			}
			move ();
			transform.rotation = current_object.transform.rotation;
		}
	}
	void move()
	{
		if (true||Vector3.Distance (transform.position, current_object.transform.position) < speed * Time.deltaTime) 
		{
			transform.position = current_object.transform.position;
		}//current_object.transform.rotation | Quaternion.AngleAxis(rot_Left_Right,Vector3.up).y+
		switch(_state)
		{
		case State_of_Camera.Mouse:
		{
			next_rotation= current_object.transform.position;
			next_rotation.y += rot_Up_Down;
			next_rotation += current_object.transform.right * rot_Left_Right;
			transform.localRotation = Quaternion.RotateTowards(transform.rotation, current_object.transform.rotation, angularspeed*Time.deltaTime);
			//mainCamera.transform.LookAt (next_rotation);
			//transform.localScale=new Vector3(1,1,1);
			break;
		}
		case State_of_Camera.Ball:
		{
			//mainCamera.transform.localRotation=Quaternion.Euler(new Vector3());
			mainCamera.transform.LookAt(BallObj.transform.position);
			break;
		}
		case State_of_Camera.Strategy:
		{
			break;
			next_rotation= current_object.transform.position;
			next_rotation.y += rot_Up_Down;
			next_rotation += current_object.transform.right * rot_Left_Right;
			transform.localRotation = Quaternion.RotateTowards(transform.rotation, current_object.transform.rotation, angularspeed*Time.deltaTime);
			mainCamera.transform.LookAt (next_rotation);
			transform.localScale=current_object.transform.localScale;
			break;
		}
		}
	}
	void Rotation_By_Mouse()
	{
		Vector3 current_mouse_position=Input.mousePosition;
		rot_Left_Right += (current_mouse_position.x - last_mouse_position.x) * angularspeed ;
		rot_Up_Down += (current_mouse_position.y - last_mouse_position.y) * angularspeed ;
		//=============normalization
		//
		rot_Left_Right = rot_Left_Right > Zone.Max_rot_left_right ? Zone.Max_rot_left_right : rot_Left_Right;
		rot_Left_Right = rot_Left_Right < Zone.Min_rot_left_right ? Zone.Min_rot_left_right : rot_Left_Right;
		rot_Up_Down = rot_Up_Down > Zone.Max_rot_up_down ? Zone.Max_rot_up_down : rot_Up_Down;
		rot_Up_Down = rot_Up_Down < Zone.Min_rot_up_down ? Zone.Min_rot_up_down : rot_Up_Down;
		//
		last_mouse_position = current_mouse_position;
		//print ("rot_Left_Right=" + rot_Left_Right+"|rot_Up_Down="+rot_Up_Down);
	}
	public void setRotation_by_Mouse(bool b)
	{
		OPTION.rotation_by_mouse = b;
	}
	public void setCurrentObject(GameObject _object)
	{
		current_object=_object;
		startTime = Time.time;
		journeyLength=Vector3.Distance (transform.position, current_object.transform.position);
	}
	public void setState(State_of_Camera state)
	{
		_state = state;
		switch(state)
		{
		case State_of_Camera.Mouse:
		{
			mainCamera.orthographic=false;
			break;
		}
		case State_of_Camera.Ball:
		{
			mainCamera.orthographic=false;
			break;
		}
		}
	}
	//========================struct======================
	[System.Serializable]
	public struct RectCameraZone
	{
		public float Max_rot_up_down;
		public float Min_rot_up_down;
		public float Max_rot_left_right;
		public float Min_rot_left_right;
	}
	[System.Serializable]
	public enum State_of_Camera{Mouse,Ball,Strategy};
	[System.Serializable]
	public struct Option
	{
		public bool rotation_by_mouse;
	}

}
